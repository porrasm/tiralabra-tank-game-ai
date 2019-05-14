using System;
using System.Collections;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;

public class UDP_Server : MonoBehaviour {

    private Thread receiveThread;
    private UdpClient client;

    private string latestPacket;
    private List<string> packets;

    public static int PORT = 5000;

    void Start() {
        InitializeServer();
    }

    public void InitializeServer() {

        print("Initializing server");

        receiveThread = new Thread(new ThreadStart(Receive));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void Receive() {

        client = new UdpClient(PORT);

        while (true) {

            try {
                // Bytes empfangen.
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);

                // Bytes mit der UTF8-Kodierung in das Textformat kodieren.
                string text = Encoding.UTF8.GetString(data);

                // Den abgerufenen Text anzeigen.
                print("Message from client: " + text);

                // latest UDPpacket
                latestPacket = text;

                // ....
                packets.Add(latestPacket);

            } catch (Exception err) {
                print(err.ToString());
            }
        }
    }
}
