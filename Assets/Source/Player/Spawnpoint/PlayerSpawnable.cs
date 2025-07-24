using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnable : MonoBehaviour
{
    public static event Action<GameObject> Spawned;

    private void Start()
    {
        Spawned?.Invoke(gameObject);
    }
}