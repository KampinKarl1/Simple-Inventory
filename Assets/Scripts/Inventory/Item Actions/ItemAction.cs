using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ItemAction : DynamicAction<SimpleInventory.Item>
{
    public override abstract UnityAction Action(SimpleInventory.Item i, int n, object extra = null);
}