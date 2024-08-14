using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private GameObject cameraobject;
    private float movementSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 tmpVector = cameraobject.transform.position;
            tmpVector[2] = cameraobject.transform.position[2] + movementSpeed * Time.deltaTime;
            cameraobject.transform.position = tmpVector;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 tmpVector = cameraobject.transform.position;
            tmpVector[0] = cameraobject.transform.position[0] - movementSpeed * Time.deltaTime;
            cameraobject.transform.position = tmpVector;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 tmpVector = cameraobject.transform.position;
            tmpVector[0] = cameraobject.transform.position[0] + movementSpeed * Time.deltaTime;
            cameraobject.transform.position = tmpVector;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 tmpVector = cameraobject.transform.position;
            tmpVector[2] = cameraobject.transform.position[2] - movementSpeed * Time.deltaTime;
            cameraobject.transform.position = tmpVector;
        }
        if (Input.GetKey(KeyCode.PageUp))
        {
            Vector3 tmpVector = cameraobject.transform.position;
            tmpVector[1] = cameraobject.transform.position[1] + movementSpeed * Time.deltaTime;
            cameraobject.transform.position = tmpVector;
        }
        if (Input.GetKey(KeyCode.PageDown))
        {
            Vector3 tmpVector = cameraobject.transform.position;
            tmpVector[1] = cameraobject.transform.position[1] - movementSpeed * Time.deltaTime;
            cameraobject.transform.position = tmpVector;
        }
    }
}
