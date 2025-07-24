using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovePrediction
{
    private int _millisecondsForPathGenerate = 100;

    private UnitMoveSettings _moveSettings;

    private UnitMove _move;
    private NavMeshAgent _agent;
    private NavMeshObstacle _obstacle;

    private Transform _transform;

    public bool CurrentPathValid { get; private set; }
    public Vector3 CurrentPoint { get; private set; }
    public NavMeshPath CurrentPath { get; private set; }

    public event Action PredictionCleared;
    public event Action<bool> PredictionCompleated;

    public UnitMovePrediction(UnitMove move, NavMeshAgent agent, NavMeshObstacle obstacle, Transform transform,
        UnitMoveSettings moveSettings)
    {
        _move = move;
        _agent = agent;
        _obstacle = obstacle;
        _transform = transform;
        _moveSettings = moveSettings;
        _move.DestinationSetted += OnDestinationSetted;
    }

    ~UnitMovePrediction()
    {
        _move.DestinationSetted -= OnDestinationSetted;
    }

    private void OnDestinationSetted()
    {
        PredictionCleared?.Invoke();
    }

    public UniTask<bool> Predict(Vector3 point)
    {
        if (_agent.enabled == true)
        {
            if (_agent.remainingDistance > 0.1f)
                return new UniTask<bool>(false);
        }

        return GetPath(point);
    }

    private async UniTask<bool> GetPath(Vector3 point)
    {
        _obstacle.enabled = false;

        await UniTask.Delay(_millisecondsForPathGenerate);

        _agent.enabled = true;

        CurrentPath = new NavMeshPath();
        if (NavMesh.CalculatePath(_transform.position, point, NavMesh.AllAreas, CurrentPath))
        {
            if (Vector3.Distance(point, _transform.position) > _moveSettings.MoveDistance)
                return false;

            CurrentPoint = point;
            PredictionCompleated?.Invoke(true);
            CurrentPathValid = true;

            return true;
        }

        CurrentPoint = new Vector3(0, 0, 0);
        PredictionCompleated?.Invoke(false);
        CurrentPathValid = false;

        return false;
    }
}