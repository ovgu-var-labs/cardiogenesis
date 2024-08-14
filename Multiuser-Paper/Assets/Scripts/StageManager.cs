using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Task is to switch between different states.
/// Handles rotation and scaling of the heart modell.
/// Sets initial scale and positioning of heart modell.
/// </summary>
public class StageManager : MonoBehaviour
{
    public GameObject[] stages;
    public float[] stagesScale;
    public float[] stageMaxScale;
    public float[] stageYOffset;
    public GameObject ownHighlights;
    public GameObject otherHighlights;

    public bool annotationsEnabled;

    public float lastScale;
    public float currentScale;
    public float lowerScaleLimit;
    public float upperScaleLimit;

    public bool animationAutomaticPlaying;
    public int currentStage = -1;


    // Start is called before the first frame update
    void Start()
    {
        currentScale = gameObject.transform.localScale.x;

        for(int i = 0; i < stages.Length; i++)
        {
            if (stages[i].activeInHierarchy)
            {
                currentStage = i;
                break;
            }

        }
        // Gets called when no stage is active in the moment
        if(currentStage == -1)
        {
            currentStage = 0;
            stages[0].SetActive(true);
        }

        // Deactivates all other stages
        for(int i = 0; i < stages.Length; i++)
        {
            if(i != currentStage)
            {
                stages[i].SetActive(false);
            }
        }
        SetScale(stagesScale[currentStage]);
        SetYOffset(stageYOffset[currentStage]);
        upperScaleLimit = stageMaxScale[currentStage];
    }

    // Update is called once per frame
    void Update()
    {
        lastScale = currentScale;
    }

