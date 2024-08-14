using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Infobar : MonoBehaviour
{

    public PlayerManager playerManager;
    public GameObject[] infoBarSpots;

    public GameObject activeInfobar;

    public GameObject playerObject;

    public string text;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = gameObject.transform.parent.gameObject;

        playerManager = FindObjectOfType<PlayerManager>();

        // playerIndex = GetPlayerIndex();
        SetTextOnInfobars(text);
        if (playerManager.isModerator)
            gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // ActivateClosestInfobar();
        MoveInfobar();
    }


    private GameObject FindClosestInfobarSpot()
    {
        GameObject closestInfobar = null;
        float shortestDistance = float.MaxValue;
        foreach(GameObject infobar in infoBarSpots)
        {
            float distance = Vector3.Distance(infobar.transform.position, Camera.main.transform.position);
            if(distance < shortestDistance)
            {
                shortestDistance = distance;
                closestInfobar = infobar;
            }
        }
        
        return closestInfobar;
    }

    private void ActivateClosestInfobar()
    {
        activeInfobar = FindClosestInfobarSpot();
        foreach (GameObject infobar in infoBarSpots)
        {
            if (infobar != activeInfobar)
                infobar.SetActive(false);
            else
                infobar.SetActive(true);
        }
    }

    private void SetTextOnInfobars(string text_)
    {
        foreach(GameObject infobar in infoBarSpots)
        {
            infobar.GetComponent<TextMeshPro>().text = text_;
        }
    }

    public void MoveInfobar()
    {
        Vector3 heartcamera = (Camera.main.transform.position - playerObject.transform.position);
        heartcamera = Helper1.MakeVectorFlat(heartcamera);
        float scaleValue = 0.3f / playerManager.gameObject.GetComponent<StageManager>().currentScale;
        activeInfobar.transform.position = heartcamera.normalized * 0.4f + Vector3.down * 0.2f;

    }
}
