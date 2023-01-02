using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class InputManager : MonoBehaviour
{
    /// <summary>
    /// Reference to GameManager
    /// </summary>
    private GameManager gameManager;
    /// <summary>
    /// Reference to NetworkGate
    /// </summary>
    private NetworkGate networkGate;
    /// <summary>
    /// Flag used to deiced weather the opponent is another human on engine
    /// </summary>
    [SerializeField] private bool engineVsHuman = true;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// Handles user input
    /// </summary>
    /// <param name="gameCircle">Clicked circle</param>
    public void HandleClicked(GameCircle gameCircle)
    {
        // Prevent the user form making a move while the engine is thinking/calculating
        if (NetworkManager.Singleton.IsClient && networkGate.IsAvalable())
        {
            int x = gameCircle.GetX();
            int y = gameCircle.GetY();
            if (gameManager.IsMoveLegal(x, y))
            {
                gameManager.MakeMove(x, y);
                if (engineVsHuman)
                {
                    networkGate.RequestAIMoveServerRpc(x);
                }
            }
        }
    }

    /// <summary>
    /// Setter for NetworkGate 
    /// </summary>
    /// <param name="networkGate">NetworkGate to set</param>
    public void SetNetworkGate(NetworkGate networkGate)
    {
        this.networkGate = networkGate;
    }
}
