using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using UnityEngine;

public class TCP_Server : ServerType {

    private Thread receiveThread;
    private Thread sendThread;

    public ManualResetEvent allDone = new ManualResetEvent(false);

    private List<Packet> requests;
    private List<Packet>[] responses;

    private Packet testPacket = new Packet(0, 0, 0, "Server response template");
    private byte[] TestData() {
        List<Packet> p = new List<Packet>();
        p.Add(testPacket);
        return Packet.ToByteData(p);
    }

    static int Main(string[] args) {
        TCP_Server s = new TCP_Server();
        s.StartServer();
        return 0;
    }

    public override Packet[] GetRequests() {

        if (requests.Count == 0) {
            return new Packet[0];
        }

        Packet[] p = requests.ToArray();
        requests.Clear();
        return p;
    }
    public override void SendResponse(Packet packet) {
        if (packet.client_id < 0) {
            for (int i = 0; i < 8; i++) {
                responses[i].Add(packet);
            }
        } else {
            responses[packet.client_id].Add(packet);
        }
    }

    public override void StartServer() {

        requests = new List<Packet>();
        responses = new List<Packet>[8];

        receiveThread = new Thread(new ThreadStart(StartServerThread));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }
    public void StartServerThread() {

        MonoBehaviour.print("Creating server");

        //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPHostEntry ipHostInfo = Dns.GetHostEntry("127.0.0.1");
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Server.PORT);

        Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        try {

            listener.Bind(localEndPoint);
            listener.Listen(100);

            while (true) {

                allDone.Reset();
                listener.BeginAccept(new AsyncCallback(ReceiveData), listener);

                allDone.WaitOne();
            }

        } catch (System.Exception e) {
            MonoBehaviour.print(e);
        }
    }

    private void ReceiveData(IAsyncResult ar) {

        allDone.Set();

        Socket listener = (Socket)ar.AsyncState;
        Socket handler = listener.EndAccept(ar);

        TcpStateObject state = new TcpStateObject();
        state.workSocket = handler;
        handler.BeginReceive(state.buffer, 0, TcpStateObject.bufferSize, 0, new AsyncCallback(ReadCallback), state);
    }

    private void ReadCallback(IAsyncResult ar) {

        TcpStateObject state = (TcpStateObject)ar.AsyncState;
        Socket handler = state.workSocket;

        int bytesRead = handler.EndReceive(ar);

        if (bytesRead > 0) {

            state.SaveBuffer(bytesRead);

            if (state.EndOfData()) {

                List<Packet> reqs = Packet.ToPacketData(state.ToByteArray());

                foreach (Packet p in reqs) {
                    requests.Add(p);
                }

                Send(handler, TestData());
            } else {
                handler.BeginReceive(state.buffer, 0, TcpStateObject.bufferSize, 0, new AsyncCallback(ReadCallback), state);
            }
        } else {
        }
    }

    private void Send(Socket handler, byte[] data) {

        if (sendThread != null) {
            if (sendThread.IsAlive) {
                MonoBehaviour.print("Server send thread has not finished");
                return;
            }
        }

        sendThread = new Thread(delegate() { SendThread(handler, data); });
        sendThread.IsBackground = true;
        sendThread.Start();
    }
    private void SendThread(Socket handler, byte[] data) {
        handler.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), handler);
    }
    private static void SendCallback(IAsyncResult ar) {
        try {  
            Socket handler = (Socket)ar.AsyncState;

            int bytesSent = handler.EndSend(ar);

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        } catch (Exception e) {
            MonoBehaviour.print(e.ToString());
        }
    }
}

