using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEditor;
using B83.ExpressionParser;

public class Production
{

    private double energyPerSecond;     //Energy that the production System uses per Second
    private double energyPerHour = 0;   //Energy that the production System uses per hour
    
    private bool graphMode;
    private bool monitoringStatus = false; // if true monitoring runs, if false monitoring pauses
    private Configuration productionConfig = new Configuration();
    

    private DisplayMonitoring monitoringText;

    private XMLReader xmlReader = new XMLReader();
    private ProductionModule[] productionModules;
    private bool[] blm;
    private bool[] olm;

    private double powerDifference = 0;
    private int numberOfActiveModules = 0;

    

    private ExpressionParser parser = new ExpressionParser();

    

    // Use this for initialization
    void Start()
    {
        // get the GameObject to display the monitoring on
        monitoringText = GameObject.Find("MonitoringText").GetComponent<DisplayMonitoring>();
        graphMode = false;
    }


    
    /// <summary>
    /// gets called if Reconfiguration fo the System is finished, gets the right config and calculates standbyPower of the config
    /// </summary>
    /// <param name="config"> the final config that is used for the production process</param>
    public void startMonitoring(Configuration config)
    {
        monitoringText = GameObject.Find("MonitoringText").GetComponent<DisplayMonitoring>();
        productionConfig = config;
        productionModules = productionConfig.getProductionModules();
        blm = productionConfig.getBiDirectionalLMs();
        olm = productionConfig.getOmniDirectionalLMs();
        energyPerHour = calculateStandbyPower();
        energyPerSecond = energyPerHour / 3600;
        
        monitoringText.setEnergy(energyPerSecond);

    }

    /// <summary>
    /// changes the energy that the system consumes per second, gets called if a service is started and if a service ends
    /// </summary>
    /// <param name="moduleStatus"> true for the start of a service, false for the end of a service</param>
    /// <param name="module"> the modulconfiguration that executes the service </param>
    /// <param name="serviceName"> name of the service that starts or ends</param>
    /// <param name="n"> can contain the "Drehzahl" of the service</param>
    /// <param name="l"> can contain the length and/or depth of the service</param>
    public void changeActiveModule(bool moduleStatus, ProductionModule module, string serviceName, string n, string l)
    {
        xmlReader.loadXml(xmlReader.getTypeOfModule(module));   // load the right xml file
        double pService = calculateServicePower(module, serviceName, n, l);

        string power = xmlReader.getModuleStandbyPower((int)module);
        double pStandby = Convert.ToDouble(power);

        powerDifference = pService - pStandby;  //calculate the difference in power that the activ modul has to its standby mode

        if (moduleStatus == true)
        {
            energyPerHour += powerDifference;
            numberOfActiveModules += 1;
            monitoringText.setMonitoringStatus(true);               // unpause the monitoring
            monitoringText.setActiveModule(module.ToString("g"));   //change the activeModule
        }
        else
        {
            energyPerHour -= powerDifference;
            numberOfActiveModules -= 1;
            if(numberOfActiveModules <= 0)
            {
                monitoringText.setActiveModule("kein Modul");
                monitoringText.setMonitoringStatus(false);      //pause the monitoring
            }
        }       
        
        energyPerSecond = energyPerHour / 3600;
        monitoringText.setEnergy(energyPerSecond); // set the energy which should be added per second
        
        return;
    }

    /// <summary>
    /// calculates the Power per hour of a Service
    /// </summary>
    /// <param name="module">the moduleconfiguartion the service belongs to</param>
    /// <param name="serviceName"> name of the service</param>
    /// <param name="n"> Drehzahl</param>
    /// <param name="l"> bohrtiefe oder frästiefe</param>
    /// <returns> power per hour of the service</returns>
    private double calculateServicePower(ProductionModule module, string serviceName, string n, string l)
    {
        string formula = xmlReader.getModuleFormula((int)module, serviceName, "PService");

        //Services with only one missing argument 
        if (module == ProductionModule.ModulBohrenA || module == ProductionModule.ModulBohrenB || module == ProductionModule.ModulBohrenC || module == ProductionModule.ModulBohrenD ||
            module == ProductionModule.ModulBohrenE || module == ProductionModule.ModulConveyorBelt  )
        {
            formula = formula.Replace("n", n);


            return parser.Evaluate(formula);
        }
        // Services with up to two missing arguments
        else if(module == ProductionModule.ModulBohrenFraesenA || module == ProductionModule.ModulBohrenFraesenB || module == ProductionModule.ModulFraesenA || module == ProductionModule.ModulFraesenB ||
                 module == ProductionModule.ModulFraesenC)
        {
            formula = formula.Replace("n", n);
            
            if (serviceName != "MetallBohrenA" && serviceName != "KunststoffBohrenA" && serviceName != "MetallBohrenD" && serviceName != "KunststoffBohrenD")
            {
                string[] lSplit = l.Split(" "[0]);
                formula = formula.Replace("a", lSplit[1]);
            }

            return parser.Evaluate(formula);
            
        }
        // Services with no missing arguments
        else if(module == ProductionModule.ModulLackierenA || module == ProductionModule.ModulLackierenB || module == ProductionModule.ModulPruefenA || module == ProductionModule.ModulPruefenB ||
                  module == ProductionModule.ModulStanzenA || module == ProductionModule.ModulStanzenB || module == ProductionModule.ModulStanzenPruefenA || module == ProductionModule.ModulStanzenPruefenB ||
                  module == ProductionModule.ModulStanzenPruefenC || module == ProductionModule.ModulStanzenPruefenD )
        {

            Debug.Log(parser.Evaluate(formula));
            return parser.Evaluate(formula);
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// starts, pauses or unpauses the monitoring
    /// </summary>
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

    /// <summary>
    /// calculates Costs per second 
    /// </summary>
    /// <returns> costs per second</returns>
    private double calculateCosts()
    {
        double costsPerSeconds = energyPerSecond * 0.23;
        return costsPerSeconds;
    }

    private void displayGraphs()
    {

    }

    public void setMonitoringStatus(bool newStatus)
    {
        monitoringText.setMonitoringStatus(newStatus);
    }

    /// <summary>
    /// calculates the StandbyPower of all the modules in the system
    /// </summary>
    /// <returns> standby power of all modules combined</returns>
    private double calculateStandbyPower()
    {
        double standbyPower = 0.0;
        for (int i = 0; i < productionModules.Length; i++)
        {
            if (productionModules[i] != ProductionModule.KeinModul)
            {
                
                xmlReader.loadXml(xmlReader.getTypeOfModule(productionModules[i]));
                string power = xmlReader.getModuleStandbyPower((int)productionModules[i]);
                double result = Convert.ToDouble(power);
                standbyPower += result;               
            }
        }
        for (int i = 0; i< blm.Length; i++)
        {
            if (blm[i] == true)
            {
                standbyPower += 0.01;
            }
        }
        for (int i = 0; i< olm.Length; i++)
        {
            if (olm[i] == true)
            {
                standbyPower += 0.01;
            }
        }
        return standbyPower;
    }
}