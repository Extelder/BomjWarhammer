using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSensetivitySettings : MonoBehaviour
{
    [SerializeField] private NetworkObject _networkObject;

    [SerializeField] private Slider _slider;
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private CinemachinePOV _cinemachinePov;

    private PlayerConfigData _config;

    private void Start()
    {
        if (!_networkObject.IsOwner)
            return;

        _config = SaveLoad.Load();


        _cinemachinePov = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>();

        SetSensetivity(_config.LookSensitivity);
        _slider.value = _config.LookSensitivity;


        _slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        SetSensetivity(value);
    }

    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void SetSensetivity(float value)
    {
        _cinemachinePov.m_HorizontalAxis.m_MaxSpeed = value;
        _cinemachinePov.m_VerticalAxis.m_MaxSpeed = value;

        _config.LookSensitivity = value;
        SaveLoad.Save(_config);
    }
}