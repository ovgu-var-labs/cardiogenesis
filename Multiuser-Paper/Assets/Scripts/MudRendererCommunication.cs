using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Communicates between different mud renderers. 
/// Updates Animationprogress, task and orientation.
/// </summary>
public class MudRendererCommunication : MonoBehaviour
{
    // Mud renderes
    public GameObject[] MudRenderers;                       // All mudrenderes which are used
    public GameObject ModeratorMudRender;                   // The mudrenderer of the moderator
    public List<GameObject> studentMudRenderers;            // The mudrenderes of the students check delete

    // public List<GameObject> markierungenDozent;
    // public List<GameObject> markierungenStudenten;

    public List<ListOfPuks> notationsElemente;              // List containing all Notation elements

    public GameObject notationsElementPrefab;
    
    public GameObject otherHighlightPrefab;

    public GameObject[] networkPlayers;

    // communicating values
    public int currentStage;

    public bool animationIsAutomaticPlaying;
    public float animationProgress;
    public float lastAnimationProgress;
    public bool isAutomaticPlaying;

    public bool annotationsEnabled;

    public bool markingIsLocked;

    public bool rotationIsLocked;
    public float angleCameraHeart;
    public float rotationAroundX;
    //TODO: rotation values

    // dependecies which give information
    public ButtonFunctionality moderatorButtons;
    public StageManager moderatorStageManager;

    // Start is called before the first frame update
    void Start()
    {
        AssignMudrenderes();
        AssignVariables();
    }

    // Update is called once per frame
    void Update()
    {
        GrabValues();
        UpdateIsAutomaticPlaying();
    }

    private void UpdateIsAutomaticPlaying()
    {
        if (lastAnimationProgress != animationProgress)
            isAutomaticPlaying = true;
        else
            isAutomaticPlaying = false;
        lastAnimationProgress = animationProgress;
    }

    /// <summary>
    /// Iterates through the list of given Muderenderes. Searches for the mudrenderer which is admin and assigns it.
    /// All other renderers get into the student list
    /// </summary>
    private void AssignMudrenderes()
    {
        for (int i = 0; i < MudRenderers.Length; i++)
        {
            if (MudRenderers[i].GetComponent<PlayerManager>().GetIsModerator())
            {
                ModeratorMudRender = MudRenderers[i];
            }
        }
    }


    private void AssignVariables()
    {
        moderatorStageManager = ModeratorMudRender.GetComponent<StageManager>();
    }

    /* Check delete
    private void CreateNotationMarkers()
    {
        for(int i = 0; i < MudRenderers.Length; i++)
        {
            ListOfPuks iList = new ListOfPuks();
            foreach(GameObject mr in MudRenderers)
            {
                if(!(MudRenderers[i].GetComponent<PlayerManager>().playerIndex == mr.GetComponent<PlayerManager>().playerIndex))
                { 
                    Debug.Log("looking at " + i + " " + mr.name);
                    GameObject tmp = Instantiate(notationsElementPrefab, mr.GetComponent<StageManager>().otherHighlights.transform);
                    tmp.SetActive(false);
                    tmp.GetComponent<PukNotationScript>().SetText((i + 1).ToString());
                    tmp.name = tmp.name + " " + (i + 1);
                    tmp.transform.SetLayer(tmp.transform.parent.gameObject.layer);
                    tmp.GetComponent<PukNotationScript>().SetStageManager(mr.GetComponent<StageManager>());
                    Color greyColor = new Color(0.69f, 0.69f, 0.69f, 1f);
                    tmp.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", greyColor);
                    iList.Add(tmp);
                    // iList.AddElement(tmp);
                }
            }
            Debug.Log("List: " + iList.Count + " " + iList[0].name + " " + iList[1].name);
            notationsElemente.Add(iList);
        }
    }
    */

