using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningUI : MonoBehaviour
{
    [SerializeField] Miner miner = null;
    private Mineable mine = null;

    [SerializeField] private Image resourceImage = null;
    [SerializeField] private Image backgroundImage = null;
    [SerializeField] private Image fillbarImage = null;

    [Header("World Canvas")]
    [SerializeField] private GameObject worldCanvas = null;

    public void InitializeUI()
    {
        resourceImage.gameObject.SetActive(false);
        backgroundImage.gameObject.SetActive(false);

        miner.onEnterMine += SetMine;
        miner.onExitMine += ExitMine;
    }

    private void Update()
    {
        if (!mine)
            return;

        UpdateUI(mine.MiningProgress);
    }

    private void SetMine(Mineable m) 
    { 
        mine = m;

        resourceImage.sprite = m.Resource.Icon;

        worldCanvas.SetActive(true);
        worldCanvas.transform.position = m.transform.position;
        worldCanvas.transform.LookAt(Camera.main.transform);
    }

    private void UpdateUI(float mineProgress) 
    {
        resourceImage.gameObject.SetActive(mineProgress > 0);
        backgroundImage.gameObject.SetActive(mineProgress > 0);

        fillbarImage.fillAmount = mineProgress;
    }

    private void ExitMine() 
    {
        mine = null;

        resourceImage.gameObject.SetActive(false);
        backgroundImage.gameObject.SetActive(false);

        worldCanvas.SetActive(false);
    }
}
