using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ZeitDisplay : MonoBehaviour {

    public TMP_Text zeitText;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void displayZeit()
    {
        zeitText.text = "Köftespieß";
    }
}
