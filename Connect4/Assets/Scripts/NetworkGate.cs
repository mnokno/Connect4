using Unity.Netcode;
using UnityEngine;

public class NetworkGate : NetworkBehaviour
{
    /// <summary>
    /// Stores reference to MarlinClient instance, can only be crated on windows machine
    /// </summary>
    private MarlinClient marlinClient;
    /// <summary>
    /// Stores reference to GameManager
    /// </summary>
    private GameManager gameManager;
    /// <summary>
    /// Controlled by the server, if true the client is waiting for a reply from the engine
    /// </summary>
    private NetworkVariable<bool> isAvalable = new NetworkVariable<bool>();
    /// <summary>
    /// Stores engine calculated move, set to -1 if no move is available
    /// </summary>
    private int aiMove = -1;

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        if (IsSpawned && IsServer)
        {
            if (aiMove != -1)
            {
                // Converts index to x, y coordinates
                int x = aiMove % 7;
                int y = (aiMove - x) / 7;
                
                // Plays the move server side
                if (!IsHost)
                {
                    gameManager.MakeMove(x, y);
                }
                // Plays the move client side
                MakeAIMoveClientRpc(x, y);
       
                aiMove = -1;
            }
            isAvalable.Value = !marlinClient.IsAwaitingReply();
        }
    }

    /// <summary>
    /// Called when a client joins the network
    /// </summary>
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            marlinClient = new MarlinClient();
            marlinClient.Connect();
            marlinClient.InitGame(5000);
        }
        if (IsClient)
        {
            FindObjectOfType<InputManager>().SetNetworkGate(this);
        }
        gameManager = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// Getter for isAvalable, isAvalable is server controlled
    /// </summary>
    /// <returns>isAvalable</returns>
    public bool IsAvalable()
    {
        return isAvalable.Value;
    }

    #region ServerRpc

    /// <summary>
    /// Calculates engine move on the server side and sends it to the client through MakeAIMoveClientRpc
    /// Call with user move:-1 to tell the server to calculate the first move
    /// </summary>
    /// <param name="x">X position of where the user played</param>
    /// <param name="y">Y position of where the user played</param>
    /// <param name="difficultyLevel">difficultyLevel for this move</param>
    [ServerRpc]
    public void RequestAIMoveServerRpc(int x, int y, int difficulty)
    {
        if (!IsHost)
        {
            gameManager.MakeMove(x, y);
        }
        marlinClient.GetMoveAsynch(x, 1000, (int result) => aiMove = result, difficulty);
    }

    /// <summary>
    /// Calculates engine move on the server side and sends it to the client through MakeAIMoveClientRpc
    /// without human move, used to tell the AI to start the game
    /// <param name="difficultyLevel">difficultyLevel for this move</param>
    /// </summary>
    [ServerRpc]
    public void RequestAIMoveServerRpc(int difficulty)
    {
        marlinClient.GetMoveAsynch(-1, 1000, (int result) => aiMove = result, difficulty);
    }

    /// <summary>
    /// Called by the client to start a new game
    /// <param name="difficultyLevel">difficultyLevel for this move</param>
    /// </summary>
    [ServerRpc]
    public void NewGameServerRpc(bool aiStart, int difficulty)
    {
        if (!IsHost)
        {
            gameManager.ResetGame();
            gameManager.SetAIStarts(aiStart);
        }
        FindObjectOfType<C4UI.UIManager>().gameOverPage.rootVisualElement.style.display = UnityEngine.UIElements.DisplayStyle.None;
        marlinClient.NewGame(TTMemoryPool:5000);
        if (aiStart)
        {
            marlinClient.GetMoveAsynch(-1, 1000, (int result) => aiMove = result, difficulty);
        }
    }

    /// <summary>
    /// Can be called by the client to shutdown the server
    /// </summary>
    [ServerRpc]
    public void EndSesionServerRpc()
    {
        NetworkManager.Singleton.Shutdown();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    #endregion

    #region ClientRpc

    /// <summary>
    /// Makes the move on the client side, used to send data back to the client
    /// </summary>
    /// <param name="aiX">X position of where the engine played</param>
    /// <param name="aiY">Y position of where the engine played</param>
    [ClientRpc]
    public void MakeAIMoveClientRpc(int aiX, int aiY)
    {
        gameManager.MakeMove(aiX, aiY);
    }

    #endregion
}
