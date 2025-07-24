using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawnpoint : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerSpawnable.Spawned += OnPlayerSpawned;
    }

    private void OnPlayerSpawned(GameObject playerObject)
    {
        playerObject.transform.position = transform.position;
    }

    private void OnDisable()
    {
        PlayerSpawnable.Spawned -= OnPlayerSpawned;
    }
}