    /// <summary>
    /// Gets values from Moderator.
    /// </summary>
    private void GrabValues()
    {
        currentStage = moderatorStageManager.GetCurrentStage();
        animationIsAutomaticPlaying = moderatorStageManager.GetAnimationAutomaticPlaying();
        animationProgress = moderatorButtons.AnimationSlider.Value;
        // angleCameraHeart = CalculateFlatAngleModeratorHeart();
        (angleCameraHeart, rotationAroundX) = CalculateFlatAngleModeratorHeartNew();
        rotationIsLocked = ModeratorMudRender.GetComponent<PlayerManager>().GetRotatingIsLocked();
        markingIsLocked = ModeratorMudRender.GetComponent<PlayerManager>().GetMarkingIsLocked();
        annotationsEnabled = moderatorStageManager.GetAnnontationsEnabled();
        // Debug.Log("Grabbed all values");
    }

    private void GrabSetValuesNotations()
    {
        for(int i = 0;i < MudRenderers.Length; i++)
        {
            // SetNotationActive(i, (MudRenderers[i].transform.parent.GetComponentInChildren<T5Wand>().GetIsNotating()));

        }
    }

    // Legacy
    /* Check delete
    private void GrabValuesMarkierung()
    {
        GetMarkierungListe(moderatorStageManager.GetOwnHighlights(), markierungenDozent);
        foreach (GameObject student in studentMudRenderers)
        {
            StageManager studentManager = student.GetComponent<StageManager>();
            GetMarkierungListe(studentManager.GetOwnHighlights(), markierungenStudenten);
        }
    }
    */

    /* Check delete
    /// <summary>
    /// Spread the received values out to the students.
    /// </summary>
    private void SpreadValues()
    {

        foreach(GameObject student in studentMudRenderers)
        {
            StageManager studentManager = student.GetComponent<StageManager>();
            // Stage
            if (currentStage != studentManager.GetCurrentStage())
            {
                studentManager.SetLoadStage(currentStage);
            }
            //isAutomaticPlaying
            if(animationIsAutomaticPlaying != studentManager.GetAnimationAutomaticPlaying())
            {
                studentManager.SetAnimationAutomaticPlaying(animationIsAutomaticPlaying);
            }
            //AnimationProgress
            if(animationProgress != studentManager.GetAnimationProgress())
            {
                studentManager.SetAnimationProgress(animationProgress);
            }
            //Rotation
            if (rotationIsLocked)
            {
                // ForceRotateHeart(angleCameraHeart, student);
                ForceRotateHeartNew(angleCameraHeart, student);
            }
            // Markierung
            if (!markingIsLocked)
            {
                // SpreadMarkierungen(markierungenDozent, studentManager.GetOtherHighlights()); 
                // TODO: Methode schreiben die markierungen der Studenten nimmt und diese untereinander und bei dem Dozenten einfügt
            }
            // Annotation
            if(annotationsEnabled != studentManager.GetAnnontationsEnabled())
            {
                studentManager.SetAnnotationsEnabled(annotationsEnabled);
            }
        }
    }
    */

    /* Check delete
    /// <summary>
    /// Calculates a angle between two vectors on the xz plane
    /// </summary>
    /// <returns></returns>
    private float CalculateFlatAngleModeratorHeart()
    {
        TiltFive.Glasses.TryGetPose(ModeratorMudRender.GetComponent<PlayerManager>().GetPlayerIndex(), out Pose pose);

        Vector2 mudVector = new Vector2(-ModeratorMudRender.transform.forward.x, -ModeratorMudRender.transform.forward.z);          // Have to negate the values
        Vector2 cameraVector = new Vector2(pose.position.x - ModeratorMudRender.transform.position.x, pose.position.z - ModeratorMudRender.transform.position.z);
        // float angle = Mathf.Rad2Deg * Mathf.Acos((mudVector.x * cameraVector.x + mudVector.y * cameraVector.y) / (Vector2.SqrMagnitude(mudVector) * Vector2.SqrMagnitude(cameraVector)));
        float leftRight = AngleDir(cameraVector, ModeratorMudRender);
        if (leftRight == 1f)
            return Vector2.Angle(cameraVector, mudVector);
        else
            return 360 - Vector2.Angle(cameraVector, mudVector);
    }
    */

