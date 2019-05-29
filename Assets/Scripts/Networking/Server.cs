using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Server : MonoBehaviour {

    public const int PORT = 3001;

    private TCP_Server tcpServer;

    public void StartServer() {
        print("Starting server");
        tcpServer = new TCP_Server();
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

public class ServerType {

    public virtual void StartServer() { }
    public virtual void StopServer() { }
    public virtual Packet[] GetRequests() { return null; }
    public virtual void SendResponse(Packet packet) { }
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