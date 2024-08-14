using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;

public class StudyController : MonoBehaviour
{
    
    [SerializeField]
    private GameObject setupGuide, nextElement;

    [SerializeField]
    bool isAnimationOnly = false;

    [SerializeField]
    private GameObject[] animControllers;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject GO in animControllers)
        {
            GO.GetComponent<GlobalAnimationController>().isAnimationOnly = isAnimationOnly;
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && setupGuide.activeSelf)
        {
            Debug.LogWarning("Setup Guide disabled!");
            setupGuide.SetActive(false);
            nextElement.SetActive(true);
        }
    }
}
