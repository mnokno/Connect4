using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Test : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            // Establish the remote endpoint for the socket.
            // Uses port 27015 on the local computer.
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 27015);

            // Creation TCP/IP Socket using
            // Socket Class Constructor
            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {

                // Connect Socket to the remote
                // endpoint using method Connect()
                sender.Connect(localEndPoint);

                // We print EndPoint information
                // that we are connected
                Debug.Log($"Socket connected to -> {sender.RemoteEndPoint.ToString()} ");

                // Creation of message that
                // we will send to Server
                byte[] messageSent = Encoding.ASCII.GetBytes("requestType:initialization,TTMemoryPool:3000");
                int byteSent = sender.Send(messageSent);

                // Data buffer
                byte[] messageReceived = new byte[1024];

                // We receive the message using
                // the method Receive(). This
                // method returns number of bytes
                // received, that we'll use to
                // convert them to string
                int byteRecv = sender.Receive(messageReceived);
                Debug.Log($"Message from Server -> {Encoding.ASCII.GetString(messageReceived, 0, byteRecv)}");

                // Close Socket using
                // the method Close()
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }

            // Manage of Socket's Exceptions
            catch (ArgumentNullException ane)
            {
                Debug.Log($"ArgumentNullException : {ane.ToString()}");
            }

            catch (SocketException se)
            {
                Debug.Log($"SocketException : {se.ToString()}");
            }

            catch (Exception e)
            {
                Debug.Log($"Unexpected exception : {e.ToString()}");
            }
        }
        catch (Exception e)
        {

            Debug.Log(e.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}