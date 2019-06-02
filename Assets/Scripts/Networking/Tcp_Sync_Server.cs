using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class Tcp_Sync_Server : ServerType {

    private Thread listenThread;

    private Packet testPacket = new Packet(0, 0, 0, "Server response template");
    private byte[] TestData() {
        List<Packet> p = new List<Packet>();
        p.Add(testPacket);
        return Packet.ToByteData(p);
    }

    public override void StartServer() {
        base.StartServer();

        //StartListening();

        //listenThread = new Thread(new ThreadStart(StartListening));
        //listenThread.IsBackground = true;
        //listenThread.Start();
    }
    public override void StopServer() {
        base.StopServer();

        listenThread.Abort();
        listenThread = null;
    }

    private void StartListening() {

        MonoBehaviour.print("Starting to listen");

        // Establish the local endpoint for the socket.  
        // Dns.GetHostName returns the name of the   
        // host running the application.  
        IPHostEntry ipHostInfo = Dns.GetHostEntry("127.0.0.1");
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Server.PORT);

        // Create a TCP/IP socket.  
        Socket listener = new Socket(ipAddress.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

        // Bind the socket to the local endpoint and   
        // listen for incoming connections.  
        try {
            listener.Bind(localEndPoint);
            listener.Listen(100);

            // Start listening for connections.  
            while (true) {
                MonoBehaviour.print("Waiting for a connection...");
                // Program is suspended while waiting for an incoming connection.  
                Socket handler = listener.Accept();

                // An incoming connection needs to be processed.  
                int receiveAmount;
                TcpStateObject receiveData = new TcpStateObject();

                while ((receiveAmount = handler.Receive(receiveData.buffer)) > 0) {
                    receiveData.SaveBuffer(receiveAmount);
                }

                if (!receiveData.EndOfData()) {
                    throw new Exception("Data was corrupted");
                }

                List<Packet> packets = Packet.ToPacketData(receiveData.ToByteArray());
                foreach (Packet p in packets) {
                    requests.Add(p);
                }

                handler.Send(TestData());
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }

        } catch (Exception e) {
            MonoBehaviour.print(e.ToString());
        }

        Console.WriteLine("\nPress ENTER to continue...");
        Console.Read();

    }
}
