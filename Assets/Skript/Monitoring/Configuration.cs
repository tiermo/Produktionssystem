using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enthält alle Produktionsmodule inklusive Konfigurationsalternativen
/// </summary>
public enum ProductionModule
{
    KeinModul = 0,
    ModulStapelMagazin = 100,
    ModulBohrenA = 200,
    ModulBohrenB= 201,
    ModulBohrenC = 202,
    ModulFraesenA = 300,
    ModulFraesenB = 301,
    ModulFraesenC = 302,
    ModulMessen = 400,
    ModulErkennen = 500,
    ModulStanzenA = 600,
    ModulStanzenB = 601,
    ModulStanzenC = 602,
    ModulBohrenFraesenA = 700,
    ModulBohrenFraesenB = 701,
    ModulBohrenFraesenC = 702,
    ModulStanzenMessenA = 800,
    ModulStanzenMessenB = 801,
    ModulStanzenMessenC = 802,
    ModulSenke = 900
}

public class Configuration : MonoBehaviour {
    private bool[] biDirectionalLMs = new bool[15];
    private bool[] omniDirectionalLMs = new bool[14];
    private ProductionModule[] productionModules = new ProductionModule[13];

    // Use this for initialization
    void Start () {
        productionModules[0] = ProductionModule.ModulStapelMagazin;
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void setProductionModules(int location, ProductionModule module)
    {
        productionModules[location] = module;
    }

    public ProductionModule[] getProductionModules()
    {
        return productionModules;
    }

    public void setBiDirectionalLMs(int location, bool operation)
    {
        biDirectionalLMs[location] = operation;
    }

    public bool[] getBiDirectionalLMs()
    {
        return biDirectionalLMs;
    }

    public void setOmniDirectionalLMs(int location, bool operation)
    {
        omniDirectionalLMs[location] = operation;
    }

    public bool[] getOmniDirectionalLMs()
    {
        return omniDirectionalLMs;
    }


}
