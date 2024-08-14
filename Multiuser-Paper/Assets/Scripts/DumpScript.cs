using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumpScript : MonoBehaviour
{

    public GameObject toMoveObject;

    // Start is called before the first frame update
    void Start()
    {
        if(toMoveObject == null)
        {
            toMoveObject = gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        toMoveObject.transform.position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
    }
}
