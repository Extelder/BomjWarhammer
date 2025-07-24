using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    [SerializeField] private int _max;

    private int _current;

    private const int DieValue = 0;

    public event Action CurrentValueBelowOrEqualDieValue;

    private void Awake()
    {
        _current = _max;
    }

    public void TakeDamage(int damage)
    {
        if (_current - damage <= DieValue)
        {
            CurrentValueBelowOrEqualDieValue?.Invoke();
            return;
        }

        _current -= damage;
    }
}