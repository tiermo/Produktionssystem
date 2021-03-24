using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurationHelper
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Helper Function that converts a given modul-name into the matching enum of the type ProductionModule
    /// </summary>
    /// <param name="modulName"> name of the module</param>
    /// <returns> enum that fits to the given name</returns>
    public ProductionModule getModuleName(string modulName)
    {
        string[] nameSplit;
        nameSplit = modulName.Split(" "[0]);

        if (nameSplit[0] == "Bohren")
        {
            if (nameSplit[1] == "A")
            {
                return ProductionModule.ModulBohrenA;
            }
            else if (nameSplit[1] == "B")
            {
                return ProductionModule.ModulBohrenB;
            }
            else if (nameSplit[1] == "C")
            {
                return ProductionModule.ModulBohrenC;
            }
            else if (nameSplit[1] == "D")
            {
                return ProductionModule.ModulBohrenD;
            }
            else
            {
                return ProductionModule.ModulBohrenE;
            }
        }
        else if (nameSplit[0] == "Bohren_Fraesen")
        {
            if (nameSplit[1] == "A")
            {
                return ProductionModule.ModulBohrenFraesenA;
            }
            else
            {
                return ProductionModule.ModulBohrenFraesenB;
            }
        }
        else if (nameSplit[0] == "Erkennen")
        {
            if (nameSplit[1] == "A")
            {
                return ProductionModule.ModulLackierenA;
            }
            else
            {
                return ProductionModule.ModulLackierenB;
            }
        }
        else if (nameSplit[0] == "Fraesen")
        {
            if (nameSplit[1] == "A")
            {
                return ProductionModule.ModulFraesenA;
            }
            else if (nameSplit[1] == "B")
            {
                return ProductionModule.ModulFraesenB;
            }
            else
            {
                return ProductionModule.ModulFraesenC;
            }
        }
        else if (nameSplit[0] == "Messen")
        {
            if (nameSplit[1] == "A")
            {
                return ProductionModule.ModulPruefenA;
            }
            else
            {
                return ProductionModule.ModulPruefenB;
            }
        }
        else if (nameSplit[0] == "Stanzen")
        {
            if (nameSplit[1] == "B")
            {
                return ProductionModule.ModulStanzenB;
            }
            else 
            {
                return ProductionModule.ModulStanzenA;
            }
           
        }
        else if (nameSplit[0] == "Stanzen_Pruefen")
        {
            if (nameSplit[1] == "A")
            {
                return ProductionModule.ModulStanzenPruefenA;
            }
            else if (nameSplit[1] == "B")
            {
                return ProductionModule.ModulStanzenPruefenB;
            }
            else if (nameSplit[1] == "C")
            {
                return ProductionModule.ModulStanzenPruefenC;
            }
            else
            {
                return ProductionModule.ModulStanzenPruefenD;
            }
        }
        else if (nameSplit[0] == "StapelMagazin")
        {
            return ProductionModule.ModulStapelMagazin;
        }
        else if (nameSplit[0] == "Senke")
        {
            return ProductionModule.ModulSenke;
        }
        else if (nameSplit[0] == "conveyorBelt")
        {
            return ProductionModule.ModulConveyorBelt;
        } else
        {
            return ProductionModule.KeinModul;
        }
    }

    /// <summary>
    /// Helper Function that converts a given name of a module into a service name ! only use this if you dont get a service name from the opc-ua network!
    /// </summary>
    /// <param name="modulName">name of the module</param>
    /// <returns>a service of the given  module</returns>
    public string getServiceName(string modulName)
    {
        string[] nameSplit;
        nameSplit = modulName.Split(" "[0]);

        if (nameSplit[0] == "Bohren")
        {
            if (nameSplit[1] == "A")
            {
                return "MetallBohrenA";
            }
            else if (nameSplit[1] == "B")
            {
                return "MetallBohrenB";
            }
            else if (nameSplit[1] == "C")
            {
                return "MetallBohrenC";
            }
            else if (nameSplit[1] == "D")
            {
                return "MetallBohrenD";
            }
            else
            {
                return "MetallBohrenE";
            }
        }
        else if (nameSplit[0] == "Bohren_Fraesen")
        {
            if (nameSplit[1] == "A")
            {
                return "MetallBohrenA";
            }
            else
            {
                return "MetallBohrenD";
            }
        }
        else if (nameSplit[0] == "Erkennen")
        {
            if (nameSplit[1] == "A")
            {
                return "LackierenSlow";
            }
            else
            {
                return "LackierenSlow";
            }
        }
        else if (nameSplit[0] == "Fraesen")
        {
            if (nameSplit[1] == "A")
            {
                return "PlanfraesenMetall";
            }
            else if (nameSplit[1] == "B")
            {
                return "MuldeFraesenMetallB";
            }
            else
            {
                return "MuldeFraesenMetallC";
            }
        }
        else if (nameSplit[0] == "Messen")
        {
            if (nameSplit[1] == "A")
            {
                return "LochPruefen";
            }
            else
            {
                return "LochPruefen";
            }
        }
        else if (nameSplit[0] == "Stanzen")
        {
            if (nameSplit[1] == "A")
            {
                return "MetallLogoStanzen";
            }

            else
            {
                return "MetallLochStanzen";
            }
        }
        else if (nameSplit[0] == "Stanzen_Pruefen")
        {
            if (nameSplit[1] == "A")
            {
                return "MetallLogoStanzen";
            }
            else if (nameSplit[1] == "B")
            {
                return "MetallLogoStanzen";
            }
            else if (nameSplit[1] == "C")
            {
                return "MetallLochStanzen";
            }
            else
            {
                return "MetallLochStanzen";
            }
        }
        else if (nameSplit[0] == "Senke")
        {
            return "Entnehmen";
        }
        else
        {
            return "Transport";
        }
    }
    /// <summary>
    /// Helper Function that gives you the right service parameters for a given service
    /// </summary>
    /// <param name="serviceName"> name of the service</param>
    /// <returns> the right parameters for the service</returns>
    public string getDrehzahl(string serviceName)
    {

        if (serviceName == "MetallBohrenA")
        {
            return "1600";
        }
        else if (serviceName == "MetallBohrenB")
        {
            return "3200";
        }
        else if (serviceName == "MetallBohrenC")
        {
            return "800";
        }
        else if (serviceName == "MetallBohrenD")
        {
            return "1000";
        }
        else if (serviceName == "MetallBohrenE")
        {
            return "1600";
        }



        else if (serviceName == "LackierenSlow")
        {
            return "0";
        }


        else if (serviceName == "PlanfraesenMetall")
        {
            return "2400";
        }
        else if (serviceName == "MuldeFraesenMetallB")
        {
            return "2400";
        }
        else if (serviceName == "MuldeFraesenMetallC")
        {
            return "1200";
        }

        else if (serviceName == "LochPruefen")
        {
            return "0";
        }


        else if (serviceName == "MetallLogoStanzen")
        {
            return "0";
        }

        else if (serviceName == "MetallLochStanzen")
        {
            return "0";
        }

        else
        {
            return "10";
        }
    }
    /// <summary>
    /// Helper function that gives you the right service parameter for a given service
    /// </summary>
    /// <param name="serviceName"> name of the service</param>
    /// <returns> parameter values for the service</returns>
    public string getLength(string serviceName)
    {
        if (serviceName == "MetallBohrenA")
        {
            return "40";
        }
        else if (serviceName == "MetallBohrenB")
        {
            return "40";
        }
        else if (serviceName == "MetallBohrenC")
        {
            return "40";
        }
        else if (serviceName == "MetallBohrenD")
        {
            return "40";
        }
        else if (serviceName == "MetallBohrenE")
        {
            return "40";
        }



        else if (serviceName == "LackierenSlow")
        {
            return "0";
        }


        else if (serviceName == "PlanfraesenMetall")
        {
            return "3600 5";
        }
        else if (serviceName == "MuldeFraesenMetallB")
        {
            return "60 5";
        }
        else if (serviceName == "MuldeFraesenMetallC")
        {
            return "60 5";
        }

        else if (serviceName == "LochPruefen")
        {
            return "0 0";
        }


        else if (serviceName == "LogoStanzen")
        {
            return "0";
        }

        else if (serviceName == "LochStanzen")
        {
            return "0";
        }

        else
        {
            return "4";
        }
    }
}
