using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHitBox : MonoBehaviour
{
    [field: SerializeField] public Unit Unit { get; private set; }

    public event Action<int> Hitted;
    public event Action InHittedZone;
    public event Action OutHittedZone;

    public virtual void Hit(int value)
    {
        Hitted?.Invoke(value);
    }

    public void InAttackableZone()
    {
        InHittedZone?.Invoke();
    }

    public void OutAttackableZone()
    {
        OutHittedZone?.Invoke();
    }
}