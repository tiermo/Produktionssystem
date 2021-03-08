using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayCosts : MonoBehaviour {

    
    public TMP_Text costsText;

    public void Start()
    {
        costsText = GameObject.Find("Kosten").GetComponent<TMP_Text>();
    }


    public void displayCosts(int costs)
    {
      costsText.text = "Kosten" + "\t" + "\t" + costs + "€";   
    }

    public void deleteCostsText()
    {
        costsText.text = " ";
    }
}
