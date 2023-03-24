using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderLabel : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderText;
    Mind instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = Mind.instance;
        slider.value = instance.sensitivity;
        sliderText.text = slider.value.ToString("0.00");
        slider.onValueChanged.AddListener((e) => {
            instance.sensitivity = e;
            sliderText.text = e.ToString("0.00");
        });
    }
}
