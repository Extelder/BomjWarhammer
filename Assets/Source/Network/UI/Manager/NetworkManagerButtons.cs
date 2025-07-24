using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerButtons : MonoBehaviour
{
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _clientButton;

    private void Start()
    {
        _hostButton.onClick.AddListener(OnHostButtonClicked);
        _clientButton.onClick.AddListener(OnClientButtonClicked);
    }

    private void OnHostButtonClicked()
    {
        NetworkManager.Singleton?.StartHost();
    }

    private void OnClientButtonClicked()
    {
        NetworkManager.Singleton?.StartClient();
    }

    private void OnDisable()
    {
        _hostButton.onClick.RemoveListener(OnHostButtonClicked);
        _clientButton.onClick.RemoveListener(OnClientButtonClicked);
    }
}