using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialOnPlay : MonoBehaviour
{

    public Material materialNotPlaying;
    public Material materialPlaying;

    public SeptierungFadeShaderScript shaderScript;

    private bool isPlaying;
    private bool lastIsPlaying;

    // Start is called before the first frame update
    void Start()
    {
        shaderScript = gameObject.GetComponent<SeptierungFadeShaderScript>();
        isPlaying = shaderScript.isPlaying;
        lastIsPlaying = isPlaying;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastIsPlaying != isPlaying)
            ChangeMaterial();
        lastIsPlaying = isPlaying;
        isPlaying = shaderScript.isPlaying;
    }

    private void ChangeMaterial()
    {
        if (isPlaying)
            gameObject.GetComponent<Renderer>().material = materialPlaying;
        else
            gameObject.GetComponent<Renderer>().material = materialNotPlaying;
    }
}
