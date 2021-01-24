using UnityEngine;
using System.Collections;

public class manuell_Messen : MonoBehaviour
{
    public string data;

    void Update()
    {
        if (string.Compare(data, "st") == 0)
        {
            GetComponent<MessenScript>().setDistanceSensorActive();
        }

    }
}
