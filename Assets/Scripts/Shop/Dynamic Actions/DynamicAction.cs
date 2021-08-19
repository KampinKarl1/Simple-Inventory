using UnityEngine;
using UnityEngine.Events;

public abstract class DynamicAction<T> : ScriptableObject
{
#if UNITY_EDITOR
    [SerializeField, Multiline] private string description;
#endif

    public abstract UnityAction Action(T i, int n, object extra = null);
}
