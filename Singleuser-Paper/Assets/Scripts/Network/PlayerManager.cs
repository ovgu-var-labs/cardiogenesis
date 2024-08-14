// using MixedReality.Toolkit.SpatialManipulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Works with different Manager scripts. Gets Information from player networking
/// </summary>
public class PlayerManager : MonoBehaviour
{
    public bool isModerator;
    // public TiltFive.PlayerIndex playerIndex;
    public GameObject ownMudRender;


    public bool markingIsLocked;           // Students can mark on the heart
    public bool rotatingIsLocked;          // students can rotate the heart model by themselves

    // Start is called before the first frame update
    void Start()
    {
        /*
        if (isModerator)
        {
            gameObject.GetComponent<ObjectManipulator>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<ObjectManipulator>().enabled = false;
        }
        */
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool GetIsModerator()
    {
        return isModerator;
    }

    /*
     * Check delete
    public TiltFive.PlayerIndex GetPlayerIndex()
    {
        return playerIndex;
    }
    */

    public bool GetMarkingIsLocked()
    {
        return markingIsLocked;
    }

    public void SetMarkingIsLocked(bool markingIsLocked_)
    {
        markingIsLocked = markingIsLocked_;
    }

    public bool GetRotatingIsLocked()
    {
        return rotatingIsLocked;
    }

    public void SetRotationIsLocked(bool rotationIsLocked_)
    {
        rotatingIsLocked = rotationIsLocked_;
    }

    public void UpdateRotationOfRenderer()
    {

    }
}
