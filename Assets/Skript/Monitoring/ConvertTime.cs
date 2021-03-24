using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using B83.ExpressionParser;

public class ConvertTime  {

    private XMLReader xmlReader = new XMLReader();
    private ExpressionParser parser = new ExpressionParser(); 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public double calculateTimeDifference(ProductionModule module, string serviceName, string n, string l)
    {
        double timeDifference = 0;
        xmlReader.loadXml(xmlReader.getTypeOfModule(module));
        string formula = xmlReader.getModuleFormula((int)module, serviceName, "TService");

        if (module == ProductionModule.ModulBohrenA || module == ProductionModule.ModulBohrenB || module == ProductionModule.ModulBohrenC || module == ProductionModule.ModulBohrenD ||
            module == ProductionModule.ModulBohrenE || module == ProductionModule.ModulConveyorBelt)
        {
            formula = formula.Replace("n", n);
            formula = formula.Replace("l", l);
            timeDifference = parser.Evaluate(formula) - getActualTime(module, serviceName);
            return timeDifference;
        }
        else if (module == ProductionModule.ModulBohrenFraesenA || module == ProductionModule.ModulBohrenFraesenB || module == ProductionModule.ModulFraesenA || module == ProductionModule.ModulFraesenB ||
                 module == ProductionModule.ModulFraesenC)
        {
            formula = formula.Replace("n", n);
            if (serviceName != "MetallBohrenA" && serviceName != "KunststoffBohrenA" && serviceName != "MetallBohrenD" && serviceName != "KunststoffBohrenD")
            {
                string[] length = l.Split(" "[0]);
                formula = formula.Replace("l", length[0]);
            }
            else
            {
                formula = formula.Replace("l", l);
            }
            timeDifference = parser.Evaluate(formula) - getActualTime(module, serviceName);
            return timeDifference;

        }
        else if (module == ProductionModule.ModulLackierenA || module == ProductionModule.ModulLackierenB || module == ProductionModule.ModulPruefenA || module == ProductionModule.ModulPruefenB ||
                module == ProductionModule.ModulStanzenA || module == ProductionModule.ModulStanzenB || module == ProductionModule.ModulStanzenPruefenA || module == ProductionModule.ModulStanzenPruefenB ||
                module == ProductionModule.ModulStanzenPruefenC || module == ProductionModule.ModulStanzenPruefenD)
        {
            timeDifference = parser.Evaluate(formula) - getActualTime(module, serviceName);
            return timeDifference;
        }
        else
        {
            return 0;
        }
    }

    private double getActualTime(ProductionModule module, string service_Name)
    {
        if (module == ProductionModule.ModulBohrenA || module == ProductionModule.ModulBohrenB || module == ProductionModule.ModulBohrenC || module == ProductionModule.ModulBohrenD || module == ProductionModule.ModulBohrenE)
        {
            return 3.0;
        }
        else if(module == ProductionModule.ModulBohrenFraesenA|| module == ProductionModule.ModulBohrenFraesenB)
        {
           if (service_Name == "MetallBohrenA"|| service_Name == "KunststoffBohrenA" || service_Name == "MetallBohrenD" || service_Name == "KunststoffBohrenD")
            {
                return 3.0;
            }
            else
            {
                return 11.5;
            }
        }
        else if(module == ProductionModule.ModulFraesenA || module == ProductionModule.ModulFraesenB || module == ProductionModule.ModulFraesenC)
        {
            return 14.0;
        }
        else if(module == ProductionModule.ModulStanzenA|| module == ProductionModule.ModulStanzenB)
        {
            return 6.3;
        }
        else if(module == ProductionModule.ModulLackierenA || module == ProductionModule.ModulLackierenB)
        {
            return 3.1;
        }
        else
        {
            return 0;
        }
    }
}
