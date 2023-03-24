using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderLabel : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderText;

    // Start is called before the first frame update
    void Start()
    {
        slider.onValueChanged.AddListener((e) => {
            sliderText.text = e.ToString("0.00");
        });
    }
}
