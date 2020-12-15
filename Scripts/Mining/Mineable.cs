using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleInventory
{
    public class Mineable : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Item resource = null;

        [SerializeField] private int minResources = 1;
        [SerializeField] private int maxResources = 5;
        private int resourcesRemaining = 0;

        [SerializeField, Tooltip("Time it takes to mine one resource from this object.")]
        private float miningTime = 1.0f;
        #endregion

        #region Properties
        public Item Resource => resource;
        public bool HasResources => resourcesRemaining > 0;
        /// <summary>
        /// Time it takes to mine this resource.
        /// </summary>
        public float MiningTime => miningTime;
        #endregion

        public delegate void OnEmpty();
        public OnEmpty onEmpty;
        
        #region Methods
        private void Start()
        {
            resourcesRemaining = Random.Range(minResources, maxResources);
        }
        
        public void MineResource()
        {
            if (resource == null || resourcesRemaining == 0)
                return;

            resourcesRemaining--;

            if (resourcesRemaining == 0) 
            {
                gameObject.SetActive(false);
                onEmpty?.Invoke();
            }
        }
        #endregion
    }
}
