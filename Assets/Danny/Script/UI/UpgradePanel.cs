using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Utils;

public class UpgradePanel : BehaviourBase
{
    enum Buttons { BtnAddDice, BtnMergeDice, BtnIncome };
    Buttons _buttontype;

    [SerializeField] Button _btnAddDice = null;
    [SerializeField] Button _btnMergeDice = null;
    [SerializeField] Button _btnIncome = null;


    [SerializeField] Text _textCurrentMoney = null;
    [SerializeField] Text _textAddDice_Price = null;
    [SerializeField] Text _textMergeDice_Price = null;
    [SerializeField] Text _textIncome_Price = null;

    public UnityEvent OnClickAddDice => _btnAddDice.onClick;
    public UnityEvent OnClickMergeDice => _btnMergeDice.onClick;
    public UnityEvent OnClickIncome => _btnIncome.onClick;

    bool canBuyAdd;
    bool canBuyMerge;
    bool canBuyIncome;

    public void Init()
    {
        UpdateMoneyText();
        UpdatePriceText();
        Check_Mergeable();
        OnClickAddDice.RemoveAllListeners();
        OnClickAddDice.AddListener(OnPressAddDiceButton);
        OnClickMergeDice.RemoveAllListeners();
        OnClickMergeDice.AddListener(OnPressMergeDiceButton);
        OnClickIncome.RemoveAllListeners();
        OnClickIncome.AddListener(OnPressIncomeButton);
    }

    void OnPressAddDiceButton()
    {
        // GameManager.Instance.
        GameManager.Instance.Data.BuyAdd();
        UpdatePriceText();
        Check_Mergeable();
    }
    void OnPressMergeDiceButton()
    {
        GameManager.Instance.Data.BuyMerge();
        UpdatePriceText();
        Check_Mergeable();
    }
    void OnPressIncomeButton()
    {
        GameManager.Instance.Data.BuyIncome();
        UpdatePriceText();
    }

    public void UpdateMoneyText()
    {
        string str = Utils.ToCurrencyString(GameManager.Instance.Data.Money);
        _textCurrentMoney.text = $"{str}";
        BuyCheck_UpgradeButton();
    }

    void UpdatePriceText()
    {
        string addPrice = Utils.ToCurrencyString(GameManager.Instance.Data.AddDicePrice);
        _textAddDice_Price.text = $"$ {addPrice}";
        string mergePrice = Utils.ToCurrencyString(GameManager.Instance.Data.MergeDicePrice);
        _textMergeDice_Price.text = $"$ {mergePrice}";
        string incomePrice = Utils.ToCurrencyString(GameManager.Instance.Data.IncomePrice);
        _textIncome_Price.text = $"$ {incomePrice}";
    }

    void Check_Mergeable()
    {
        bool mergerble = GameManager.Instance.DiceManager.IsMergeable;
        _btnMergeDice.gameObject.SetActive(mergerble);
    }

    void BuyCheck_UpgradeButton()
    {
        if (GameManager.Instance.Data.Money < GameManager.Instance.Data.AddDicePrice)
            _btnAddDice.image.color = Color.gray;
        else
            _btnAddDice.image.color = Color.white;

        if (GameManager.Instance.Data.Money < GameManager.Instance.Data.MergeDicePrice)
            _btnMergeDice.image.color = Color.gray;
        else
            _btnMergeDice.image.color = Color.white;

        if (GameManager.Instance.Data.Money < GameManager.Instance.Data.IncomePrice)
            _btnIncome.image.color = Color.gray;
        else
            _btnIncome.image.color = Color.white;
    }


#if UNITY_EDITOR
    protected override void OnBindSerializedField()
    {
        base.OnBindSerializedField();
        _btnAddDice = transform.Find("btnAddDice").GetComponent<Button>();
        _btnMergeDice = transform.Find("btnMergeDice").GetComponent<Button>();
        _btnIncome = transform.Find("btnIncome").GetComponent<Button>();

        _textCurrentMoney = GameObject.Find("textCurrentMoney").GetComponent<Text>();
        _textAddDice_Price = GameObject.Find("textAddDicePrice").GetComponent<Text>();
        _textMergeDice_Price = GameObject.Find("textMergeDice").GetComponent<Text>();
        _textIncome_Price = GameObject.Find("textIncome").GetComponent<Text>();

    }
#endif // UNITY_EDITOR
}
