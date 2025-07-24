using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Unit/MoveSettings")]
public class UnitMoveSettings : ScriptableObject
{
    [field: SerializeField] public int MoveDistance { get; private set; }
}