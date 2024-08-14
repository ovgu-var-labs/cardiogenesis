using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltTipDIY : MonoBehaviour
{
    public bool hasLine;

    public LineRenderer lineRenderer;
    public Material lineMaterial;
    public float width;
    public TiltFive.PlayerIndex playerIndex;
    public GameObject BottomAnkerPointAnnotation;
    public GameObject TopAnkerPointAnnotation;
    public GameObject ankerPointTarget;

    public GameObject hintObject;

    public string annotationText;

    

    public AnkerPoint ankerPoint;

    public enum AnkerPoint
    {
        Top,
        Bottom
    }

    // Start is called before the first frame update
    void Start()
    {
        playerIndex = gameObject.transform.parent.parent.GetComponentInParent<PlayerManager>().GetPlayerIndex();
        gameObject.GetComponentInChildren<TextMeshPro>().text = annotationText;
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (!hasLine)
            lineRenderer.enabled = false;
        playerIndex = GetPlayerIndex();
        if (ankerPointTarget.transform.position.y >= 0)
            ankerPoint = AnkerPoint.Top;
        else
            ankerPoint = AnkerPoint.Bottom;
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowardPlayer();
        if(hasLine)
            UpdateLine();
    }

    private void RotateTowardPlayer()
    {
        // TiltFive.Glasses.TryGetPose(playerIndex, out Pose pose);
        Vector3 vectorCameraObject = Camera.main.transform.position - gameObject.transform.position;
        gameObject.transform.forward = Vector3.RotateTowards(gameObject.transform.forward, vectorCameraObject, 360f, 0f);
    }

    private void UpdateLine()
    {
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        if(ankerPoint == AnkerPoint.Bottom)
        {
            lineRenderer.SetPosition(0, BottomAnkerPointAnnotation.transform.position);
        }
        else if(ankerPoint == AnkerPoint.Top)
        {
            lineRenderer.SetPosition(0, TopAnkerPointAnnotation.transform.position);
        }
        else
        {
            lineRenderer.SetPosition(0, BottomAnkerPointAnnotation.transform.position);
        }
        
        lineRenderer.SetPosition(1, ankerPointTarget.transform.position);
    }

    private Vector3 InvertVector3(Vector3 v3)
    {
        return new Vector3(-v3.x, -v3.y, -v3.z);
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
