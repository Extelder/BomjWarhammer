using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private NetworkObject _ownerPlayer;

    private void Start()
    {
        if (!_ownerPlayer.IsOwner)
            Destroy(gameObject);
        transform.parent = null;
    }
}