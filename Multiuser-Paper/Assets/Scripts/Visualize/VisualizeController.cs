using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public TiltFive.PlayerIndex playerIndex;

    private bool isTracked;
    private bool lastIsTracked;
    public List<GameObject> buttonList;

    public Material pressedMaterial;
    private Material plasticMaterial;

    // Start is called before the first frame update
    void Start()
    {
        playerIndex = GetPlayerIndex();
    }

    // Update is called once per frame
    void Update()
    {
        lastIsTracked = isTracked;
        isTracked = TiltFive.Wand.IsTracked(playerIndex: playerIndex);
        if (lastIsTracked != isTracked)
            VisualizeWand(isTracked);

        if (isTracked)
        {
            
        }
    }

    public void VisualizeWand(bool isTracked)
    {
        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(isTracked);
        }
    }

    public void CheckIfButtonIsPressed(TiltFive.Input.WandButton button, int buttonNumber)
    {
        if (TiltFive.Input.TryGetButton(button, out bool buttonIsPressed, playerIndex: playerIndex))
        {
            SetColorButton(buttonIsPressed, buttonNumber);
        }
    }

    public void SetColorButton(bool buttonIsPressed_, int buttonNumber)
    {
        if (buttonIsPressed_)
            buttonList[buttonNumber].transform.GetChild(0).GetComponent<MeshRenderer>().material = pressedMaterial;
        else
            buttonList[buttonNumber].transform.GetChild(0).GetComponent<MeshRenderer>().material = plasticMaterial;
    }

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
