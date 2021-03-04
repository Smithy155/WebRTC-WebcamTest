using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.MixedReality.WebRTC;
using Microsoft.MixedReality.WebRTC.Unity;
using TMPro;
using UnityEngine;

public class ToggleCamera : MonoBehaviour
{
    public WebcamSource WebcamSource;
    public TextMeshPro TextMeshPro;

    private string text;
    private Stopwatch stopwatch = new Stopwatch();
    
    // Start is called before the first frame update
    void Start()
    {
        text = TextMeshPro.text;
    }

    public void OnCLick()
    {
        stopwatch.Start();
        WebcamSource.enabled = !WebcamSource.enabled;
    }

    /// <summary>
    /// We measure here the time between clicking the webcam button and the first streamed frame
    /// </summary>
    /// <param name="source"></param>
    public void StopTheStopwatch(IVideoSource source)
    {
        stopwatch.Stop();
        if(stopwatch.ElapsedMilliseconds > 0)
            TextMeshPro.text = text + stopwatch.Elapsed;
        stopwatch.Reset();
    }
}
