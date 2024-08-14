using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine.SceneManagement;
using System;

public class GlobalAnimationController : MonoBehaviour
{
    // StageManager stageManager;

    public DetectActivity detectActivity;
    public int stageId;
    public TaskHandler taskHandler;

    [SerializeField]
    public bool isAnimationOnly = false;
    
    [SerializeField]
    public float globalAnimationProgress = 0;

    [SerializeField]
    private GameObject progressBar, progressSlider;
    private ProgressIndicatorLoadingBar progressIndicatorBar;
    private PinchSlider pinchSlider;

    [SerializeField]
    private GameObject achievementNotification, playPauseToggle, audioGuideToggle, advancedAudioSettingsToggle, nextButton, transformBtn, modelBtn, showAnnotationBtn, hideAnnotationBtn, skipButton, resetButton, annotations, leftHandHighlight, rightHandHighlight, transformObj, modellingObj, audioObj;

    [SerializeField]
    private GameObject[] animatedObjects;

    private ObjectAnimationController[] animControllers;

    private AudioSource audioSource;

    private float stepSize = 0.005f;

    public bool isPlaying = false, isSolved = false;

    private bool isfirstTimeSolved = true;

    // Variables for creating meshes
    public bool automove = false;
    public System.DateTime startTime;
    public System.DateTime endTime;

    private void Awake()
    {
        //stageManager = StageManager.Instance;

        nextButton.SetActive(true);
        var nextStageButton = nextButton.GetComponentInChildren<Interactable>();
        var nextStagePressReceiver = nextStageButton.GetReceiver<InteractableOnPressReceiver>();
        nextStagePressReceiver.OnPress.AddListener(nextStage);

        nextButton.SetActive(false);

    }

    // Start is called before the first frame update
    void Start()
    {
        animControllers = new ObjectAnimationController[animatedObjects.Length];
        for (int i = 0; i < animatedObjects.Length; i++)
        {
            animControllers[i] = animatedObjects[i].GetComponent<ObjectAnimationController>();
        }

        pinchSlider = progressSlider.GetComponent<PinchSlider>();

        progressIndicatorBar = progressBar.GetComponent<ProgressIndicatorLoadingBar>();
        progressIndicatorBar.Progress = 0;
        OpenProgressIndicator();

        pinchSlider.gameObject.SetActive(isAnimationOnly);
        achievementNotification.SetActive(false);
        playPauseToggle.SetActive(isAnimationOnly);

        audioSource = audioObj.GetComponent<AudioSource>();
        audioGuideToggle.SetActive(false);
        advancedAudioSettingsToggle.SetActive(false);

        if (isAnimationOnly)
        {
            leftHandHighlight.GetComponent<FollowHand>().SetStateToAnimationOnly();
            rightHandHighlight.GetComponent<FollowHand>().SetStateToAnimationOnly();
        }

        transformObj.GetComponent<BoxCollider>().enabled = false;
        modellingObj.GetComponent<BoxCollider>().enabled = true;
        
    }

    public void ToggleAnnotations(Boolean isShowing)
    {
        showAnnotationBtn.SetActive(!isShowing);
        hideAnnotationBtn.SetActive(isShowing);
        annotations.SetActive(isShowing);
    }

    public void ToggleInteractionMode(Boolean isManipulating)
    {
        transformBtn.SetActive(!isManipulating);
        modelBtn.SetActive(isManipulating);

        transformObj.GetComponent<BoxCollider>().enabled = isManipulating;
        modellingObj.GetComponent<BoxCollider>().enabled = !isManipulating;
    }

