using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sliderScript : MonoBehaviour
{

    public Slider slider;       //Referenve to slider
    public Text myText;         //Reference to time scale text

    void Start()
    {
        //Setting the slider to 1f initially
        slider.value = 1f;
    }

    void Update()
    {
        //Getting the slider value
        SliderControl(slider.value);

        //Adjusting the time scale text
        myText.text = "Time Scale : " + slider.value.ToString("f2");
    }

    //Function to find the slider value 
    public void SliderControl(float val)
    {
        Time.timeScale = val;
    }
}
