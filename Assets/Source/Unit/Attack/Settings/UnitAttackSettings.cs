using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit/AttackSettings")]
public class UnitAttackSettings : ScriptableObject
{
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public int Range { get; private set; }
    [field: SerializeField] public LayerMask AttackLayerMask { get; private set; }
    [field: SerializeField] public int MaxObjectsToOverlap { get; private set; } = 20;
}