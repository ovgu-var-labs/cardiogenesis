using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderAudioControl : MonoBehaviour
{
    
    //private bool isMuted = false;
    //private AudioSource[] sources;

    public void SetMute(bool isMuted)
    {
        AudioSource[] sources = gameObject.GetComponents<AudioSource>();
        
        if (sources.Length == 2)
        {
            sources[1].mute = isMuted;
        }
        else
        {
            Debug.LogWarning("SliderAudioControl: " + sources.Length + " Audio Source Components detected!");
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
