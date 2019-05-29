using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class TCP_Client : ClientType {

    private Thread responseThread;
    private Thread sendThread;

    private ManualResetEvent connectDone = new ManualResetEvent(false);
    private ManualResetEvent sendDone = new ManualResetEvent(false);
    private ManualResetEvent receiveDone = new ManualResetEvent(false);

    private Socket client;

    private List<Packet> requests;
    private List<Packet> responses;

    public override void Connect() {
        requests = new List<Packet>();
        responses = new List<Packet>();

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

            client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
            connectDone.WaitOne();

        } catch (Exception e) {
            MonoBehaviour.print(e);
        }
    }
    public override void Disconnect() {

        Socket client = null;

        if (client == null) {
            return;
        }

        client.Shutdown(SocketShutdown.Both);
        client.Close();
        client = null;
    }

    public override Packet[] GetResponses() {

        if (responses.Count == 0) {
            return new Packet[0];
        }

        Packet[] p = responses.ToArray();
        responses.Clear();
        return p;
    }
    public override void AddRequestToQueue(Packet packet) {
        requests.Add(packet);
    }
    public override void SendRequests() {

        if (sendThread == null || !sendThread.IsAlive) {
            
            if (sendThread != null) {
                sendThread.Abort();
            }

            sendThread = new Thread(new ThreadStart(SendRequestsThread));
            sendThread.IsBackground = true;
            sendThread.Start();
        } else {
            MonoBehaviour.print("Send has not finished yet");
        }     
    }
    public void SendRequestsThread() {

        byte[] data = Packet.ToByteData(requests);
        requests.Clear();

        Send(client, data);
        sendDone.WaitOne();

        Receive(client);
        receiveDone.WaitOne();
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

    private void Send(Socket client, byte[] data) {
        client.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), client);
    }

    private void SendCallback(IAsyncResult ar) {
        try {
            // Retrieve the socket from the state object.  
            Socket client = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.  
            int bytesSent = client.EndSend(ar);

            // Signal that all bytes have been sent.  
            sendDone.Set();
        } catch (Exception e) {
            MonoBehaviour.print(e.ToString());
        }
    }
}
