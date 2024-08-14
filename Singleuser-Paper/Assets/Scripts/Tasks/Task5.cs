using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task5 : MonoBehaviour
{
  [SerializeField]
    private float globalAnimationProgress = 0;
    
    [SerializeField]
    private float targetValue = 0f;

    [SerializeField]
    private GameObject slidingHandle;


    GlobalAnimationController gAC;

    private float previousProgressValue = -1f, initialTransform = -1;

    private Vector3 startPos;

    public void SkipTask()
    {
        if (!gAC.isAnimationOnly)
        {
            slidingHandle.SetActive(false);
            gAC.SkipTask();
        }
    }

    public void ResetTask()
    {
        if (!gAC.isAnimationOnly)
        {
            slidingHandle.SetActive(true);
            slidingHandle.transform.localPosition = new Vector3(-0.093f,0.341f,0.537f);
            previousProgressValue = initialTransform;
            globalAnimationProgress = 0;
            gAC.ResetTask();
        }
    }

    
    // Start is called before the first frame update
    void Start()
    {
        gAC = this.GetComponent<GlobalAnimationController>();
        
        startPos = slidingHandle.transform.localPosition;
        initialTransform = slidingHandle.transform.localPosition.y;
        previousProgressValue = initialTransform;

        if (gAC.isAnimationOnly)
        {
            slidingHandle.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
            float currentHandleTransform = 0;
            
            if (initialTransform > 0)
            {
                if (slidingHandle.transform.localPosition.y > initialTransform + 0.001f)
                {
                    slidingHandle.transform.localPosition = startPos;
                    return;
                }
                
                currentHandleTransform = Mathf.Clamp(slidingHandle.transform.localPosition.y, 0, initialTransform);
            }
            else
            {
                if (slidingHandle.transform.localPosition.y < initialTransform - 0.001f)
                {
                    slidingHandle.transform.localPosition = startPos;
                    return;
                }
                
                currentHandleTransform = Mathf.Clamp(slidingHandle.transform.localPosition.y, initialTransform, 0);
            }

            if (previousProgressValue != currentHandleTransform)
            {
                globalAnimationProgress = (initialTransform-currentHandleTransform) / (initialTransform-targetValue);
                previousProgressValue = currentHandleTransform;
                gAC.globalAnimationProgress = globalAnimationProgress;
            }

            if (globalAnimationProgress > 0.999f)
            {
                slidingHandle.GetComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>().ForceEndManipulation();
                slidingHandle.SetActive(false);
            }
    }
}


