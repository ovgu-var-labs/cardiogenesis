using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshStorageScript : MonoBehaviour
{
    public List<Mesh> previousMeshes;
    public List<Mesh> currentMeshes;
    public List<Mesh> nextMeshes;

    public List<int> numberMeshesperStage;
    public List<int> startMeshNumberWith;

    public int stageID;
    public float progressValue;
    private int numberLevels = 10;

    public MudRendererCommunication mudRendererCommunication;

    private bool setupDone = false;


    // Start is called before the first frame update
    void Start()
    {
        stageID = mudRendererCommunication.currentStage;
        // numberLevels = mudRendererCommunication.moderatorStageManager.stages.Length;
        /*
        int prevStageID = mod((stageID - 1), numberLevels);
        Debug.Log("Current Stage: " + stageID + " " + prevStageID);
        LoadMeshsIntoList(previousMeshes, prevStageID, numberMeshesperStage[prevStageID], startMeshNumberWith[prevStageID]);
        int nextStageID = mod((mudRendererCommunication.currentStage + 1), numberLevels);
        LoadMeshsIntoList(nextMeshes, nextStageID, numberMeshesperStage[nextStageID], startMeshNumberWith[nextStageID]);
        */
        // LoadMeshsIntoList(currentMeshes, stageID, numberMeshesperStage[stageID], startMeshNumberWith[stageID]);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (stageID != mudRendererCommunication.currentStage)
        {
            if(stageID > mudRendererCommunication.currentStage)             // went one stage back
            {
                nextMeshes = currentMeshes;
                currentMeshes = previousMeshes;
                int prevStageID = mod((mudRendererCommunication.currentStage - 1), numberLevels);
                LoadMeshsIntoList(previousMeshes, prevStageID, numberMeshesperStage[prevStageID], startMeshNumberWith[prevStageID]);    
            }
            else if(stageID < mudRendererCommunication.currentStage)        // went one stage forward
            {
                previousMeshes = currentMeshes;
                currentMeshes = nextMeshes;
                int nextStageID = mod((mudRendererCommunication.currentStage + 1), numberLevels);
                LoadMeshsIntoList(nextMeshes, nextStageID, numberMeshesperStage[nextStageID], startMeshNumberWith[nextStageID]);
            }
        }
        */
    }

    /// <summary>
    /// Loads meshes into list
    /// </summary>
    /// <param name="list">List to load into</param>
    /// <param name="StageID">which stage should be loaded</param>
    /// <param name="numberMeshesInList">How many meshes are there for this animation</param>
    /// <param name="startMeshNumberWith_"></param>
    private void LoadMeshsIntoList(List<Mesh> list, int StageID, int numberMeshesInList, int startMeshNumberWith_)
    {
        Debug.Log("Start Loading Meshes " + gameObject.name);
        int c = list.Count;
        if (numberMeshesInList > 0)
            c = numberMeshesInList;
        list.Clear();
        list.TrimExcess();
        for (int i = 0; i < c; i++)
        {
            Debug.Log("Loaded MeshStorage " + (i + startMeshNumberWith_) + " StageID " + (StageID + 1));
            //meshes[i] = Resources.Load<Mesh>("New/Stage_" + stageNumber + "_Low/Stage_" + stageNumber + " " + (i + startCountWithNumber));
            list.Add(Resources.Load<Mesh>("New/Stage_" + (StageID + 1) + "_Low/Stage_" + (StageID + 1) + " " + (i + startMeshNumberWith_)));
        }
    }

    public Mesh GetMeshAt(float progress)
    {
        int number = Mathf.RoundToInt(progress * currentMeshes.Count);
        Debug.Log("Send Mesh at position " + number);
        Mesh tmpMesh = currentMeshes[number];
        Debug.Log("Send Mesh at position " + number + " " + tmpMesh.name);
        return tmpMesh;
    }

    private int mod(int x, int m)
    {
        return (x % m + m) % m;
    }

    public int GetNumberMeshesInStage(int stage)
    {
        return currentMeshes.Count;
    }

    public int GetCurrentStageID()
    {
        return stageID;
    }

    public int GetNumberMeshesInCurrentStage()
    {
        return numberMeshesperStage[stageID];
    }
}
