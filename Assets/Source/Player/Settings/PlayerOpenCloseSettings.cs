using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerOpenCloseSettings : MonoBehaviour
{
    [SerializeField] private NetworkObject _networkObject;

    [SerializeField] private GameObject _settingsPanel;

    [SerializeField] private PlayerCursor _cursor;

    private void Update()
    {
        if (!_networkObject.IsOwner)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenClose();
        }
    }

    private void OpenClose()
    {
        _settingsPanel.SetActive(!_settingsPanel.activeSelf);

        switch (_settingsPanel.activeSelf)
        {
            case true:
                _cursor.Show();
                break;
            case false:
                _cursor.Hide();
                break;
        }
    }
}