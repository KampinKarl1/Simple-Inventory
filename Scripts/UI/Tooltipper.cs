using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltipper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TooltipType typeOfTooltip = TooltipType.Default;

    [SerializeField] private string tooltip = null;

    [SerializeField] private TooltipInfo info = new TooltipInfo();

    [SerializeField] private float timeToTooltip = .5f;
    private float countdown = 0;

    #region Pointer Events Implementation
    public void OnPointerEnter(PointerEventData eventData)
    {
        OpenTooltip(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ScreenTooltipManager.PointerOnTooltip(eventData.pointerCurrentRaycast.gameObject))
            return;

        CloseTooltip();
    }
    #endregion

    #region Builtin Function
    void Start() 
    {
        countdown = timeToTooltip;
    }
    //Must have collider to use this callback
    private void OnMouseOver()
    {
        if (CountdownToTooltip())
            OpenTooltip(false);
    }

    private void OnMouseExit()
    {
        CloseTooltip();

        countdown = timeToTooltip;
    }

    private void OnDestroy()
    {
        CloseTooltip();
    }
    #endregion

    private bool CountdownToTooltip() 
    {
        if (countdown > 0)
        {
            countdown -= Time.deltaTime;
            return false;
        }
        return true;
    }

    private void OpenTooltip(bool is2DObject) 
    {
        if (ScreenTooltipManager.Instance)
            ScreenTooltipManager.Instance.OpenTooltipAtPos(typeOfTooltip, transform.position, info, tooltip, is2DObject);
    }

    private void CloseTooltip() 
    {
        if (ScreenTooltipManager.Instance)
            ScreenTooltipManager.Instance.DeactivateTooltip();
    }

    public void SetMessage(string message) => tooltip = message;

    public void SetTooltipInfo(TooltipInfo _info) => info = _info;
    public void SetTooltipInfo(string title, string category, string main, string secondary) => info = new TooltipInfo(title, category, main, secondary);
}
