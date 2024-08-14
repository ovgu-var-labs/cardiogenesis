using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// TODO: Script umschreiben auf MRTK 2
/// </summary>
public class ButtonFunctionality : MonoBehaviour
{
    public GameObject mudRender;
    public GameObject stageManager;
    public GameObject antwortObject;
    public GameObject frageObjeckt;
    public GameObject infobar;

    public bool isShowingText = true;

    public bool sliderIsToggled;

    public PinchSlider AnimationSlider;
    public bool isAutomaticPlaying;
    public float progressSliderValue;

    public bool rotationIsLocked;

    public bool markierungIsLocked;

    public bool annotationsEnabled;

    public bool taskIsDeactivated = false;

    public float cooldownTime;              // Cooldown Time since last button press
    public float timeSinceLastPress;

    public AsyncOperation asyncOperation;

    public bool wantToLoadNextScene = false;

    // Start is called before the first frame update
    void Start()
    {
        annotationsEnabled = false;
        timeSinceLastPress = 0f;
        int nextSceneID = Mathf.Abs((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
        // asyncOperation = SceneManager.LoadSceneAsync(nextSceneID);
        //asyncOperation = SceneManager.LoadSceneAsync(Mathf.Abs((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCount));

        // asyncOperation.allowSceneActivation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!sliderIsToggled)
            // ChangeSliderValue();
        if (isAutomaticPlaying)
        {

        }
        progressSliderValue = AnimationSlider.SliderValue;
        timeSinceLastPress += Time.deltaTime;

        if (Input.GetKeyDown("space"))
            SceneManager.LoadScene(0);
        if (Input.GetKeyDown(KeyCode.H))
        {
            NextStage();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            PreviousStage();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            ToggleRotationIsLocked();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleAnnotationsEnabled();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            ToggleIsAutomaticPlaying();
        }

        /*
        if (!wantToLoadNextScene && stageManager.GetComponent<StageManager>().GetCurrentStage() == SceneManager.GetActiveScene().buildIndex)
        {
            Debug.Log("Loading Scene " + SceneManager.GetActiveScene().buildIndex+1);
            asyncOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            asyncOperation.allowSceneActivation = false;
            wantToLoadNextScene = true;
        }
        */
    }

    public void FinishTask()
    {
        Debug.Log("Finish Task");
        
        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(!gameObject.transform.GetChild(i).gameObject.activeSelf);
        }
        taskIsDeactivated = true;
            
        
    }

    /*
     * Check delete
    public void rorateHeart(Slider slider)
    {
        var rotationAngle = (slider.Value * 360) - 180;
        mudRender.transform.eulerAngles = new Vector3(mudRender.transform.eulerAngles.x, rotationAngle, mudRender.transform.eulerAngles.z);
    }
    */
    /// <summary>
    /// Has to be assigned on rotationslider
    /// </summary>
    public void ChangeSliderIsToggled()
    {
        sliderIsToggled = !sliderIsToggled;
    }


    public void ChangeAnimationSliderValue(float sliderValue_)
    {
        AnimationSlider.SliderValue = sliderValue_;
    }


    public void ToggleIsAutomaticPlaying()
    {
        isAutomaticPlaying = !isAutomaticPlaying;
        stageManager.GetComponent<StageManager>().SetAnimationAutomaticPlaying(isAutomaticPlaying);

    }
    /// <summary>
    /// Called when slider value is changed
    /// </summary>
    public void AnimationProgressSliderUpdated()
    {
        stageManager.GetComponent<StageManager>().SetAnimationProgress(AnimationSlider.SliderValue);
    }

    public void NextStage()
    {
        AnimationSlider.SliderValue = 0;
        stageManager.GetComponent<StageManager>().LoadNextStage();
        isAutomaticPlaying = false;
    }

    public void PreviousStage()
    {
        AnimationSlider.SliderValue = 0;
        stageManager.GetComponent<StageManager>().LoadPreviousStage();
        isAutomaticPlaying = false;
    }

    public void NextStageScene()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        wantToLoadNextScene = true;
        asyncOperation.allowSceneActivation = true;
    }

    public void PreviousStageScene()
    {
        SceneManager.LoadScene(Mathf.Abs((SceneManager.GetActiveScene().buildIndex - 1) % SceneManager.sceneCountInBuildSettings));
    }

    public void ToggleRotationIsLocked()
    {
        rotationIsLocked = !rotationIsLocked;
        stageManager.GetComponent<PlayerManager>().SetRotationIsLocked(rotationIsLocked);
    }

    public bool GetRotationIsLocked()
    {
        return rotationIsLocked;
    }

    public void ToggleMarkerungIsLocked()
    {
        markierungIsLocked = !markierungIsLocked;
        stageManager.GetComponent<PlayerManager>().SetMarkingIsLocked(markierungIsLocked);
    }

    public bool GetMarkierungIsLocked()
    {
        return markierungIsLocked;
    }

    public void ToggleAnnotationsEnabled()
    {
        annotationsEnabled = !annotationsEnabled;
        stageManager.GetComponent<StageManager>().SetAnnotationsEnabled(annotationsEnabled);
    }

    public bool GetAnnotationsEnabled()
    {
        return annotationsEnabled;
    }

    // Is not used in quest version of project
    /// <summary>
    /// Method which coordinates the interface and text
    /// </summary>
    /// <param name="value">1 for task, 2 for answer</param>
    public void ChangeInterfaceText(int value)
    {
        Debug.Log("Text .");
        if (isShowingText)
        {
            EnableInterface();
            return;
        }
        else if(value == 1)
        {
            EnableTask();
            return;
        }
        else if(value == 2)
        {
            EnableAnswer();
            return;
        }
    }

    private void EnableInterface()
    {
        foreach(Transform childTransform in gameObject.transform)
        {
            var child = childTransform.gameObject;
            if ((child != antwortObject && child != frageObjeckt) || child == infobar)
                child.SetActive(true);
            else
                child.SetActive(false);

        }
        isShowingText = false;
        Debug.Log("Text Interface");
    }

    private void EnableTask()
    {
        foreach (Transform childTransform in gameObject.transform)
        {
            var child = childTransform.gameObject;
            if (child == frageObjeckt || child == infobar)
                child.SetActive(true);
            else
                child.SetActive(false);
        }
        isShowingText = true;
        Debug.Log("Text Task");
    }

    private void EnableAnswer()
    {
        foreach (Transform childTransform in gameObject.transform )
        {
            var child = childTransform.gameObject;
            if (child == antwortObject || child == infobar)
                child.SetActive(true);
            else
                child.SetActive(false);
        }
        isShowingText = true;
        Debug.Log("Text Answer");
    }

    public bool AllowedToPress()
    {
        if(timeSinceLastPress > cooldownTime)
        {
            timeSinceLastPress = 0f;
            return true;
        }
        return false;
    }
}
