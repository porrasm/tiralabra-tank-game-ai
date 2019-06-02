using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class TCP_Client : ClientType {

    private Thread responseThread;
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

    private ManualResetEvent connectDone = new ManualResetEvent(false);
    private ManualResetEvent sendDone = new ManualResetEvent(false);
    private ManualResetEvent receiveDone = new ManualResetEvent(false);

    private Socket client;

    
    public override void Connect() {
        base.Connect();

        sendThreads = new List<ThreadTime>();

        responseThread = new Thread(new ThreadStart(ConnectThread));
        responseThread.IsBackground = true;
        responseThread.Start();
    }
    public void ConnectThread() {

        MonoBehaviour.print("Connecting to server...");

        try {

            //IPHostEntry ipHostInfo = Dns.GetHostEntry("");
            IPHostEntry ipHostInfo = Dns.GetHostEntry("127.0.0.1");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, Server.PORT);

            client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            MonoBehaviour.print("Client connect status: " + client.Connected);
            client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
            connectDone.WaitOne();
            
            MonoBehaviour.print("Client connect status: " + client.Connected);

        } catch (Exception e) {
            MonoBehaviour.print(e);
        }
    }
    private void ConnectCallback(IAsyncResult ar) {
        try {
            // Retrieve the socket from the state object.  
            Socket client = (Socket)ar.AsyncState;

            // Complete the connection.  
            client.EndConnect(ar);

            MonoBehaviour.print($"Socket connected to {client.RemoteEndPoint.ToString()}");

            // Signal that the connection has been made.  
            connectDone.Set();
        } catch (Exception e) {
            MonoBehaviour.print(e.ToString());
        }
    }


    public override void Disconnect() {

        base.Disconnect();

        Socket client = null;

        if (client == null) {
            return;
        }

        client.Shutdown(SocketShutdown.Both);
        client.Close();
        client = null;
    }
   
    
    private void Receive(Socket client) {
        try {
            // Create the state object.  
            TcpStateObject state = new TcpStateObject();
            state.workSocket = client;

            // Begin receiving the data from the remote device.  
            client.BeginReceive(state.buffer, 0, TcpStateObject.bufferSize, 0, new AsyncCallback(ReceiveCallback), state);
        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }
    }
    private void ReceiveCallback(IAsyncResult ar) {
        try {
            // Retrieve the state object and the client socket   
            // from the asynchronous state object.  
            TcpStateObject state = (TcpStateObject)ar.AsyncState;
            Socket client = state.workSocket;

            // Read data from the remote device.  
            int bytesRead = client.EndReceive(ar);

            if (bytesRead > 0) {
                // There might be more data, so store the data received so far.  
                state.SaveBuffer(bytesRead);

                // Get the rest of the data.  
                client.BeginReceive(state.buffer, 0, TcpStateObject.bufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            } else {

                state.EndOfData();

                MonoBehaviour.print("Client received data: " + state.data.Count);

                List<Packet> ress = Packet.ToPacketData(state.ToByteArray());

                foreach (Packet p in ress) {
                    responses.Add(p);
                }
   
                receiveDone.Set();
            }
        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }
    }


    public override void SendRequests() {

        FlushThreads();

        ThreadTime t = ThreadTime.New(new ThreadStart(SendRequestsThread));
        sendThreads.Add(t);
        t.thread.Start();
    }
    private void SendRequestsThread() {

        MonoBehaviour.print("Sending " + requests.Count + " requests");

        byte[] data = Packet.ToByteData(requests);
        requests.Clear();

        Send(client, data);
        sendDone.WaitOne();

        Receive(client);
        receiveDone.WaitOne();
    }
    private void Send(Socket client, byte[] data) {

        MonoBehaviour.print("Client sends data: " + data.Length);
        client.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), client);
    }
    private void SendCallback(IAsyncResult ar) {
        try {
            // Retrieve the socket from the state object.  
            Socket client = (Socket)ar.AsyncState;
            MonoBehaviour.print("Send start: " + client.Connected);

            // Complete sending the data to the remote device.  
            int bytesSent = client.EndSend(ar);

            // Signal that all bytes have been sent.  
            sendDone.Set();

            MonoBehaviour.print("Send done: " + bytesSent);

        } catch (Exception e) {
            MonoBehaviour.print(e.ToString());
        }
    }
}
