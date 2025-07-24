using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit/UnitTeam")]
public class UnitTeam : ScriptableObject
{
    [field: SerializeField] public ulong Id { get; private set; }
    [field: SerializeField] public Material Material { get; private set; }
}