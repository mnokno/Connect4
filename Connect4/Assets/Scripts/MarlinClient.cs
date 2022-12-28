using UnityEngine;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class MarlinClient
{
    Socket sender;

    /// <summary>
    /// Establishes connection to Marlin server
    /// </summary>
    public void Connect()
    {
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
    }

    /// <summary>
    /// Sends a request to the Marlin server to setup game
    /// </summary>
    /// <param name="TTMemoryPool">How much ram in MB the server will dedicate for the game</param>
    public void InitGame(int TTMemoryPool)
    {
        // Creation of message that we will send to Server
        byte[] messageSent = Encoding.ASCII.GetBytes("requestType:initialization,TTMemoryPool:" + TTMemoryPool.ToString());
        int byteSent = sender.Send(messageSent);

        // Data buffer
        byte[] messageReceived = new byte[1024];

        // We receive the message using the method Receive().
        // This method returns number of bytes received, that we'll use to convert them to string
        int byteRecv = sender.Receive(messageReceived);
        Debug.Log($"Message from Server -> {Encoding.ASCII.GetString(messageReceived, 0, byteRecv)}");
    }

    /// <summary>
    /// Send played move to Marlin server and wait for reply
    /// </summary>
    /// <param name="playedFile">File on which the player played</param>
    /// <param name="miliseconds">How much time the server will have to calculate best move</param>
    /// <returns></returns>
    public int GetMove(int playedFile, int miliseconds)
    {
        // Creation of message that we will send to Server
        byte[] messageSent = Encoding.ASCII.GetBytes("requestType:moveCalculation,opponentMove:" + playedFile.ToString() + ",timeLimit:" + miliseconds.ToString());
        int byteSent = sender.Send(messageSent);

        // Data buffer
        byte[] messageReceived = new byte[1024];

        // We receive the message using the method Receive().
        // This method returns number of bytes received, that we'll use to convert them to string
        int byteRecv = sender.Receive(messageReceived);
        Debug.Log($"Message from Server -> {Encoding.ASCII.GetString(messageReceived, 0, byteRecv)}");
        return 0;    
    }
}
