using UnityEngine;
using System.Collections;

public class DrawSegmentedUser : MonoBehaviour {
    private PXCMSenseManager senseManager;
    private PXCM3DSeg segmentationModule;

    private Texture2D segmentationTexture = null;

    // Use this for initialization
    void Start () {
        if (!InitializeRealSense())
        {
            senseManager = null;
            Destroy(gameObject.transform.parent.gameObject);
        }
	}

    private bool InitializeRealSense()
    {
        senseManager = PXCMSenseManager.CreateInstance();

        if (senseManager == null)
        {
            Debug.LogError("Unable to create SenseManager.");
            return false;
        }

        if (senseManager.Enable3DSeg().IsError())
        {
            Debug.LogError("Couldn't enable the Face Module.");
            return false;
        }

        segmentationModule = senseManager.Query3DSeg();

        if (segmentationModule == null)
        {
            Debug.LogError("Couldn't query the Face Module.");
            return false;
        }

        if (senseManager.Init().IsError())
        {
            Debug.LogError("Unable to initialize SenseManager.");
            return false;
        }

        return true;
    }

    void OnDestroy()
    {
        if (segmentationModule != null)
        {
            segmentationModule.Dispose();
        }

        if (senseManager != null)
        {
            senseManager.ReleaseFrame();
            senseManager.Close();
            senseManager.Dispose();
        }
    }

    void Update () {
        if (senseManager == null)
        {
            return;
        }

        if (senseManager.AcquireFrame(false).IsError())
        {
            Debug.LogError("Failed to acquire frame.");
        }

        PXCMImage image = segmentationModule.AcquireSegmentedImage();

        if (segmentationTexture == null)
        {
            segmentationTexture = new Texture2D(image.info.width, image.info.height, TextureFormat.ARGB32, false);
            gameObject.GetComponent<Renderer>().sharedMaterial.mainTexture = segmentationTexture;
        }

        PXCMImage.ImageData imageData;

        if (image.AcquireAccess(PXCMImage.Access.ACCESS_READ, PXCMImage.PixelFormat.PIXEL_FORMAT_RGB32, out imageData).IsSuccessful())
        {
            imageData.ToTexture2D(0, segmentationTexture);
            image.ReleaseAccess(imageData);
            segmentationTexture.Apply();
        }

        senseManager.ReleaseFrame();
	}
}
