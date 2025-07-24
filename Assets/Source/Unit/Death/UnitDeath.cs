using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(UnitHealth))]
public class UnitDeath : NetworkBehaviour
{
    private UnitHealth _health;

    private void Awake()
    {
        _health = GetComponent<UnitHealth>();
    }

    private void OnEnable()
    {
        _health.CurrentValueBelowOrEqualDieValue += OnCurrentValueBelowOrEqualDieValue;
    }

    private void OnCurrentValueBelowOrEqualDieValue()
    {
        DieServerRPC();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DieServerRPC()
    {
        DieClientRPC();
    }

    [ClientRpc]
    public void DieClientRPC()
    {
        Debug.Log("DIe");
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        _health.CurrentValueBelowOrEqualDieValue -= OnCurrentValueBelowOrEqualDieValue;
    }
}