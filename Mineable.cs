using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mineable : MonoBehaviour
{
    Miner currentMiner = null;

    [SerializeField] private SimpleInventory.Item resource = null;

    private bool beingMined = false;

    [SerializeField] private int minResources = 1;
    [SerializeField] private int maxResources = 5;
    private int resourcesRemaining = 0;

    [SerializeField] private float miningTime = 1.0f;
    private float countdown = 0f;

    public Sprite Icon => resource.Icon; //Give away the least info possible
    public float MiningProgress => countdown / miningTime;

    public delegate void OnEmpty();
    public OnEmpty onEmpty;

    private void Start()
    {
        resourcesRemaining = Random.Range(minResources, maxResources);
    }

    private void Update()
    {
        if (!beingMined && countdown > 0)
            countdown -= Time.deltaTime;

        beingMined = false;
    }

    public void EnterMine(Miner m) 
    {
        currentMiner = m;
    }

    public void ExitMine()
    {
        currentMiner = null;
    }

    public void MineResource() 
    {
        if (resource == null || resourcesRemaining == 0)
            return;

        beingMined = true;

        countdown += Time.deltaTime * currentMiner.MiningSpeed;

        if (countdown >= miningTime)
        {
            countdown = 0;

            currentMiner.ReceiveResource(resource);
            resourcesRemaining--;
        }
        
        if (resourcesRemaining == 0)
        {
            gameObject.SetActive(false);
            onEmpty?.Invoke();
        }
    }
}
