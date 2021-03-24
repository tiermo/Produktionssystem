using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;
using System.Threading;

public class Reconfiguration 
{

    private int time;
    private int costs;
    private int energy;
    private Configuration configOne = new Configuration();
    private Configuration configTwo = new Configuration();
    private GameObject test = new GameObject();
    private XMLReader xmlReader = new XMLReader();
    
    private int removedPMs;
    private int addedPMs;
    private int timeForConfiguratedPMs;
    private int differentLMs;

    private ToggleReader toggleTime;
    private ToggleReader toggleCosts;
    private ToggleReader toggleEnergy;



    public void startReconfiguration(Configuration Config)
    {
        toggleTime = GameObject.Find("Checkbox-Zeit").GetComponent<ToggleReader>();
        toggleCosts = GameObject.Find("Checkbox-Kosten").GetComponent<ToggleReader>();
        toggleEnergy = GameObject.Find("Checkbox-Energie").GetComponent<ToggleReader>();
        

        configOne = Config;
     
    }


    public void endReconfiguration(Configuration config)
    {
        
        configTwo = config;
        compareConfig();
    }

    private int calculateTime()
    {
        time = addedPMs * 60 + removedPMs * 30 + timeForConfiguratedPMs + differentLMs *45;
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
    /// writes the differences in removedPMs, addedPms, timeForConfiguratedPMs and differentLMs.
    /// </summary>
    private void compareConfig()
    {
        removedPMs = 0;
        addedPMs = 0;
        timeForConfiguratedPMs = 0;
        differentLMs = 0;
        
        timeForConfiguratedPMs = compareProductionModules();
        compareLogistikModules("OLM");
        compareLogistikModules("BLM");
        displayValues();
        
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
            startLMs = configOne.getBiDirectionalLMs();
            finalLMs = configTwo.getBiDirectionalLMs();
           
        }
        else
        {
            startLMs = configOne.getOmniDirectionalLMs();
            finalLMs = configTwo.getOmniDirectionalLMs();
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
    private int compareProductionModules()
    {
        ProductionModule[] startModules = configOne.getProductionModules();
        ProductionModule[] finalModules = configTwo.getProductionModules();
        int installTime = 0;

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
                xmlReader.loadXml(xmlReader.getTypeOfModule(startModules[i]));          
                installTime += compareSingleModules(startModules[i], finalModules[i]);
            }
            else if (d != 0 && d >= 10 || d <= -10)
            {
                addedPMs++;
                removedPMs++;
            }
        }
        return installTime;
    }

    /// <summary>
    /// comnpares two modules and calculates the install time
    /// </summary>
    /// <param name="startModule"> enum of the module that was removed</param>
    /// <param name="finalModule"> enum of the module that replaces the removed module</param>
    /// <returns> install time in minutes</returns>
    private int compareSingleModules( ProductionModule startModule, ProductionModule finalModule)
    {
       
        string componentList1 = xmlReader.getComponentList((int)startModule);
        string componentList2 = xmlReader.getComponentList((int)finalModule);
        return compareComponentLists(componentList1, componentList2);
    }

    private int compareComponentLists(string componentList1,string componentList2 )
    {
        char[] list1;
        char[] list2;
        list1 = componentList1.ToCharArray(0, componentList1.Length);
        list2 = componentList2.ToCharArray(0, componentList2.Length);
        int installTime = 0;
        string value;
        for (int i =0; i<list1.Length; i++)
        {
            if (list1[i] != list2[i])
            {
                if (list1[i] == '0')
                {
                    value = xmlReader.getComponentValue(i+1, "inst", "zeit");
                }
                else
                {
                    value = xmlReader.getComponentValue(i+1, "deinst", "zeit");
                }
                installTime += System.Convert.ToInt32(value);                
            }
        }
        return installTime;
    }


    
    /// <summary>
    /// displays the time, costs and energy needed for the reconfiguration depending on the state of the assosiated toggle
    /// </summary>
    private void displayValues()
    {
        DisplayTime displayTime = GameObject.Find("Zeit").GetComponent<DisplayTime>();
        DisplayCosts displayCosts = GameObject.Find("Kosten").GetComponent<DisplayCosts>();
        DisplayEnergy displayEnergy = GameObject.Find("Energie").GetComponent<DisplayEnergy>();

        if (toggleTime.getToggleValueTime())
        {
            displayTime.displayTime(calculateTime());
        }
        else
        {
            displayTime.deleteTimeText();
        }
        if (toggleCosts.getToggleValueCosts())
        {
            calculateTime();
            displayCosts.displayCosts(calculateCosts());
        }
        else
        {
            displayCosts.deleteCostsText();
        }
        if (toggleEnergy.getToggleValueEnergy())
        {
            displayEnergy.displayEnergy(calculateEnergy());
        }
        else
        {
            displayEnergy.deleteEnergyText();
        }


    }

}