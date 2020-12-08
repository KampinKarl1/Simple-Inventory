using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WalletUI : MonoBehaviour
{
    [SerializeField] private Wallet wallet = null;

    [SerializeField] private string currencySymbol = "$";
    [SerializeField] private TextMeshProUGUI titleOfWalletHolder = null;
    [SerializeField] private TextMeshProUGUI moneyText = null;

    void Start()
    {
        titleOfWalletHolder.text = wallet.gameObject.name + "'s Money";

        wallet.onMoneyChange += UpdateWalletUI;

        UpdateWalletUI(0, wallet.Money);
    }

    private void UpdateWalletUI(int change, int money) 
    {
        moneyText.text = currencySymbol + money.ToString();
    }
}
