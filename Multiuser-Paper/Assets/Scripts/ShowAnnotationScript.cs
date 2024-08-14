using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAnnotationScript : MonoBehaviour
{

    public List<TooltTipDIY> annotationObjects;
    public TiltFive.PlayerIndex playerIndex;
    public GameObject activeAnnotation;
    public float annotationScale;

    public GameObject rahmen;

    public StageManager stageManager;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.TryGetComponent<TooltTipDIY>(out TooltTipDIY tooltTipDIY))
                annotationObjects.Add(tooltTipDIY);
            // annotationObjects.Add(transform.GetChild(i).GetComponent<TooltTipDIY>());
        }
        DisableAllAnnotations();
        playerIndex = GetPlayerIndex();
        stageManager = gameObject.transform.parent.transform.parent.GetComponent<StageManager>();
        rahmen = Helper1.FindComponentInChildWithTag(stageManager.gameObject, "Rahmen");
        rahmen.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        MoveAnnotation(activeAnnotation.GetComponent<TooltTipDIY>());
        if (gameObject.activeSelf)
            UpdateScaleAnnotation();
    }

    public bool EnableClosestAnnotation(Vector3 hitPoint)
    {
        Debug.Log("Annotations " + playerIndex + " " + hitPoint);
        float shortestDistanceHitAnnotation = float.MaxValue;
        TooltTipDIY tooltTipToActivate = null;
        Debug.Log("Annotations " +annotationObjects.Count);
        foreach(TooltTipDIY tooltTipDIY in annotationObjects)
        {
            float distanceHitAnnotation = Vector3.Distance(hitPoint, tooltTipDIY.ankerPointTarget.transform.position);
            Debug.Log("Annotations " + playerIndex + " " + distanceHitAnnotation);
            if (distanceHitAnnotation < shortestDistanceHitAnnotation)
            {
                shortestDistanceHitAnnotation = distanceHitAnnotation;
                tooltTipToActivate = tooltTipDIY;
            }
        }
        if(tooltTipToActivate == null)
        {
            return false;
        }
        ActivateAnnotation(tooltTipToActivate);
        activeAnnotation = tooltTipToActivate.gameObject;
        foreach(TooltTipDIY tooltTipDIY in annotationObjects)
        {
            if (tooltTipDIY != tooltTipToActivate)
                DeactivateAnnotation(tooltTipDIY);
        }
        return true;
    }

    public void DisableAllAnnotations()
    {
        foreach(TooltTipDIY tooltTipDIY in annotationObjects)
        {
            DeactivateAnnotation(tooltTipDIY);
        }
    }

    private void DeactivateAnnotation(TooltTipDIY annotation)
    {
        annotation.gameObject.SetActive(false);
    }

    private void ActivateAnnotation(TooltTipDIY annotation)
    {
        // Move object to the right or left
        
        annotation.gameObject.SetActive(true);
    }
    
    public void MoveAnnotation(TooltTipDIY annotation)
    {
        Vector3 annotationCamera = new Vector3(Camera.main.transform.position[0] - annotation.transform.position[0], Camera.main.transform.position[1] - annotation.transform.position[1], Camera.main.transform.position[2] - annotation.transform.position[2]);
        Vector3 annotationCameraNormalized = annotationCamera.normalized;
        Vector3 annotationCameraNormFlat = Vector3.Normalize(Helper1.MakeVectorFlat(annotationCameraNormalized));
        // TiltFive.Glasses.TryGetPose(playerIndex, out Pose pose);
        float angle = Vector2.SignedAngle(new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.z), new Vector2(annotation.ankerPointTarget.transform.position.x, annotation.ankerPointTarget.transform.position.z));
        if (angle >= 0)
        {
            // Right
            Vector3 newPos = Quaternion.AngleAxis(-90, Vector3.up) * annotationCameraNormFlat * 0.3f + stageManager.transform.position;
            annotation.gameObject.transform.position = newPos;
            // annotation.gameObject.transform.localPosition = ( Quaternion.AngleAxis(-90, Vector3.up) * Vector3.Normalize(new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z)) )* 1f;
            // annotation.gameObject.transform.position = Vector3.Normalize(new Vector3(-pose.position.z, 0, pose.position.x)) * 0.2f;
            Debug.Log("Rotate Right " + annotation.gameObject.transform.localPosition + " " + angle);
        }
        else
        {
            // Left
            Vector3 newPos = Quaternion.AngleAxis(90, Vector3.up) * annotationCameraNormFlat * 0.3f + stageManager.transform.position;
            annotation.gameObject.transform.position = newPos;

            //annotation.gameObject.transform.localPosition = (Quaternion.AngleAxis(90f, Vector3.up) * Vector3.Normalize(new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z))) * 1f;
            //annotation.transform.position = Vector3.Normalize(new Vector3(-pose.position.x, 0, pose.position.z)) * 0.2f;
            Debug.Log("Rotate Left " + annotation.gameObject.transform.localPosition);
        }
    }

    private void OnDisable()
    {
        rahmen.SetActive(false);
    }

    private void OnEnable()
    {
        rahmen.SetActive(true);
    }

    private void UpdateScaleAnnotation()
    {
        float newScale = (0.3f / stageManager.currentScale) * annotationScale;
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }
    /*
    private GameObject GetRahmenOutOfScene()
    {
        GameObject[] rahmen_ = GameObject.FindGameObjectsWithTag()
    }
    */
    private TiltFive.PlayerIndex GetPlayerIndex()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Player One"))
        {
            return TiltFive.PlayerIndex.One;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Player Two"))
        {
            return TiltFive.PlayerIndex.Two;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Player Three"))
        {
            return TiltFive.PlayerIndex.Three;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Player Four"))
        {
            return TiltFive.PlayerIndex.Four;
        }
        else
        {
            return TiltFive.PlayerIndex.One;
        }
    }
}
