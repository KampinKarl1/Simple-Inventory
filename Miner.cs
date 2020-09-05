using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleInventory;

public class Miner : MonoBehaviour
{
    [SerializeField] Inventory inventory = null;

    [SerializeField] Mineable currentMine = null;
    [SerializeField] MiningUI mineUI = null;

    [SerializeField] private float miningSpeed = 2.0f;
    public float MiningSpeed => miningSpeed;

    public delegate void OnEnterMine(Mineable m);
    public OnEnterMine onEnterMine;

    public delegate void OnExitMine();
    public OnExitMine onExitMine;

    private void Start()
    {
        mineUI.InitializeUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mine"))
        {
            Mineable m;
            
            if (other.TryGetComponent(out m))
                EnterMine(m);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mine"))
            ExitMine();
    }

    private void EnterMine(Mineable m) 
    {
        currentMine = m;

        currentMine.EnterMine(this);

        currentMine.onEmpty += ExitMine;

        onEnterMine?.Invoke(currentMine);
    }

    private void ExitMine() 
    {
        if (currentMine) 
        {
            currentMine.onEmpty -= ExitMine;

            currentMine.ExitMine();
        }

        currentMine = null;

        onExitMine?.Invoke();
    }

    private bool CursorOverCurrentMine()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.SphereCast(ray, .5f, out hit, 100f))
        {
            Mineable m;
            if (hit.collider.TryGetComponent(out m))
                return m == currentMine;
        }
        return false;
    }

    #region Public Methods

    public void MineCurrent() 
    {
        if (currentMine && CursorOverCurrentMine())
            currentMine.MineResource();
    }

    public void ReceiveResource(Item i) 
    {
        if (inventory)
            inventory.AddToInventory(i);
        else
            Debug.LogError("Theres no inventory on " + name);
    }
    #endregion
}
