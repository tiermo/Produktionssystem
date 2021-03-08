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
}
