using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOnMouse : MonoBehaviour {

    private float myCameraMoveSpeed = 12.0f;
    //private bool cameraIsOn = false;
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
            Vector3 MousePosition = Input.mousePosition;
            //Debug.Log("MousePosition"+MousePosition);

            if (MousePosition.x < 50)
            {
                Vector3 CameraPosition = gameObject.transform.position;
                CameraPosition.z += Time.deltaTime * myCameraMoveSpeed;
                gameObject.transform.position = CameraPosition;
            }

            if (MousePosition.x > Screen.width - 50)
            {
                Vector3 CameraPosition = gameObject.transform.position;
                CameraPosition.z -= Time.deltaTime * myCameraMoveSpeed;
                gameObject.transform.position = CameraPosition;
            }

            if (MousePosition.y < 50)
            {
                Vector3 CameraPosition = gameObject.transform.position;
                CameraPosition.x -= Time.deltaTime * myCameraMoveSpeed;
                gameObject.transform.position = CameraPosition;
            }

            if (MousePosition.y > Screen.height - 50)
            {
                Vector3 CameraPosition = gameObject.transform.position;
                CameraPosition.x += Time.deltaTime * myCameraMoveSpeed;
                gameObject.transform.position = CameraPosition;
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                GetComponent<Camera>().fieldOfView--;
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                GetComponent<Camera>().fieldOfView++;
            }
        
	}

    /*public void switchCameraMode()
    {
        if (cameraIsOn == true)
        {
            cameraIsOn = false;
        }
        else
        {
            cameraIsOn = true;
        }
    }**/
}
