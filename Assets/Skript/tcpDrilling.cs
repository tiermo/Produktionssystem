using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System;

//Author: Sagar Nayak
//Date: 26.10.2017

//tcpDrilling controls tcp communication for drlling machines
public class tcpDrilling : MonoBehaviour {
	public int port = 6501;
	public string moveUp = "10501010";      // service number definitions
    public string moveDown = "10501011";
	public string stop = "10501012";
	public string turnOn = "10501013";
	public string turnOff = "10501014";
	public string limitSensorUp = "10501015";
	public string limitSensorDown = "10501016";
	private ServerClient client;
	private TcpListener server;
	private bool serverStarted = false;

	void Start(){
		try{
			server = new TcpListener(IPAddress.Any, port);  //listen on host adress of PC
            server.Start();
			StartListening();
			serverStarted = true;
		}
		catch(Exception e){
			Debug.Log ("socket error: "+ e.Message);
		}
	}

	void Update(){
		if (!serverStarted)
			return;

		//is the client still connected?
		if(client!=null) {
			if (!isConnected (client.tcp)) {
				client.tcp.Close ();
					
			}
			//check for message from the client
			else {
				NetworkStream s = client.tcp.GetStream ();
				if(s.DataAvailable){
					StreamReader reader = new StreamReader (s, true);
					string data = reader.ReadLine ();
					if (data != null) {
						onIncoming (client, data);
					}
				}
			}
		}

	}

	private void onIncoming (ServerClient client, string data) {  //process requests depending on string message received
		if(string.Compare(data, "up")==0) {
			GetComponent<drillingArmScript> ().moveUp ();
			sendBackMessage (moveUp);
		}
		if(string.Compare(data, "down")==0) {
			GetComponent<drillingArmScript> ().moveDown ();
			sendBackMessage (moveDown);
		}
		if(string.Compare(data, "on")==0) {
			GetComponent<drillingArmScript> ().turnOn ();
			sendBackMessage (turnOn);
		}
		if(string.Compare(data, "off")==0) {
			GetComponent<drillingArmScript> ().turnOff ();
			sendBackMessage (turnOff);
		}
		if(string.Compare(data, "stop")==0) {
			GetComponent<drillingArmScript> ().stopMovement ();
			sendBackMessage (stop);
		}
		if(string.Compare(data, "limitU")==0) {
			GetComponent<drillingArmScript> ().callLimitSensorUp ();
		}
		if(string.Compare(data, "limitD")==0) {
			GetComponent<drillingArmScript> ().callLimitSensorDown ();
		}
		if(string.Compare(data, "st")==0) {
			StreamWriter writer = new StreamWriter (client.tcp.GetStream (), Encoding.ASCII);  
			data = GetComponent<drillingArmScript>().getMachineStatus().ToString();
			writer.WriteLine(data);
			writer.Flush ();
		}
	}

	private void sendBackMessage (string data) {                    // send service number as acknowledgement
		StreamWriter writer = new StreamWriter (client.tcp.GetStream (), Encoding.ASCII);
		writer.WriteLine(data);
		writer.Flush ();
	}

	public void LimitSwitchesReached(string data)
    {                     // send service number as acknowledgement when limit switches are called
        if (string.Compare (data, "limitD") == 0) {
			sendBackMessage (limitSensorDown);
		}
		if (string.Compare (data, "limitU") == 0) {
			sendBackMessage (limitSensorUp);
		}
	}

	private bool isConnected(TcpClient c){      // check if client is connected
		try{
			if(c!=null && c.Client!=null && c.Client.Connected){
				if(c.Client.Poll(0, SelectMode.SelectRead)){
					return !(c.Client.Receive(new byte[1], SocketFlags.Peek) ==0);
				}
				return true;
			}
			else
				return false;
		}
		catch{
			return false;
		}
	}

	private void StartListening(){
		server.BeginAcceptSocket (AcceptTcpClient, server);
	}

	private void AcceptTcpClient(IAsyncResult ar){
		TcpListener listener = (TcpListener)ar.AsyncState;
		client = new ServerClient (listener.EndAcceptTcpClient(ar));
		StartListening ();
	}

}