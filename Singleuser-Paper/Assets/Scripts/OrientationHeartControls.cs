using Microsoft.MixedReality.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationHeartControls : MonoBehaviour
{
    public GameObject heart;
    public GameObject controls;
    public GlobalAnimationController animationController;

    public GameObject MRSceneContent;

    // Start is called before the first frame update
    void Start()
    {
        animationController = FindObjectOfType<GlobalAnimationController>();
        heart.transform.rotation = controls.transform.rotation;

        MRSceneContent = FindObjectOfType<MixedRealitySceneContent>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(!animationController.GetIsSolved())
            heart.transform.eulerAngles = new Vector3(0, controls.transform.rotation.y, 0);
    }


}
