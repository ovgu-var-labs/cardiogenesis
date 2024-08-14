using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderNectScript : MonoBehaviour
{
    // Only used for one case

    public PinchSlider slider;

    public GameObject nextButton;
    public GameObject textField;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateNextButton()
    {
        if(slider.SliderValue >= 1)
        {
            nextButton.SetActive(true);
            slider.gameObject.SetActive(false);
            textField.SetActive(false);
        }

    }
}
