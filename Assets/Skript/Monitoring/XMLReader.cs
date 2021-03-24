using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

/// <summary>
/// Helper Class to load XMLFiles, and get informations out of XMLFiles
/// </summary>

public class XMLReader  {

    XmlDocument moduleDataXml; //variable that the XMLFile gets loaded into 

	/// <summary>
    /// loads the XML with "nameOfXml" from the Folder Ressource/XML
    /// </summary>
    /// <param name="nameOfXml"> contains the name of the XMLFile to load</param>
    public void loadXml(string nameOfXml)
    {
        TextAsset xmlTextAsset = Resources.Load<TextAsset>("XML/" + nameOfXml);
        moduleDataXml = new XmlDocument();
        moduleDataXml.LoadXml(xmlTextAsset.text);
    }

    /// <summary>
    /// searches for the Component with the given ID , expenses and returns the unique Attribute as a String
    /// </summary>
    /// <param name="ID"> ID of the Component e.g. 0,1... </param>
    /// <param name="expenses"> "inst" for installation-expenses , "deinst" for deinstallation-expenses</param>
    /// <param name="uniqueAttribute"> name of the Attribute you want to get, "zeit" for the Time, "kosten" for the Costs</param>
    /// <returns></returns>
    public string getComponentValue(int ID, string expenses, string uniqueAttribute)
    {
        XmlNode xmlNode = moduleDataXml.SelectSingleNode("/Modul/Komponente[@ID='" + ID + "']"); //select the first Node with the given ID
        if (expenses == "inst")
        {
            xmlNode = xmlNode.FirstChild; // get the first child of xmlNode
        } else if (expenses == "deinst")
        {
            xmlNode = xmlNode.LastChild; // get the last child of xmlNode
            
        }
        if(uniqueAttribute == "zeit")
        {
            return xmlNode.Attributes["Zeit"].Value; // return the Value for "Zeit"
        }
        else
        {
            return xmlNode.Attributes["Kosten"].Value; // retunr the Value for "Kosten"
        }
    }

    /// <summary>
    /// get the StandbyPower of one Module
    /// </summary>
    /// <param name="ID"> ID of the Moduleconfiguration</param>
    /// <returns> STanby power of the given module</returns>
    public string getModuleStandbyPower(int ID)
    {
        XmlNode xmlNode = moduleDataXml.SelectSingleNode("/Modul/Modulkonfiguration[@ID = '" + ID + "']");
        xmlNode = xmlNode.FirstChild;
        return xmlNode.Attributes["PStandby"].Value;
    }

    /// <summary>
    /// gets the formula for either the service power or the service time of a service out of the opened XMLFile
    /// </summary>
    /// <param name="ID"> module ID</param>
    /// <param name="serviceName"> service name</param>
    /// <param name="formulaName"> PService for power, TService for time</param>
    /// <returns>formula for either the power or the time of a service </returns>
    public string getModuleFormula(int ID, string serviceName, string formulaName)
    {
        Debug.Log("ID: " + ID + "SErvicename + " + serviceName + "formulaName: " + formulaName);
        XmlNode xmlNode = moduleDataXml.SelectSingleNode("/Modul/Modulkonfiguration[@ID = '" + ID + "']");
        xmlNode = xmlNode.FirstChild;

        for(int i = 0; i<10; i++)
        {
            
            if(xmlNode.Name != "NichtFunktionale_Parameter")
            {
                if (xmlNode.Attributes["Name"].Value == serviceName)
                {
                    break;
                }
            }
            
            xmlNode = xmlNode.NextSibling;
        }

        if (formulaName == "TService")
        {
            xmlNode = xmlNode.FirstChild;
        }
        else
        {
            xmlNode = xmlNode.LastChild;
        }
        return xmlNode.Attributes["Formel"].Value;
    }

    /// <summary>
    /// searches for the Component List of the Module with the given ID
    /// </summary>
    /// <param name="ModuleID"> the ID of the Module you want the Component lost from</param>
    /// <returns> Component List of the Module with the given ID e.g. "1010001"</returns>
    public string getComponentList(int ModuleID)
    {
        XmlNode firstChild = moduleDataXml.SelectSingleNode("/Modul/Modulkonfiguration[@ID = '" + ModuleID + "']"); // get the first Child-Node with the given ID
        Debug.Log(firstChild.Attributes["Komponentenliste"].Value); 
        return firstChild.Attributes["Komponentenliste"].Value; // returns the value from "Komponentenliste"
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="iD"></param>
    /// <returns></returns>
    public string getNodeInformation(string iD)
    {
        XmlNode curNode = moduleDataXml.SelectSingleNode(iD);
        if(curNode == null)
        {
            Debug.LogError("Error, could not find Node with ID: " + iD);
            return null;
        }
        Debug.Log(curNode.InnerText);
        return curNode.InnerText;
    }

    /// <summary>
    /// converts the enum of a module to its XML-File Name
    /// </summary>
    /// <param name="module"> enum of a module</param>
    /// <returns>name of the XML-File</returns>
    public string getTypeOfModule(ProductionModule module)
    {
        if ((module == ProductionModule.ModulBohrenA) || (module == ProductionModule.ModulBohrenB) ||
            (module == ProductionModule.ModulBohrenC) || (module == ProductionModule.ModulBohrenD) ||
            (module == ProductionModule.ModulBohrenE))
        {
            return "Bohren_Modul";
        }
        
        else if (module == ProductionModule.ModulStapelMagazin)
        {
            return "StapelMagazin_Modul";
        }
        else if(module == ProductionModule.ModulSenke)
        {
            return "Senke_Modul";
        }
        else if (module == ProductionModule.ModulFraesenA || module == ProductionModule.ModulFraesenB || module == ProductionModule.ModulFraesenC)
        {
            return "Fraesen_Modul";
        }
        else if (module == ProductionModule.ModulPruefenA || module == ProductionModule.ModulPruefenB)
        {
            return "Pruefen_Modul";
        }
        else if (module == ProductionModule.ModulLackierenA || module == ProductionModule.ModulLackierenB)
        {
            return "Lackieren_Modul";
        }
        else if (module == ProductionModule.ModulStanzenA || module == ProductionModule.ModulStanzenB )
        {
            return "Stanzen_Modul";
        }
        else if (module == ProductionModule.ModulBohrenFraesenA || module == ProductionModule.ModulBohrenFraesenB)
        {
            return "BohrenFraesen_Modul";
        }
        else if (module == ProductionModule.ModulStanzenPruefenA || module == ProductionModule.ModulStanzenPruefenB || module == ProductionModule.ModulStanzenPruefenC || module == ProductionModule.ModulStanzenPruefenD)
        {
            return "Stanzen_Pruefen_Modul";
        }
        else
        {
            return "Logistik_Modul";
        }

    }
}
