using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MeshVisualizer : MonoBehaviour
{
    public List<Mesh> meshes = new List<Mesh>();            // List containing all meshes for this stage

    public Mesh currentMesh;
    public Mesh currentHighMesh;

    public int stageID;         
    public string MeshName;                                 // Name of the meshes in the resource folder. Excluding the number at the end
    public int numberMeshes;
    public int startCountWithNumber;
    // public GlobalAnimationManager globalAnimation;          // GlobalAnimationManager of this Stage
    public GlobalAnimationController globalAnimationController;

    public MeshFilter meshFilter;                           
    public MeshRenderer meshRenderer;
    public MeshCollider meshCollider;

    public MeshStorageScript meshStorage;

    public bool isPlaying;

    public int counter;                                     // which mesh should be projected

    public float progress;                                 // How far the animation has progressed
    private float lastProgress;                             // Progress value of last frame

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("Number Meshes " + CountAssetsInFolder(1));
        if (globalAnimationController == null)
        {
            globalAnimationController = FindAnyObjectByType<GlobalAnimationController>();
            if (globalAnimationController == null)
                Debug.LogError("No GlobalAnimationManager Reference");
        }

        if(meshStorage == null)
        {
            meshStorage = FindObjectOfType<MeshStorageScript>();
        }
        
        meshFilter = gameObject.GetComponent<MeshFilter>();
        if (meshFilter == null)
            Debug.LogError("No Mesh Filter on Object");
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
            Debug.LogError("No Mesh Renderer on Object");
        meshCollider = gameObject.GetComponent<MeshCollider>();
        if (meshCollider == null)
            Debug.LogError("No Mesh Collider on Object");

        counter = 0;
        /*
        if (MeshName == "")
            LoadMeshesIntoList(stageID);
        else
            LoadMeshesIntoList(stageID, MeshName);
        */
        ShowMeshAtValue(counter);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateProgress();
        UpdateMeshList();

        lastProgress = progress;
    }


    public void UpdateMeshList()
    {

        isPlaying = (lastProgress != progress);
        // int meshNumber = Mathf.RoundToInt(progress * fbxStorage.GetNumberMeshesInCurrentStage());
        if (isPlaying)  // mesh is currently animationg
        {
            // LoadMeshAt(meshNumber);
            ShowMeshAtValue(progress);
        }
        else
            LoadHighMeshAt(/*meshNumber*/ 100);
        //Debug.Log("TODO: Add load high quality mesh");
        meshCollider.sharedMesh = currentMesh;
    }

    public void LoadMeshAt(int numberMesh)
    {
        
        currentMesh = meshes[numberMesh];
        meshFilter.mesh = currentMesh;

    }

    private void ShowMeshAtValue(float progress_)
    {
        GetMeshFromMeshStorage(progress_);
        meshFilter.mesh = currentMesh;
    }

    public void GetMeshFromMeshStorage(float progress_)
    {
        currentMesh = meshStorage.GetMeshAt(progress_);
    }

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
        */
    }


    private void LoadMeshesIntoList(int stageNumber)
    {
        int c = meshes.Count;
        if (numberMeshes > 0)
            c = numberMeshes;
        
        for(int i = 0; i < c; i++)
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
            meshes[i] = Resources.Load<Mesh>("New/Stage_" + stageNumber + "_Low/"+ meshName_ + " " + (i + startCountWithNumber));
        }
    }

    private float GetProgressFromAnimationManager(GlobalAnimationController globalAnimation)
    {
        return globalAnimation.globalAnimationProgress;
    }

    private void UpdateProgress()
    {
        progress = GetProgressFromAnimationManager(globalAnimationController);
    }


    private int CountAssetsInFolder(int stageNumber)
    {
        var info = new DirectoryInfo("Resources/GeneratedMeshes/Stage_" + stageNumber + "_low");
        var fileInfo = info.GetFiles();
        return fileInfo.Length;
    }

    private Mesh GetCurrentMeshLow(int meshNumber)
    {
        return meshes[meshNumber];
    }
}
