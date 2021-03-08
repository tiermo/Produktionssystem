using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleReader : MonoBehaviour {

    Toggle toggleTime;
    Toggle toggleCosts;
    Toggle toggleEnergy;
    bool toggleTimeStatus;
    bool toggleCostsStatus;
    bool toggleEnergyStatus;

    private void Start()
    {
        toggleTime = GameObject.Find("Checkbox-Zeit").GetComponent<Toggle>();
        toggleCosts = GameObject.Find("Checkbox-Kosten").GetComponent<Toggle>();
        toggleEnergy = GameObject.Find("Checkbox-Energie").GetComponent<Toggle>();
    }

    // Update is called once per frame
    void Update () {


		
	}

    public void setToggleTimeValue()
    {
        toggleTimeStatus = toggleTime.isOn;
        
    }
    public void setToggleCostsValue()
    {
        
        toggleCostsStatus = toggleCosts.isOn;
        Debug.Log(toggleCostsStatus);
    }
    public void setToggleEnergyValue()
    {
        toggleEnergyStatus = toggleEnergy.isOn;
    }

    public bool getToggleValueTime()
    {
        
        return toggleTimeStatus;
    }
    public bool getToggleValueCosts()
    {
        return toggleCostsStatus;
    }
    public bool getToggleValueEnergy()
    {
        return toggleEnergyStatus;
    }
}
