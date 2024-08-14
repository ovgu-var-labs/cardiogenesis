using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotateOver_
{
    x,
    y,
    z
};

public class NewBillboard : MonoBehaviour
{
    public TiltFive.PlayerIndex playerIndex;

    public RotateOver_ rotateOver;
    public Transform initialTransform;


    // Start is called before the first frame update
    void Start()
    {
        initialTransform = transform;
        playerIndex = GetPlayerIndex();
    }

    // Update is called once per frame
    void Update()
    {
        if (rotateOver == RotateOver_.x)
            UpdateX();
    }

    private void UpdateX()
    {
        TiltFive.Glasses.TryGetPose(playerIndex, out Pose pose);
        transform.localEulerAngles = new Vector3(calculateAngle(transform.position, pose.position), transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="p1">Position of point</param>
    /// <param name="p2">Position of camera</param>
    /// <returns></returns>
    private float calculateAngle(Vector3 p1, Vector3 p2)
    {
        float m = (p2.y - p1.y) / (Vector3.Distance(new Vector3(p2.x, p1.y, p2.z), p1));
        float angle = (Mathf.Atan(m));
        angle = Mathf.Rad2Deg * angle;
        Debug.Log("m " + m + " " + angle);
        return angle;
    }


    private TiltFive.PlayerIndex GetPlayerIndex()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Player One"))
        {
            playerIndex = TiltFive.PlayerIndex.One;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Player Two"))
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
