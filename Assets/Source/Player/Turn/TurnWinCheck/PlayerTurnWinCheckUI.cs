using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerTurnWinCheckUI : MonoBehaviour
{
    [SerializeField] private PlayerTurnWinCheck _winCheck;

    [SerializeField] private TextMeshProUGUI _winText;

    private void OnEnable()
    {
        _winCheck.TeamWin += OnTeamWin;
    }

    private void OnTeamWin(UnitTeam team)
    {
        _winText.enabled = true;

        _winText.text = team.name + " Wins";
    }

    private void OnDisable()
    {
        _winCheck.TeamWin -= OnTeamWin;
    }
}