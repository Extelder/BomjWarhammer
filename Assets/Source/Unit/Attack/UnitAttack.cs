using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitMove), typeof(UnitHitBox), typeof(Unit))]
public class UnitAttack : MonoBehaviour
{
    [SerializeField] private UnitAttackSettings _settings;

    private UnitMove _move;

    private UnitHitBox _hitBox;

    private Unit _unit;

    public event Action Bootstrapped;

    public static event Action Attacked;

    public UnitAttackPredict AttackPredict { get; private set; }

    private void Awake()
    {
        _unit = GetComponent<Unit>();
        _move = GetComponent<UnitMove>();
        _hitBox = GetComponent<UnitHitBox>();

        _move.Bootstrapped += OnMoveBootstrapped;
    }

    public void TryAttack(UnitHitBox unitHitBox)
    {
        if (unitHitBox == _hitBox)
            return;

        if (unitHitBox.Unit.Team == _unit.Team)
            return;
        Collider[] others = new Collider[_settings.MaxObjectsToOverlap];
        Physics.OverlapSphereNonAlloc(transform.position, _settings.Range, others, _settings.AttackLayerMask);

        foreach (var other in others)
        {
            if (other == null)
                continue;
            if (other.TryGetComponent<UnitHitBox>(out UnitHitBox hitBox))
            {
                if (unitHitBox == hitBox)
                {
                    Attacked?.Invoke();
                    Debug.Log(unitHitBox.gameObject.name + " Attacked");
                    unitHitBox.Hit(_settings.Damage);
                    return;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _settings.Range);
    }

    private void OnMoveBootstrapped()
    {
        AttackPredict = new UnitAttackPredict(_move, _settings, _unit);
        Bootstrapped?.Invoke();
    }

    private void OnDisable()
    {
        _move.Bootstrapped -= OnMoveBootstrapped;
    }
}