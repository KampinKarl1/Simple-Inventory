using UnityEngine;

[CreateAssetMenu(menuName = "Affordance/New Affordance Info")]
public class AffordanceInfo : ScriptableObject
{
    [SerializeField] private int designatedLayer = -1;

    [SerializeField] private Texture2D affordanceSprite = null;

    public int Layer => designatedLayer;
    public Texture2D AffordanceSprite => affordanceSprite;
}