using UnityEngine;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {
    public enum InputSource { REALSENSE, KEYBOARD }
    public InputSource Source;

    private float horizontalAxis;
    private float verticalAxis;

    public float VerticalAxis
    {
        get { return verticalAxis; }
    }

    public float HorizontalAxis
    {
        get { return horizontalAxis; }
    }

    private Dictionary<string, bool> buttonHold;
    private Dictionary<string, bool> buttonUp;
    private Dictionary<string, bool> buttonDown;
    
    public const string JUMP_BUTTON = "Jump";
    public const string FIRE_BUTTON = "Fire1";

    void Start()
    {
        InitializeButtonMapping();
        
        if (Source == InputSource.REALSENSE)
        {
            if (!InitializeRealSense())
            {
                Source = InputSource.KEYBOARD;    
            }
        }
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
        return false;
    }

    void Update() 
    {
        switch (Source)
        {
            case InputSource.REALSENSE:
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
        verticalAxis = 0.0f;
        horizontalAxis = 0.0f;
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
}
