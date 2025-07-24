using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerTeam : NetworkBehaviour
{
    [SerializeField] private UnitTeam[] _teams;

    public UnitTeam CurrentTeam { get; private set; }

    private void Start()
    {
        Debug.Log(OwnerClientId);

        for (int i = 0; i < _teams.Length; i++)
        {
            if (_teams[i].Id == OwnerClientId)
            {
                CurrentTeam = _teams[i];
                return;
            }
        }
    }
}