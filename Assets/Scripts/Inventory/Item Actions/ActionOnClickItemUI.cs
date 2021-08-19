using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleInventory
{
    public abstract class ActionOnClickItemUI : DynamicAction<Item>
    {
        public override abstract UnityAction Action(Item i, int n, object extra = null);
    }
}