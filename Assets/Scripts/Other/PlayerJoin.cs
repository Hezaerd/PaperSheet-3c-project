using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoin : MonoBehaviour
{
    private int nbPlayers;
    public Transform[] spawnLocations = new Transform[4];
    // Start is called before the first frame update

    private void Start()
    {
        var player = GetComponent<PlayerInputManager>();
        nbPlayers = UI_Menu.nbPlayer;

        for (int i = 0; i < nbPlayers; i++)
        {
            player.JoinPlayer(i);
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        playerInput.gameObject.GetComponent<PlayerDetails>().playerID = playerInput.playerIndex + 1;

        playerInput.gameObject.GetComponent<PlayerDetails>().startPos = spawnLocations[playerInput.playerIndex].position;
    }
}