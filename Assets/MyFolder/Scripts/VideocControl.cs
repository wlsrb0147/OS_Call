using System;
using UnityEngine;
using UnityEngine.Video;

public class VideocControl : MonoBehaviour
{
    private VideoPlayer vp;

    private void Awake()
    {
        vp = gameObject.GetComponent<VideoPlayer>();
        vp.started += VpOnstarted; 
    }

    private void VpOnstarted(VideoPlayer source)
    {
        Invoke(nameof(DisableWebcam),22f);
    }

    private void DisableWebcam()
    {
        
    }
}
