using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reconfiguration : MonoBehaviour
{

    private int time;
    private int costs;
    private int energy;
    private Configuration startConfig = new Configuration();
    private Configuration finalConfig = new Configuration();
    private int removedPMs;
    private int addedPMs;
    private int configuratedPMs;
    private int differentLMs;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void startReconfiguration(Configuration config)
    {
        startConfig = config;
    }

    public void endReconfiguration(Configuration config)
    {
        finalConfig = config;
    }

    private int calculateTime()
    {
        time = addedPMs * 60 + removedPMs * 30 + configuratedPMs * 15 + differentLMs * 45;
        //Calculate the Reconfiguration Time here
        return time;
    }

    private int calculateCosts()
    {
        costs = (time * 33 / 100);
        return costs;
    }

    private int calculateEnergy()
    {
        //calculate the energy here
        return energy;
    }


    /// <summary>
    /// Compares startConfig with finalConfig, 
    /// writes the differences in removedPMs, addedPms, configuredPMs and differentLMs.
    /// </summary>
    private void compareConfig()
    {
        removedPMs = 0;
        addedPMs = 0;
        configuratedPMs = 0;
        differentLMs = 0;

        compareProductionModules();
        compareLogistikModules("OLM");
        compareLogistikModules("BLM");
    }

    /// <summary>
    /// Compares one of the arrays with logistics modules,
    /// writes the differences in differenLMs
    /// </summary>
    /// <param name="typeOfLM">BLM for Bidirectional LMs, OLM for Omnidirectional LMs</param> "ddd"

    private void compareLogistikModules(string typeOfLM)
    {
        bool[] startLMs;
        bool[] finalLMs;

        if ( typeOfLM == "BLM")
        {
            startLMs = startConfig.getBiDirectionalLMs();
            finalLMs = finalConfig.getBiDirectionalLMs();
        }
        else
        {
            startLMs = startConfig.getOmniDirectionalLMs();
            finalLMs = finalConfig.getOmniDirectionalLMs();
        }

        for (int i = 0; i < startLMs.Length; i++)
        {
            if (startLMs[i] != finalLMs[i])
            {
                differentLMs++;
            }
        }
        
    }

    /// <summary>
    /// Compares arrays of production modules of the given configurations
    /// </summary>
    private void compareProductionModules()
    {
        ProductionModule[] startModules = startConfig.getProductionModules();
        ProductionModule[] finalModules = finalConfig.getProductionModules();


        for (int i = 0; i < 13; i++)
        {
            int d = (int)startModules[i] - (int)finalModules[i];
            if (d == 0)
            {
                continue;
            }
            if ((int)startModules[i] == 0 && (int)finalModules[i] != 0)
            {
                addedPMs++;
            }
            else if ((int)startModules[i]!= 0 && (int)finalModules[i] == 0)
            {
                removedPMs++;
            }
            else if (d != 0 && -10 < d && d < 10)
            {
                configuratedPMs++;
            }
            else if (d != 0 && d >= 10 && d <= -10)
            {
                addedPMs++;
                removedPMs++;
            }
        }
    }

}