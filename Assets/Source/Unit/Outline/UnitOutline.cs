using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitOutline : MonoBehaviour
{
    [SerializeField] private Outline _outline;

    [SerializeField] private Unit _unit;

    private void OnEnable()
    {
        _unit.UnitSelected += OnUnitSelected;
        _unit.UnitDeSelected += OnUnitDeSelected;
    }

    private void OnUnitSelected()
    {
        _outline.enabled = true;
    }

    private void OnUnitDeSelected()
    {
        _outline.enabled = false;
    }

    private void OnDisable()
    {
        _unit.UnitSelected -= OnUnitSelected;
        _unit.UnitDeSelected -= OnUnitDeSelected;
    }
}