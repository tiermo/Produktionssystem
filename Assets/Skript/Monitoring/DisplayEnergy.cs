using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayEnergy : MonoBehaviour {

    public TMP_Text energyText;

    public void Start()
    {
        energyText = GameObject.Find("Energie").GetComponent<TMP_Text>();
    }

    public void displayEnergy(int energy)
    {
        //energyText.text = energy + "kWh";
        energyText.text = "Energie" + "\t" + "kWh";
    }

    public void deleteEnergyText()
    {
        energyText.text = " ";
    }
}