    private (float,float) CalculateFlatAngleModeratorHeartNew()
    {
        // TiltFive.Glasses.TryGetPose(ModeratorMudRender.GetComponent<PlayerManager>().GetPlayerIndex(), out Pose pose);
        Vector2 mudVector = new Vector2(-ModeratorMudRender.transform.forward.x, -ModeratorMudRender.transform.forward.z);
        Vector2 cameraVector = new Vector2(Camera.main.transform.position.x - ModeratorMudRender.transform.position.x, Camera.main.transform.position.z - ModeratorMudRender.transform.position.z);

        float angle = Vector2.SignedAngle(mudVector, cameraVector);
        float rotationAroundX_ = ModeratorMudRender.transform.localEulerAngles.x;

        return (angle, rotationAroundX_);

    }

    /* Check delete
    /// <summary>
    /// Checks if camera is right or left of heart
    /// </summary>
    /// <param name="rotationgvector">Vector from heart to camere</param>
    /// <returns>1 if camera is on the left side of the heart, otherwise -1</returns>
    float AngleDir(Vector2 rotationgvector, GameObject middleObject) // TODO: soft Gameobject übergeben
    {
        Vector2 rightOfModel = new Vector2(middleObject.transform.right.x, middleObject.transform.right.z); 
        Vector2 leftOfModel = new Vector2(-middleObject.transform.right.x, -middleObject.transform.right.z);
        if (Vector2.Angle(rotationgvector, rightOfModel) <= Vector2.Angle(rotationgvector, leftOfModel))
            return -1f;
        else
            return 1f;
    }
    */

