using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task1 : MonoBehaviour
{
    
    [SerializeField]
    private float globalAnimationProgress = 0;
    
    [SerializeField]
    private float targetValue = 0.5f;

    [SerializeField]
    private GameObject scalingHandle;


    GlobalAnimationController gAC;

    private float previousProgressValue = -1f, initialTransform = -1;


    public void SkipTask()
    {
        if (!gAC.isAnimationOnly)
        {
            scalingHandle.SetActive(false);
            gAC.SkipTask();
        }
    }

    public void ResetTask()
    {
        if (!gAC.isAnimationOnly)
        {
            scalingHandle.SetActive(true);
            scalingHandle.transform.localScale = new Vector3(1,1,1);
            previousProgressValue = initialTransform;
            globalAnimationProgress = 0;
            gAC.ResetTask();
        }
    }


    
    // Start is called before the first frame update
    void Start()
    {
        gAC = this.GetComponent<GlobalAnimationController>();
        
        initialTransform = scalingHandle.transform.localScale.x;
        previousProgressValue = initialTransform;

        if (gAC.isAnimationOnly)
        {
            scalingHandle.SetActive(false);
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
            float handleTransform = Mathf.Clamp(scalingHandle.transform.localScale.x, 0, initialTransform);

            if (previousProgressValue != handleTransform)
            {
                globalAnimationProgress = 1.0f - ((handleTransform - targetValue) / (initialTransform - targetValue));
                previousProgressValue = handleTransform;
                gAC.globalAnimationProgress = globalAnimationProgress;
            }

            if (globalAnimationProgress > 0.999f)
            {
                scalingHandle.GetComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>().ForceEndManipulation();
                scalingHandle.SetActive(false);
            }
    }
}
