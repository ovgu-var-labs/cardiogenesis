using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectActivity : MonoBehaviour
{

    private GameObject leftHand, rightHand;

    public float totalTime = 0.0f;
    public float activeTime;

    private bool colliding;
    private Collider collidingObject;

    // Start is called before the first frame update
    void Start()
    {
        leftHand = GameObject.Find("/LeapAttachmentHands/Attachment Hand (Left)/IndexKnuckle/IndexMiddleJoint/IndexDistalJoint/IndexTip");

        rightHand = GameObject.Find("/LeapAttachmentHands/Attachment Hand (Right)/IndexKnuckle/IndexMiddleJoint/IndexDistalJoint/IndexTip");
        
    }

    // Update is called once per frame
    void Update()
    {
        totalTime += Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (colliding && other!=collidingObject)
            return;

        colliding = true;
        collidingObject = other;

        activeTime += Time.deltaTime;
        //Debug.Log(other.name + " " + activeTime);
    }

    private void OnTriggerExit(Collider other)
    {
       if(colliding && other == collidingObject)
        {
            colliding = false;
            collidingObject = null;
            Debug.Log("exit");
        }
       
    }
}
