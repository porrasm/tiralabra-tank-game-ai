using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections.TCP;
using System;
using NetworkCommsDotNet.Connections;
using System.Threading;

public class NetworkCommsClientWrapper : ClientType {

    private string ip = "127.0.0.1";
    private string sendType = "LPacketS";
    private string responseType = "LPacketR";

    ThreadTimeout threads;

    public override void Connect() {
        base.Connect();

        threads = new ThreadTimeout();

        NetworkComms.AppendGlobalIncomingPacketHandler<byte[]>(responseType, ResponseHandler);

        byte[] intData = BitConverter.GetBytes(4);

        NetworkComms.SendObject(sendType, ip, Server.PORT, intData);
    }
    public override void Disconnect() {
        base.Disconnect();

        threads.Kill();
        threads = null;

        NetworkComms.Shutdown();
    }

    public override void SendRequests() {
        base.SendRequests();

        byte[] data = Packet.ToByteData(requests);
        requests = new List<Packet>();
        MonoBehaviour.print("Sending to server: " + data.Length);

        threads.FlushThreads();

        int id = ThreadTimeout.ID();

        ThreadTime send = ThreadTime.New(() => SendRequestsThread(data, id));
        send.id = id;  
        threads.Threads.Add(send);
        send.Start();
    }
    private void SendRequestsThread(byte[] data, int id) {
        try {
            NetworkComms.SendReceiveObject<byte[], byte[]>(sendType, "127.0.0.1", Server.PORT, responseType, Server.TIMEOUT, data);
        } catch (Exception e) {
            MonoBehaviour.print("Server did not respond in time. (Alive sendThreads: " + threads.Threads.Count);
        }

        threads.KillID(id);
    }

    private void ResponseHandler(PacketHeader header, Connection connection, byte[] data) {

        MonoBehaviour.print("Client received packet of size: " + data.Length);
        MonoBehaviour.print("Client receive time: " + (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond));

        List<Packet> packets = Packet.ToPacketData(data);

        foreach (Packet p in packets) {
            responses.Add(p);
        }
    }
}
