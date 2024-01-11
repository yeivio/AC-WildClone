using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Wallet", menuName = "Custom Assets/Wallet")]
public class Wallet : ScriptableObject
{

    public int money;

    public bool CanBuy(int price)
    {
        return money >= price;
    }
    public bool Buy(int price)
    {
        if (!CanBuy(price))
            return false;
        money -= price;
        return true;
    }
    public bool Sell(int price)
    {
        money += price;
        return true;
    }

}

