using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UnitAttacker : MonoBehaviour
{
    [SerializeField] private NetworkObject _networkObject;

    [SerializeField] private RaycastSettings _raycastSettings;

    [SerializeField] private UnitSelector _unitSelector;

    private RaycastHit _hit;

    private void Update()
    {
        if (!_networkObject.IsOwner)
            return;

        if (_unitSelector.CurrentUnit == null)
            return;
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (Physics.Raycast(_raycastSettings.Origin.position, _raycastSettings.Origin.forward, out _hit,
                _raycastSettings.Range, _raycastSettings.LayerMask))
            {
                if (_hit.collider.TryGetComponent<UnitHitBox>(out UnitHitBox unitHitBox))
                {
                    _unitSelector.CurrentUnit.TryAttack(unitHitBox);
                }
            }
        }
    }
}