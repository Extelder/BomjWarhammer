using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class UnitMaterial : MonoBehaviour
{
    [SerializeField] private Unit _unit;

    private MeshRenderer _meshRenderer;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material = _unit.Team.Material;
    }
}