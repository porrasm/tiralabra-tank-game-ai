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

    private Thread responseThread;
    IPEndPoint remoteEndPoint;
    UdpClient client;

    public KeyCode messageKey;
    public Packet packet;


    private void Update() {
        if (Input.GetKeyDown(KeyCode.C)) {
            ConnectToServer();
        }
        if (Input.GetKeyDown(messageKey)) {
            Send(packet);
        }
    }

    public void ConnectToServer() {

        print("Connecting to server");

        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), UDP_Server.PORT);
        client = new UdpClient();;

        return;
        responseThread = new Thread(new ThreadStart(Receive));
        responseThread.IsBackground = true;
        responseThread.Start();

    }

    private void Receive() {

        while (true) {

            try {

                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);

                print("Receiving");

                byte[] data = client.Receive(ref remoteEndPoint);


                Packet packet = Packet.BytesToPacket(data);
                print("Client received packet: " + packet);


            } catch (Exception err) {
                print(err.ToString());
            }
        }
    }

    public void JoinGame() {
        ConnectToServer();


    }

    public void Send(Packet packet) {
        try {

            // Request
            byte[] data = packet.ToByteArray();

            client.Send(data, data.Length, remoteEndPoint);

            // Response
            byte[] r_data = client.Receive(ref remoteEndPoint);

            Packet r_packet = Packet.BytesToPacket(r_data);
            print("Client received packet from server: " + r_packet);
        } catch (Exception err) {
            print(err.ToString());
        }
    }
}
