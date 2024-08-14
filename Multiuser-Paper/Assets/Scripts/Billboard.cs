using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public bool lockX;
    public bool lockY;
    public bool lockz;

    public Vector3 offsetVector;
    public bool keepInFront;

    public bool inverse;

    public TiltFive.PlayerIndex playerIndex;



    // Start is called before the first frame update
    void Start()
    {
        playerIndex = GetPlayerIndex();
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowardsAll();
        if (keepInFront)
            KeepInFront(offsetVector);
    }

    public void RotateTowardsAll()
    {
        Transform prevTransform = gameObject.transform;
        TiltFive.Glasses.TryGetPose(playerIndex, out Pose pose);
        Vector3 vectorCameraObject = pose.position - gameObject.transform.position;
        gameObject.transform.forward = Vector3.RotateTowards(gameObject.transform.forward, vectorCameraObject, 360f, 0f);
        
        if (lockX)
        {
            gameObject.transform.eulerAngles = new Vector3(prevTransform.eulerAngles.x, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);
        }
        if (lockY)
        {
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, prevTransform.eulerAngles.y, gameObject.transform.eulerAngles.z);
        }
        if (lockz)
        {
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, prevTransform.eulerAngles.z);
        }
        if (inverse)
            gameObject.transform.forward = -gameObject.transform.forward;
    }

    public void RotateTowardsX()
    {

    }

    public void KeepInFront(Vector3 vector3)
    {
        TiltFive.Glasses.TryGetPose(playerIndex, out Pose pose);
        gameObject.transform.position = pose.position + pose.forward * vector3.x + pose.right * vector3.y;
    }

    private TiltFive.PlayerIndex GetPlayerIndex()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Player One"))
        {
            playerIndex = TiltFive.PlayerIndex.One;
        }
        else if(gameObject.layer == LayerMask.NameToLayer("Player Two"))
        {
            playerIndex = TiltFive.PlayerIndex.Two;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Player Three"))
        {
            playerIndex = TiltFive.PlayerIndex.Three;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Player Four"))
        {
            playerIndex = TiltFive.PlayerIndex.Four;
        }
        else
        {
            playerIndex = TiltFive.PlayerIndex.One;
        }
        return playerIndex;
    }
}
