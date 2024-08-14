using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrintTransformToTMP : MonoBehaviour
{
    public Transform transform_;
    public TextMeshPro textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textMeshPro.text = transform_.localPosition + "\n" + transform_.localEulerAngles;
    }
}
