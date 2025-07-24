using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct Generatable
{
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public int MinToSpawn { get; private set; }
    [field: SerializeField] public int MaxToSpawn { get; private set; }
}

public class LevelGenerator : NetworkBehaviour
{
    [SerializeField] private SeedGenerator _seedGenerator;

    [SerializeField] private NavMeshSurface _surface;

    [SerializeField] private Vector3 _origin;

    [SerializeField] private Vector2 _xRange;
    [SerializeField] private Vector2 _zRange;

    [SerializeField] private Generatable[] _generatables;

    private void OnEnable()
    {
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        NetworkManager.Singleton.OnClientStarted += OnClientStarted;
    }

    private void OnClientStarted()
    {
        Debug.Log(_seedGenerator.Seed.Value);
        StartCoroutine(WaitForSynchronize());
    }

    private void OnServerStarted()
    {
        _seedGenerator.OnServerStarted();
        OnConnectedToServer();
    }

    private IEnumerator WaitForSynchronize()
    {
        yield return new WaitUntil(() => _seedGenerator.Seed.Value != 0);
        OnConnectedToServer();
    }


    private void OnConnectedToServer()
    {
        Random.InitState(_seedGenerator.Seed.Value);

        for (int i = 0; i < _generatables.Length; i++)
        {
            for (int j = 0; j < Random.Range(_generatables[i].MinToSpawn, _generatables[i].MaxToSpawn); j++)
            {
                Vector3 randomPosition = new Vector3(Random.Range(_xRange.x, _xRange.y), _origin.y,
                    Random.Range(_zRange.x, _zRange.y));

                Vector3 randomRotation = new Vector3(0, Random.Range(-180, 180),
                    0);

                Transform transform = Instantiate(_generatables[i].Prefab, randomPosition, Quaternion.identity)
                    .transform;

                transform.eulerAngles = randomRotation;
            }
        }

        _surface.BuildNavMesh();
        NetworkManager.Singleton.OnClientStarted -= OnClientStarted;
        NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
    }
}