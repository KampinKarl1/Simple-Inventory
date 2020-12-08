using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum TooltipType { Default, Object, Invalid }

public class ScreenTooltipManager : MonoBehaviour
{
    #region Singleton
    private static ScreenTooltipManager instance = null;
    public static ScreenTooltipManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            DestroyImmediate(this);
    }
    #endregion

    private Vector3 centerScreenPos = new Vector3(Screen.width / 2, Screen.height / 2);

    [SerializeField] private ObjectTooltip objectTooltip = null;
    [SerializeField] private GameObject objectTooltipObject = null;

    [SerializeField] private GameObject tooltipObject = null;
    [SerializeField] private GameObject tooltipPanel = null;
    [SerializeField] private RectTransform transformTooltip = null;
    [SerializeField] private TextMeshProUGUI textTooltip = null;
    

    private GameObject currentTooltip = null;

    private void Start()
    {
        currentTooltip = tooltipObject;
    }

    public static bool PointerOnTooltip(GameObject currentHit) 
    {
        return (currentHit == instance.tooltipObject || currentHit == instance.textTooltip || currentHit == instance.tooltipPanel);
    }

    public void OpenTooltipAtPos(TooltipType type, Vector3 pos, TooltipInfo info, string message, bool isTwoDimension = true)
    {
        if (info.Title == "")
            info = new TooltipInfo();
        SwapTooltipObjects(type == TooltipType.Default ? tooltipObject : objectTooltipObject);

        Vector3 _pos = isTwoDimension ? GetPos_2D(pos) : GetPos_3D(pos);

        if (type == TooltipType.Default)
        {
            textTooltip.text = message;

            transformTooltip.position = _pos;
        }
        else         
            objectTooltip.OpenTooltipAtPosWithInfo(_pos, info);
        
    }

    public void DeactivateTooltip() => currentTooltip.SetActive(false);

    private void SwapTooltipObjects(GameObject swapTo)
    {
        currentTooltip.SetActive(false);
        currentTooltip = swapTo;
        currentTooltip.SetActive(true);
    }

    private Vector3 GetPos_2D(Vector3 pos) => pos + PosTowardCenter(pos);
    private Vector3 GetPos_3D(Vector3 pos) 
    { 
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
        return screenPos + PosTowardCenter(screenPos);
    }

    private Vector3 PosTowardCenter(Vector3 pos) => (centerScreenPos - pos).normalized * transformTooltip.rect.width;
}
