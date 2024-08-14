using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyRotation : MonoBehaviour
{
    [SerializeField]
    private GameObject main;

    // Update is called once per frame
    void Update()
    {
        this.gameObject.SetActive(main.gameObject.activeInHierarchy);
        this.transform.localRotation = main.transform.localRotation;
    }
}
