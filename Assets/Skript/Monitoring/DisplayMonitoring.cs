using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity;

public class DisplayMonitoring : MonoBehaviour {
    public TMP_Text monitoringText;
    private double energy;
    private double costs;
    private double time;
    private double energyPerSecond;
    private string activeModule = "keinModul";
    private bool resetFlag = false;

    private bool monitoringStatus = false;

	// Use this for initialization
	void Start () {
        monitoringText = GameObject.Find("MonitoringText").GetComponent<TMP_Text>();
       
	}
	
	// Update is called once per frame
	void Update () {
        displayMonitoringText(time, energy, costs, activeModule);
        
	}

    private void FixedUpdate()
    {
        if (resetFlag == true)
        {
            time = 0.0;
            energy = 0.0;
            costs = 0.0;
            resetFlag = false;
        }
        else if (monitoringStatus == true)
        {
            time += 0.02;
            energy += energyPerSecond/50;
            costs = energy * 0.23;
        }

    }

    public void resetMonitoring()
    {
        resetFlag = true;
    }
    /// <summary>
    /// displays the time, energy and the costs of the system in the simulation
    /// </summary>
    /// <param name="timeToDisplay"> time you want to display in seconds</param>
    /// <param name="energyToDisplay">energy you want to display in kWh</param>
    /// <param name="costsToDisplay">costs you want to display in €</param>
    /// <param name="activeModule"> name of the currently active module</param>
    public void displayMonitoringText(double timeToDisplay, double energyToDisplay, double costsToDisplay, string activeModule)
    {

        monitoringText.text = "Zeit" + "\t" + "\t" + transformTime(timeToDisplay) + " min" + "\n" +
                              "Energie" + "\t" + energyToDisplay.ToString(format: "0.000") + " kWh" + "\n" +
                              "Kosten" + "\t" + costsToDisplay.ToString(format: "0.00") + " €" + "\n";
                               
    }
    /// <summary>
    /// transforms time from seconds into minutes:seconds
    /// </summary>
    /// <param name="timeToTransform">time you want to transform in seconds</param>
    /// <returns> string with minutes:seconds e.g. 12:09</returns>
    private string transformTime(double timeToTransform)
    {
        int minutes = (int)timeToTransform / 60;
        int seconds = (int)timeToTransform - minutes * 60;
        string niceSeconds = string.Format("{0:0}:{1:00}", minutes, seconds);
        return niceSeconds;
    }

    public void setEnergy(double newEnergy)
    {
        energyPerSecond = newEnergy;
        
    }

    public void setMonitoringStatus(bool newStatus)
    {
        monitoringStatus = newStatus;
    }

    public void setActiveModule(string module)
    {
        activeModule = module;
    }
}
