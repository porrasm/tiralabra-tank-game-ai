using System;
using System.Collections;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;

public class UDP_Client : MonoBehaviour {

    string IP = "127.0.0.1";

    IPEndPoint remoteEndPoint;
    UdpClient client;

    public KeyCode messageKey;
    public string message;


    private void Update() {
        if (Input.GetKeyDown(KeyCode.C)) {
            Connect();
        }
        if (Input.GetKeyDown(messageKey)) {
            Send(message);
        }
    }

    public void Connect() {
        // Endpunkt definieren, von dem die Nachrichten gesendet werden.
        print("UDPSend.init()");


        // ----------------------------
        // Senden
        // ----------------------------
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), UDP_Server.PORT);
        client = new UdpClient();

        // status
        print("Sending to " + IP + " : " + UDP_Server.PORT);
        print("Testing: nc -lu " + IP + " : " + UDP_Server.PORT);
    }


    private void Send(string message) {
        try {

            byte[] data = Encoding.UTF8.GetBytes(message);

            client.Send(data, data.Length, remoteEndPoint);
        } catch (Exception err) {
            print(err.ToString());
        }
    }
}
