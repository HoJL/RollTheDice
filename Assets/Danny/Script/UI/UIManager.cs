using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : BehaviourBase
{

    [SerializeField] Canvas _canvas;
    [SerializeField] UpgradePanel _upgradePanel;

    public void Init()
    {

    }

#if UNITY_EDITOR
    protected override void OnBindSerializedField()
    {
        base.OnBindSerializedField();

        _canvas = this.GetComponent<Canvas>();
        _upgradePanel = this.GetComponentInChildren<UpgradePanel>();
    }
#endif // UNITY_EDITOR
}
