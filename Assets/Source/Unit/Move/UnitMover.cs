using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UnitMover : MonoBehaviour
{
    [SerializeField] private NetworkObject _networkObject;

    [SerializeField] private RaycastSettings _raycastSettings;

    [SerializeField] private UnitSelector _unitSelector;
    [SerializeField] private float _secondClickForMoveCayuoutTime;

    private bool _waitingCayoutTime;

    private RaycastHit _hit;

    private void Update()
    {
        if (!_networkObject.IsOwner)
            return;
        if (_unitSelector.CurrentUnit == null)
            return;
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (_waitingCayoutTime)
            {
                _unitSelector.CurrentUnit.TryMove();
                return;
            }

            StopAllCoroutines();
            _waitingCayoutTime = false;

            if (Physics.Raycast(_raycastSettings.Origin.position, _raycastSettings.Origin.forward, out _hit,
                _raycastSettings.Range, _raycastSettings.LayerMask))
            {
                if (!_hit.collider.TryGetComponent<UnitHitBox>(out UnitHitBox UnitHitBox))
                    _unitSelector.CurrentUnit.VizualizeMovePath(_hit.point);
            }

            StartCoroutine(WaitCayoutTime());
        }
    }

    private IEnumerator WaitCayoutTime()
    {
        _waitingCayoutTime = true;
        yield return new WaitForSeconds(_secondClickForMoveCayuoutTime);
        _waitingCayoutTime = false;
    }
}