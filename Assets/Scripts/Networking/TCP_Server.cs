using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using UnityEngine;

public class TCP_Server : ServerType {

    #region Threads
    private Thread receiveThread;
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
    #endregion

    public ManualResetEvent allDone = new ManualResetEvent(false);

    private Packet testPacket = new Packet(0, 0, 0, "Server response template");
    private byte[] TestData() {
        List<Packet> p = new List<Packet>();
        p.Add(testPacket);
        return Packet.ToByteData(p);
    }

    private Socket listener;

    static int Main(string[] args) {
        TCP_Server s = new TCP_Server();
        s.StartServer();
        return 0;
    }

    public override void StartServer() {
        base.StartServer();

        sendThreads = new List<ThreadTime>();

        receiveThread = new Thread(new ThreadStart(StartServerThread));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }
    public override void StopServer() {
        base.StopServer();
    }

    public void StartServerThread() {

        MonoBehaviour.print("Creating server");

        //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPHostEntry ipHostInfo = Dns.GetHostEntry("127.0.0.1");
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Server.PORT);

        listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        try {

            listener.Bind(localEndPoint);
            listener.Listen(100);

            while (true) {

                allDone.Reset();
                MonoBehaviour.print("Starting to listen");
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

                MonoBehaviour.print("Server received data: " + state.data.Count);

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

        FlushThreads();

        ThreadTime t = ThreadTime.New(delegate () { SendThread(handler, data); });
        sendThreads.Add(t);
        t.thread.Start();
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

