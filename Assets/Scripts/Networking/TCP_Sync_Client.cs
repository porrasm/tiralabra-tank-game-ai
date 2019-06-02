using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class TCP_Sync_Client : ClientType {

    private List<ThreadTime> sendThreads;
    private void FlushThreads() {

        if (sendThreads == null) {
            return;
        }

        for (int i = 0; i < sendThreads.Count; i++) {
            if (sendThreads[i].Done()) {
                sendThreads.RemoveAt(i);
                i--;
            }
        }
    }

    private Thread thread;

    private Socket sender;

    IPHostEntry ipHostInfo;
    IPAddress ipAddress;
    private IPEndPoint remoteEP;

    public override void Connect() {
        base.Connect();

        //sendThreads = new List<ThreadTime>();

        //thread = new Thread(new ThreadStart(ConnectThread));
        //thread.IsBackground = true;
        //thread.Start();
    }

    // Thread methods
    private void ConnectThread() {
        // Data buffer for incoming data.  
        byte[] bytes = new byte[1024];

        // Connect to a remote device.  
        try {
            // Establish the remote endpoint for the socket.  
            // This example uses port 11000 on the local computer.  
            ipHostInfo = Dns.GetHostEntry("127.0.0.1");
            ipAddress = ipHostInfo.AddressList[0];
            remoteEP = new IPEndPoint(ipAddress, Server.PORT);

            // Create a TCP/IP  socket.  
            sender = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint. Catch any errors.  
            try {
                sender.Connect(remoteEP);

            } catch (ArgumentNullException ane) {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            } catch (SocketException se) {
                Console.WriteLine("SocketException : {0}", se.ToString());
            } catch (Exception e) {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }

        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }
    }

    private void SendRequestsThread() {

        MonoBehaviour.print("Starting to send: " + requests.Count);

        byte[] data = Packet.ToByteData(requests);
        requests = new List<Packet>();

        try {

            if (!sender.Connected) {
                MonoBehaviour.print("not connected");
                sender.Connect(remoteEP);
            }

            int bytesSent = sender.Send(data);

            MonoBehaviour.print("Send " + bytesSent + " bytes");

            byte[] buffer = new byte[1024];

            int receiveAmount;
            TcpStateObject receiveData = new TcpStateObject();

            while ((receiveAmount = sender.Receive(receiveData.buffer)) > 0) {
                receiveData.SaveBuffer(receiveAmount);
            }

            if (!receiveData.EndOfData()) {
                throw new Exception("Data was corrupted");
            }

            List<Packet> packets = Packet.ToPacketData(receiveData.ToByteArray());
            foreach (Packet p in packets) {
                responses.Add(p);
            }
            

        } catch (ArgumentNullException ane) {
            MonoBehaviour.print("ArgumentNullException : " + ane.ToString());
        } catch (SocketException se) {
            MonoBehaviour.print("SocketException : " + se.ToString());
        } catch (Exception e) {
            MonoBehaviour.print("Unexpected exception : " + e.ToString());
        }
    }

    public override void Disconnect() {
        base.Disconnect();

        sender.Shutdown(SocketShutdown.Both);
        sender.Close();
    }


    public override void SendRequests() {

        FlushThreads();

        ThreadTime t = ThreadTime.New(new ThreadStart(SendRequestsThread));
        sendThreads.Add(t);
        t.thread.Start();
    }
}
