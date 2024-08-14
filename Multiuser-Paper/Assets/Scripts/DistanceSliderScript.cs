using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MixedReality.Toolkit.UX;


public class DistanceSliderScript : MonoBehaviour
{
    public Slider animationProgressSlider;
    public int colliderNumber;
    public BoxCollider boxCollider;
    private Vector3 sizeCollider;
    private Vector3 centerCollider;

    // Start is called before the first frame update
    void Start()
    {
        sizeCollider = boxCollider.size;
        centerCollider = boxCollider.center;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveSliderToPosition(int numberCollider)
    {
        animationProgressSlider.Value = (1 / 8) * numberCollider;
    }

    public void MoveSliderToPosition()
    {
        float newValue = 0.125f * colliderNumber;
        animationProgressSlider.Value = newValue;
        Debug.Log("Shooting ray.: new Value " + newValue);
    }

    public void MoveSliderToRelativePosition(RaycastHit hit)
    {
        float tmp = Mathf.InverseLerp(centerCollider.x - sizeCollider.x, centerCollider.x + sizeCollider.x, hit.transform.localPosition.x);
        Debug.Log("Slider left " + (centerCollider.x - sizeCollider.x) + " right " + (centerCollider.x + sizeCollider.x) + " hit " + hit.point.x);
        // float xValueHit = sizeCollider.x / 2 + hit.transform.localPosition.x;
        // animationProgressSlider.Value = xValueHit / sizeCollider.x;
        animationProgressSlider.Value = tmp;
    }
}
