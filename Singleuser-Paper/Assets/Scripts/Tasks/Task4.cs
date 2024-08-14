using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task4 : MonoBehaviour
{
  [SerializeField]
    private float globalAnimationProgress = 0;
    
    [SerializeField]
    private float targetValue = 0f;

    [SerializeField]
    private GameObject slidingHandle;


    GlobalAnimationController gAC;

    private float previousProgressValue = -1f, initialTransform = -1;


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
            slidingHandle.transform.localPosition = new Vector3(-0.2f,0.075f,0.6f);
            previousProgressValue = initialTransform;
            globalAnimationProgress = 0;
            gAC.ResetTask();
        }
    }

    
    // Start is called before the first frame update
    void Start()
    {
        gAC = this.GetComponent<GlobalAnimationController>();
        
        initialTransform = slidingHandle.transform.localPosition.z;
        previousProgressValue = initialTransform;

        if (gAC.isAnimationOnly)
        {
            slidingHandle.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
            if (slidingHandle.transform.localPosition.z > 0.6001f)
            {
                slidingHandle.transform.localPosition = new Vector3(-0.2f,0.075f,0.6f);
                return;
            }
            
            float currentHandleTransform = Mathf.Clamp(slidingHandle.transform.localPosition.z, 0, 0.6f);

            if (previousProgressValue != currentHandleTransform)
            {
                globalAnimationProgress = (0.6f-currentHandleTransform) / 0.6f;
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

