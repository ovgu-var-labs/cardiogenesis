using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RollOver
{
    X,
    Y,
    Z,
    XY,
    XZ,
    YZ,
    XYZ
}
/// <summary>
/// Heavily inspired by https://github.com/microsoft/MixedRealityToolkit-Unity/blob/main/Assets/MRTK/SDK/Features/UX/Scripts/Utilities/Billboard.cs
/// </summary>
public class RotateOver : MonoBehaviour
{
    public RollOver rollOver;

    public TiltFive.PlayerIndex playerIndex;

    // Start is called before the first frame update
    void Start()
    {
        playerIndex = GetPlayerIndex();
    }

    // Update is called once per frame
    void Update()
    {
        // TiltFive.Glasses.TryGetPose(playerIndex, out Pose pose);
        Vector3 directionToPlayer = Camera.main.transform.position - transform.position;

        switch (rollOver)
        {
            case RollOver.X:
                directionToPlayer.x = 0f;
                break;
            case RollOver.Y:
                directionToPlayer.y = 0f;
                break;
            case RollOver.Z:
                directionToPlayer.x = 0f;
                directionToPlayer.y = 0f;
                break;
            case RollOver.XY:
                break;
            case RollOver.XZ:
                directionToPlayer.x = 0f;
                break;
            case RollOver.YZ:
                directionToPlayer.y = 0f;
                break;
            default:
                break;
        }

        transform.rotation = Quaternion.LookRotation(-directionToPlayer);
    }



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
}
