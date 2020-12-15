using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleInventory
{
    public class Miner : MonoBehaviour
    {
        #region Variables
        [SerializeField] Inventory inventory = null;

        [SerializeField] Mineable currentMine = null;

        [SerializeField] LayerMask mineableMask = 1 << 9;

        [SerializeField] private float miningSpeed = 2.0f;

        [SerializeField, Header("If within this distance of a Mineable, the miner can mine that resource.")]
        private float validDistance = 5.0f;

        private float countdown = 0;

        private IEnumerator mineCountdown = null;
        #endregion

        #region Events
        public delegate void OnMiningProgressChange(float progress);
        public OnMiningProgressChange onMiningProgressChange;

        public delegate void OnEnterMine(Mineable m);
        public OnEnterMine onEnterMine;

        public delegate void OnExitMine();
        public OnExitMine onExitMine;
        #endregion

        #region Private Methods
        private void Start()
        {
            mineCountdown = DoMineCountdown(0);
        }

        private void EnterMine(Mineable m)
        {
            if (currentMine == m)
                return;

            currentMine = m;

            onEnterMine?.Invoke(currentMine);
        }

        private void ReceiveResource(Item i)
        {
            if (inventory)
                inventory.AddToInventory(i);
            else
                Debug.LogError("Theres no inventory on " + name +". Add an Inventory component to their Miner Component in the inspector.");
        }

        private void LeaveEmptyMine()
        {
            currentMine = null;

            onExitMine?.Invoke();
        }

        private IEnumerator DoMineCountdown(float mineTime)
        {
            while (countdown > 0)
            {
                countdown -= Time.deltaTime;
                onMiningProgressChange?.Invoke(countdown / mineTime);
                yield return null;
            }
        }
        #endregion

        #region Public Methods
        //Called by Input Manager
        public void MineCurrent(GameObject objectThatTheMouseIsOver)
        {
           Mineable m;

            if (!objectThatTheMouseIsOver.TryGetComponent(out m)
                || Vector3.Distance(transform.position, objectThatTheMouseIsOver.transform.position) > validDistance)
            {
                ExitMine();
                return;
            }

            //--------Entering a new mine------------
            if (currentMine == null)
            {
                StopCoroutine(mineCountdown);
                mineCountdown = DoMineCountdown(m.MiningTime);

                EnterMine(m);
            }
            //---------------------------------------

            //Mining progress
            countdown += Time.deltaTime * miningSpeed;

            //----------Upon achieving full mining progress---------
            if (countdown >= m.MiningTime)
            {
                countdown = 0;

                m.MineResource();
                ReceiveResource(m.Resource);

                if (!m.HasResources)
                {
                    //Mine is empty.
                    LeaveEmptyMine();
                }
            }
            //---------------------------------------------------

            onMiningProgressChange?.Invoke(countdown / m.MiningTime);
        }

        public void ExitMine()
        {
            if (currentMine == null)
                return;

            currentMine = null;

            StartCoroutine(mineCountdown);
        }
        #endregion
    }
}
