using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnSphere : MonoBehaviour
{

    public GameObject activeObject;
    public GameObject toDestroy;
    public GameObject prefabObject;
    public Transform sphereTransform;

    // Start is called before the first frame update
    void Start()
    {
        sphereTransform = activeObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(activeObject.transform.position.y < -1)
        {
            toDestroy = activeObject;
            activeObject = Instantiate(prefabObject);
            Destroy(toDestroy);
            
        }
    }

    private void RespawnPrefab()
    {
        Destroy(activeObject, 3);
        activeObject = Instantiate(prefabObject);
    }
}
