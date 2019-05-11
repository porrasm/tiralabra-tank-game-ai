
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCP_Server : MonoBehaviour {

    private TcpListener listener;
    private Thread listenerThread;
    private TcpClient client;
    private TcpClient[] clients;

    private int port = 5001;

    // Start is called before the first frame update
    void Start() {

    }

    public void StartServer() {

        print("Creating server thread");
        listenerThread = new Thread(new ThreadStart(Listen));
        listenerThread.IsBackground = true;
        listenerThread.Start();
    }
    private void Listen() {
        try {
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            listener.Start();
            print("Server is listening");

            Byte[] bytes = new Byte[1024];

            while (true) {
                using (client = listener.AcceptTcpClient()) {
                    using (NetworkStream stream = client.GetStream()) {

                        int length;

                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) {
                            byte[] data = new byte[length];
                            Array.Copy(bytes, 0, data, 0, length);

                        }
                    }
                }
            }
        } catch (Exception e) {
            print("Failed to create server");
            print(e);
        }
    }
}
