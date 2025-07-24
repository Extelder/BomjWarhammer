using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitHitBox))]
public class UnitAttackableIndicator : MonoBehaviour
{
    [SerializeField] private GameObject _indicator;

    private UnitHitBox _hitBox;


    private void Awake()
    {
        _hitBox = GetComponent<UnitHitBox>();
    }

    private void OnEnable()
    {
        _hitBox.InHittedZone += AttackableIndicatorEnable;
        _hitBox.OutHittedZone += AttackableIndicatorDisable;
    }

    private void OnDisable()
    {
        _hitBox.InHittedZone -= AttackableIndicatorEnable;
        _hitBox.OutHittedZone -= AttackableIndicatorDisable;
    }

    public void AttackableIndicatorEnable()
    {
        _indicator.SetActive(true);
    }

    public void AttackableIndicatorDisable()
    {
        _indicator.SetActive(false);
    }
}