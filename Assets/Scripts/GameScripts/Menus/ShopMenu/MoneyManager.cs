using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public Wallet wallet;   // player's wallet
    public TextMeshProUGUI text;    // Money representation

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        text.text = wallet.money.ToString();
    }
    private void OnEnable()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        text.text = wallet.money.ToString();
    }

}
