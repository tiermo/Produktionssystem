using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class LoadStartConveyors : MonoBehaviour {


    private GameObject conveyor; //clone the instance of conveyor belt
    private Color originalcolorConveyor;  //save the original color of the gameobject

    private string localEulerAngles; //in order to change the rotation of conveyor belt

    private LayerMask mask; //Calculate the number of "Plane" layermask
    RaycastHit hit;

    private string CollidernameConveyor; //the name of colider
    private string Conveyorname;

    private int numConv = 0; //the number of conveyorbelt

    private GameObject omni;       //clone the instance of modul
    private Color originalcolorOmni;   //save the original color of the gameobject
    
    
    private string CollidernameOmni;   //the name of colider
    private string Omniname;       //this moduls's name
    private int numOmni = 0;           //count the number of modul

    private GameObject magazin; //clone the instance of modul
    private Color originalcolorMagazin;  //save the original color of the gameobject
    
    
    private string CollidernameMagazin; //the name of colider
    private string Magazinname;

    private int numMagazin = 0; //the number of modul

    private GameObject modulSenke; //clone the instance of modul
    private GameObject sonSenke;   //son object is arm of drilling machine
    private Color originalcolorSenke;  //save the original color of the gameobject
    private Color originalcolorSonSenke;

    private string CollidernameSenke; //the name of colider
    private string ModulnameSenke;

    private int numSenke = 0; //the number of modul

    private GameObject modul; //clone the instance of modul
    private GameObject son;   //son object is arm of drilling machine
    private Color originalcolor;  //save the original color of the gameobject
    private Color originalcolorSon;

    private string Collidername; //the name of colider
    private string Modulname;

    private int numBohren = 0; //the number of modul

    private int numStanzen = 0;

    public void Start()
    {
        
        StartCoroutine(loadConveyors());
        
    }
    private IEnumerator loadConveyors()
    {
        Vector3 firstConveyor = new Vector3(-12.9f, 0.0f, -25.6f);
        createStartConveyor("Conveyor10", firstConveyor);
        yield return new WaitForSeconds(3);
        Vector3 secondConveyor = new Vector3(-12.9f, 0.0f, -76.8f);
        createStartConveyor("Conveyor30", secondConveyor);
        yield return new WaitForSeconds(3);
        Vector3 thirdConveyor = new Vector3(-12.9f, 0.0f, -128.0f);
        createStartConveyor("Conveyor50", thirdConveyor);
        yield return new WaitForSeconds(3);
        Vector3 fourthConveyor = new Vector3(-12.9f, 0.0f, -178.2f);
        createStartConveyor("Conveyor70", fourthConveyor);
        yield return new WaitForSeconds(3);
        Vector3 firstOmni = new Vector3(-12.9f, 0.0f, -51.2f);
        createStartOmnis("Omni20", firstOmni);
        yield return new WaitForSeconds(3);
        Vector3 secondOmni = new Vector3(-12.9f, 0.0f, -102.4f);
        createStartOmnis("Omni20", secondOmni);
        yield return new WaitForSeconds(3);
        Vector3 thirdOmni = new Vector3(-12.9f, 0.0f, -153.6f);
        createStartOmnis("Omni20", thirdOmni);
        yield return new WaitForSeconds(3);
        Vector3 stapelMag = new Vector3(-1.9f, 0.0f, -25.4f);
        createStartMagazin("Modul120", stapelMag);
        yield return new WaitForSeconds(3);
        Vector3 senke = new Vector3(-1.9f, 0.0f, -178.0f);
        createStartSenke("Modul720", senke);
        yield return new WaitForSeconds(3);
        Vector3 bohren = new Vector3(-1.9f, 0.0f, -76.6f);
        createStartBohren("Modul520", bohren);
        yield return new WaitForSeconds(3);
        Vector3 stanzen = new Vector3(-1.9f, 0.0f, -127.8f);
        createStartStanzen("Modul920", stanzen);
    }

    private void createStartConveyor(string hitCollidername, Vector3 conveyorPosition)
    {
        conveyor = Instantiate(Resources.Load("conveyorBelt")) as GameObject;  //clone Prefab from Folder "Resources"
        originalcolorConveyor = conveyor.GetComponent<MeshRenderer>().material.color;
        conveyor.GetComponent<MeshRenderer>().material.color = originalcolorConveyor;

        numConv++;
        conveyor.name = "conveyorBelt " + numConv.ToString();
        Conveyorname = conveyor.name;
        CollidernameConveyor = hitCollidername;


        Vector3 _offset_row = new Vector3(0, -0.46f, 9.75f);

        conveyor.transform.position = conveyorPosition - _offset_row; // conveyor.transform.position = Conveyor10.transform.position - _offset_column;

        GameObject.Find(CollidernameConveyor).GetComponent<BoxCollider>().enabled = false; //hit.collider = Conveyor10
        ConfigManager.changeConfig("BLM", CollidernameConveyor, ProductionModule.KeinModul, true);

        conveyor.AddComponent<Drag_Conveyor>();
        conveyor.GetComponent<ConstrutorClient_ConveyorBelt>().enabled = true;
        conveyor = null;

    }

    private void createStartOmnis(string hitCollidername, Vector3 omniPosition)
    {
        omni = Instantiate(Resources.Load("omniBelt")) as GameObject;      //clone Prefab from Folder "Resources"
        originalcolorOmni = omni.GetComponent<MeshRenderer>().material.color;  //get originalcolor
        omni.GetComponent<MeshRenderer>().material.color = originalcolorOmni;

        numOmni++;   //number increase 1 for create the name of modul
        omni.name = "omniBelt " + numOmni.ToString();
        Omniname = omni.name;

        CollidernameOmni = hitCollidername;

        Vector3 _offset = new Vector3(0.2f, -0.55f, 0);
        omni.transform.position = omniPosition - _offset;

        GameObject.Find(CollidernameOmni).GetComponent<BoxCollider>().enabled = false;
        ConfigManager.changeConfig("OLM", CollidernameOmni, ProductionModule.KeinModul, true); // Update the current Config

        omni.AddComponent<Drag_Omni>();
        omni.GetComponent<ConstructorClient_Omni>().enabled = true;
        omni = null;
    }

    private void createStartMagazin(string hitCollidername, Vector3 magazinPosition)
    {
        magazin = Instantiate(Resources.Load("Modul_StapelMagazin")) as GameObject;  //clone Prefab from Folder "Resources"
        originalcolorMagazin = magazin.GetComponent<MeshRenderer>().material.color;
        magazin.GetComponent<MeshRenderer>().material.color = originalcolorMagazin;

        numMagazin++;
        magazin.name = "StapelMagazin " + numMagazin.ToString();
        Magazinname = magazin.name;

        CollidernameMagazin = hitCollidername;

        Vector3 _offset_row = new Vector3(9.52f, -7.78f, 0.01f);
        magazin.transform.position = magazinPosition - _offset_row;

        GameObject.Find(CollidernameMagazin).GetComponent<BoxCollider>().enabled = false;

        ConfigManager.changeConfig("PM", CollidernameMagazin, ProductionModule.ModulStapelMagazin, true); // Update the current Config

        magazin.AddComponent<Drag_StapelMagazin>();
        magazin.GetComponent<ConstructorClient_StapelMagazin>().enabled = true;

        magazin = null;
    }

    private void createStartSenke(string hitCollidername, Vector3 senkePosition)
    {
        modulSenke = Instantiate(Resources.Load("Modul_Senke")) as GameObject;  //clone Prefab from Folder "Resources"
        sonSenke = modulSenke.transform.Find("Arm").gameObject;
        originalcolorSenke = modulSenke.GetComponent<MeshRenderer>().material.color;
        originalcolorSonSenke = sonSenke.GetComponent<MeshRenderer>().material.color;
        modulSenke.GetComponent<MeshRenderer>().material.color = originalcolorSenke;
        sonSenke.GetComponent<MeshRenderer>().material.color = originalcolorSonSenke;

        numSenke++;
        modulSenke.name = "Senke " + numSenke.ToString();
        ModulnameSenke = modulSenke.name;

        CollidernameSenke = hitCollidername;

        Vector3 _offset_row = new Vector3(11.27f, -7.72f, 0.06f);
        modulSenke.transform.position = senkePosition - _offset_row;

        GameObject.Find(CollidernameSenke).GetComponent<BoxCollider>().enabled = false;

        ConfigManager.changeConfig("PM", CollidernameSenke, ProductionModule.ModulSenke, true); // Update the current Config

        modulSenke.AddComponent<Drag_Senke>();
        modulSenke.GetComponent<ConstructorClient_Senke>().enabled = true;

        modulSenke = null;

    }

    private void createStartBohren(string hitCollidername, Vector3 bohrenPosition)
    {
        modul = Instantiate(Resources.Load("Modul_BohrenA")) as GameObject;  //clone Prefab from Folder "Resources"
        son = modul.transform.Find("Arm").gameObject;
        originalcolor = modul.GetComponent<MeshRenderer>().material.color;
        originalcolorSon = son.GetComponent<MeshRenderer>().material.color;
        modul.GetComponent<MeshRenderer>().material.color = originalcolor;
        son.GetComponent<MeshRenderer>().material.color = originalcolorSon;
        numBohren++;
        modul.name = "Bohren A"  + " " + numBohren.ToString();
        Modulname = modul.name;

        Collidername = hitCollidername;

        Vector3 _offset_row = new Vector3(10.85f, -7.72f, 1.56f);
        modul.transform.position = bohrenPosition - _offset_row;

        GameObject.Find(Collidername).GetComponent<BoxCollider>().enabled = false;

        ConfigManager.changeConfig("PM", Collidername, ProductionModule.ModulBohrenA, true);

        modul.AddComponent<Drag_Bohren>();
        modul.GetComponent<ConstructorClient_Bohren>().enabled = true;

        modul = null;
    }

    private void createStartStanzen(string hitCollidername, Vector3 stanzenPosition)
    {
        modul = Instantiate(Resources.Load("Modul_StanzenA")) as GameObject;  //clone Prefab from Folder "Resources"
        son = modul.transform.Find("Arm").gameObject;
        originalcolor = modul.GetComponent<MeshRenderer>().material.color;
        originalcolorSon = son.GetComponent<MeshRenderer>().material.color;

        modul.GetComponent<MeshRenderer>().material.color = originalcolor;
        son.GetComponent<MeshRenderer>().material.color = originalcolorSon;

        numStanzen++;
        modul.name = "Stanzen A" + " " + numStanzen.ToString();
        Modulname = modul.name;

        Collidername = hitCollidername;

        Vector3 _offset_row = new Vector3(11.58f, -7.72f, 1.56f);
        modul.transform.position = stanzenPosition - _offset_row;

        GameObject.Find(Collidername).GetComponent<BoxCollider>().enabled = false;

        ConfigManager.changeConfig("PM", Collidername, ProductionModule.ModulStanzenA, true); // Update the current Config

        modul.AddComponent<Drag_Stanzen>();
        modul.GetComponent<ConstructorClient_Stanzen>().enabled = true;
        modul = null;
    }

}
