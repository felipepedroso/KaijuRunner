using UnityEngine;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {
    public enum InputSource { REALSENSE, KEYBOARD }

    public const string JUMP_BUTTON = "Jump";
    public const string FIRE_BUTTON = "Fire1";

    private float horizontalAxis;
    private float verticalAxis;
    private Dictionary<string, bool> buttonHold;
    private Dictionary<string, bool> buttonUp;
    private Dictionary<string, bool> buttonDown;

    private Dictionary<PXCMFaceData.ExpressionsData.FaceExpression, bool> expressionsLastStatus;
    private Dictionary<PXCMFaceData.ExpressionsData.FaceExpression, float> expressionsThreshold;
    

    private PXCMSenseManager senseManager;
    private PXCMFaceModule faceModule;
    private PXCMFaceData faceData;
    private PXCMSmoother.Smoother2D smoother2D;

    public float VerticalAxis
    {
        get { return verticalAxis; }
    }

    public float HorizontalAxis
    {
        get { return horizontalAxis; }
    }
    
    public InputSource Source;
    public PXCMFaceData.ExpressionsData.FaceExpression JumpExpression;
    public PXCMFaceData.ExpressionsData.FaceExpression FireExpression;
    public int MaxYaw;
    public int MaxPitch;
    public float JumpThreshold;
    public float FireThreshold;

    void Start()
    {
        InitializeButtonMapping();
        
        if (Source == InputSource.REALSENSE)
        {
            if (!InitializeRealSense())
            {
                Source = InputSource.KEYBOARD;    
            }
            InitializeExpressionMapping();
        }
    }

    private void InitializeExpressionMapping()
    {
        expressionsLastStatus = new Dictionary<PXCMFaceData.ExpressionsData.FaceExpression, bool>();
        expressionsThreshold = new Dictionary<PXCMFaceData.ExpressionsData.FaceExpression, float>();

        InitializeExpression(JumpExpression, JumpThreshold);
        InitializeExpression(FireExpression, FireThreshold);
    }

    private void InitializeExpression(PXCMFaceData.ExpressionsData.FaceExpression expression, float expressionThreshold)
    {
        expressionsLastStatus.Add(expression, false);
        expressionsThreshold.Add(expression, expressionThreshold);
    }

    private void InitializeButtonMapping()
    {
        buttonHold = new Dictionary<string, bool>();
        buttonUp = new Dictionary<string, bool>();
        buttonDown = new Dictionary<string, bool>();

        InitializeButton(JUMP_BUTTON);
        InitializeButton(FIRE_BUTTON);
    }

    private void InitializeButton(string buttonName)
    {
        buttonHold.Add(buttonName, false);
        buttonUp.Add(buttonName, false);
        buttonDown.Add(buttonName, false);
    }

    private bool InitializeRealSense()
    {
        pxcmStatus status;
        senseManager = PXCMSenseManager.CreateInstance();

        if (senseManager == null)
        {
            Debug.Log("Unable to create SenseManager.");
            return false;
        }

        status = senseManager.EnableFace();

        if (status != pxcmStatus.PXCM_STATUS_NO_ERROR)
        {
            Debug.Log("Couldn't enable the Face Module.");
            return false;
        }

        faceModule = senseManager.QueryFace();

        if (faceModule == null)
        {
            Debug.Log("Couldn't query the Face Module.");
            return false;
        }

        PXCMFaceConfiguration faceConfiguration = faceModule.CreateActiveConfiguration();

        if (faceConfiguration == null)
        {
            Debug.Log("Couldn't create an active configuration.");
            return false;
        }

        faceConfiguration.pose.isEnabled = true;
        faceConfiguration.pose.maxTrackedFaces = 1;

        PXCMFaceConfiguration.ExpressionsConfiguration expressionsConfiguration = faceConfiguration.QueryExpressions();
        expressionsConfiguration.Enable();

        status = expressionsConfiguration.EnableExpression(JumpExpression);
        if (status != pxcmStatus.PXCM_STATUS_NO_ERROR)
        {
            Debug.Log("Unable to enable the expression " + JumpExpression + ".");
            return false;
        }

        status = expressionsConfiguration.EnableExpression(FireExpression);
        if (status != pxcmStatus.PXCM_STATUS_NO_ERROR)
        {
            Debug.Log("Unable to enable the expression " + FireExpression + ".");
            return false;
        }

        status = faceConfiguration.ApplyChanges();

        if (status != pxcmStatus.PXCM_STATUS_NO_ERROR)
        {
            Debug.Log("Unable to apply configuration settings.");
            return false;
        }

        faceData = faceModule.CreateOutput();
        if (faceData == null)
        {
            Debug.Log("Couldn't create the data output object.");
            return false;
        }

        status = senseManager.Init();

        if (status != pxcmStatus.PXCM_STATUS_NO_ERROR)
        {
            Debug.Log("Unable to initialize SenseManager.");
            return false;
        }

        PXCMSession session = senseManager.QuerySession();

        PXCMSmoother smoother;
        status = session.CreateImpl<PXCMSmoother>(out smoother);

        if (status != pxcmStatus.PXCM_STATUS_NO_ERROR)
        {
            Debug.Log("Failed to create the smoother.");
            return false;
        }

        smoother2D = smoother.Create2DWeighted(10);

        return true;
    }

    void Update() 
    {
        switch (Source)
        {
            case InputSource.REALSENSE:
                UpdateKeyboard();
                UpdateRealSense();
                
                break;
            case InputSource.KEYBOARD:
            default:
                UpdateKeyboard();
                break;
        }
    }

    private void UpdateRealSense()
    {
        if (senseManager == null)
        {
            Debug.Log("RealSense needs to be intialized properly. Changing the input to keyboard.");
            Source = InputSource.KEYBOARD;
            return;
        }

        if (senseManager.AcquireFrame(false) < pxcmStatus.PXCM_STATUS_NO_ERROR)
        {
            Debug.Log("Failed to acquire the frame.");
            return;
        }

        if (faceData != null)
        {
            faceData.Update();

            int numberOfFaces = faceData.QueryNumberOfDetectedFaces();

            if (numberOfFaces > 0)
            {
                PXCMFaceData.Face face = faceData.QueryFaceByIndex(0);

                ProcessPose(face);
                ProcessExpressions(face);
            }
        }
        senseManager.ReleaseFrame();
    }

    private void ProcessExpressions(PXCMFaceData.Face face)
    {
        PXCMFaceData.ExpressionsData expressionsData = face.QueryExpressions();

        if (expressionsData != null)
        {
            ProcessExpressionAsButton(expressionsData, JumpExpression, JUMP_BUTTON);
            ProcessExpressionAsButton(expressionsData, FireExpression, FIRE_BUTTON);
        }
    }

    private void ProcessExpressionAsButton(PXCMFaceData.ExpressionsData expressionsData, PXCMFaceData.ExpressionsData.FaceExpression expression, string button)
    {
        PXCMFaceData.ExpressionsData.FaceExpressionResult expressionResult;

        if (expressionsData.QueryExpression(expression, out expressionResult))
        {
            bool lastStatus = expressionsLastStatus[expression];
            bool currentStatus = expressionResult.intensity >= expressionsThreshold[expression];
            Debug.Log(expressionResult.intensity);

            buttonDown[button] = !lastStatus && currentStatus;
            buttonUp[button] = lastStatus && !currentStatus;
            buttonHold[button] = currentStatus;

            expressionsLastStatus[expression] = currentStatus;
        }
    }

    private void ProcessPose(PXCMFaceData.Face face)
    {
        PXCMFaceData.PoseData poseData = face.QueryPose();

        if (poseData != null)
        {
            PXCMFaceData.PoseEulerAngles poseEulerAngles;

            if (poseData.QueryPoseAngles(out poseEulerAngles))
            {
                //Debug.Log(string.Format("Roll {0}, Yaw {1}, Pitch {2}", poseEulerAngles.roll, poseEulerAngles.yaw, poseEulerAngles.pitch));

                PXCMPointF32 smoothedAngles = smoother2D.SmoothValue(new PXCMPointF32(poseEulerAngles.yaw, poseEulerAngles.pitch));

                float yaw = Mathf.Clamp(smoothedAngles.x, -Mathf.Abs(MaxYaw), Mathf.Abs(MaxYaw));
                float pitch = Mathf.Clamp(smoothedAngles.y, -Mathf.Abs(MaxPitch), Mathf.Abs(MaxPitch));

                //Debug.Log(string.Format("Yaw:{0} Pitch: {1}", yaw, pitch));

                horizontalAxis = yaw / MaxYaw;
                verticalAxis = pitch / MaxPitch;
            }
        }
    }

    private void UpdateKeyboard()
    {
        verticalAxis = Input.GetAxis("Vertical");
        horizontalAxis = Input.GetAxis("Horizontal");

        buttonHold[JUMP_BUTTON] = Input.GetButton(JUMP_BUTTON);
        buttonUp[JUMP_BUTTON] = Input.GetButtonUp(JUMP_BUTTON);
        buttonDown[JUMP_BUTTON] = Input.GetButtonDown(JUMP_BUTTON);

        buttonHold[FIRE_BUTTON] = Input.GetButton(FIRE_BUTTON);
        buttonUp[FIRE_BUTTON] = Input.GetButtonUp(FIRE_BUTTON);
        buttonDown[FIRE_BUTTON] = Input.GetButtonDown(FIRE_BUTTON);
    }

    public bool GetButton(string buttonName)
    {
        return buttonHold.ContainsKey(buttonName) ? buttonHold[buttonName] : false;
    }

    public bool GetButtonUp(string buttonName)
    {
        return buttonUp.ContainsKey(buttonName) ? buttonUp[buttonName] : false;
    }

    public bool GetButtonDown(string buttonName)
    {
        return buttonDown.ContainsKey(buttonName) ? buttonDown[buttonName] : false;
    }

    public void OnDestroy()
    {
        if (senseManager != null)
        {
            senseManager.Dispose();
        }
        if (smoother2D != null)
        {
            smoother2D.Dispose();
        }
    }
}
