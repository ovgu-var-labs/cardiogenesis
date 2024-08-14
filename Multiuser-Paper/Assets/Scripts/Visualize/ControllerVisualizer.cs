using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerVisualizer : MonoBehaviour
{
    public TiltFive.PlayerIndex playerIndex;

    public bool showLaser;

    private bool isTracked;
    private bool lastIsTracked;
    public List<GameObject> buttonList;
    public List<bool> isButtonPressedList;
    public GameObject stick;
    public GameObject trigger;

    public bool oneIsPressed;
    public bool twoIsPressed;
    public bool AIsPressed;
    public bool BIsPressed;
    public bool XIsPressed;
    public bool YIsPressed;

    public Material pressedMaterial;
    private Material plasticMaterial;

    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        playerIndex = GetPlayerIndex();
        plasticMaterial = gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material;
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (!TiltFive.Player.IsConnected(playerIndex))
        {
            gameObject.SetActive(false);
            return;
        }
        */
        if (showLaser)
        {
            if (TiltFive.Input.TryGetTrigger(out float triggerValue, playerIndex: playerIndex))
            {
                if (triggerValue > 0.1f)
                {
                    lineRenderer.enabled = true;
                    Vector3 wandPosition = gameObject.GetComponentInParent<T5Wand>().wandAimPoint.transform.position;
                    Vector3 wandDirection = CalculateDirectionVectorWand(gameObject.GetComponentInParent<T5Wand>().wandGripPoint, gameObject.GetComponentInParent<T5Wand>().wandAimPoint);
                    createLine(wandPosition, wandDirection, 5, Color.blue, 0.005f);
                }
                else
                {
                    lineRenderer.enabled = false;
                }
            }
            else
            {
                lastIsTracked = isTracked;
                isTracked = TiltFive.Wand.IsTracked(playerIndex: playerIndex);
                if (lastIsTracked != isTracked)
                    VisualizeWand(isTracked);

                if (isTracked)
                {
                    Debug.Log("Pressing");
                    CheckIfButtonsArePressed();

                    for (int i = 0; i < buttonList.Count; i++)
                    {
                        ChangeColorButton(isButtonPressedList[i], buttonList[i]);
                    }

                    CheckIfStickisMoved();
                    CheckIfTriggerIsMoved();
                }
            }


        }
    }

    public void VisualizeWand(bool isTracked)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(isTracked);
        }
    }

    public void CheckIfButtonsArePressed()
    {
        if (TiltFive.Input.TryGetButton(TiltFive.Input.WandButton.One, out bool buttonOneIsPressed, playerIndex: playerIndex))
        {
            oneIsPressed = buttonOneIsPressed;
            isButtonPressedList[2] = buttonOneIsPressed;
            Vector3 wandPosition = gameObject.GetComponentInParent<T5Wand>().wandAimPoint.transform.position;
            Vector3 wandDirection = CalculateDirectionVectorWand(gameObject.GetComponentInParent<T5Wand>().wandGripPoint, gameObject.GetComponentInParent<T5Wand>().wandAimPoint);
            createLine(wandPosition, wandDirection, 5, Color.blue, 0.005f);
        }
        if (oneIsPressed)
            lineRenderer.enabled = true;
        else
            lineRenderer.enabled = false;

        if (TiltFive.Input.TryGetButton(TiltFive.Input.WandButton.Two, out bool buttonTowIsPressed, playerIndex: playerIndex))
        {
            twoIsPressed = buttonTowIsPressed;
            isButtonPressedList[4] = buttonTowIsPressed;
        }

        if (TiltFive.Input.TryGetButton(TiltFive.Input.WandButton.A, out bool buttonAIsPressed, playerIndex: playerIndex))
        {
            AIsPressed = buttonAIsPressed;
            isButtonPressedList[0] = buttonAIsPressed;
        }

        if (TiltFive.Input.TryGetButton(TiltFive.Input.WandButton.B, out bool buttonBIsPressed, playerIndex: playerIndex))
        {
            BIsPressed = buttonBIsPressed;
            isButtonPressedList[1] = buttonBIsPressed;
        }

        if (TiltFive.Input.TryGetButton(TiltFive.Input.WandButton.X, out bool buttonXIsPressed, playerIndex: playerIndex))
        {
            XIsPressed = buttonXIsPressed;
            isButtonPressedList[5] = buttonXIsPressed;
        }

        if (TiltFive.Input.TryGetButton(TiltFive.Input.WandButton.Y, out bool buttonYIsPressed, playerIndex: playerIndex))
        {
            YIsPressed = buttonYIsPressed;
            isButtonPressedList[6] = buttonYIsPressed;
        }
    }

    private void ChangeColorButton(bool buttonIsActive, GameObject gameobjectToColor)
    {
        if (buttonIsActive)
        {
            gameobjectToColor.GetComponent<MeshRenderer>().material = pressedMaterial;
        }
        else
        {
            gameobjectToColor.GetComponent<MeshRenderer>().material = plasticMaterial;
        }
    }


    private void CheckIfTriggerIsMoved()
    {
        if(TiltFive.Input.TryGetTrigger(out float triggerValue, playerIndex: playerIndex))
        {
            if(triggerValue >= 0.8f)
            {
                trigger.transform.localEulerAngles = new Vector3(triggerValue * 15, 0, 0);
            }
        }
        ChangeColorButton((triggerValue >= 0.8f), trigger);
    }

    public void CheckIfStickisMoved()
    {
        if(TiltFive.Input.TryGetStickTilt(out Vector2 stickTilt, playerIndex: playerIndex))
        {
            stick.transform.localEulerAngles = new Vector3(stickTilt.y * 15, 0, -(stickTilt.x * 15));
        }
        if(stickTilt != Vector2.zero)
        {
            stick.GetComponent<MeshRenderer>().material = pressedMaterial;
        }
        else
        {
            stick.GetComponent<MeshRenderer>().material = plasticMaterial;
        }
    }

    public void createLine(Vector3 startPosition, Vector3 directionVector, int length, Color color, float width)
    {
        Debug.Log("Player " + playerIndex + " is shooting ray");
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, startPosition + length * (directionVector));
    }

    private Vector3 CalculateDirectionVectorWand(GameObject gripPoint, GameObject aimPoint)
    {
        return aimPoint.transform.position - gripPoint.transform.position;
    }


    private TiltFive.PlayerIndex GetPlayerIndex()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Player One") || gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Player One"))
        {
            return TiltFive.PlayerIndex.One;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Player Two") || gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Player Two"))
        {
            return TiltFive.PlayerIndex.Two;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Player Three") || gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Player Three"))
        {
            return TiltFive.PlayerIndex.Three;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Player Four") || gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Player Four"))
        {
            return TiltFive.PlayerIndex.Four;
        }
        else
        {
            return TiltFive.PlayerIndex.One;
        }
    }
}
