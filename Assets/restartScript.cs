using UnityEngine;

//Author: Sagar Nayak
//Date: 26.10.2017

//restartScript contains functions to restart the simulation.
public class restartScript : MonoBehaviour {

    public void RestartGame() {
		System.Diagnostics.Process.Start(Application.dataPath.Replace("_Data", ".exe"));  //new program 
		Application.Quit();                                                               //kill current process
	}
}