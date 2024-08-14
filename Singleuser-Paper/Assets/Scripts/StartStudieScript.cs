using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartStudieScript : MonoBehaviour
{
    public AsyncOperation asyncOperation;


    // Start is called before the first frame update
    void Start()
    {
        int nextSceneID = Mathf.Abs((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
        asyncOperation = SceneManager.LoadSceneAsync(nextSceneID);
        asyncOperation.allowSceneActivation = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartStudie()
    {
        asyncOperation.allowSceneActivation = true;
    }
}
