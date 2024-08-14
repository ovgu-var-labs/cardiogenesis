using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DIYAnchor : MonoBehaviour
{
    /*
    public GameObject prefab;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Guid CreateAnchor(Transform transform_)
    {
        GameObject anchor = Instantiate(prefab, transform_);
        anchor.AddComponent<OVRSpatialAnchor>();
        StartCoroutine(anchorCreated(workingAnchor));
        return anchor.GetComponent<OVRSpatialAnchor>().Uuid;
    }

    public IEnumerator anchorCreated(anchor)
    {
        // keep checking for a valid and localized spatial anchor state
        while (!workingAnchor.Created && !workingAnchor.Localized)
        {
            yield return new WaitForEndOfFrame();
        }

        //when ready, save the spatial anchor using OVRSpatialAnchor.Save()
        _anchor.Save((anchor, success) =>
        {
            if (!success) return;

            // The save is successful. Now we can save the spatial anchor
            // to a global List for convenience.
            _allAnchors.Add(workingAnchor);
        });
    }
    */
}
