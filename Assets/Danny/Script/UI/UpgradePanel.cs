using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradePanel : BehaviourBase
{
    [SerializeField] Button _btnAddDice = null;
    [SerializeField] Button _btnMergeDice = null;
    [SerializeField] Button _btnIncome = null;




#if UNITY_EDITOR
    protected override void OnBindSerializedField()
    {
        base.OnBindSerializedField();


    }
#endif // UNITY_EDITOR
}
