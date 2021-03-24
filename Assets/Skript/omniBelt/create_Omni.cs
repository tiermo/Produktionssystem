using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class create_Omni : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private GameObject omni;       //clone the instance of modul
    private Color originalcolor;   //save the original color of the gameobject
    private LayerMask mask;        //Calculate the number of "Omni" layermask
    RaycastHit hit;
    private string Collidername;   //the name of colider
    private string Omniname;       //this moduls's name
    private int num = 3;           //count the number of modul

    //private ConfigManager ConfigManager = new ConfigManager(); // so the Config can be updated

    public void OnBeginDrag(PointerEventData data)
    {
        omni = Instantiate(Resources.Load("omniBelt")) as GameObject;      //clone Prefab from Folder "Resources"
        originalcolor = omni.GetComponent<MeshRenderer>().material.color;  //get originalcolor
        omni.GetComponent<MeshRenderer>().material.color = Color.red;      //make the color red when at the begin of dragging

        num++;   //number increase 1 for create the name of modul
        omni.name = "omniBelt " + num.ToString();
        Omniname = omni.name;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //drag the gameobject with mouse
        if (omni != null)
        {
            //Clone GameObject move with mouse
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(40, 0, 40));
            if (pos.y > 0.55f == false)
            {
                pos.y = 2f;
            }
            omni.transform.position = pos;
        }
        omni.GetComponent<MeshRenderer>().material.color = Color.red;

        //determine if the shot point can be placed 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask.value))
        {
            Collidername = hit.collider.name;
            omni.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else
        {
            omni.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (omni.GetComponent<MeshRenderer>().material.color == Color.green)
        {
            Vector3 _offset = new Vector3(0.2f, -0.55f, 0);
            omni.transform.position = hit.collider.transform.position - _offset;
            Debug.Log(hit.collider.transform.position);
            omni.GetComponent<MeshRenderer>().material.color = originalcolor;
            hit.collider.GetComponent<BoxCollider>().enabled = false;

            ConfigManager.changeConfig("OLM", Collidername, ProductionModule.KeinModul, true); // Update the current Config

            omni.AddComponent<Drag_Omni>();
            omni.GetComponent<ConstructorClient_Omni>().enabled = true;

            omni = null; //delet gameobject otherwise conflict will appear between update() here and OnDrag in Script Drag_Conveyor
        }
        else
        {
            Destroy(omni);
            num--;
        }
    }

    void Start()
    {
        mask = 1 << (LayerMask.NameToLayer("Omni"));
    }

    public string SendColliderName()
    {
        return Collidername;
    }

    public string SendOmniName()
    {
        return Omniname;
    }
}
