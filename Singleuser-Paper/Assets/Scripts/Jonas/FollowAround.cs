using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAround : MonoBehaviour
{

    public GameObject masterObject;
    public GameObject followObject;
    
    // Relative in relation to master
    public bool relTranslation;
    public bool relXRotation;
    public bool relYRotation; 
    public bool relZRotation;

    // Fixed to initial position of slave
    public bool fixPosition;
    public bool fixRotation;

    private Vector3 initialTranslationOffset;
    private Vector3 initialRotationOffset;

    private Vector3 initialPosition;
    private Vector3 initialRotation;    // in euler angles

    // Start is called before the first frame update
    void Start()
    {
        masterObject = gameObject;
        initialTranslationOffset = followObject.transform.position - masterObject.transform.position;
        initialRotationOffset = new Vector3(followObject.transform.eulerAngles.x - masterObject.transform.eulerAngles.x, followObject.transform.eulerAngles.y - masterObject.transform.eulerAngles.y, followObject.transform.eulerAngles.z - masterObject.transform.eulerAngles.z);
        initialPosition = gameObject.transform.position;
        initialRotation = gameObject.transform.rotation.eulerAngles;
        //if(gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.name.Contains("ObjMan"))
        followObject = gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (relTranslation)
            changeTranslation();
        if (relXRotation)
            changeXRotation();
        if (relYRotation)
            changeYRotation();
        if (relZRotation)
            changeZRotation();
        if (fixPosition)
            fixXYZPosition();
    }

    private void changeTranslation()
    {
        followObject.transform.position = masterObject.transform.position + initialTranslationOffset;
    }

    private void changeXRotation()
    {
        followObject.transform.eulerAngles = new Vector3(masterObject.transform.eulerAngles.x + initialRotationOffset[0], followObject.transform.eulerAngles.y, followObject.transform.eulerAngles.z);
    }

    private void changeYRotation()
    {
        followObject.transform.eulerAngles = new Vector3(followObject.transform.eulerAngles.x, masterObject.transform.eulerAngles.y + initialRotationOffset[1], followObject.transform.eulerAngles.z);
    }

    private void changeZRotation()
    {
        followObject.transform.eulerAngles = new Vector3(followObject.transform.eulerAngles.x, followObject.transform.eulerAngles.y, masterObject.transform.eulerAngles.z + initialRotationOffset[2]);
    }

    public void ScaleMasterUp()
    {
        masterObject.transform.localScale = masterObject.transform.localScale * 2;
    }

    public void ScaleMasterDown()
    {
        masterObject.transform.localScale = masterObject.transform.localScale / 2;
    }

    public void StopMovement()
    {
        gameObject.transform.position = masterObject.transform.position;
    }

    public void fixXYZPosition()
    {
        gameObject.transform.position = initialPosition;
    }

}
