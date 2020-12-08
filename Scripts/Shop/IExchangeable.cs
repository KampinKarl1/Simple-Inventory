using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExchangeable <T>
{
    T GetPrice();
}
