using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PukNotationScript : MonoBehaviour
{

    // public TiltFive.PlayerIndex playerIndex;
    public string text;
    public GameObject backPlate;
    public GameObject textField;
    public StageManager stageManager;
    public float overlapAngle;
    public float overlapOffset;

    public Color pukColor;

    private Vector3 normalOnMesh;
    private Vector3 vectorCameraObject;

    public bool isOnObject { get; set; }

    public bool isNetwork;
    public bool netIsOnObject;

    // Start is called before the first frame update
    void Start()
    {
       // playerIndex = GetPlayerIndex();    
    }

    // Update is called once per frame
    void Update()
    {
        if (!stageManager)
        {
            stageManager = FindAnyObjectByType<StageManager>();
        }
        vectorCameraObject = CreateVectorCameraObject();
        RotateTowardPlayer();
        UpdatePosition();
        // CheckTextPosition();
        if (isOnObject && !isNetwork)
            UpdateScale();
        else if (isNetwork && netIsOnObject)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
            UpdateScale();
            Debug.Log("updating Scale");
        }
        else
            this.transform.localScale = Vector3.one;
    }

    // Check delete
    private Vector3 CreateVectorCameraObject()
    {
        //TiltFive.Glasses.TryGetPose(playerIndex, out Pose pose);
        Vector3 vectorCameraObject_ =  gameObject.transform.position;
        return vectorCameraObject_;
    }

    private void RotateTowardPlayer()
    {
        //backPlate.transform.forward = Vector3.RotateTowards(backPlate.transform.forward, vectorCameraObject, 360f, 0f); check delete
    }

    private void UpdatePosition()
    {
        // backPlate.transform.position = gameObject.transform.position + (gameObject.transform.up * 0.025f);
    }

    public void SetText(string text_)
    {
        text = text_;
        textField.GetComponent<TextMeshPro>().text = text;
    }

    public void SetNormalOnMesh(Vector3 vector3)
    {
        normalOnMesh = vector3;
    }

    public void SetPukColor(Color color_)
    {
        Debug.Log("Change Color to " + color_);
        GetComponentInChildren<MeshRenderer>().material.color = color_;
    }

    /*
     * Check delete
    private void CheckTextPosition()
    {
        Vector3 PukTextVector = textField.transform.position - gameObject.transform.position;
        if (Vector3.Angle(PukTextVector, vectorCameraObject) < overlapAngle)
        {
            // Move Object to side
            backPlate.transform.position = backPlate.transform.position + (backPlate.transform.right * overlapOffset);
        }
    }
    */

    private void UpdateScale()
    {
        float newScale = (stageManager.stagesScale[stageManager.currentStage] / stageManager.currentScale) * 1;
        gameObject.transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    public void SetStageManager(StageManager stagemanager_)
    {
        stageManager = stagemanager_;
    }

    /*
    private TiltFive.PlayerIndex GetPlayerIndex()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Player One"))
        {
            return TiltFive.PlayerIndex.One;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Player Two"))
        {
            return TiltFive.PlayerIndex.Two;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Player Three"))
        {
            return TiltFive.PlayerIndex.Three;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Player Four"))
        {
            return TiltFive.PlayerIndex.Four;
        }
        else
        {
            return TiltFive.PlayerIndex.One;
        }
    }
    */
}
