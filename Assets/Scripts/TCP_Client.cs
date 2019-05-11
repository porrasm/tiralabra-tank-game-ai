using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;


public class TCP_Client : MonoBehaviour {


    private TcpClient connection;
    private Thread receiveThread;

    public KeyCode messageKey;
    public string message;

    private void Start() {
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.C)) {
            Connect();
        }
        if (Input.GetKeyDown(messageKey)) {
            Send(message);
        }
    }

    private void Connect() {

        print("Connecting client");

        try {
            receiveThread = new Thread(new ThreadStart(ListenForData));
            receiveThread.IsBackground = true;
            receiveThread.Start();
        } catch (Exception e) {
            Debug.Log("On client connect exception " + e);
        }
    }


    private void ListenForData() {
        try {
            connection = new TcpClient("localhost", TCP_Server.PORT);
            Byte[] bytes = new Byte[1024];
            while (true) {

                using (NetworkStream stream = connection.GetStream()) {
                    int length;

                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) {

                        var data = new byte[length];
                        Array.Copy(bytes, 0, data, 0, length);

                        string serverMessage = Encoding.ASCII.GetString(data);
                        Debug.Log("server message received as: " + serverMessage);
                    }
                }
            }
        } catch (SocketException socketException) {
            Debug.Log("Socket exception: " + socketException);
        }
    }


    private void Send(string message) {
        if (connection == null) {
            return;
        }
        try {

            NetworkStream stream = connection.GetStream();
            if (stream.CanWrite) {
                string clientMessage = "This is a message from one of your clients.";

                byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);

                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                Debug.Log("Client sent his message - should be received by server");
            }
        } catch (SocketException socketException) {
            Debug.Log("Socket exception: " + socketException);
        }
    }
}
