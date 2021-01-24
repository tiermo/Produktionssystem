using UnityEngine;
using System.Collections;

public class StapelMagazinSkript : MonoBehaviour
{
    private GameObject workpiece;
    private float height;

    public void CreateRed(string high)
    {
        HeightSelet(high);
        workpiece = Instantiate(Resources.Load("RedCube"), transform.position, transform.rotation) as GameObject;
        workpiece.GetComponent<Rigidbody>().useGravity = false;
        workpiece.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        workpiece.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        workpiece.transform.localScale += new Vector3(0f, height, 0f);
        workpiece.GetComponent<Rigidbody>().useGravity = true;
        GetComponent<tcpServer_StapelMagazin>().sendBackMessage("created");
    }

    public void CreateBlack(string high)
    {
        HeightSelet(high);
        workpiece = Instantiate(Resources.Load("BlackCube"), transform.position, transform.rotation) as GameObject;
        workpiece.GetComponent<Rigidbody>().useGravity = false;
        workpiece.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        workpiece.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        workpiece.transform.localScale += new Vector3(0f, height, 0f);
        workpiece.GetComponent<Rigidbody>().useGravity = true;
        GetComponent<tcpServer_StapelMagazin>().sendBackMessage("created");
    }

    public void CreateMetall(string high)
    {
        HeightSelet(high);
        workpiece = Instantiate(Resources.Load("MetallCube"), transform.position, transform.rotation) as GameObject;
        workpiece.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        workpiece.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        workpiece.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        workpiece.transform.localScale += new Vector3(0f, height, 0f);
        workpiece.GetComponent<Rigidbody>().useGravity = true;
        GetComponent<tcpServer_StapelMagazin>().sendBackMessage("created");
    }

    void HeightSelet(string high)
    {
        switch (high)
        {
            case "short":
                height = 0f;              
                break;
            case "tall":
                height = 1f;
                break;
        }
    } 
}
