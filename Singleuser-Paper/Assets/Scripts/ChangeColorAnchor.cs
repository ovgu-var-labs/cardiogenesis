using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorAnchor : MonoBehaviour
{
    public Material redMaterial;

    // Start is called before the first frame update
    void Start()
    {
        if (TryGetComponent<OVRSpatialAnchor>(out OVRSpatialAnchor oVRSpatialAnchor))
        {
            gameObject.GetComponent<MeshRenderer>().material = redMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (TryGetComponent<OVRSpatialAnchor>(out OVRSpatialAnchor oVRSpatialAnchor))
        {
            gameObject.GetComponent<MeshRenderer>().material = redMaterial;
        }
    }
}
