using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    float AddDivePriceRatio = 1.1f;
    float MergeDicePriceRatio = 1.3f;
    float IncomePriceRatio = 1.4f;

    double _money = 0;
    int _addDiceLv = 0;
    int _mergeDiceLv = 0;
    int _incomeLv = 0;

    double _addDiceBacePrice = 0.0f;
    double _mergeDiceBacePrice = 0.0f;
    double _incomeBacePrice = 100.0f;

    float _Upgrade_Defult_Price = 150f;
    float _Upgrade_Defult_Price_Add = 5f;

    int _priceLvCheckCount = 10;
    int _priceLvCheckCount_add = 0;
    int _priceLvCheckCount_merge = 0;
    int _priceLvCheckCount_income = 0;

    int _priceLv_add = 0;
    int _priceLv_merge = 0;
    int _priceLv_income = 0;

    public int IncomeLv
    {
        get { return _incomeLv; }
    }

    public double Money
    {
        get { return _money; }
        set
        {
            _money = value;
            GameManager.Instance.UI.UpgdateMoneyText();
        }
    }

    public double AddDicePrice
    {
        get
        {
            double round =
            (_Upgrade_Defult_Price_Add * _addDiceLv +
            (_Upgrade_Defult_Price_Add * _addDiceLv * Mathf.Pow(AddDivePriceRatio, _priceLv_add)));
            return _addDiceBacePrice + ((long)round / 10) * 10;
            ;
        }
    }
    public double MergeDicePrice
    {
        get
        {
            return _mergeDiceBacePrice +
             (_Upgrade_Defult_Price * _mergeDiceLv +
             (_Upgrade_Defult_Price * _mergeDiceLv * Mathf.Pow(MergeDicePriceRatio, _priceLv_merge)));
        }
    }
    public double IncomePrice
    {
        get
        {
            return _incomeBacePrice +
             (_Upgrade_Defult_Price * _incomeLv +
             (_Upgrade_Defult_Price * _incomeLv * Mathf.Pow(IncomePriceRatio, _priceLv_income)));
        }
    }

    public void BuyAdd()
    {
        if (_money < AddDicePrice)
            return;

        GameManager.Instance.DiceManager.AddDice();
        Money -= AddDicePrice;
        GameManager.Instance.UI.UpgdateMoneyText();

        if (_priceLvCheckCount_add < _priceLvCheckCount)
            _priceLvCheckCount_add++;
        else
        {
            _priceLvCheckCount_add = 0;
            _priceLv_add++;
        }
        _addDiceLv++;
    }
    public void BuyMerge()
    {
        if (_money < MergeDicePrice)
            return;

        GameManager.Instance.DiceManager.MergeDice();
        Money -= MergeDicePrice;

        if (_priceLvCheckCount_merge < _priceLvCheckCount)
            _priceLvCheckCount_merge++;
        else
        {
            _priceLvCheckCount_merge = 0;
            _priceLv_merge++;
        }
        _mergeDiceLv++;
    }
    public void BuyIncome()
    {
        if (_money < IncomePrice)
            return;

        // Income();
        Money -= IncomePrice;

        if (_priceLvCheckCount_income < _priceLvCheckCount)
            _priceLvCheckCount_income++;
        else
        {
            _priceLvCheckCount_income = 0;
            _priceLv_income++;
        }
        _incomeLv++;
    }

    void Income()
    {

    }

}
