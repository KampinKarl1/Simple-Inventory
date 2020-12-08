using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct TooltipInfo 
{
    public string Title;
    public string Category;
    public string MainText;
    public string SecondaryText;

    public TooltipInfo(string title, string category, string main, string secondary) 
    {
        Title = title;
        Category = category;
        MainText = main;
        SecondaryText = secondary;
    }
}

public class ObjectTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText = null;
    [SerializeField] private TextMeshProUGUI categoryText = null;
    [SerializeField] private TextMeshProUGUI mainText = null;
    [SerializeField] private TextMeshProUGUI secondaryText = null;

    public void OpenTooltipAtPosWithInfo (Vector3 pos, TooltipInfo info) 
    {
        transform.position = pos;

        titleText.text = info.Title ;
        categoryText.text = info.Category;
        mainText.text = info.MainText;
        secondaryText.text = info.SecondaryText;
    }
}
