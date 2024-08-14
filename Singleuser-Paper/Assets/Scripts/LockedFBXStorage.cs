using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum storageArt
{
    FBX,
    Mesh,
    Addressables,
    AssetBundles
}

public class LockedFBXStorage : LockedStorage
{
    [Tooltip("Art der gespeicherten und genutzten Objekte in dem Storage.\nMuss mit der Art in dem MeshRender übereinstimmen")]
    public storageArt storageArt; 
    public int SceneNumber;
    public List<GameObject> fbxStorage;
    public List<Mesh> meshesStorage;

    void Start()
    {
        foreach (GameObject fbx in fbxStorage)
        {
            meshesStorage.Add((fbx.GetComponent<MeshFilter>().sharedMesh));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override Mesh GetMeshAt(float progress)
    {
        int number = Mathf.RoundToInt(progress * meshesStorage.Count);
        Mesh tmpMesh = meshesStorage[number];
        Debug.Log("Send Mesh at position " + number + " " + tmpMesh.name);
        return tmpMesh;
    }

    public GameObject GetFBXAt(float progress)
    {
        int number = Mathf.RoundToInt(progress * fbxStorage.Count);
        return fbxStorage[number];
    }
}
