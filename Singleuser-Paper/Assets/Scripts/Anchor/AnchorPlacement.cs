using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPlacement : MonoBehaviour
{
    public GameObject anchorPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateSpatialAnchor(Vector3 position, Quaternion rotation)
    {
        GameObject prefab = Instantiate(anchorPrefab, position, rotation);
        // prefab.AddComponent<OVRSpatialAnchor>();

    }

    public void CreateSpatialAnchorAtTipOfController()
    {
        CreateSpatialAnchor(OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch), OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch));
    }
}
