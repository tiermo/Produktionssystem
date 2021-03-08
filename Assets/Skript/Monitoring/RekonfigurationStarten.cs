using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RekonfigurationStarten : MonoBehaviour {

    

    /// <summary>
    /// starts the Reconfiguration process when startReconfiguration is pushed
    /// </summary>
    public void startReconfiguration()
    {
       
        ConfigManager.onStartReconfig();
        
    }

    /// <summary>
    /// ends the Reconfiguration process whe endReconfiguration is pushed
    /// </summary>
    public void endReconfiguration()
    {
        
        ConfigManager.onEndConfig();
        
    }
}
