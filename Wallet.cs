using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private int startMoney = 0;

    private int money = 0;

    public delegate void OnMoneyChange(int changeAmount, int finalMoney);
    public OnMoneyChange onMoneyChange;

    public int Money => money;

    public bool CanAfford(int amount) => money >= amount;

    private void Start()
    {
        AddMoney(startMoney);
    }

    public void AddMoney(int amount) 
    { 
        money += amount;
        onMoneyChange?.Invoke(amount, money);
    }

    public void RemoveMoney(int amount)
    {
        money -= amount;
        onMoneyChange?.Invoke(-amount, money);
    }
}
