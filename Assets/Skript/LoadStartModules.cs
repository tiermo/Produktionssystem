using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadStartModules : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
        /*GameObject logistikModul = new GameObject();
        logistikModul = GameObject.Find("Logistikmodul");
        Debug.Log(logistikModul);
        StartCoroutine(EnablethisShit(logistikModul));
        */

    }

    // Update is called once per frame
    void Update () {
		
	}

    private IEnumerator EnablethisShit(GameObject logistikModul)
    {
        logistikModul.SetActive(false);
        logistikModul.SetActive(true);
        yield return new WaitForSeconds(1);

        logistikModul.SetActive(false);

    }
}

