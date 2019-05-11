using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

public class Server : MonoBehaviour {

    private HttpListener listener;

    void Start() {
        CreateLocalServer();
    }

    public void CreateLocalServer() {

        listener = new HttpListener();

        print("Starting server...");
        listener.Prefixes.Add("http://localhost:5000/"); // add prefix "http://localhost:5000/"
        listener.Start(); // start server (Run application as Administrator!)
        print("Server started.");

        Thread responseThread = new Thread(ResponseThread);
        responseThread.Start();

        Thread _responseThread = new Thread(ResponseThread);
        _responseThread.Start(); // start the response thread
    }
    private void ResponseThread() {

        while (true) {
            HttpListenerContext context = listener.GetContext(); // get a context
                                                                      // Now, you'll find the request URL in context.Request.Url
            byte[] _responseArray = Encoding.UTF8.GetBytes("<html><head><title>Localhost server -- port 5000</title></head>" +
            "<body>Welcome to the <strong>Localhost server</strong> -- <em>port 5000!</em></body></html>"); // get the bytes to response
            context.Response.OutputStream.Write(_responseArray, 0, _responseArray.Length); // write bytes to the output stream
            //context.Response.KeepAlive = false; // set the KeepAlive bool to false
            context.Response.Close(); // close the connection
            print("Respone given to a request.");


        }
    }

    void Update() {

    }
}
