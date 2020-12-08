using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SharedBool : ScriptableObject
{
    [SerializeField] private bool value = false;

    public bool Value => value;

    public void SetValue(bool value) => this.value = value;
}
