using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    private LayerMask mask;  //Calculate the number of "Plane" layermask
    RaycastHit hit;

    void Start()
    {
        mask = 1 << (LayerMask.NameToLayer("Default"));
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask.value))
        {
            Debug.Log("hit collider name" + hit.collider.name);
            if (Input.GetMouseButton(0))
            {
                if (GameObject.Find("Logistikmodul") != null)
                {
                    GameObject.Find("Logistikmodul").SetActive(false);
                }

                if (GameObject.Find("Bearbeitungsmodul") != null)
                {
                    GameObject.Find("Bearbeitungsmodul").SetActive(false);
                }
            }
        }      
    }
}
