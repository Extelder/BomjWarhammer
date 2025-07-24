using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(NavMeshObstacle))]
public class UnitMove : NetworkBehaviour
{
    [SerializeField] private UnitMoveSettings _settings;

    private NavMeshAgent _agent;
    private NavMeshObstacle _obstacle;

    private CompositeDisposable _disposable = new CompositeDisposable();

    public event Action DestinationSetted;
    public event Action DestinationCompleated;

    public event Action Bootstrapped;

    public static event Action Moved;
    
    public UnitMovePrediction MovePrediction { get; private set; }


    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _obstacle = GetComponent<NavMeshObstacle>();

        MovePrediction = new UnitMovePrediction(this, _agent, _obstacle, transform, _settings);
        Bootstrapped?.Invoke();
    }

    public bool TryMove()
    {
        if (_agent.remainingDistance > 0.1f)
            return false;

        if (MovePrediction.CurrentPathValid)
        {
            Moved?.Invoke();
            DestinationSetted?.Invoke();
            SetDestinationServerRPC(MovePrediction.CurrentPoint);
            return true;
        }

        return false;
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestinationCompleatedServerRPC()
    {
        DestinationCompleatedClientRPC();
    }

    [ClientRpc()]
    private void DestinationCompleatedClientRPC()
    {
        _disposable?.Clear();
        DestinationCompleated?.Invoke();

        _agent.enabled = false;
        _obstacle.enabled = true;
    }


    [ServerRpc(RequireOwnership = false)]
    private void SetDestinationServerRPC(Vector3 point)
    {
        SetDestinationClientRPC(point);
    }

    [ClientRpc()]
    private void SetDestinationClientRPC(Vector3 point)
    {
        _obstacle.enabled = false;
        _agent.enabled = true;
        _agent.SetDestination(point);

        _disposable?.Clear();
        Observable.EveryUpdate().Subscribe(_ =>
        {
            if (_agent.remainingDistance <= 0.1f)
            {
                DestinationCompleated?.Invoke();

                DestinationCompleatedServerRPC();
                _disposable.Clear();
            }
        }).AddTo(_disposable);
    }

    private void OnDisable()
    {
        _disposable?.Clear();
    }
}