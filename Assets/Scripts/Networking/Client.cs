using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour {

    public Packet packet;
    public KeyCode messageKey;

    private TCP_Client tcpClient;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.C)) {
            ConnectToServer();
        }
        if (Input.GetKeyDown(messageKey)) {
            SendPacket(packet);
        }

        if (tcpClient == null) {
            return;
        }

        var ress = tcpClient.GetResponses();
        foreach (Packet p in ress) {
            print("CLIENT: " + p);
        }
    }

    public void ConnectToServer() {
        tcpClient = new TCP_Client();
        tcpClient.Connect();
    }
    public void SendPacket(Packet packet) {

        for (int i = 0; i < 50; i++) {
            tcpClient.AddRequestToQueue(packet);
        }

        tcpClient.SendRequests();
    }
}

public class ClientType {
    public virtual void Connect() { }
    public virtual void Disconnect() { }
    public virtual void AddRequestToQueue(Packet packet) { }
    public virtual void SendRequests() { }
    public virtual Packet[] GetResponses() { return null; }
}