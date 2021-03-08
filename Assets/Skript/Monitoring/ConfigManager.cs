using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// handles the Configurations during the Reconfiguration and the Production phase
/// </summary>
public static class ConfigManager  {
    private static Configuration startConfig = new Configuration(); 
    private static Configuration finalConfig = new Configuration();
    private static Production production = new Production();
    private static Reconfiguration reconfiguration = new Reconfiguration();
    private static Configuration configToChange = new Configuration();

	
    /// <summary>
    /// executed when the User start the Reconfiguration
    /// pushes startConfig to Reconfiguration
    /// </summary>
    public static void onStartReconfig() //pass the startConfig to Reconfiguration
    {

        startConfig = configToChange.copy();
        //bool[] testArray = startConfig.getBiDirectionalLMs();
        //for (int i = 0; i < testArray.Length; i++)
        //{
        //    Debug.Log(testArray[i]);
        //}
        pushConfig(startConfig, "startConfig");
    }

    /// <summary>
    /// executed when the User is finished with Reconfiguration
    /// pushes finalConfig to Reconfiguration and Production
    /// </summary>
    public static void onEndConfig() //pass the finalConfig to Reconfiguration and Production
    {
        finalConfig = configToChange.copy();
        pushConfig(finalConfig, "finalConfig");
    }

    /// <summary>
    /// Changes something in the current configuration
    /// </summary>
    /// <param name="nameOfFunction">"setPMs" for PMs, "setBLMs" for BLMs, "setOLMs for OLMs </param>
    /// <param name="location">Array position</param>
    /// <param name="module">Name of the Production Module</param>
    /// <param name="operation">"true" for existing LM, "false" for missing LM</param>
    public static void changeConfig(string nameOfFunction, string location, ProductionModule module, bool operation)
    {
        if(nameOfFunction == "PM")
        {
            configToChange.setProductionModules(convertModulePosition(location, nameOfFunction), module);            
        }else if(nameOfFunction == "BLM")
        {
            configToChange.setBiDirectionalLMs(convertModulePosition(location, nameOfFunction), operation);
        }else if(nameOfFunction == "OLM")
        {
            configToChange.setOmniDirectionalLMs(convertModulePosition(location, nameOfFunction), operation);
        }
        
    }

    /// <summary>
    /// Passes configuration to Production and/or Reconfiguration
    /// </summary>
    /// <param name="configToPush"></param>
    private static void pushConfig(Configuration configToPush, string nameOfConfig)
    {
        if(nameOfConfig == "startConfig")
        {
            
            reconfiguration.startReconfiguration(configToPush);
        }else if(nameOfConfig == "finalConfig")
        {
           
            reconfiguration.endReconfiguration(configToPush);
            //production.startMonitoring(configToPush);
        }
    }

    /// <summary>
    /// Converts the name of the Module-position from its "Unity-Name" to the number of the position its in
    /// </summary>
    /// <param name="position"> "Unity-name" of the Module-Position</param>
    /// <param name="nameOfFunction"> "PM" for Production-Module, "BLM" for Bidirectional Logistikmodule, "OLM" for Omnidirectional Logistikmodule</param>
    /// <returns> Module Position as a Number; PM: [0,13], BLM [0,13], OLM [0,8]</returns>
    private static int convertModulePosition(string position, string nameOfFunction)
    {
        int realPosition = -1;

        if (nameOfFunction == "PM")
        {
            string[] modulePosition = { "Modul120", "Modul212", "Modul252", "Modul520",
                                        "Modul522", "Modul524", "Modul412", "Modul452",
                                        "Modul920", "Modul922", "Modul924", "Modul612",
                                        "Modul652", "Modul122" };
            realPosition = Array.IndexOf(modulePosition, position); // gets the Number of the Position the given "position" has in the Array
        }
        else if (nameOfFunction == "BLM")
        {
            string[] BLMPosition = { "Conveyor10", "Conveyor21", "Conveyor23", "Conveyor30",
                                     "Conveyor32", "Conveyor34", "Conveyor41", "Conveyor43",
                                     "Conveyor50", "Conveyor52", "Conveyor54", "Conveyor61",
                                     "Conveyor63", "Conveyor12" };
            realPosition = Array.IndexOf(BLMPosition, position);
        }
        else if (nameOfFunction == "OLM")
        {
            string[] OLMPosition = { "Omni20", "Omni22", "Omni24", "Omni40", "Omni42",
                                     "Omni44", "Omni60", "Omni62", "Omni64" };
            realPosition = Array.IndexOf(OLMPosition, position);
        }
       
        return realPosition;
    }


}
