using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AucioMasterController : MonoBehaviour
{
    [SerializeField]
    private GameObject warningIndicator;
    
    [SerializeField]
    private AudioSource[] AudioObjects;
    
    public void StopAllAudioPlaying()
    {
        foreach (AudioSource aSource in AudioObjects)
        {
            aSource.Stop();
            warningIndicator.SetActive(false);
        }
    }

    void Update ()
    {
        foreach (AudioSource aSource in AudioObjects)
        {
            if (aSource.isPlaying && aSource.gameObject.activeInHierarchy)
            {
                warningIndicator.SetActive(true);
                break;
            }
            else
            {
                warningIndicator.SetActive(false);
            }
        }
    }
}
