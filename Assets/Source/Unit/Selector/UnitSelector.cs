using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    [SerializeField] private NetworkObject _networkObject;

    [SerializeField] private PlayerTeam _team;

    [SerializeField] private RaycastSettings _raycastSettings;

    private RaycastHit _hit;

    public Unit CurrentUnit { get; private set; }

    private void Update()
    {
        if (!_networkObject.IsOwner)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(_raycastSettings.Origin.position, _raycastSettings.Origin.forward, out _hit,
                _raycastSettings.Range, _raycastSettings.LayerMask))
            {
                if (_hit.collider.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit.Team != _team.CurrentTeam)
                        return;
                    if (CurrentUnit == unit)
                        return;
                    SelectUnit(unit);
                }
            }
        }
    }

    public void SelectUnit(Unit unit)
    {
        CurrentUnit?.DeSelect();
        CurrentUnit = unit;
        CurrentUnit.Select();
    }
}