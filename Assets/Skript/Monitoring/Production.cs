using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Production : MonoBehaviour
{

    private double energy;
    private ProductionModule activeModule;
    private bool graphMode;
    private Configuration productionConfig = new Configuration();

    // Use this for initialization
    void Start()
    {
        graphMode = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void startMonitoring(Configuration config)
    {
        productionConfig = config;
    }

    public void changeActiveModule(ProductionModule module)
    {
        activeModule = module;
    }

    public void changeMonitoringMode()
    {
        if (graphMode == false)
        {
            graphMode = true;
        }
        else
        {
            graphMode = false;
        }
    }

    private string transformTime()
    {
        int minutes = Mathf.FloorToInt(Time.time / 60F);
        int seconds = Mathf.FloorToInt(Time.time - minutes * 60);
        string niceSeconds = string.Format("{0:0}:{1:00}", minutes, seconds);
        return niceSeconds;
    }

    private void displayTime()
    {
        // timeText.text = "Time: " + "\t" + "\t" + transformTime();
    }

    private double calculateEnergy()
    {
        //hier Energieberechnung einfügen 

        return energy;
    }

    private double calculateCosts()
    {
        double costsPerSeconds = energy * 0.23;
        return costsPerSeconds;
    }

    private void displayGraphs()
    {

    }
}