/*using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System;

//Author: Sagar Nayak
//Date: 26.10.2017

//tcpOmniConveyor controls tcp communication for omni-conveyor belts
public class tcpOmniConveyor : MonoBehaviour {
	public int port = 6201;
	public string conveyorLeft = "10201010";        // service number definitions
    public string conveyorRight = "10201011";
	public string conveyorUp = "10201012";
	public string conveyorDown = "10201013";
	public string presenceSensor = "10201020";
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
		if(string.Compare(data, "st")==0) {
			GetComponent<OmniConveyorControl> ().setMonitorFlag ();
		}
		if(string.Compare(data, "left")==0) {
			GetComponent<OmniConveyorControl> ().moveLeft ();
			sendBackMessage (conveyorLeft);
		}
		if(string.Compare(data, "right")==0) {
			GetComponent<OmniConveyorControl> ().moveRight ();
			sendBackMessage (conveyorRight);
		}
		if(string.Compare(data, "up")==0) {
			GetComponent<OmniConveyorControl> ().moveUp ();
			sendBackMessage (conveyorUp);
		}
		if(string.Compare(data, "down")==0) {
			GetComponent<OmniConveyorControl> ().moveDown ();
			sendBackMessage (conveyorDown);
		}
	}

	private void sendBackMessage (string data)
    {                    // send service number as acknowledgement
        StreamWriter writer = new StreamWriter (client.tcp.GetStream (), Encoding.ASCII);
		writer.WriteLine(data);
		writer.Flush ();
	}

	public void onObjectDetection(){
		sendBackMessage (presenceSensor);
	}

	private bool isConnected(TcpClient c)
    {      // check if client is connected
        try
        {
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

}*/