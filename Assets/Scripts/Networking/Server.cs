using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Server : MonoBehaviour {

    public const int PORT = 3001;
    public const int TIMEOUT = 1000;

    private NetworkCommsServerWrapper tcpServer;

    public void StartServer() {
        print("Starting server");
        tcpServer = new NetworkCommsServerWrapper();
        tcpServer.StartServer();
    }

    private void Update() {

        if (tcpServer == null) {
            return;
        }
    
        var reqs = tcpServer.GetRequests();
        foreach (Packet p in reqs) {
            print("SERVER: " + p);
        }
    }
}

public abstract class ServerType {

    protected List<Packet> requests;
    protected List<Packet>[] responses;

    public virtual void StartServer() {
        requests = new List<Packet>();
        responses = new List<Packet>[8];
    }
    public virtual void StopServer() {
        requests = null;
        responses = null;
    }
    public Packet[] GetRequests() {

        if (requests.Count == 0) {
            return new Packet[0];
        }

        Packet[] p = requests.ToArray();
        requests.Clear();
        return p;
    }
    public void SendResponse(Packet packet) {
        if (packet.client_id < 0) {
            for (int i = 0; i < 8; i++) {
                responses[i].Add(packet);
            }
        } else {
            responses[packet.client_id].Add(packet);
        }
    }
}

public class TcpStateObject {
    public Socket workSocket = null;
    public const int bufferSize = 1024;
    public byte[] buffer = new byte[bufferSize];
    public List<byte> data = new List<byte>();
    public int dataLength = -1;

    public void SaveBuffer(int byteAmount) {
        for (int i = 0; i < byteAmount; i++) {
            data.Add(buffer[i]);
        }
    }
    public bool EndOfData() {

        if (dataLength >= 4) {
            if (data.Count == dataLength) {
                return true;
            }
        }

        if (data.Count < 4) {
            return false;
        }

        byte[] intBytes = new byte[4];

        for (int i = 0; i < 4; i++) {
            intBytes[i] = data[i];
        }

        dataLength = BitConverter.ToInt32(intBytes, 0);

        return data.Count == dataLength;
    }
    public byte[] ToByteArray() {
        return data.ToArray();
    }
}