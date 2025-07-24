using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacterMove : NetworkBehaviour
{
    [SerializeField] private float _speed;

    private Rigidbody _rigidbody;

    private void Start()
    {
        if (!IsOwner)
            return;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!IsOwner)
            return;
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Fly");
        float zInput = Input.GetAxis("Vertical");

        _rigidbody.velocity = transform.rotation * new Vector3(xInput, yInput, zInput) * _speed;
    }
}