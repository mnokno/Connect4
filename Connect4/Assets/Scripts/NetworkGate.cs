using Unity.Netcode;

public class NetworkGate : NetworkBehaviour
{
    /// <summary>
    /// Stores reference to MarlinClient instance, can only be crated on windows machine
    /// </summary>
    private MarlinClient marlinClient;

    /// <summary>
    /// On network spawn initializes marlin client if its the sever side
    /// </summary>
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            marlinClient = new MarlinClient();
            marlinClient.InitGame(10000);
        }
    }

    /// <summary>
    /// Calculates engine move on the server side and sends it to the client through MakeAIMoveClientRpc
    /// Call with user move:-1 to tell the server to calculate the first move
    /// </summary>
    /// <param name="userMove">Move made by the user on the client side</param>
    [ServerRpc]
    public void RequestAIMoveServerRpc(int userMove)
    {

    }

    /// <summary>
    /// Makes the move on the client side, used to send data back to the client
    /// </summary>
    /// <param name="aiMove">Move made by the engine</param>
    [ClientRpc]
    public void MakeAIMoveClientRpc(int aiMove)
    {

    }
}
