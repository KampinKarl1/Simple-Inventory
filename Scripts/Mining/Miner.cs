using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleInventory;

public class Miner : MonoBehaviour
{
    #region Variables
    [SerializeField] Inventory inventory = null;

    [SerializeField] Mineable currentMine = null;

    [SerializeField] private float miningSpeed = 2.0f;

    [SerializeField, Header("If within this distance of a Mineable, the miner can mine that resource.")]
    private float validDistance = 5.0f;
    #endregion

    #region Properties and Events
    public float MiningSpeed => miningSpeed;

    public delegate void OnEnterMine(Mineable m);
    public OnEnterMine onEnterMine;

    public delegate void OnExitMine();
    public OnExitMine onExitMine;
    #endregion

    private void EnterMine(Mineable m)
    {
        currentMine = m;

        currentMine.onEmpty += ExitMine;

        onEnterMine?.Invoke(currentMine);
    }

    #region Public Methods
    //Called by Input Manager
    public void MineCurrent()
    {
        //----------------If cursor over a Mineable-----------------
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //if (Physics.SphereCast(ray, .5f, out hit, 100f))
        if (Physics.Raycast(ray, out hit, 100f))
        {
            Mineable m;

            //If within distance of a valid mineable object
            if (hit.collider.TryGetComponent(out m)
                && Vector3.Distance(transform.position, hit.point) <= validDistance)
            {
                EnterMine(m);

                m.MineResource(this);
            }
        }
    }

    public void ExitMine()
    {
        if (currentMine)
        {
            currentMine.onEmpty -= ExitMine;
        }

        currentMine = null;

        onExitMine?.Invoke();
    }

    //Called by the mine
    public void ReceiveResource(Item i)
    {
        if (inventory)
            inventory.AddToInventory(i);
        else
            Debug.LogError("Theres no inventory on " + name);
    }
    #endregion
}

