using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FBXVisualizer_AllInOne : MonoBehaviour
{
    // public List<Mesh> meshes = new List<Mesh>();            // List containing all meshes for this stage Check delete

    public Mesh currentMesh;
    public GameObject currentFBX;
    public GameObject newFBX;
    // public Mesh currentHighMesh;  Check delete

    public GlobalAnimationManager globalAnimationManager;
    public GlobalAnimationController globalAnimationController;
    private bool useGlobalAnimationManager;

    public LockedFBXStorage fbxStorage;

    public int counter;                                     // which mesh should be projected

    public float progress;                                 // How far the animation has progressed
    private float lastProgress;                             // Progress value of last frame
    private int number;
    private TextMeshPro tmPro;

    // Start is called before the first frame update
    void Start()
    {
        tmPro = gameObject.transform.parent.GetComponentInChildren<TextMeshPro>();

        if (globalAnimationManager == null)
        {
            globalAnimationManager = FindAnyObjectByType<GlobalAnimationManager>(FindObjectsInactive.Exclude);
            useGlobalAnimationManager = true;
            if (globalAnimationManager == null)
            {
                globalAnimationController = FindAnyObjectByType<GlobalAnimationController>(FindObjectsInactive.Exclude);
                useGlobalAnimationManager = false;
            }
            if(globalAnimationController == null && globalAnimationManager == null)
            {
                Debug.LogError("No GlobalAnimationManager or controller Reference");
            }
            
        }

        if (fbxStorage == null)
        {
            fbxStorage = FindObjectOfType<LockedFBXStorage>(false);
        }

        counter = 0;
        currentFBX = transform.GetChild(0).gameObject;
        ShowFBXAtValue(counter);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateProgress();
        UpdateFBXList();

        lastProgress = progress;

        tmPro.text = "LocalProgress: " + progress + "\nManagerProgress: " + globalAnimationController.globalAnimationProgress + "\nNumber: " + number;
    }


    public void UpdateFBXList()
    {
        ShowFBXAtValue(progress);
    }

    private void ShowFBXAtValue(float progress_)
    {
        if(transform.childCount > 0)
        {
            number = Mathf.RoundToInt(transform.childCount * progress_);
            if (progress_ == 1)
                number = transform.childCount - 1;
            // Debug.Log(number);
            if(currentFBX!=null)
                currentFBX.SetActive(false);
            currentFBX = transform.GetChild(number).gameObject;
            currentFBX.SetActive(true);
            tmPro.text = tmPro.text + "\nNumber: " + number;
            return;
        }
        // GetFBXFromFBXStorage(progress_); // alter weg, bei welchem locked fbx storage objekte erstellt wurden
        Object.Destroy(currentFBX);
        currentFBX = Instantiate(newFBX);
    }

    public void GetFBXFromFBXStorage(float progress_)
    {
        newFBX = fbxStorage.GetFBXAt(progress_);
    }

    /*
    * Check delete
    /// <summary>
    /// Loads a mesh with higher resolution from ressources
    /// </summary>
    /// <param name="numberMesh">Number of mesh</param>
    private void LoadHighMeshAt(int numberMesh)
    {
        //Debug.Log("Load High Mesh " + numberMesh);
        // meshFilter.mesh = Resources.Load<Mesh>("New/Stage_" + stageID + "_High/Stage_" + stageID + " " + numberMesh);
        currentHighMesh = Resources.Load<Mesh>("New/Stage_" + stageID + "_High/Stage_" + stageID + " " + numberMesh);
        int c = 1;
        /*
        while(meshFilter.mesh == null)
        {
            meshFilter.mesh = Resources.Load<Mesh>("New/Stage_" + stageID + "_High/Stage_" + stageID + " " + (numberMesh - c));
            c += 1;
        }

    }



private void LoadMeshesIntoList(int stageNumber)
    {
        int c = meshes.Count;
        if (numberMeshes > 0)
            c = numberMeshes;

        for (int i = 0; i < c; i++)
        {
            //meshes[i] = Resources.Load<Mesh>("New/Stage_" + stageNumber + "_Low/Stage_" + stageNumber + " " + (i + startCountWithNumber));
            meshes.Add(Resources.Load<Mesh>("New/Stage_" + stageNumber + "_Low/Stage_" + stageNumber + " " + (i + startCountWithNumber)));
        }
    }

    private void LoadMeshesIntoList(int stageNumber, string meshName_)
    {
        int c = meshes.Count;
        for (int i = 0; i < c; i++)
        {
            meshes[i] = Resources.Load<Mesh>("New/Stage_" + stageNumber + "_Low/" + meshName_ + " " + (i + startCountWithNumber));
        }
    }
    */

    private float GetProgressFromAnimationManager(GlobalAnimationManager globalAnimation)
    {
        return globalAnimation.animationProgress;
    }

    /// <summary>
    /// Updates progress value saved in script
    /// </summary>
    private void UpdateProgress()
    {
        if (useGlobalAnimationManager)
            progress = GetProgressFromAnimationManager(globalAnimationManager);
        else
            progress = globalAnimationController.globalAnimationProgress;
    }

    /*
     * Check delete
    private Mesh GetCurrentMeshLow(int meshNumber)
    {
        return meshes[meshNumber];
    }
    */
}
