using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StopWatch : MonoBehaviour
{
    public TMP_Text stopWatchText;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if ((time / 60) < 1)
        {
            if ((time % 60) < 10)
            {
                stopWatchText.text = "Time: 0" + (int)time;
            }
            else
            {
                stopWatchText.text = "Time: " + (int)time;
            }
        }
        else
        {
            int minute = (int)(time / 60);
            if ((time % 60) < 10)
            {
                stopWatchText.text = "Time: " + minute + ":0" + (int)(time % 60);
            }
            else
            {
                stopWatchText.text = "Time: " + minute + ":" + (int)(time % 60);
            }
        }
    }
}
