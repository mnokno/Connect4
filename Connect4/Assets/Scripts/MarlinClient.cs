using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.Threading;

public class MarlinClient
{
    /// <summary>
    /// Reference to socket used to communicate with the server
    /// </summary>
    private Socket sender;
    /// <summary>
    /// Set to true when waiting for server reply, used to prevent 
    /// making requests before receiving results from previous requests  
    /// </summary>
    private bool awaitingReply;

    /// <summary>
    /// Establishes connection to Marlin server
    /// </summary>
    public void Connect()
    {
        // Default awaitingReply
        awaitingReply = false;

        // Establish the remote endpoint for the socket.
        // Uses port 27015 on the local computer.
        IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddr = ipHost.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 27015);

        // Creation TCP/IP Socket using
        // Socket Class Constructor
        sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        // Connect Socket to the remote endpoint using method Connect()
        sender.Connect(localEndPoint);

        // We print EndPoint information that we are connected
        Debug.Log($"Socket connected to -> {sender.RemoteEndPoint.ToString()} ");
    }

    /// <summary>
    /// Terminates connection to Marlin server
    /// </summary>
    public void Disconnect()
    {
        // Close Socket using the method Close()
        sender.Shutdown(SocketShutdown.Both);
        sender.Close();
        // defaults awaitingReply
        awaitingReply = false;
    }

    /// <summary>
    /// Sends a request to the Marlin server to setup game
    /// </summary>
    /// <param name="TTMemoryPool">How much ram in MB the server will dedicate for the game</param>
    public void InitGame(int TTMemoryPool)
    {
        // Prevent desynchronization on client side (this side)
        if (awaitingReply)
        {
            throw new Exception("Marline client is wait reply from a different call!");
        }

        // Reserves right to make request
        awaitingReply = true;

        // Creation of message that we will send to Server
        byte[] messageSent = Encoding.ASCII.GetBytes("requestType:initialization,TTMemoryPool:" + TTMemoryPool.ToString() + ",a:void");
        int byteSent = sender.Send(messageSent);
        Debug.Log($"Message to Server -> {Encoding.ASCII.GetString(messageSent, 0, byteSent)}");

        // Data buffer
        byte[] messageReceived = new byte[1024];

        // We receive the message using the method Receive().
        // This method returns number of bytes received, that we'll use to convert them to string
        int byteRecv = sender.Receive(messageReceived);
        Debug.Log($"Message from Server -> {Encoding.ASCII.GetString(messageReceived, 0, byteRecv)}");

        // Releases request privilege
        awaitingReply = false;
    }

    /// <summary>
    /// Send played move to Marlin server and wait for reply
    /// </summary>
    /// <param name="playedFile">File on which the player played</param>
    /// <param name="miliseconds">How much time the server will have to calculate best move</param>
    /// <returns></returns>
    public int GetMove(int playedFile, int miliseconds)
    {
        // Prevent desynchronization on client side (this side)
        if (awaitingReply)
        {
            throw new Exception("Marline client is wait reply from a different call!");
        }

        // Reserves right to make request
        awaitingReply = true;

        // Creation of message that we will send to Server
        byte[] messageSent = Encoding.ASCII.GetBytes("requestType:moveCalculation,playedFile:" + playedFile.ToString() + ",timeLimit:" + miliseconds.ToString());
        int byteSent = sender.Send(messageSent);
        Debug.Log($"Message to Server -> {Encoding.ASCII.GetString(messageSent, 0, byteSent)}");
        
        // Data buffer
        byte[] messageReceived = new byte[1024];

        // We receive the message using the method Receive().
        // This method returns number of bytes received, that we'll use to convert them to string
        int byteRecv = sender.Receive(messageReceived);
        Debug.Log($"Message from Server -> {Encoding.ASCII.GetString(messageReceived, 0, byteRecv)}");

        string[] parts = Encoding.ASCII.GetString(messageReceived, 0, byteRecv).Split(",");
        foreach (string part in parts)
        {
            string[] a = part.Split(":");
            if (a[0] == "move") 
            {
                // Releases request privilege
                awaitingReply = false;
                return int.Parse(a[1]);
            }
        }
        // Releases request privilege
        awaitingReply = false;
        return -1;    
    }

    /// <summary>
    /// Send played move to Marlin server and wait for reply in an asynchronous manner, 
    /// once we revive reply from the server callback is called with the result as a parameter
    /// </summary>
    /// <param name="playedFile">File on which the player played</param>
    /// <param name="miliseconds">How much time the server will have to calculate best move</param>
    /// <param name="callback">Callback function called when we receive result from the server</param>
    /// <returns></returns>
    public void GetMoveAsynch(int playedFile, int miliseconds, Action<int> callback)
    {
        new Thread(() => GetMoveAsynchTask(playedFile, miliseconds, callback, this)).Start();
    }

    /// <summary>
    /// Task used to make a request and wait for reply
    /// </summary>
    /// <param name="playedFile">File on which the player played</param>
    /// <param name="miliseconds">How much time the server will have to calculate best move</param>
    /// <param name="callback">Callback function called when we receive result from the server</param>
    /// <param name="marlinClient">Reference to marlin client that will be used to make the call</param>
    public static void GetMoveAsynchTask(int playedFile, int miliseconds, Action<int> callback, in MarlinClient marlinClient)
    {
        // Prevent desynchronization on client side (this side)
        if (marlinClient.awaitingReply)
        {
            throw new Exception("Marline client is wait reply from a different call!");
        }

        // Reserves right to make request
        marlinClient.awaitingReply = true;

        // Creation of message that we will send to Server
        byte[] messageSent = Encoding.ASCII.GetBytes("requestType:moveCalculation,playedFile:" + playedFile.ToString() + ",timeLimit:" + miliseconds.ToString());
        int byteSent = marlinClient.sender.Send(messageSent);
        Debug.Log($"Message to Server -> {Encoding.ASCII.GetString(messageSent, 0, byteSent)}");

        // Data buffer
        byte[] messageReceived = new byte[1024];

        // We receive the message using the method Receive().
        // This method returns number of bytes received, that we'll use to convert them to string
        int byteRecv = marlinClient.sender.Receive(messageReceived);
        Debug.Log($"Message from Server -> {Encoding.ASCII.GetString(messageReceived, 0, byteRecv)}");

        string[] parts = Encoding.ASCII.GetString(messageReceived, 0, byteRecv).Split(",");
        foreach (string part in parts)
        {
            string[] a = part.Split(":");
            if (a[0] == "move")
            {
                // Releases request privilege
                marlinClient.awaitingReply = false;
                // Tells the invoker that we revived result through callback function they provided
                callback.Invoke(int.Parse(a[1]));
                return;
            }
        }
        // Releases request privilege
        marlinClient.awaitingReply = false;
        // Tells the invoker that we revived result through callback function they provided
        callback.Invoke(-1);
    }
}
