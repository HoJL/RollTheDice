using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : BehaviourBase
{
    [SerializeField] DiceManager _diceManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

#if UNITY_EDITOR
    protected override void OnBindSerializedField()
    {
        base.OnBindSerializedField();
        _diceManager = this.GetComponentInChildren<DiceManager>();
    }
#endif

}
