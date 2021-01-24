using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System;

//Author: Sagar Nayak
//Date: 26.10.2017

//tcpConveyor controls tcp communication for conveyor belts
/*public class tcpConveyor : MonoBehaviour {
	public int port = 6001;                         // open port at 6001
	public string convevorForward = "10101010";     // service number definitions
	public string convevorBackward = "10101011";
	public string convevorOff = "10101012";
	private ServerClient client;
	private TcpListener server;
	private bool serverStarted = false;
    public string speed;

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
		if(string.Compare(data, "forw")==0) {
			GetComponent<ConveyorScript>().ConveyorOn ();
			GetComponent<ConveyorScript> ().setConveyorDirectionUpLeft(string speed);
			sendBackMessage (convevorForward);
		}
		if(string.Compare(data, "off")==0) {
			GetComponent<ConveyorScript>().ConveyorOff ();
			sendBackMessage (convevorOff);
		}
		if(string.Compare(data, "backw")==0) {
			GetComponent<ConveyorScript>().ConveyorOn ();
			GetComponent<ConveyorScript> ().setConveyorDirectionDownRight (string speed);
			sendBackMessage (convevorBackward);
		}
		if(string.Compare(data, "st")==0) {
			StreamWriter writer = new StreamWriter (client.tcp.GetStream (), Encoding.ASCII);
			data = GetComponent<ConveyorScript> ().getConveyorObjectSensorStatus().ToString();
			writer.WriteLine(data);
			writer.Flush ();
		}
	}

	private void sendBackMessage (string data) {                    // send service number as acknowledgement
		StreamWriter writer = new StreamWriter (client.tcp.GetStream (), Encoding.ASCII);
		writer.WriteLine(data);
		writer.Flush ();
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

}

public class ServerClient
{
	public TcpClient tcp;
	public string clientName;

	public ServerClient(TcpClient clientSocket)
	{
		clientName = "guest";
		tcp = clientSocket;
	}

}*/