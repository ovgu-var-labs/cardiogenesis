using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBall : MonoBehaviour
{
    public GameObject spawnPosition;
    public GameObject prefabKugel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckNumberChildren())
        {
            Instantiate(prefabKugel, spawnPosition.transform.position, spawnPosition.transform.rotation, this.transform);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>True if child count zero</returns>
    public bool CheckNumberChildren()
    {
        if (transform.childCount > 0)
            return false;
        else
            return true;
    }
}
