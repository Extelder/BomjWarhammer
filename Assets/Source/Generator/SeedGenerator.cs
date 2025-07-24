using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class SeedGenerator : NetworkBehaviour
{
    public NetworkVariable<int> Seed = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    public event Action SeedGenerated;

    public void OnServerStarted()
    {
        if (!IsServer)
            return;

        Seed.Value = Random.Range(1, 99999);
        SeedGenerated?.Invoke();
    }
}