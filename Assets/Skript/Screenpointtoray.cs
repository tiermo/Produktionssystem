using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenpointtoray : MonoBehaviour {

    private int rayLength = 10;
    RaycastHit hit;
    Camera cam;
    
    // Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {

        //Vector3 up = transform.TransformDirection(Vector3.up) * rayLength;  // direction of ray
        
        if(Input.GetMouseButton(0)){
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction*10, Color.green);  // project green ray

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 hitpoint = hit.point;
                //Debug.Log("collider name" + hit.collider.name+", hitpoint"+hitpoint.ToString());
                //Debug.Log("collider position" + hit.collider.transform.position);
            }
        }
       

        //Ray ray = new Ray(transform.position, -transform.up); 

         /*if (Physics.Raycast(transform.position, up, out hit, 10))
        {
            //如果射线与平面碰撞，打印碰撞物体信息 
            Debug.Log("碰撞对象: " + hit.collider.name);
            Debug.Log("hit");
            // 在场景视图中绘制射线 
            //Debug.DrawLine(ray.origin, hit.point, Color.red); 
        }
        else {
            Debug.Log("did not hit");
        }*/
        //HitCheck();
		
	}

    /*void HitCheck() {
        Vector3 up = transform.TransformDirection(Vector3.down) * rayLength;  // direction of ray
        Debug.DrawRay(transform.position, up, Color.green);  // project green ray

        //Ray ray = new Ray(transform.position, -transform.up); 

        if (Physics.Raycast(transform.position, up, out hit, 10))
        {
            //如果射线与平面碰撞，打印碰撞物体信息 
            Debug.Log("碰撞对象: " + hit.collider.name);
            Debug.Log("hit");
            // 在场景视图中绘制射线 
            //Debug.DrawLine(ray.origin, hit.point, Color.red); 
        }
        else
        {
            Debug.Log("did not hit");
        }
    }*/
}
