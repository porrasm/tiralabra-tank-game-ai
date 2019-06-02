using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using System.Threading;
using System;

public class NetworkCommsServerWrapper : ServerType {

    private Thread listenThread;

    private string ip = "127.0.0.1";
    private string sendType = "LPacketS";
    private string responseType = "LPacketR";

    private Packet testPacket = new Packet(0, 0, 0, "Server response template");
    private byte[] TestData() {
        List<Packet> p = new List<Packet>();
        p.Add(testPacket);
        return Packet.ToByteData(p);
    }


    public override void StartServer() {
        base.StartServer();

        listenThread = new Thread(() => {
            NetworkComms.AppendGlobalIncomingPacketHandler<byte[]>(sendType, RequestHandler);
            Connection.StartListening(ConnectionType.TCP, new IPEndPoint(IPAddress.Any, Server.PORT));
        });
        listenThread.IsBackground = true;
        listenThread.Start();
    }

    public override void StopServer() {
        base.StopServer();

        NetworkComms.Shutdown();
        listenThread.Abort();
        listenThread = null;
    }

    private void RequestHandler(PacketHeader header, Connection connection, byte[] data) {

        MonoBehaviour.print("Server received packet of size: " + data.Length);
        MonoBehaviour.print("Server receive time: " + (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond));

        List<Packet> packets = Packet.ToPacketData(data);

        foreach (Packet p in packets) {
            requests.Add(p);
        }

        connection.SendObject(responseType, TestData());
    }
}
