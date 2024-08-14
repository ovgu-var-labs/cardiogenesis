using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManagement : MonoBehaviour
{
    public List<GameObject> stages;
    public int activeStage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadStage(int stage_)
    {
        stages[activeStage].SetActive(false);
        stages[stage_].SetActive(true);
        activeStage = stage_;
    }

    public void LoadNextStage()
    {
        LoadStage(activeStage + 1);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
