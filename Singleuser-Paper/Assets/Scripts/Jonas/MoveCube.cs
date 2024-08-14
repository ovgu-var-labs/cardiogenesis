using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{

    public Vector3 cubePosition;

    // Start is called before the first frame update
    void Start()
    {
        cubePosition = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // this.gameObject.transform.position = cubePosition;
        Debug.Log(this.gameObject.transform.position);
    }
}
