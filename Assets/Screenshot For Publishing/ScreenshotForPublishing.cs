using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// NOTE: Place the "screenshot for publishing" prefab in the first scene
// Press 1/2/3/4 for taking a screenshot



// IOS resolutions
// 2048 * 2732 - 12.9 inch
// 1242 * 2688 -  6.5 inch
// 1242 * 2208 -  5.5 inch

// ANDROID resolutions
// feature 1024*500

public class ScreenshotForPublishing : MonoBehaviour
{

    public static ScreenshotForPublishing instance;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            print("Screenshot 1: "+Screen.width + "x" + Screen.height+" taken");
            ScreenCapture.CaptureScreenshot(Screen.width + "x" + Screen.height + "_1.png");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            print("Screenshot 2: " + Screen.width + "x" + Screen.height + " taken");
            ScreenCapture.CaptureScreenshot(Screen.width + "x" + Screen.height + "_2.png");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            print("Screenshot 3: " + Screen.width + "x" + Screen.height + " taken");
            ScreenCapture.CaptureScreenshot(Screen.width + "x" + Screen.height + "_3.png");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            print("Screenshot 4: " + Screen.width + "x" + Screen.height + " taken");
            ScreenCapture.CaptureScreenshot(Screen.width + "x" + Screen.height + "_4.png");
        }
    }
}
