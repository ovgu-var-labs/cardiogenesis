using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Is used the press the UI buttons from the controller
/// </summary>
public class DistanceButtonPress : MonoBehaviour
{

    /* 
     * Check delete
    public bool isActive;
    public GameObject wandAimPoint;
    public GameObject wandGripPoint;
    // public TiltFive.PlayerIndex playerIndex;

    public ButtonFunctionality buttonFunctionality;
    public PressableButton automaticPlayingButton;
    public PressableButton annotationEnabledButton;

    public LayerMask buttonLayer;

    public float CoolDownTimeStage;
    public float timeSinceLastPressStage;

    public float CoolDownTimeAnimation;
    public float timeSinceLastPressAnimation;

    public float CoolDownTimeAnnotation;
    public float timeSinceLastPressAnnotation;

    // Start is called before the first frame update
    void Start()
    {
        playerIndex = GetPlayerIndex();
    }

    // Update is called once per frame
    void Update()
    {
        // if (isActive)
        if (TiltFive.Input.TryGetButton(TiltFive.Input.WandButton.A, out bool AIsPressed, TiltFive.ControllerIndex.Right, playerIndex))
        {
            if (AIsPressed && timeSinceLastPressStage > CoolDownTimeStage)
            {
                // buttonFunctionality.PreviousStage();
                timeSinceLastPressStage = 0f;
            }
        }

        if (TiltFive.Input.TryGetButton(TiltFive.Input.WandButton.Y, out bool YIsPressed, TiltFive.ControllerIndex.Right, playerIndex))
        {
            if (YIsPressed && timeSinceLastPressStage > CoolDownTimeStage)
            {
                // buttonFunctionality.NextStage();
                timeSinceLastPressStage = 0f;
            }
        }

        if (TiltFive.Input.TryGetButton(TiltFive.Input.WandButton.X, out bool XIsPressed, TiltFive.ControllerIndex.Right, playerIndex))
        {
            if (XIsPressed && timeSinceLastPressAnimation > CoolDownTimeAnimation)
            {
                // buttonFunctionality.ToggleIsAutomaticPlaying();
                timeSinceLastPressAnimation = 0f;
                // automaticPlayingButton.ForceSetToggled(!automaticPlayingButton.IsToggled);
                buttonFunctionality.ChangeInterfaceText(1);
            }
        }

        if (TiltFive.Input.TryGetButton(TiltFive.Input.WandButton.B, out bool bIsPressed, playerIndex: playerIndex))
        {
            if (bIsPressed && timeSinceLastPressAnnotation > CoolDownTimeAnnotation) // Enable annotations
            {
                // buttonFunctionality.ToggleAnnotationsEnabled();
                timeSinceLastPressAnnotation = 0f;
                // annotationEnabledButton.ForceSetToggled(!annotationEnabledButton.IsToggled);
                buttonFunctionality.ChangeInterfaceText(2);
            }
                    
        }

        
        timeSinceLastPressStage += Time.deltaTime;
        timeSinceLastPressAnimation += Time.deltaTime;
        timeSinceLastPressAnnotation += Time.deltaTime;
    }

    /// <summary>
    /// Checks if Button was hit with ray and activates its click, if pressed
    /// </summary>
    /// <param name="ray">Ray from wand</param>
    /// <param name="notationIsFixed_">If notation is currently fixed</param>
    /// <returns>true if button was pressed, otherwise false</returns>
    public bool CheckForDistancePress(Ray ray, bool notationIsFixed_)
    {
        float triggerPressed = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        bool hitButton = false;
        // if (TiltFive.Input.TryGetTrigger(out float triggerPressed, TiltFive.ControllerIndex.Right, playerIndex) && !notationIsFixed_)
        {
            (bool hitSomething, RaycastHit hit) = RaycastFromAimPoint(ray);

            if ((triggerPressed) > 0.2f)       // Have to multiply the triggervalue by two because the value is somehow halfed with by using it also in t5 wand
            {
                hitButton = hit.transform.parent.TryGetComponent<ButtonFunctionality>(out ButtonFunctionality buttonFunctionality_) || hit.transform.TryGetComponent<DistanceSliderScript>(out DistanceSliderScript distanceSliderScript);
                Debug.Log("Shooting ray.: " + hit.transform.name + " " + triggerPressed);
                if (hit.transform.parent.GetComponent<ButtonFunctionality>() && buttonFunctionality_.AllowedToPress())
                {
                    hit.transform.gameObject.GetComponent<PressableButton>().ForceSetToggled(!hit.transform.gameObject.GetComponent<PressableButton>().IsToggled);
                    return true;
                }
                else if (hit.transform.GetComponent<DistanceSliderScript>()) // Evtl hier den allowed to press rausnehmen
                {
                    // Debug.Log("Time since last press " + buttonFunctionality.timeSinceLastPress);
                    hit.transform.GetComponent<DistanceSliderScript>().MoveSliderToPosition();
                    buttonFunctionality.timeSinceLastPress = 0f;
                    // buttonFunctionality.timeSinceLastPress = 0f;
                    // hit.transform.GetComponent<DistanceSliderScript>().MoveSliderToRelativePosition(hit);
                    return true;
                }
                
            }
        }
        return hitButton;

    }


    private (bool, RaycastHit) RaycastFromAimPoint(Ray ray_)
    {
        RaycastHit hit;
        bool hitSomething = Physics.Raycast(ray_, out hit);
        return (hitSomething, hit);
    }

    private Vector3 CalculateDirectionVectorWand(GameObject gripPoint, GameObject aimPoint)
    {
        return aimPoint.transform.position - gripPoint.transform.position;
    }

    private void ResetTimer()
    {
        timeSinceLastPressStage = 0f;
    }



    /*
    public TiltFive.PlayerIndex GetPlayerIndex()
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
    */
}
