using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitAttack))]
public class UnitAttackPredictSphere : MonoBehaviour
{
    [SerializeField] private GameObject _sphere;

    private UnitAttack _attack;
    
    private UnitAttackPredict _attackPredict;

    private void Awake()
    {
        _attack = GetComponent<UnitAttack>();
        _attack.Bootstrapped += OnAttackBootstrapped;
    }

    private void OnAttackBootstrapped()
    {
        _attackPredict = _attack.AttackPredict;
        _attackPredict.AttackPredicted += OnAttackPredicted;
        _attackPredict.AttackPredictedCleared += OnAttackPredictedCleared;
    }

    private void OnAttackPredictedCleared()
    {
        _sphere.transform.localScale = new Vector3(0, _sphere.transform.localScale.y, 0);
    }

    private void OnAttackPredicted(Vector3 point, float range)
    {
        _sphere.transform.position = point;
        _sphere.transform.localScale = new Vector3(range, _sphere.transform.localScale.y, range);
    }

    private void OnDisable()
    {
        _attack.Bootstrapped -= OnAttackBootstrapped;
        _attackPredict.AttackPredicted -= OnAttackPredicted;
        _attackPredict.AttackPredictedCleared -= OnAttackPredictedCleared;
    }
}