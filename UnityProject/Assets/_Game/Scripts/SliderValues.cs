using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValues : MonoBehaviour
{
    [SerializeField] private Text txt = null;
    [SerializeField] private string textToShow;
    private Slider slider = null;
    
    void Awake ()
    {
        TryGetComponent(out slider);
        ShowSliderValue();
    }

    public void ShowSliderValue () {
        string sliderMessage = textToShow + ": " + slider.value.ToString("n3");
        txt.text = sliderMessage;
    }

}
