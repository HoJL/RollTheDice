using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Utils;

public class ShowUpMoneyText : MonoBehaviour
{
    [SerializeField] Text _moneyText;

    public void SetText(double money)
    {
        string str = Utils.ToCurrencyString(money);
        _moneyText.text = $"+${str}";
    }
}
