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

    private Queue<Packet> packets;

    public static int PORT = 5000;

    void Start() {
    }

    private void Update() {
        ProcessRequests();
    }

    private void ProcessRequests() {

        if (packets == null) {
            return;
        }

        if (packets.Count == 0) {
            return;
        }

        while (packets.Count > 0) {
            ProcessRequest(packets.Dequeue());
        }
    }
    private void ProcessRequest(Packet packet) {

    }

    public void StartServer() {

        packets = new Queue<Packet>();

        print("Starting server");

        receiveThread = new Thread(new ThreadStart(Receive));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }
    private void Receive() {

        client = new UdpClient(PORT);

        while (true) {

            try {

                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);


                Packet packet = Packet.BytesToPacket(data);
                packet.IPAddress = anyIP.Address;

                ReceivePacket(packet);

                //client.Send(data, data.Length, anyIP);
            } catch (Exception err) {
                print(err.ToString());
            }
        }
    }
    private void ReceivePacket(Packet packet) {

        print("Server received packet: " + packet);
        packets.Enqueue(packet);
    }

    public void StopServer() {

        print("Stopping server");

        if (receiveThread == null) {
            return;
        }

        client.Close();

        receiveThread.Abort();
        receiveThread = null;

        packets = null;
    }
}
