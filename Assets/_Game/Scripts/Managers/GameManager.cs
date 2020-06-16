using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public List<PlayerController> Players { get; private set; } = new List<PlayerController>();

    private void Update()
    {
        Debug.Log(NetworkManager.singleton.networkAddress);
    }

    public void AddPlayer(PlayerController player)
    {
        Players.Add(player);
    }

    public void RemovePlayer(PlayerController player)
    {
        Players.Remove(player);
    }
}
