using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task3 : MonoBehaviour
{
  [SerializeField]
    private float globalAnimationProgress = 0;
    
    [SerializeField]
    private float targetValue = 60f;

    [SerializeField]
    private GameObject rotationHandle;


    GlobalAnimationController gAC;

    private float previousProgressValue = -1f, initialTransform = -1;


    public void SkipTask()
    {
        if (!gAC.isAnimationOnly)
        {
            rotationHandle.SetActive(false);
            gAC.SkipTask();
        }
    }

    public void ResetTask()
    {
        if (!gAC.isAnimationOnly)
        {
            rotationHandle.SetActive(true);
            rotationHandle.transform.localEulerAngles = new Vector3(0,0,0);
            previousProgressValue = initialTransform;
            globalAnimationProgress = 0;
            gAC.ResetTask();
        }
    }

    
    // Start is called before the first frame update
    void Start()
    {
        gAC = this.GetComponent<GlobalAnimationController>();
        
        initialTransform = 360-rotationHandle.transform.eulerAngles.x;
        previousProgressValue = initialTransform;

        if (gAC.isAnimationOnly)
        {
            rotationHandle.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (rotationHandle.gameObject.activeInHierarchy && rotationHandle.GetComponent<BoxCollider>().enabled)
        {
            if (360-rotationHandle.transform.eulerAngles.x > targetValue+1)
            {
                rotationHandle.transform.localEulerAngles = new Vector3(0,0,0);
                return;
            }
            
            float currentHandleTransform = Mathf.Clamp(360-rotationHandle.transform.eulerAngles.x, 0, targetValue);

            if (previousProgressValue != currentHandleTransform)
            {
                globalAnimationProgress = (currentHandleTransform) / (targetValue);
                previousProgressValue = currentHandleTransform;
                gAC.globalAnimationProgress = globalAnimationProgress;
            }

            if (globalAnimationProgress > 0.999f)
            {
                rotationHandle.GetComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>().ForceEndManipulation();
                rotationHandle.SetActive(false);
            }
        }
    }
}
