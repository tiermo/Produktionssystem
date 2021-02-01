using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : MonoBehaviour {
    private Configuration startConfig = new Configuration();
    private Configuration finalConfig = new Configuration();
    private Production production = new Production();
    private Reconfiguration reconfiguration = new Reconfiguration();
    private Configuration configToChange = new Configuration();

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onStartReconfig() //pass the startConfig to Reconfiguration
    {
        startConfig = configToChange;
        pushConfig(startConfig);
    }

    
    public void onEndConfig() //pass the finalConfig to Reconfiguration and Production
    {
        finalConfig = configToChange;
        pushConfig(finalConfig);
    }

    /// <summary>
    /// Changes something in the current configuration
    /// </summary>
    /// <param name="nameOfFunction">"setPMs" for PMs, "setBLMs" for BLMs, "setOLMs for OLMs </param>
    /// <param name="location">Array position</param>
    /// <param name="module">Name of the Production Module</param>
    /// <param name="operation">"true" for existing LM, "false" for missing LM</param>
    public void changeConfig(string nameOfFunction, int location, ProductionModule module, bool operation)
    {
        if(nameOfFunction == "setPMs")
        {
            configToChange.setProductionModules(location, module);
        }else if(nameOfFunction == "setBLMs")
        {
            configToChange.setBiDirectionalLMs(location, operation);
        }else if(nameOfFunction == "setOLMs")
        {
            configToChange.setOmniDirectionalLMs(location, operation);
        }
    }

    /// <summary>
    /// Passes configuration to Production and/or Reconfiguration
    /// </summary>
    /// <param name="configToPush"></param>
    private void pushConfig(Configuration configToPush)
    {
        if(configToPush == startConfig)
        {
            reconfiguration.startReconfiguration(configToPush);
        }else if(configToPush == finalConfig)
        {
            reconfiguration.endReconfiguration(configToPush);
            production.startMonitoring(configToPush);
        }
    }


}
