using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleInventory
{
    public class Mineable : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Item resource = null;

        private bool beingMined = false;

        [SerializeField] private int minResources = 1;
        [SerializeField] private int maxResources = 5;
        private int resourcesRemaining = 0;

        [SerializeField, Tooltip("Time it takes to mine one resource from this object.")]
        private float miningTime = 1.0f;
        private float countdown = 0f;
        #endregion

        #region Properties and Events
        public Item Resource => resource;
        public float MiningProgress => countdown / miningTime;

        public delegate void OnEmpty();
        public OnEmpty onEmpty;
        #endregion

        #region Start and Update
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
        #endregion

        public void MineResource(Miner miner)
        {
            if (resource == null || resourcesRemaining == 0)
                return;

            beingMined = true;

            countdown += Time.deltaTime * miner.MiningSpeed;

            if (countdown >= miningTime)
            {
                countdown = 0;

                miner.ReceiveResource(resource);
                resourcesRemaining--;
            }

            if (resourcesRemaining == 0)
            {
                gameObject.SetActive(false);
                onEmpty?.Invoke();
            }
        }
    }
}