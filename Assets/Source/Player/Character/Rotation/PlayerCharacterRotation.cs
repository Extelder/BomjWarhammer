using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerCharacterRotation : NetworkBehaviour
{
    [SerializeField] private Transform _target;

    private void Update()
    {
        if (!IsOwner)
            return;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, _target.eulerAngles.y, transform.eulerAngles.z);
    }
}