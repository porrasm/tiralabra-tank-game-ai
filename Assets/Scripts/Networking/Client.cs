using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour {

    public Packet packet;
    public KeyCode messageKey;

    private NetworkCommsClientWrapper tcpClient;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.C)) {
            ConnectToServer();
        }
        if (Input.GetKeyDown(messageKey)) {
            SendPacket(packet);
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            print("----------------------------------------");
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
        tcpClient = new NetworkCommsClientWrapper();
        tcpClient.Connect();
    }
    public void SendPacket(Packet packet) {

        for (int i = 0; i < 1; i++) {
            tcpClient.AddRequestToQueue(packet);
        }

        tcpClient.SendRequests();
    }
}

public abstract class ClientType {

    protected List<Packet> requests;
    protected List<Packet> responses;

    public virtual void Connect() {
        requests = new List<Packet>();
        responses = new List<Packet>();
    }
    public virtual void Disconnect() {
        requests = null;
        responses = null;
    }

    public void AddRequestToQueue(Packet packet) {

        if (requests == null) {
            return;
        }

        requests.Add(packet);
    }
    public Packet[] GetResponses() {

        if (responses == null) {
            return new Packet[0];
        }

        if (responses.Count == 0) {
            return new Packet[0];
        }

        Packet[] p = responses.ToArray();
        responses.Clear();
        return p;
    }
    
    public virtual void SendRequests() { }
}