using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class DisplayScript : MonoBehaviour
{
    public int framerate = 120;
    public int vSync = 0;

    public bool Apply = false;

    public Camera[] displays;

    public bool multiscreen = false;
    // Use this for initialization
    void Start()
    {
        displays[0].targetDisplay = 0;
        int idx = 0;
        while (displays.Length>idx && Display.displays.Length > idx)
        {
            Debug.Log("Display:" + idx + "[" + Display.displays[idx].systemWidth+","+ Display.displays[idx].systemHeight + "]");
            Display.displays[idx].Activate(/*Display.displays[1].systemWidth, Display.displays[1].systemHeight, 10*/);
            Display.displays[idx].SetRenderingResolution(Display.displays[idx].systemWidth, Display.displays[idx].systemHeight);
            displays[idx].targetDisplay = idx;
            ++idx;
        }
        
        Apply = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (Apply)
        {
            Apply = false;
            Application.targetFrameRate = framerate;
            QualitySettings.vSyncCount = vSync;
        }
    }
}