    public void ResetRotation()
    {
        ForceRotateHeart(0f, gameObject);
        gameObject.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);
        gameObject.transform.localScale = new Vector3(stagesScale[currentStage], stagesScale[currentStage], stagesScale[currentStage]);
        SetScale(stagesScale[currentStage]);
    }

    public void LoadNextStage()
    {
        int stageToLoadID = (currentStage + 1) % stages.Length;
        SetLoadStage(stageToLoadID);
    }

    public void LoadPreviousStage()
    {
        int stageToLoadID = (currentStage - 1) % stages.Length;
        SetLoadStage(stageToLoadID);
    }

    public void SetLoadStage(int stageToLoad)
    {
        int lastStage = currentStage;
        Debug.Log("Loading stage " + stageToLoad);
        stages[currentStage].SetActive(false);
        currentStage = stageToLoad;
        stages[currentStage].SetActive(true);
        SetAnimationAutomaticPlaying(false);
        SetAnimationProgress(0f);
        SetScale(stagesScale[currentStage]);
        if (annotationsEnabled)
        {
            SetAnnotationsEnabled(false, lastStage);
            SetAnnotationsEnabled(true, stageToLoad);
        }

    }

    public void SetAnimationAutomaticPlaying(bool animationAutomaticPlaying_)
    {
        animationAutomaticPlaying = animationAutomaticPlaying_;
        Debug.Log("Name of Stage " + stages[currentStage].transform.name);
        stages[currentStage].GetComponent<GlobalAnimationManager>().SetAnimationAutomaticPlaying(animationAutomaticPlaying);
    }

    public void SetAnimationProgress(float animationProgress)
    {
        stages[currentStage].GetComponent<GlobalAnimationManager>().UpdateAnimationTo(animationProgress);
    }

    public int GetCurrentStage()
    {
        return currentStage;
    }

    public bool GetAnimationAutomaticPlaying()
    {
        return animationAutomaticPlaying;
    }

    public float GetAnimationProgress()
    {
        return stages[currentStage].GetComponent<GlobalAnimationManager>().GetAnimationProgress();
    }


    float AngleDir(Vector2 rotationgvector, GameObject middleObject) // TODO: soft Gameobject übergeben
    {
        Vector2 rightOfModel = new Vector2(middleObject.transform.right.x, middleObject.transform.right.z);
        Vector2 leftOfModel = new Vector2(-middleObject.transform.right.x, -middleObject.transform.right.z);
        if (Vector2.Angle(rotationgvector, rightOfModel) <= Vector2.Angle(rotationgvector, leftOfModel))
            return 1f;
        else
            return -1f;
    }

    public void SetAnnotationsEnabled(bool annotationsEnabled_)
    {
        annotationsEnabled = annotationsEnabled_;

        stages[currentStage].GetComponent<GlobalAnimationManager>().annotations.SetActive(annotationsEnabled);

        /*
        for(int i = 0; i < stages[currentStage].transform.childCount; i++)
        {
            if(stages[currentStage].transform.GetChild(i).GetComponentInChildren<TooltTipDIY>(true) != null)
            {
                stages[currentStage].transform.GetChild(i).GetComponentInChildren<TooltTipDIY>(true).gameObject.transform.parent.gameObject.SetActive(annotationsEnabled);
            }
        }
        */
        // stages[currentStage].transform.Find("Annotations").gameObject.SetActive(annotationsEnabled);

        // Infobar mit darüber enablen
        gameObject.transform.parent.GetComponentInChildren<Infobar>(true).gameObject.SetActive(annotationsEnabled);
    }

    /// <summary>
    /// Does not change annotationIsEnabled
    /// </summary>
    /// <param name="annotationsEnabled_"></param>
    /// <param name="stage_"></param>
    public void SetAnnotationsEnabled(bool annotationsEnabled_, int stage_)
    {
        // annotationsEnabled = annotationsEnabled_;
        for (int i = 0; i < stages[stage_].transform.childCount; i++)
        {
            if (stages[stage_].transform.GetChild(i).GetComponentInChildren<TooltTipDIY>(true) != null)
            {
                stages[stage_].transform.GetChild(i).GetComponentInChildren<TooltTipDIY>(true).gameObject.transform.parent.gameObject.SetActive(annotationsEnabled_);
            }
        }
        // stages[currentStage].transform.Find("Annotations").gameObject.SetActive(annotationsEnabled);
    }

    public bool GetAnnontationsEnabled()
    {
        return annotationsEnabled;
    }

    /// <summary>
    /// Rotates the heart object to an desired angle.
    /// </summary>
    /// <param name="angleToAchieve">Angle the <paramref name="heartToRotate"/> should have to the player</param>
    /// <param name="heartToRotate">Object that is rotatet</param>
    private void ForceRotateHeart(float angleToAchieve, GameObject heartToRotate)
    {

        float currentAngle = 0f;
        TiltFive.Glasses.TryGetPose(heartToRotate.GetComponent<PlayerManager>().GetPlayerIndex(), out Pose pose);

        Vector2 mudVector = new Vector2(-heartToRotate.transform.forward.x, -heartToRotate.transform.forward.z);          // Have to negate the values
        Vector2 cameraVector = new Vector2(pose.position.x - heartToRotate.transform.position.x, pose.position.z - heartToRotate.transform.position.z);
        float leftRight = AngleDir(cameraVector, heartToRotate);
        if (leftRight == 1f)
            currentAngle = Vector2.Angle(cameraVector, mudVector);
        else
            currentAngle = 360 - Vector2.Angle(cameraVector, mudVector);

        float difference = Mathf.Abs(currentAngle - angleToAchieve);
        if (difference < 1f || difference > 359f)
            return;
        Debug.Log("Rotating " + heartToRotate.transform.name + "\nAngle1: " + angleToAchieve + " Angle2: " + currentAngle + " Difference: " + difference);
        float newYRotation = heartToRotate.transform.eulerAngles.y - difference;
        Debug.Log("Rotating " + heartToRotate.transform.name + " OldY: " + heartToRotate.transform.eulerAngles.y + " NewY: " + newYRotation);
        heartToRotate.transform.eulerAngles = new Vector3(heartToRotate.transform.eulerAngles.x, newYRotation, heartToRotate.transform.eulerAngles.z);

    }

    public GameObject GetOwnHighlights()
    {
        return ownHighlights;
    }

    public GameObject GetOtherHighlights()
    {
        return otherHighlights;
    }


    public void ScaleMudRenderer(float scaleValue)
    {
        float newScale = Mathf.Clamp(currentScale + scaleValue * Time.deltaTime, lowerScaleLimit, upperScaleLimit);
        currentScale = newScale;
        gameObject.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
    }

    public void SetScale(float newScale)
    {
        currentScale = newScale;
        gameObject.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
    }

    private void SetYOffset(float yOffset)
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, yOffset, gameObject.transform.position.z);
    }

    /// <summary>
    /// If stage is currently scaling
    /// </summary>
    /// <returns>True if stage is scaling, else false</returns>
    public bool GetIsScaling()
    {
        return lastScale != currentScale;
    }
}
