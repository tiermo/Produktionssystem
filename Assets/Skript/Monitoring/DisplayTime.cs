using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayTime : MonoBehaviour {
    public TMP_Text timeText;
    

    public void Start()
    {
        timeText = GameObject.Find("Zeit").GetComponent<TMP_Text>();
    }

    public void displayTime(int time)
    {
        timeText.text = "Zeit" + "\t" + "\t"  + makeTimeLookNice(time) + "h";
    }

    public void deleteTimeText()
    {
        timeText.text = " ";
    }

    private string makeTimeLookNice(int wholeTime)
    {
        int hours = wholeTime /60;
        int minutes = wholeTime - hours * 60;
        string niceMinutes = string.Format("{0:0}:{1:00}", hours, minutes);
        return niceMinutes;
    }
}
