using System.Collections.Generic;
using UnityEngine;

public class Affordance : MonoBehaviour
{
    [SerializeField] Texture2D defaultAffordance = null;

    [SerializeField] AffordanceInfo[] affordanceInfos = null;

    private Dictionary<int, Texture2D> affordanceDictionary = new Dictionary<int, Texture2D>();

    private void Start()
    {
        for (int i = 0; i < affordanceInfos.Length; ++i)
        {
            if (!affordanceDictionary.ContainsKey(affordanceInfos[i].Layer))
                affordanceDictionary.Add(affordanceInfos[i].Layer, affordanceInfos[i].AffordanceSprite);
            else
                Debug.LogWarning("Apparent duplicate of affordanceInfo in Affordance");
        }
    }

    public Texture2D GetInfoForLayer(int layerIndex)
    {
        if (layerIndex == -1 || !affordanceDictionary.ContainsKey(layerIndex))
            return defaultAffordance;

        return affordanceDictionary[layerIndex];        
    }
}