    /// <summary>
    /// Called for students.
    /// Should rotate heart object of students in a way that they see own heart in same angle as moderator sees his heart.
    /// Is not used in current version of software.
    /// </summary>
    /// <param name="angleToAchieve">Angle of moderator</param>
    /// <param name="heartToRotate">heartObject of students</param>
    public void ForceRotateHeartNew(float angleToAchieve, GameObject heartToRotate)
    {
        float currentAngle = 0f;
        // TiltFive.Glasses.TryGetPose(heartToRotate.GetComponent<PlayerManager>().GetPlayerIndex(), out Pose pose);
        Vector2 mudVector = new Vector2(-heartToRotate.transform.forward.x, -heartToRotate.transform.forward.z);          // Have to negate the values
        Vector2 cameraVector = new Vector2(Camera.main.transform.position.x - heartToRotate.transform.position.x, Camera.main.transform.position.z - heartToRotate.transform.position.z);

        currentAngle = Vector2.SignedAngle(mudVector, cameraVector);
        float differenceAngle = angleToAchieve - currentAngle;

        heartToRotate.transform.forward = -(Quaternion.AngleAxis(differenceAngle, Vector3.up) * new Vector3(-heartToRotate.transform.forward.x, 0, -heartToRotate.transform.forward.z));
        // New way to rotate around all axis
        heartToRotate.transform.localEulerAngles = new Vector3(ModeratorMudRender.transform.localEulerAngles.x, heartToRotate.transform.localEulerAngles.y, heartToRotate.transform.localEulerAngles.z);
        // Update Scale
        heartToRotate.GetComponent<StageManager>().SetScale(ModeratorMudRender.GetComponent<StageManager>().currentScale);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="angleToAchieve">Angle of moderator</param>
    /// <param name="rotAroundX">Roatation around x-axis from moderator</param>
    /// <param name="heartToRotate">heartObject which should be rotatet</param>
    public void NetForceRotateHeartNew(float angleToAchieve, float rotAroundX, GameObject heartToRotate)
    {
        float currentAngle = 0f;
        // TiltFive.Glasses.TryGetPose(heartToRotate.GetComponent<PlayerManager>().GetPlayerIndex(), out Pose pose);
        Vector2 mudVector = new Vector2(-heartToRotate.transform.forward.x, -heartToRotate.transform.forward.z);          // Have to negate the values
        Vector2 cameraVector = new Vector2(Camera.main.transform.position.x - heartToRotate.transform.position.x, Camera.main.transform.position.z - heartToRotate.transform.position.z);

        currentAngle = Vector2.SignedAngle(mudVector, cameraVector);
        float differenceAngle = angleToAchieve - currentAngle;

        heartToRotate.transform.forward = -(Quaternion.AngleAxis(differenceAngle, Vector3.up) * new Vector3(-heartToRotate.transform.forward.x, 0, -heartToRotate.transform.forward.z));
        // New way to rotate around all axis
        heartToRotate.transform.localEulerAngles = new Vector3(rotAroundX, heartToRotate.transform.localEulerAngles.y, heartToRotate.transform.localEulerAngles.z);
        // Update Scale
        heartToRotate.GetComponent<StageManager>().SetScale(ModeratorMudRender.GetComponent<StageManager>().currentScale);
    }

    /// <summary>
    /// Iterates through the children of the highlight parents
    /// </summary>
    /// <param name="ownHighlights">Parent of highlights</param>
    /// <param name="listToAdd">To wich list the highlights should be added</param>
    public void GetMarkierungListe(GameObject ownHighlights, List<GameObject> listToAdd)
    {
        // Bad method. Copys objects multiple times into list if they are not destroyed at runtime
        for(int i = 0; i < ownHighlights.transform.childCount; i++)
        {
            listToAdd.Add(ownHighlights.transform.GetChild(i).gameObject);
        }
    }

    /* Check delete
    public void SpreadMarkierungen(GameObject spreadObject, TiltFive.PlayerIndex playerIndex)
    {
        foreach(GameObject player in MudRenderers)
        {
            if(player.GetComponent<PlayerManager>().GetPlayerIndex() != playerIndex)
            {
                GameObject tmp = Instantiate(otherHighlightPrefab, player.GetComponent<StageManager>().GetOtherHighlights().transform);
                tmp.transform.localPosition = spreadObject.transform.localPosition;
                tmp.transform.localRotation = spreadObject.transform.localRotation;
                tmp.transform.localScale = spreadObject.transform.localScale;
                Destroy(tmp, 5f);
            }
        }
    }
    */

    public void SpreadNotationPosition(Vector3 position, TiltFive.PlayerIndex playerIndex)
    {
        /*
        foreach(GameObject player in MudRenderers)
        {
            if (player.GetComponent<PlayerManager>().GetPlayerIndex() != playerIndex)
            {
                // player.GetComponent<StageManager>().otherHighlights;
            }
        }
        */
    }

    private void SetNotationActive(int notationID, bool active)
    {
        /*
        foreach(GameObject notation in notationsElemente[notationID])
        {
            notation.SetActive(active);
            if (active)
            {
                MoveNotationActive(notationID, notation);
            }
        }
        */
    }

    private void MoveNotationActive(int numberRenderer, GameObject gameObjectToMove)
    {
        Debug.Log("List: is called");
        var pukOriginal = MudRenderers[numberRenderer].GetComponent<StageManager>().GetOwnHighlights().GetComponentInChildren<PukNotationScript>().gameObject;
        // float correctScale = (0.3f / gameObjectToMove.GetComponent<PukNotationScript>().stageManager.currentScale) * 1;
        float correctScale = (0.3f / MudRenderers[numberRenderer].GetComponent<StageManager>().currentScale) * 1;
        gameObjectToMove.transform.localPosition = pukOriginal.transform.localPosition;
        gameObjectToMove.transform.localRotation = pukOriginal.transform.localRotation;
    }

    public bool GetRotationsIsLocked()
    {
        return rotationIsLocked;
    }
}
