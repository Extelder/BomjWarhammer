using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UnitTurn : NetworkBehaviour
{
    [field: SerializeField] public int CurrentTurnIteration { get; private set; }

    public ulong CurrentStepPlayerOwnId = 0;

    public event Action TurnChanged;

    [ServerRpc(RequireOwnership = false)]
    public void AddStepServerRPC()
    {
        Debug.Log("Server");

        AddStepClientRpc();
    }

    private void OnEnable()
    {
        PlayerTurnChanger.OnStepsEnded += AddStepServerRPC;
    }

    private void OnDisable()
    {
        PlayerTurnChanger.OnStepsEnded -= AddStepServerRPC;
    }

    [ClientRpc]
    public void AddStepClientRpc()
    {
        Debug.Log("Clients " + OwnerClientId);

        if (CurrentStepPlayerOwnId == 0)
            CurrentStepPlayerOwnId = 1;
        else
            CurrentStepPlayerOwnId = 0;

        CurrentTurnIteration++;
        TurnChanged?.Invoke();
    }
}