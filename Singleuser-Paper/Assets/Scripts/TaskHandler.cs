using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TaskHandler : MonoBehaviour
{
    // StageManager stageManager;

    public List<GameObject> tasks;

    public Interactable playButton;
    public Interactable pauseButton;
    public Interactable selectStageButton;

    private GameObject currentTask;

    // Start is called before the first frame update
    void Start()
    {
        // stageManager = StageManager.Instance;
        // Debug.Log(stageManager.currentStage.id + "has been opened");
        // StartCoroutine(setTask(stageManager.currentStage.id));

        var playPressReceiver = playButton.GetReceiver<InteractableOnPressReceiver>();
        playPressReceiver.OnPress.AddListener(() => play());

        var pausePressReceiver = pauseButton.GetReceiver<InteractableOnPressReceiver>();
        pausePressReceiver.OnPress.AddListener(() => pause());

        var selectStageReceiver = selectStageButton.GetReceiver<InteractableOnPressReceiver>();
        selectStageReceiver.OnPress.AddListener(() => selectStage());

        playButton.gameObject.SetActive(false);
    }

    private void play()
    {
        Debug.Log("Resumed");
        Time.timeScale=1;

        togglePausePlayButton();
    }

    private void pause()
    {
        Debug.Log("Paused");
        Time.timeScale = 0;

        togglePausePlayButton();
    }

    void togglePausePlayButton()
    {
        playButton.gameObject.SetActive(!playButton.gameObject.activeSelf);
        pauseButton.gameObject.SetActive(!pauseButton.gameObject.activeSelf);
    }

    private void selectStage()
    {
        Debug.Log("Back to Select Stage Scene");

        Time.timeScale = 1;
        //SceneManager.LoadScene("SelectStageScene"); // Old
        //Util.Instance.TransitionToScene("SelectStageScene");
    }

    /*
    // Update is called once per frame
    public IEnumerator setTask(int stageId)
    {
        currentTask = null;

        foreach (GameObject task in tasks)
        {
            GlobalAnimationController gac = task.GetComponentInChildren<GlobalAnimationController>();
            int id = gac.stageId;

            if (id == stageId)
            {
                task.SetActive(true);
                currentTask = task;
                Debug.Log("starting task " + stageId);
                yield return StartCoroutine(stageManager.notifyStageStarted(stageId));
            }
            else
            {
                task.SetActive(false);
            }
        }

        if (currentTask == null)
        {
            //go to select Stage
            selectStage();
        }else 
            Debug.Log("Current task is " + stageId);

    }
        */
}
