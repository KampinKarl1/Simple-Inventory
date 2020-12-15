using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleInventory
{
    public class MiningUI : MonoBehaviour
    {
        [SerializeField] Miner miner = null;

        [SerializeField] private Image resourceImage = null;
        [SerializeField] private Image backgroundImage = null;
        [SerializeField] private Image fillbarImage = null;

        [Header("World Canvas")]
        [SerializeField] private GameObject worldCanvas = null;

        #region Initialization
        private void Start()
        {
            if (miner == null)
                StartCoroutine(FindMiner());

            InitializeUI();
        }

        IEnumerator FindMiner()
        {
            float maxWaitTime = 5.0f;
            float countdown = maxWaitTime;

            while (miner == null && countdown > 0)
            {
                yield return new WaitForSeconds(.1f);
                miner = FindObjectOfType<Miner>();
                countdown -= .1f;
            }

            if (miner)
                InitializeUI();
        }

        public void InitializeUI()
        {
            resourceImage.gameObject.SetActive(false);
            backgroundImage.gameObject.SetActive(false);

            miner.onEnterMine += EnterMine;
            miner.onExitMine += ExitMine;
            miner.onMiningProgressChange += UpdateUI;
        }
        #endregion

        #region Event Based Methods
        private void EnterMine(Mineable m)
        {
            worldCanvas.SetActive(true);
            worldCanvas.transform.position = m.transform.position;
            worldCanvas.transform.LookAt(Camera.main.transform);

            resourceImage.sprite = m.Resource.Icon;
        }

        private void UpdateUI(float mineProgress)
        {
            print($"Progress is {mineProgress}");

            resourceImage.gameObject.SetActive(mineProgress > 0);
            backgroundImage.gameObject.SetActive(mineProgress > 0);

            fillbarImage.fillAmount = mineProgress;
        }

        private void ExitMine()
        {
            resourceImage.gameObject.SetActive(false);
            backgroundImage.gameObject.SetActive(false);

            worldCanvas.SetActive(false);
        }
        #endregion
    }
}
