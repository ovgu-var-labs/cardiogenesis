using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedMeshStorage : LockedStorage
{
    public int SceneNumber;
    public List<Mesh> meshesStage;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="progress">Progress of animation</param>
    /// <returns>Saved Mesh relative to progress</returns>
    public override Mesh GetMeshAt(float progress)
    {
        int number = Mathf.RoundToInt(progress * meshesStage.Count);
        Mesh tmpMesh = meshesStage[number];
        // Debug.Log("Send Mesh at position " + number + " " + tmpMesh.name);
        return tmpMesh;
    }

}
