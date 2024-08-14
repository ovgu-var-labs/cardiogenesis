using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfobarModText : MonoBehaviour
{
    public TextMeshPro infobar;

    public StageManager stageManager;

    public string[] stageTextArray;

    private int currentScene;

    // Start is called before the first frame update
    void Start()
    {
        currentScene = stageManager.currentStage;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentScene != stageManager.currentStage)
        {
            currentScene = stageManager.currentStage;
            infobar.text = stageTextArray[currentScene];
        }
    }
}
