using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class MarlinClient
{
    /// <summary>
    /// Establishes connection to Marlin server
    /// </summary>
    public void Connect()
    {
        
    }

    /// <summary>
    /// Terminates connection to Marlin server
    /// </summary>
    public void Disconnect()
    {
        
    }

    /// <summary>
    /// Sends a request to the Marlin server to setup game
    /// </summary>
    /// <param name="TTMemoryPool">How much ram in MB the server will dedicate for the game</param>
    public void InitGame(int TTMemoryPool)
    {
        
    }

    /// <summary>
    /// Send played move to Marlin server and wait for reply
    /// </summary>
    /// <param name="playedFile">File on which the player played</param>
    /// <param name="miliseconds">How much time the server will have to calculate best move</param>
    /// <returns></returns>
    public int GetMove(int playedFile, int miliseconds)
    {
        return 0;    
    }
}
