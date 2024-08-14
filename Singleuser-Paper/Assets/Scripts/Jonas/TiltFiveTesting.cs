using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltFiveTesting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PrintOnConsole("Hallo");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrintOnConsole(string output)
    {
        Debug.Log(output);
    }
}
