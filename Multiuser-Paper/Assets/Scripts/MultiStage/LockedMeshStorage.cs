using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedMeshStorage : MonoBehaviour
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

    public Mesh GetMeshAt(float progress)
    {
        int number = Mathf.RoundToInt(progress * meshesStage.Count);
        Mesh tmpMesh = meshesStage[number];
        Debug.Log("Send Mesh at position " + number + " " + tmpMesh.name);
        return tmpMesh;
    }

}
