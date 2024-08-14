using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T5Wand : MonoBehaviour
{

    public bool isModerator;

    public GameObject wandAimPoint;
    public GameObject wandGripPoint;

    public GameObject allObject;

    public GameObject notationSphere;

    public GameObject ParentHighlight;          // Must have a MudRenderer as parent
    public GameObject mudRender;
    private Vector3 mudRendererRotation;
    private LineRenderer lineRenderer;

    // public int maxNumberHighlights;
    public float rotationSpeed;
    public float scaleSpeed;

    public bool showingLine;

    private float maxXRotatingAngle = 89;
    

    public bool isNotating;
    public bool notationIsFixed = false;
    public float notationFixCooldown;
    private float timeSinceLastFix = 0;         

    [SerializeField] private LayerMask layermask;

    public ShowAnnotationScript annotationScript;

    public MudRendererCommunication mudRendererCommunication;

    private RaycastHit raycastHit;
    private bool hitSomethingP;


    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        mudRendererRotation = mudRender.transform.rotation.eulerAngles;
        notationSphere = ParentHighlight.transform.GetChild(0).gameObject;
        StageManager stageManager = mudRender.GetComponent<StageManager>();
        annotationScript = stageManager.stages[stageManager.currentStage].transform.GetComponentInChildren<ShowAnnotationScript>(includeInactive: true);
        if (mudRendererCommunication == null)
            mudRendererCommunication = FindObjectOfType<MudRendererCommunication>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (TiltFive.Wand.IsTracked())
        {
            Vector2 stickTilt = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
            bool AButton = OVRInput.Get(OVRInput.Button.One);
            bool BButton = OVRInput.Get(OVRInput.Button.Two);
            bool YButton = OVRInput.Get(OVRInput.Button.Four);
            float triggerValueMiddle = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);
            float triggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

            float distanceControllerAll = Vector3.Distance(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), allObject.transform.position);

            if (YButton)
            {
                mudRender.GetComponent<PlayerManager>().isModerator = !mudRender.GetComponent<PlayerManager>().isModerator;
            }

            if (triggerValueMiddle > 0.5f)
            {
                allObject.transform.position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch) + Vector3.Normalize((OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward)) * (distanceControllerAll + stickTilt.y * Time.deltaTime);
                Vector3 directionToPlayer = Camera.main.transform.position - allObject.transform.position;
                directionToPlayer.x = 0f;
                directionToPlayer.y = 0f;
                allObject.transform.rotation = Quaternion.LookRotation(-directionToPlayer);
                allObject.GetComponent<RotateOver>().enabled = true;

            }
            else if (stickTilt != Vector2.zero && !mudRendererCommunication.GetRotationsIsLocked() )
            {
                if (stickTilt != Vector2.zero) // Rotate model
                    RotateMudRenderer(stickTilt);
                allObject.GetComponent<RotateOver>().enabled = false;
            }
            else
            {
                allObject.GetComponent<RotateOver>().enabled = false;
            }
            
            {
                if (AButton || Input.GetKey(KeyCode.UpArrow)) // Scale down
                    mudRender.GetComponent<StageManager>().ScaleMudRenderer(-scaleSpeed);
            }
            
            {
                if (BButton || Input.GetKey(KeyCode.DownArrow)) // Scale up
                                    mudRender.GetComponent<StageManager>().ScaleMudRenderer(scaleSpeed);
            }

            {
                showingLine = true;
                bool hitSomething;
                bool canUnlcokByClicking;
                (raycastHit, hitSomething) = shootRay(triggerValue);
                hitSomethingP = hitSomething;
                canUnlcokByClicking = !isNotating && triggerValue > 0.2f;
                CheckAndSetNotationIsFixedOutside(canUnlcokByClicking);
            }
        }
        if (annotationScript != null && mudRendererCommunication.annotationsEnabled)
            EnableNearestAnnotation();


        

        lineRenderer.enabled = showingLine;
        timeSinceLastFix += Time.deltaTime;
    }

    public (RaycastHit, bool) shootRay(float triggerPressed)
    {
        
        Vector3 wandPosition = wandAimPoint.transform.position;
        Vector3 wandDirection = CalculateDirectionVectorWand(wandGripPoint, wandAimPoint);

        wandPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        wandDirection = (OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward) * 10 + wandPosition;
        // Create LayerMask so that ray only hits own heart
        layermask = (1 << gameObject.layer);
        if (isModerator)
            layermask |= (1 << gameObject.GetComponent<DistanceButtonPress>().buttonLayer);
        // Does not work. Second layer in layermask is not recognized. Current solution is to put Buttons under "Player one" layer.
        // Problems can happen when we want to have a multiplayer and player one is not always the moderator.

        (RaycastHit hit, bool hitSomething_) = RaycastFromAimPoint(wandPosition, wandDirection, layermask);
        
        // Move Notation and adjust line
        if (hitSomething_)
        {
            createLine(wandPosition, hit.point, 0.005f); 
            // Debug.Log("Hit this : " + hit.transform.name);
            // notationSphere.SetActive(true);
            notationSphere.GetComponent<PukNotationScript>().isOnObject = true;
            if (!notationIsFixed)
                MoveNotation(hit.point, hit.normal);
            if (hit.transform.tag == "Buttons")
                isNotating = false;
            else
                isNotating = true;         // Bug möglich das andere mitspieler sehen wenn man buttons anwählt
        }
        else
        {
            
            createLine(wandPosition, wandDirection, 2, Color.blue, 0.005f);
            // notationSphere.SetActive(false);
            // Try to set the sphere to 0,0,0 scale instead of 
            notationSphere.GetComponent<PukNotationScript>().isOnObject = false;
            isNotating = false;
        }

        // Adjsut if notation is active
        if (notationIsFixed)
            notationSphere.SetActive(true);

        // Check if Button is currently in ray and is pressed
        bool buttonWasPressed =  gameObject.GetComponent<DistanceButtonPress>().CheckForDistancePress(new Ray(wandPosition, wandDirection), notationIsFixed);



        // If conditions for fixing or unfixing the notation
        bool canFixAgain = timeSinceLastFix > notationFixCooldown;
        
        if (notationIsFixed && triggerPressed > 0.2f && canFixAgain)
        {
            notationIsFixed = false;
            timeSinceLastFix = 0f;
        }
        else if(!notationIsFixed && triggerPressed > 0.2f && canFixAgain && !buttonWasPressed)
        {
            notationIsFixed = true;
            timeSinceLastFix = 0f;
        }
        
        return (hit, hitSomething_);
    }


    public void createLine(Vector3 startPosition, Vector3 directionVector, int length, Color color, float width)
    {
        lineRenderer.startColor = new Color(1, 1, 1, 1);
        lineRenderer.endColor = new Color(1, 1, 1, 0);
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, startPosition + length * (directionVector));
        
    }

    private void createLine(Vector3 startPosition, Vector3 endPoint, float width)
    {
        lineRenderer.startColor = new Color(1, 1, 1, 1);
        lineRenderer.endColor = new Color(1, 1, 1, 0);
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPoint);

    }

    /// <summary>
    /// Calculates the scale the mudbun renderer needs to have 
    /// </summary>
    /// <returns></returns>
    private float CalculateScaleMudBunCylinder()
    {
        return (Vector3.Distance(ParentHighlight.transform.position, wandAimPoint.transform.position) * 4f )/ mudRender.transform.localScale.x;
    }

    private Vector3 CalculateDirectionVectorWand(GameObject gripPoint, GameObject aimPoint)
    {
        return aimPoint.transform.position - gripPoint.transform.position;
    }


    private void RotateMudRenderer(Vector2 rotateVector)
    {
        float newXValue = (mudRender.transform.eulerAngles.x + rotateVector.y * Time.deltaTime * rotationSpeed)%360;
        float newYValue = mudRender.transform.eulerAngles.y + rotateVector.x * Time.deltaTime * rotationSpeed;
        if ((90 < Mathf.Abs(newXValue) && Mathf.Abs(newXValue) < 270))
        {
            newXValue = mudRender.transform.eulerAngles.x;
        }
        mudRender.transform.eulerAngles = new Vector3(newXValue, newYValue, mudRender.transform.eulerAngles.z);
    }

    private (RaycastHit, bool) RaycastFromAimPoint(Vector3 origin_, Vector3 direction_, int layerMask_)
    {
        Ray ray = new Ray(origin_, direction_);
        RaycastHit hit;
        bool hitSomething = Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask_);
        return (hit, hitSomething);
    }

    private void MoveNotation(Vector3 movePosition, Vector3 normalOnMesh)
    {
        notationSphere.transform.position = movePosition;
        // notationSphere.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        notationSphere.transform.up = normalOnMesh;
        notationSphere.GetComponent<PukNotationScript>().SetNormalOnMesh(normalOnMesh);
    }

    public void MoveNotation(Vector3 movePosition, Vector3 normalOnMesh, GameObject notationObject)
    {
        notationObject.transform.position = movePosition;
        notationObject.transform.up = normalOnMesh;
    }

    private void EnableNearestAnnotation()
    {
        if(!hitSomethingP || hitSomethingP == null)
        {
            annotationScript.DisableAllAnnotations();
            
        }
        else if (raycastHit.transform.gameObject.GetComponent<MeshCollider>())
        {
            annotationScript.EnableClosestAnnotation(raycastHit.point);
            return;
        }
        else
        {
            annotationScript.DisableAllAnnotations();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>True if Notation is on mesh or is fixed, otherwise false</returns>
    public bool GetIsNotating()
    {
        return (isNotating || notationIsFixed);
    } 

    /// <summary>
    /// Is used to check if object is currently sclaing or animationg, then the notation should be unfixed
    /// </summary>
    private void CheckAndSetNotationIsFixedOutside(bool canUnlockByClicking_)
    {
        if(mudRender.GetComponent<StageManager>().animationAutomaticPlaying || mudRender.GetComponent<StageManager>().GetIsScaling() || canUnlockByClicking_) // is scaling fehlt
        {
            notationIsFixed = false;
        }
    }

}
