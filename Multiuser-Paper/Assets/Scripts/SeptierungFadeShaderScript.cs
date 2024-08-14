using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeptierungFadeShaderScript : MonoBehaviour
{
    public TiltFive.PlayerIndex playerIndex;
    public MudRendererCommunication rendererCommunication;
    public bool isPlaying;

    // Start is called before the first frame update
    void Start()
    {
        playerIndex = GetPlayerIndex();
        rendererCommunication = FindObjectOfType<MudRendererCommunication>(false);
    }

    // Update is called once per frame
    void Update()
    {
        isPlaying = rendererCommunication.isAutomaticPlaying;
        TiltFive.Glasses.TryGetPose(playerIndex, out Pose pose);
        gameObject.GetComponent<Renderer>().material.SetVector("_cameraVector", pose.position);
        gameObject.GetComponent<Renderer>().material.SetInt("_isPlaying", Convert.ToInt32(isPlaying));
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
