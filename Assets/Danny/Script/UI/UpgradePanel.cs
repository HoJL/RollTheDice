using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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


    public void Init()
    {
        OnClickAddDice.RemoveAllListeners();
        OnClickAddDice.AddListener(OnPressUpgradeButton);
        OnClickMergeDice.RemoveAllListeners();
        OnClickMergeDice.AddListener(OnPressUpgradeButton);
        OnClickIncome.RemoveAllListeners();
        OnClickIncome.AddListener(OnPressUpgradeButton);
    }

    void OnPressUpgradeButton()
    {
        switch (_buttontype)
        {
            case Buttons.BtnAddDice:
                //add

                break;
            case Buttons.BtnMergeDice:
                //merge

                break;
            case Buttons.BtnIncome:
                //income

                break;
        }
    }

    public void UpdateMoneyText()
    {
        _textCurrentMoney.text = $"{GameManager.Instance.Data.Money}";
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
