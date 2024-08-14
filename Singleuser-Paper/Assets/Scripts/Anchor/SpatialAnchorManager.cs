using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpatialAnchorManager : MonoBehaviour
{
    public OVRSpatialAnchor anchorPrefab;

    public const string NumUuidsPlayerPref = "numUuids";

    private List<OVRSpatialAnchor> anchors = new List<OVRSpatialAnchor>();
    public OVRSpatialAnchor lastCreatedAnchor;
    public AnchorLoader anchorLoader;

    // Start is called before the first frame update
    void Start()
    {
        anchorLoader = GetComponent<AnchorLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateSpatialAnchor(Transform transform_)
    {
        OVRSpatialAnchor workingAnchor = Instantiate(anchorPrefab, transform_);

        StartCoroutine(AnchorCreated(workingAnchor));
    }

    private IEnumerator AnchorCreated(OVRSpatialAnchor workingAnchor)
    {
        while(!workingAnchor.Created && !workingAnchor.Localized)
        {
            yield return new WaitForEndOfFrame();
        }

        Guid anchorGuid = workingAnchor.Uuid;
        anchors.Add(workingAnchor);
        lastCreatedAnchor = workingAnchor;
    }

    public void SaveLastCreateAnchor()
    {
        lastCreatedAnchor.Save((lastCreatedAnchor, success) =>
        {
            if (success)
            {
                Debug.Log("saved");
            }
        }
        );
        SaveUuidToPlayerPrefs(lastCreatedAnchor.Uuid);
    }

    public void SaveUuidToPlayerPrefs(Guid uuid)
    {
        if (!PlayerPrefs.HasKey(NumUuidsPlayerPref))
        {
            PlayerPrefs.SetInt(NumUuidsPlayerPref, 0);
        }

        int playerNumUuids = PlayerPrefs.GetInt(NumUuidsPlayerPref);
        PlayerPrefs.SetString("uuid" + playerNumUuids, uuid.ToString());
        PlayerPrefs.SetInt(NumUuidsPlayerPref, ++playerNumUuids);
    }

    public void LoadSavedAnchors()
    {
        anchorLoader.LoadAnchorsByUuid();
    }
}
