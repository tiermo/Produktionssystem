using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Create_ConveyorBelt : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private GameObject conveyor; //clone the instance of conveyor belt
    private Color originalcolor;  //save the original color of the gameobject

    private string localEulerAngles; //in order to change the rotation of conveyor belt

    private LayerMask mask; //Calculate the number of "Plane" layermask
    RaycastHit hit;

    private string Collidername; //the name of colider
    private string Conveyorname;

    private int num = 0; //the number of conveyorbelt

    //private ConfigManager ConfigManager = new ConfigManager(); // so the Config can be updated

    public void OnBeginDrag(PointerEventData data)
    {
        //conveyor=Instantiate(conveyor) as GameObject;  //clone the latest clone, will be clone more Skript when used AddComponent
        conveyor = Instantiate(Resources.Load("conveyorBelt")) as GameObject;  //clone Prefab from Folder "Resources"
        originalcolor=conveyor.GetComponent<MeshRenderer>().material.color;
        conveyor.GetComponent<MeshRenderer>().material.color=Color.red;

        num++;
        conveyor.name = "conveyorBelt " + num.ToString();
        Conveyorname = conveyor.name;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //drag the gameobject in order to move with mouse
        if (conveyor != null)
        {
            //Clone GameObject move with mouse
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(50,0,80));
            if (pos.y > 0.46f == false)
            {
                pos.y = 2f;
            }
            conveyor.transform.position = pos;
        }
        conveyor.GetComponent<MeshRenderer>().material.color = Color.red;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask.value))
            {
                Collidername = hit.collider.name;
                switch (localEulerAngles)
                {
                    case "(0.0, 270.0, 90.0)": //is related to Conveyor1/3/5
                        if (int.Parse(Collidername.Substring(8, 1)) % 2 != 0)   //Format is "Conveyor1/3/5"oder"Conveyor0/2/4/6",get the number and decided if it is odd or even number.
                        {
                            conveyor.GetComponent<MeshRenderer>().material.color = Color.green;
                        }
                        else
                        {
                            conveyor.GetComponent<MeshRenderer>().material.color = Color.red;
                        }
                        break;
                    case "(0.0, 180.0, 90.0)": //is related to Conveyor0/2/4/6
                        if (int.Parse(Collidername.Substring(8, 1)) % 2 == 0)   //Format is "Conveyor1/3/5"oder"Conveyor0/2/4/6",get the number and decided if it is odd or even number.
                        {
                            conveyor.GetComponent<MeshRenderer>().material.color = Color.green;
                        }
                        else
                        {
                            conveyor.GetComponent<MeshRenderer>().material.color = Color.red;
                        }
                        break;
                    default:
                        break;
                }
            }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (conveyor.GetComponent<MeshRenderer>().material.color == Color.green)
        {
            switch (localEulerAngles)
            {
                case "(0.0, 270.0, 90.0)": 
                    //Debug.Log("hit.collider.transform.position" + hit.collider.transform.position);
                    Vector3 _offset_row = new Vector3(0, -0.46f, 9.75f);
                    conveyor.transform.position = hit.collider.transform.position - _offset_row;
                    //Debug.Log("trans.position" + conveyor.transform.position);
                    //conveyor.GetComponent<ConveyorScript>().enabled = true;      //script for horizontal conveyor belt
                    //conveyor.GetComponent<ConveyorScript_Vertikal>().enabled = false; 
                    break;
                case "(0.0, 180.0, 90.0)":
                    //Debug.Log("hit.collider.transform.position" + hit.collider.transform.position);
                    Vector3 _offset_column = new Vector3(-9.67f, -0.46f, 0);
                    conveyor.transform.position = hit.collider.transform.position - _offset_column;
                    //Debug.Log("trans.position" + conveyor.transform.position);
                    //conveyor.GetComponent<ConveyorScript>().enabled = false; 
                    //conveyor.GetComponent<ConveyorScript_Vertikal>().enabled = true; // script for horizontal conveyor belt
                    break;
                default:
                    break;
            }
            conveyor.GetComponent<MeshRenderer>().material.color = originalcolor;
            hit.collider.GetComponent<BoxCollider>().enabled = false;

            ConfigManager.changeConfig("BLM", Collidername, ProductionModule.KeinModul, true); // Update the current Config

            conveyor.AddComponent<Drag_Conveyor>();
            conveyor.GetComponent<ConstrutorClient_ConveyorBelt>().enabled = true;

            conveyor = null; //delete gameobject otherwise conflict will appear between update() here and OnDrag in Script Drag_Conveyor
        }
        else
        {
            Destroy(conveyor);
            num--;
        }
    }

    void Start(){
        mask = 1 << (LayerMask.NameToLayer("Plane"));
    }

    void Update() {

        if (conveyor != null)
        {
            localEulerAngles = conveyor.transform.localEulerAngles.ToString();
            //Debug.Log("localeulerangles" + conveyor.transform.localEulerAngles);
            if (Input.GetKeyDown(KeyCode.W))
            {
                switch (localEulerAngles)
                {
                    case "(0.0, 270.0, 90.0)":
                        conveyor.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 90));
                        break;
                    case "(0.0, 180.0, 90.0)":
                        conveyor.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 90));
                        break;
                    default:
                        conveyor.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 90));
                        break;
                }
            }
        }
    }

    public string SendColliderName() {
        return Collidername;
    }

    public string SendConveyorName()
    {
        return Conveyorname;
    }
}
