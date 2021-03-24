using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System;

//Author: Yu Xiang
//Date: 

public class tcpServer_ConveyorBelt : MonoBehaviour {

    private int port=6001;             // listening port

	private ServerClient client;
	private TcpListener server;
	private bool serverStarted = false;

    public string direction;     // substring get from data, represent the move direction
    public string speed;         // substring get from data, represent the move speed
    public int spaceposition;    //space position in the data

    //directionformat: forw,backw,off
    //speedforamt: low,norm,fast

	void Start()
    {
        port = GetComponent<ConstrutorClient_ConveyorBelt>().getConveyorPortNr();
        if (port==0)
        {
            port = 6001;
        }
        Debug.Log("conveyor port " + port);

        try{
			server = new TcpListener(IPAddress.Any, port);  //listen on host adress of PC
			server.Start();
			StartListening();
			serverStarted = true;
            Debug.Log("server has been started, and waiting for the connection...");
		}
		catch(Exception e){
			Debug.Log ("socket error: "+ e.Message);
		}
	}

	void Update(){
        
        //localEulerAngles = transform.localEulerAngles.ToString();
        if (!serverStarted)
            return;

        //is the client still connected?
        if (client != null)
        {
            if (!isConnected(client.tcp))
            {
                //Debug.Log("Socket closed");
                client.tcp.Close();

            }
            //check for message from the client
            else
            {
                NetworkStream s = client.tcp.GetStream();
                if (s.DataAvailable)
                {
                    StreamReader reader = new StreamReader(s, true);
                    string data = reader.ReadLine();
                    if (data != null)
                    {
                        onIncoming(client, data);
                    }
                }
            }
        }
	}

	private void onIncoming (ServerClient client, string data) {  //process requests depending on string message received
            
        if (string.Compare(data, "off") == 0)
        {
            GetComponent<ConveyorScript>().ConveyorOff();
            sendBackMessage("finished");
            spaceposition = 0;
        }
       
        
        if (data.Contains("service"))
        {
            GetComponent<ConveyorScript>().forwardInformation(data);
            
        }
        else if (string.Compare(data, "st") == 0)
        {
            StreamWriter writer = new StreamWriter(client.tcp.GetStream(), Encoding.ASCII);
            data = GetComponent<ConveyorScript>().getConveyorObjectSensorStatus().ToString();
            writer.WriteLine(data);
            writer.Flush();
        }
        else
        {
            
            spaceposition = data.IndexOf(' ');
            if (spaceposition >= 0)
            {
                direction = data.Substring(0, spaceposition);
                speed = data.Substring(spaceposition + 1);

                if (string.Compare(direction, "forw") == 0)
                {
                    GetComponent<ConveyorScript>().ConveyorOn();
                    GetComponent<ConveyorScript>().setConveyorDirectionDownRight(speed);
                    sendBackMessage("finished");
                }
                if (string.Compare(direction, "backw") == 0)
                {
                    GetComponent<ConveyorScript>().ConveyorOn();
                    GetComponent<ConveyorScript>().setConveyorDirectionUpLeft(speed);
                    sendBackMessage("finished");
                }
            }
            
            
        }
	}

	public void sendBackMessage (string data) {                    
        // send finish as acknowledgement
		StreamWriter writer = new StreamWriter (client.tcp.GetStream (), Encoding.ASCII);
		writer.WriteLine(data);
		writer.Flush();
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


