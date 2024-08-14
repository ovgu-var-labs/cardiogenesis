using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FollowHand : MonoBehaviour
{
    
    
    private bool isGrabbing = false, isSolved = false, isAnimationOnly = false;

    [SerializeField]
    private bool isLeft;

    [SerializeField]
    private GameObject taskHint;

    public GameObject hand;

    // private MudBun.MudMaterial mat;
    private Color defaultColor;


    public void SetStateToGrabbing(bool value)
    {
        isGrabbing = value;
    }

    public void SetStateToSolved(bool value)
    {
        isSolved = value;
    }

    public void SetStateToAnimationOnly()
    {
        taskHint.SetActive(false);
        taskHint.transform.localScale = Vector3.zero;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if(isLeft)
        {
            OVRControllerHelper[] items = FindObjectsOfType<OVRControllerHelper>();
            foreach(OVRControllerHelper hand_ in items)
            {
                if (hand_.m_controller == OVRInput.Controller.LTouch)
                    hand = hand_.gameObject;
            }
        }
        if (!isLeft)
        {
            OVRControllerHelper[] items = FindObjectsOfType<OVRControllerHelper>();
            foreach (OVRControllerHelper hand_ in items)
            {
                if (hand_.m_controller == OVRInput.Controller.RTouch)
                    hand = hand_.gameObject;
            }
        }
        /*
        if (isLeft)
        {
            hand = GameObject.Find("/LeapAttachmentHands/Attachment Hand (Left)/IndexKnuckle/IndexMiddleJoint/IndexDistalJoint/IndexTip");
        }
        else
        {
            hand = GameObject.Find("/LeapAttachmentHands/Attachment Hand (Right)/IndexKnuckle/IndexMiddleJoint/IndexDistalJoint/IndexTip");
        }
        */

        // mat = gameObject.GetComponent<MudMaterial>();
        // defaultColor = mat.Emission;

        if (isAnimationOnly)
        {
            taskHint.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (hand.activeSelf)
        {
            //Debug.Log(hand.gameObject.transform.position);
            transform.position = hand.gameObject.transform.position;
        }
        else
        {
            transform.position = Vector3.zero;
        }
        */
        if (!isSolved)
        {
            taskHint.SetActive(true);
            if (isGrabbing)
            {
                // mat.Emission = new Color(0.9f,0.9f,0.9f,1);
                //taskHints.SetActive(false);
            }
            else
            {
                // mat.Emission = defaultColor;
                //taskHints.SetActive(true);
            }
        }
        else
        {
            // mat.Emission = defaultColor;
            taskHint.SetActive(false);
            //taskHint.transform.localScale = Vector3.zero;

            //isGrabbing = false;
            isSolved = true;
        }
    }
}
