using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : BehaviourBase
{

    [SerializeField] Canvas _canvas;
    [SerializeField] UpgradePanel _upgradePanel;
    [SerializeField] MoneyExpolsionManager _expManager;


    public void Init()
    {
        _upgradePanel.Init();
    }

    public void UpgdateMoneyText()
    {
        _upgradePanel.UpdateMoneyText();
    }

    public void Check_Mergeable()
    {
        _upgradePanel.Check_Mergeable();
    }

#if UNITY_EDITOR
    protected override void OnBindSerializedField()
    {
        base.OnBindSerializedField();

        _canvas = this.GetComponent<Canvas>();
        _upgradePanel = this.GetComponentInChildren<UpgradePanel>();
        _expManager = this.GetComponentInChildren<MoneyExpolsionManager>();
    }
#endif // UNITY_EDITOR
}