    public void SkipTask()
    {
        isSolved = true;
        isPlaying = false;
        pinchSlider.gameObject.SetActive(true);
        achievementNotification.SetActive(false);
        playPauseToggle.SetActive(true);
        nextButton.SetActive(true);
        globalAnimationProgress = 1;
        leftHandHighlight.GetComponent<FollowHand>().SetStateToSolved(true);
        rightHandHighlight.GetComponent<FollowHand>().SetStateToSolved(true);

        transformBtn.SetActive(true);
        // transformBtn.GetComponent<Interactable>().IsEnabled = false;
        // transformObj.GetComponent<BoxCollider>().enabled = true;
        transformObj.GetComponent<BoxCollider>().enabled = false;
        modellingObj.GetComponent<BoxCollider>().enabled = false;
        modelBtn.SetActive(false);

        showAnnotationBtn.SetActive(false);
        hideAnnotationBtn.SetActive(true);
        annotations.SetActive(true);

        resetButton.SetActive(true);
        skipButton.SetActive(false);

        if (isfirstTimeSolved)
        {
            StartCoroutine(AudioPlaying(0.1f));
        }


        //-------------------
        detectActivity.totalTime =-1;
        detectActivity.activeTime =-1;
    }


    public void ResetTask()
    {
        isSolved = false;
        pinchSlider.gameObject.SetActive(false);
        achievementNotification.SetActive(false);
        
        if (isPlaying)
        {
            playPauseToggle.GetComponent<Interactable>().TriggerOnClick(true);
        }
        isPlaying = false;
        playPauseToggle.SetActive(false);

        transformBtn.SetActive(true);
        //transformBtn.GetComponent<Interactable>().IsEnabled = true;
        transformObj.GetComponent<BoxCollider>().enabled = false;
        modellingObj.GetComponent<BoxCollider>().enabled = true;
        modelBtn.SetActive(false);

        showAnnotationBtn.SetActive(true);
        hideAnnotationBtn.SetActive(false);
        annotations.SetActive(false);

        resetButton.SetActive(false);
        skipButton.SetActive(true);

        globalAnimationProgress = 0;
        leftHandHighlight.GetComponent<FollowHand>().SetStateToSolved(false);
        rightHandHighlight.GetComponent<FollowHand>().SetStateToSolved(false);

        //-------------------
        detectActivity.totalTime =0;
        detectActivity.activeTime =0;
    }

    public void SetAutoPlay(bool isAutoPlaying)
    {
        isPlaying = isAutoPlaying;
        
        // if (isPlaying)
        // {
        //     StartCoroutine(AudioPlaying(0.5f));
        // }

        // if (isfirstTimeSolved)
        // {
        //     isfirstTimeSolved = false;
        //     nextButton.SetActive(true);
        // }

    }

    public void ToggleSetAutoPlay()
    {
        SetAutoPlay(!isPlaying);
    }

    public void ChangeStepSize(int step)
    {
        float changedStepSize = stepSize + (float)step*0.0025f;

        if (changedStepSize < 0.00125f)
        {
            changedStepSize = stepSize + (float)step*0.00025f;
        }

        stepSize = Mathf.Clamp(changedStepSize, 0.00025f, 0.015f);
    }


    public void SetAnimationProgressToSliderValue()
    {
        if (isSolved)
        {
            if (pinchSlider != null)
            {
                globalAnimationProgress = pinchSlider.SliderValue;
            }

            if (progressIndicatorBar != null)
            {
                progressIndicatorBar.Progress = Mathf.Clamp01(globalAnimationProgress);
                UpdateAnimations();
            }
        }
        

    }

    private void UpdateAnimations()
    {
        for (int i = 0; i < animControllers.Length; i++)
        {
            animControllers[i].SetAnimProgress(globalAnimationProgress);
        }
    }
    
    

    private async void OpenProgressIndicator()
    {
        await progressIndicatorBar.OpenAsync();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isPlaying)
        {
            //resetButton.SetActive(false);
            //resetButton.GetComponent<Interactable>().IsEnabled = false;
                

            
            
            globalAnimationProgress += stepSize;
            globalAnimationProgress = (globalAnimationProgress <= 1.0f) ? globalAnimationProgress : 0;

            pinchSlider.SliderValue = Mathf.Clamp01(globalAnimationProgress);
        }
        else
        {

            if (automove)
            {
                AutoMove();
            }
            // globalAnimationProgress is updated by Task Script
            resetButton.SetActive(!isAnimationOnly && isSolved);
            //resetButton.GetComponent<Interactable>().IsEnabled = true;

            pinchSlider.SliderValue = Mathf.Clamp01(globalAnimationProgress);

        }

