using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;

public class AudioController : MonoBehaviour
{

    public float audioProgress = 0;
    public float sliderValue;
    public int skipTime = 0;
    public AudioSource audio;
    public GameObject playPauseButton;
    public Texture playTexture;
    public Texture pauseTexture;

    public GameObject extraObjectsCollection;
    public PinchSlider pinchslider;
    public TextMeshPro timeText;

    public bool showSlider;
    public bool isGrabbed = false;
    public bool isPlaying;

    public System.DateTime startTime;
    public System.DateTime endTime;



    // Start is called before the first frame update
    void Start()
    {
        pinchslider = gameObject.GetComponent<PinchSlider>();
        sliderValue = gameObject.GetComponent<PinchSlider>().SliderValue;
        showSlider = gameObject.activeInHierarchy;
        timeText = gameObject.GetComponentInChildren<TextMeshPro>();
        playPauseButton.GetComponent<ButtonConfigHelper>().SetQuadIcon(pauseTexture);
        extraObjectsCollection.SetActive(false);
        //extraObjectsCollection = gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            UpdateProgessBar();
            UpdateText();
        }

    }

    private void Awake()
    {
        Debug.Log("Test Awake funktion");
    }

    /// <summary>
    /// To be called when user changed pinch slider
    /// </summary>

    /// <summary>
    /// To be called when user preses forward or backward button
    /// </summary>
    public void MoveAudioProgess()
    {

    }

    /// <summary>
    /// Start timer which checks the time the button was pressed
    /// </summary>
    public void StartTimer()
    {
        startTime = System.DateTime.UtcNow;
        Debug.Log("Timer startet at" + startTime);
    }

    /// <summary>
    /// Checks if button was pressed long enough to trigger slidder
    /// </summary>
    public void EndTimer()
    {
        
        if (System.DateTime.UtcNow.AddSeconds(-2) >= startTime)
        {
            Debug.Log("Timer eneded at" + System.DateTime.UtcNow);
            showSlider = !showSlider;
            ToggleAdvancedControlls();
        }
        
    }

    public void ToggleAdvancedControlls()
    {
        extraObjectsCollection.SetActive(!extraObjectsCollection.activeInHierarchy);
    }

    private void UpdateProgessBar()
    {
        audioProgress = audio.time / audio.clip.length;
        pinchslider.SliderValue = audioProgress;
    }

    private void UpdateProgressBar(float audioProgress_)
    {
        audioProgress = audioProgress_;
        pinchslider.SliderValue = audioProgress;
    }

    public void SetAudioProgress()
    {
        audioProgress = pinchslider.SliderValue;
        audio.time = pinchslider.SliderValue * audio.clip.length;
        audio.Play();
    }

    /// <summary>
    /// Called when pinch slider is grabbed
    /// </summary>
    public void SliderGrabbed()
    {
        isPlaying = audio.isPlaying;
        audio.Pause();
        isGrabbed = true;
    }

    /// <summary>
    /// called when pinch slider is released
    /// </summary>
    public void SliderReleased()
    {
        isGrabbed = false;
        if(isPlaying)
            audio.Play();
    }

    /// <summary>
    /// Called over update so that text is always correct
    /// </summary>
    private void UpdateText()
    {
        string t = string.Format("{0:0}:{1:00}", Mathf.Floor(pinchslider.SliderValue * audio.clip.length / 60), Mathf.Floor(pinchslider.SliderValue * audio.clip.length % 60));
        timeText.SetText(t);
    }

    /// <summary>
    /// called when slider value is updated
    /// </summary>
    public void TestAudioUpdate()
    {
        if (isGrabbed)
        {
            audioProgress = pinchslider.SliderValue;
            audio.time = pinchslider.SliderValue * audio.clip.length;
        }

    }

    public void SkipBackwards()
    {
        audio.time = pinchslider.SliderValue * audio.clip.length - skipTime;
        //audio.Play();
    }

    public void SkipForward()
    {
        audio.time = pinchslider.SliderValue * audio.clip.length + skipTime;
        //audio.Play();
    }

    public void PlayPauseSwitch()
    {
        if (isPlaying)
        {
            float audioProgress_ = audio.time / audio.clip.length;
            UpdateProgressBar(audioProgress_);
            audio.Pause();
            // Play element
            playPauseButton.GetComponent<ButtonConfigHelper>().SetQuadIcon(playTexture);
        }
        else
        {
            audio.Play();
            // Pause element
            playPauseButton.GetComponent<ButtonConfigHelper>().SetQuadIcon(pauseTexture);
        }
        isPlaying = !isPlaying;
    }
    public void PlayPauseSwitch(bool isPlaying_)
    {
        if (isPlaying_)
        {
            float audioProgress_ = audio.time / audio.clip.length;
            UpdateProgressBar(audioProgress_);
            audio.Pause();
            // Play element
            playPauseButton.GetComponent<ButtonConfigHelper>().SetQuadIcon(playTexture);
        }
        else
        {
            audio.Play();
            // Pause element
            playPauseButton.GetComponent<ButtonConfigHelper>().SetQuadIcon(pauseTexture);
        }
        isPlaying = isPlaying_;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="partOfName_"></param>
    /// <returns>Nearest audiosource in Unityhierarchy</returns>
    private AudioSource FindNearestAudio(string partOfName_)
    {
        AudioSource audioSource = null;
        GameObject levelGameobject = gameObject;

        do
        {
            if (levelGameobject.GetComponentInChildren<AudioSource>() != null && levelGameobject.GetComponentInChildren<AudioSource>().name.Contains(partOfName_))
                audioSource = levelGameobject.GetComponentInChildren<AudioSource>();
            else
                levelGameobject = levelGameobject.transform.parent.gameObject;
        }
        while (audioSource == null);

        return audioSource;
    }
}
