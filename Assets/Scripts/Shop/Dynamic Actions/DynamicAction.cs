using UnityEngine;
using UnityEngine.Events;

public abstract class DynamicAction<T> : ScriptableObject
{
#if UNITY_EDITOR
    [SerializeField, Multiline] private string description;
#endif

    public abstract UnityAction Action(T i, int n, object extra = null);
}

public abstract class ItemAction : DynamicAction <SimpleInventory.Item>
{
    public override abstract UnityAction Action(SimpleInventory.Item i, int n, object extra = null);
}

public abstract class CraftableAction : DynamicAction<SimpleInventory.Crafting.Craftable>
{
    public override abstract UnityAction Action(SimpleInventory.Crafting.Craftable i, int n, object extra = null);
}