        if (globalAnimationProgress > 0.999f && !isSolved)
        {
            pinchSlider.gameObject.SetActive(true);
            
            if (!isAnimationOnly && isfirstTimeSolved)
            {
                achievementNotification.SetActive(true);
                StartCoroutine(WaitAndHideAchievement(3f));
                StartCoroutine(AudioPlaying(1.5f));
            }

            playPauseToggle.SetActive(true);
            nextButton.SetActive(true);

            transformBtn.SetActive(true);
            //transformBtn.GetComponent<Interactable>().IsEnabled = false;
            transformObj.GetComponent<BoxCollider>().enabled = false;
            modellingObj.GetComponent<BoxCollider>().enabled = false;
            modelBtn.SetActive(false);

            showAnnotationBtn.SetActive(false);
            hideAnnotationBtn.SetActive(true);
            annotations.SetActive(true);

            skipButton.SetActive(false);
            resetButton.SetActive(!isAnimationOnly);

            leftHandHighlight.GetComponent<FollowHand>().SetStateToSolved(true);
            rightHandHighlight.GetComponent<FollowHand>().SetStateToSolved(true);

            isSolved = true;
            isfirstTimeSolved = false;
            // isPlaying = true;

            notifyStageCompleted();
        }

        if (!audioSource.isPlaying)
        {
            audioGuideToggle.GetComponent<Interactable>().IsToggled = false;
        }
    }

    IEnumerator WaitAndHideAchievement (float timeInSeconds)
    {
        yield return new WaitForSecondsRealtime(timeInSeconds);
        achievementNotification.SetActive(false);
    }

    IEnumerator AudioPlaying(float delayInSecondsRealTime)
    {
        if (!audioSource.isPlaying && !audioSource.mute)
        {
            // audioGuideToggle.SetActive(true);
            advancedAudioSettingsToggle.SetActive(true);
            yield return new WaitForSecondsRealtime(delayInSecondsRealTime);
            playPauseToggle.GetComponent<Interactable>().TriggerOnClick(true);
            // Hoffe das dann problem weggeht
            audioGuideToggle.GetComponent<Interactable>().TriggerOnClick(true);
        }

        // audioPlayingIndicatorObj.SetActive(true);
        // yield return new WaitWhile(() => (source.isPlaying && source.gameObject.activeInHierarchy));

        // audioPlayingIndicatorObj.SetActive(false);
    }

    private void notifyStageCompleted()
    {
        float totalTime = detectActivity.totalTime;
        float activeTime = detectActivity.activeTime;
        detectActivity.gameObject.SetActive(false);

        // stageManager.notifyStageFinished(stageManager.currentStage.id, totalTime, activeTime);
    }
    void nextStage()
    {
        //for testing purposes

        notifyStageCompleted();

        ////////
        detectActivity.totalTime = 0;
        detectActivity.activeTime = 0;

        int nextStageId = stageId + 1;
       // taskHandler.StartCoroutine(taskHandler.setTask(nextStageId));
    }

    /// <summary>
    /// Methode used to create mesh models of the heart.
    /// Automatically moves the progress bar by 1% every 10 seconds. In meantime mesh can be saved
    /// </summary>
    void AutoMove()
    {
        if (startTime == null)
            startTime = System.DateTime.UtcNow;
        if (automove && (System.DateTime.UtcNow.AddSeconds(-5) >= startTime))
        {
            Debug.Log("Are now moving");
            globalAnimationProgress = globalAnimationProgress + 0.005f;
            startTime = System.DateTime.UtcNow;
        }
    }

    public bool GetIsSolved()
    {
        return isSolved;
    }

}
