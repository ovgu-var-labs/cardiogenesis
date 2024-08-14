using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sits on each task.
/// Task is to update animation of mudbun in current stage.
/// </summary>
public class GlobalAnimationManager : MonoBehaviour
{

    public GameObject[] animatedObjects;
    public ObjectAnimationController[] animationControllers;

    public GameObject annotations;

    public ButtonFunctionality buttons;

    public bool animationAutomaticPlaying;
    public float stepSize = 0.2f;
    public float animationProgress;

    // Start is called before the first frame update
    void Start()
    {
        animationControllers = new ObjectAnimationController[animatedObjects.Length];
        for (int i = 0; i < animatedObjects.Length; i++)
        {
            animationControllers[i] = animatedObjects[i].GetComponent<ObjectAnimationController>();
        }

        buttons = FindObjectOfType<ButtonFunctionality>();
        if (buttons == null)
        {
            Debug.LogError("buttons must be referenced in Global Animation Manager");
            
        }

        annotations = gameObject.GetComponentInChildren<ShowAnnotationScript>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (animationAutomaticPlaying)
            UpadtePlayingAnimation();
    }

    public void SetAnimationAutomaticPlaying(bool animationAutomaticPlaying_)
    {
        animationAutomaticPlaying = animationAutomaticPlaying_;
    }

    /// <summary>
    /// When playing animation is activated, the animation plays by itself.
    /// Also the progress slider gets updated
    /// </summary>
    public void UpadtePlayingAnimation()
    {
        animationProgress = (animationProgress + stepSize * Time.deltaTime) % 1;
        UpdateAnimationTo(animationProgress,true);
    }


    public void UpdateAnimationTo(float animationValue, bool changeSliderPosition)
    {
        animationProgress = animationValue;
        for(int i = 0; i < animationControllers.Length; i++)
        {
            animationControllers[i].SetAnimProgress(animationValue);
        }
        if(changeSliderPosition)
            buttons.ChangeAnimationSliderValue(animationProgress);  // Hier liegt das problem....
    }

    public float GetAnimationProgress()
    {
        return animationProgress;
    }
}
