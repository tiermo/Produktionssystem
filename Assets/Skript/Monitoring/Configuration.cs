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
    ModulBohrenD = 203,
    ModulBohrenE = 204,
    ModulFraesenA = 300,
    ModulFraesenB = 301,
    ModulFraesenC = 302,
    ModulPruefenA = 400,
    ModulPruefenB = 401,
    ModulLackierenA = 500,
    ModulLackierenB = 501,
    ModulStanzenA = 600,
    ModulStanzenB = 601,
    ModulStanzenC = 602,
    ModulBohrenFraesenA = 700,
    ModulBohrenFraesenB = 701,
    ModulStanzenPruefenA = 800,
    ModulStanzenPruefenB = 801,
    ModulStanzenPruefenC = 802,
    ModulStanzenPruefenD = 803,
    ModulSenke = 900
}

public class Configuration  {
    private bool[] biDirectionalLMs = new bool[15];
    private bool[] omniDirectionalLMs = new bool[14];
    private ProductionModule[] productionModules = new ProductionModule[14];

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

    public Configuration copy()
    {
        Configuration copy = new Configuration();
        copy.biDirectionalLMs = (bool[])this.biDirectionalLMs.Clone();
        copy.omniDirectionalLMs = (bool[])this.omniDirectionalLMs.Clone();
        copy.productionModules = (ProductionModule[])this.productionModules.Clone();
        return copy;
    }


}
