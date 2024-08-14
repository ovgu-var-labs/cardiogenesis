using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectTask : MonoBehaviour
{
    public bool useMultiSceneStuff;

    public GameObject[] arrayTasks;
    public GameObject[] arrayArrows;
    public GameObject[] arraySubtaks;

    public SaveHighestTask save;

    // können später private gestellt werden
    public int activeTaskID = 0;
    public int highestTaskID = 0;
    public int scenesBeforeTimeline;

    // Start is called before the first frame update
    void Start()
    {
        if (!useMultiSceneStuff)
        {
            activeTaskID = SceneManager.GetActiveScene().buildIndex - scenesBeforeTimeline;
            highestTaskID = StaticData.highestTaskID;
            if (highestTaskID < activeTaskID)
                StaticData.highestTaskID = activeTaskID;
            UpdateTaskArrow();
            HighlightSubtask();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Changes to task which was selected on the Timeline.
    /// </summary>
    /// <param name="taskID"> Selected task on timeline</param>
    public void ChangeToTask(int taskID)
    {
        //Find active Task ID
        activeTaskID = getActiveTaskID();
        // Update highestTaskID
        UpdateHighestTaskID();
        if (taskID > highestTaskID)
            return;
        arrayTasks[activeTaskID].SetActive(false);
        activeTaskID = taskID;
        arrayTasks[taskID].SetActive(true);
        UpdateTaskArrow();
        HighlightSubtask();
    }

    /// <summary>
    /// Changes to scene with given task
    /// </summary>
    /// <param name="SceneID">ID of scene in Builder subtractet ith the scenes in front of it</param>
    public void ChangeToScene(int SceneID)
    {
        if (SceneID > highestTaskID)
            return;
        SceneManager.LoadScene(SceneID - scenesBeforeTimeline);
    }

    /// <summary>
    /// Is activated by pushing the "Weiter" button at end of each task.
    /// </summary>
    /// <param name="activeTaskID_"></param>
    public void SetActiveTaskID(int activeTaskID_)
    {
        activeTaskID = activeTaskID_;
        UpdateHighestTaskID();
        UpdateTaskArrow();
        HighlightSubtask();
    }


    public bool IsTaskActive(int taskID_)
    {
        return arrayTasks[taskID_].activeInHierarchy;
    }

    /// <summary>
    /// Iterates over Array to find task which is active in the moment
    /// </summary>
    /// <returns>Number which represents the task</returns>
    public int getActiveTaskID()
    {
        bool foundActiveTask = false;
        int count = -1;
        do
        {
            count++;
            foundActiveTask = arrayTasks[count].activeInHierarchy;
        }
        while (!foundActiveTask);
        return count;
    }

    private void UpdateHighestTaskID()
    {
        if (highestTaskID < activeTaskID)
            highestTaskID = activeTaskID;
    }
    
    /// <summary>
    /// Updates position of the arrow which shows which task is active.
    /// </summary>
    public void UpdateTaskArrow()
    {
        activeTaskID = getActiveTaskID();
        int c = 0;
        foreach(GameObject arrow in arrayArrows)
        {
            if (c == activeTaskID)
            {
                arrow.SetActive(true); 
            }
            else
                arrow.SetActive(false);
            c++;
        }
        
    }

    /// <summary>
    /// Highlights the subtask
    /// </summary>
    private void HighlightSubtask()
    {
        int c = 0;
        foreach (GameObject subtask in arraySubtaks)
        {
            if (c == activeTaskID - 5)
            {
                subtask.SetActive(true);
            }
            else
                subtask.SetActive(false);
            c++;
        }
    }

    public void SetToPreviousTask()
    {
        activeTaskID = activeTaskID - 1;
        UpdateHighestTaskID();
        UpdateTaskArrow();
        HighlightSubtask();
    }
}
