using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneHandler : MonoBehaviour
{
    public GameObject controllerText;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(loadMenu());
    }

    public void LoadSingleUser()
    {
        SceneManager.LoadScene("Scenes/Multi_Stage");
    }

    public bool IsControllerUsed()
    {
        Debug.Log("Test");
        if (OVRPlugin.GetHandTrackingEnabled())
        {
            Debug.Log("Test.");
            StartCoroutine(ShowMessage(2));
            // Hier Feedback einfügen das Controller genutzt werden muss
            return false;
        }
        else
            return true;
    }

    public void LoadMultiUser_Host()
    {
        if (!IsControllerUsed())
        {
            return;
        }
        StartCoroutine(ControllerIsWorking());
        PlayerPrefs.SetInt("LoadHost", 1);
        StaticClass.isHost = true;
        SceneManager.LoadScene("Scenes/MultiuserScene");     // ToDo: Multiuser hinzufügen
    }

    public void LoadMultiUser_Client()
    {
        if (!IsControllerUsed())
        {
            return;
        }
        StartCoroutine(ControllerIsWorking());
        PlayerPrefs.SetInt("LoadHost", 0);
        StaticClass.isHost = false;
        SceneManager.LoadScene("Scenes/MultiuserScene");
        //SceneManager.LoadScene("");     // ToDo: Multiuser hinzufügen
    }

    IEnumerator ShowMessage(float delay)
    {
        Debug.Log("Tes");
        controllerText.SetActive(true);
        yield return new WaitForSeconds(delay);
        controllerText.SetActive(false);
    }

    IEnumerator ControllerIsWorking()
    {
        controllerText.GetComponent<TextMeshPro>().text = "Controller funktioniert";
        controllerText.SetActive(true);
        yield return new WaitForSeconds(2);
    }

}
