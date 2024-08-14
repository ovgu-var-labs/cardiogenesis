using Microsoft.MixedReality.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderScript : MonoBehaviour
{
    public AsyncOperation asyncOperation;

    public GameObject MRSceneContent;
    public SpatialAnchorManager anchorManager;
    public bool firstFrame = true;
    public bool moved = false;

    // Start is called before the first frame update
    void Start()
    {
        int nextSceneID = Mathf.Abs((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
        asyncOperation = SceneManager.LoadSceneAsync(nextSceneID);
        asyncOperation.allowSceneActivation = false;
        MRSceneContent = FindObjectOfType<MixedRealitySceneContent>().gameObject;
        anchorManager = FindObjectOfType<SpatialAnchorManager>();
        // if (!(SceneManager.GetActiveScene().buildIndex == 0) && firstFrame == false)
        //     MoveSceneContent();
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            FindObjectOfType<AnchorTutorialUIManager>().CreateAnchorAt(MRSceneContent.transform.position, MRSceneContent.transform.rotation);
        }
        else
        {
            if(FindObjectOfType<AnchorTutorialUIManager>().Get_allSavedAnchorsLength() > 0)
            {
                FindObjectOfType<AnchorTutorialUIManager>().LoadAllAnchors();
                MRSceneContent.transform.position = FindObjectOfType<AnchorTutorialUIManager>().GetActiveAnchor().gameObject.transform.position;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(moved == false)
        {
            if (!(SceneManager.GetActiveScene().buildIndex == 0) && firstFrame == false)
            {
                MoveSceneContent();
                moved = true;
            }
                
        }
        /*
        float tmp = (float)(MRSceneContent.transform.position.x + Time.deltaTime * 0.1);
        MRSceneContent.transform.position = new Vector3(tmp, MRSceneContent.transform.position.y, MRSceneContent.transform.position.z);
        
        firstFrame = false;
        */
    }

    public void MoveSceneContent()
    {
        Vector3 rightDirection = Quaternion.AngleAxis(CameraOffset.angle, Vector3.up) * new Vector3(CameraOffset.distance, 0);
        Debug.Log("Camera offset " + rightDirection);
        MRSceneContent.transform.position = Camera.main.transform.position + rightDirection;
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadPreloadedScene()
    {
        // Save the translation and rotation values of the camera
        CameraOffset.translation = Camera.main.transform.position - MRSceneContent.transform.position;
        CameraOffset.rotation = Camera.main.transform.eulerAngles;
        Debug.Log("Camera " + CameraOffset.translation);
        SaveValuesInCameraOffset();

        asyncOperation.allowSceneActivation = true;

    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="centerObject">Transform of center object</param>
    /// <param name="positionSecondObject">Position of Object to which we want to calculate the angle</param>
    /// <returns>1. The angle 2. The distance between objects</returns>
    public (float, float) CalculateAngleAndDistance(Transform centerObject, Vector3 positionSecondObject)
    {
        Vector2 objForward2D = new Vector2(centerObject.forward.x, centerObject.forward.z);
        Vector2 positionSecondObject2D = new Vector2(positionSecondObject.x - centerObject.position.x, positionSecondObject.z - centerObject.position.z);
        return (Vector2.SignedAngle(objForward2D, positionSecondObject2D), Vector2.SqrMagnitude(positionSecondObject2D));
    }

    public void SaveValuesInCameraOffset()
    {
        float angle, distance;
        (angle, distance) = CalculateAngleAndDistance(Camera.main.transform, MRSceneContent.transform.position);
        CameraOffset.angle = angle;
        CameraOffset.distance = distance;
    }

}
