using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceNumber : BehaviourBase
{
    [SerializeField] int _number;
    bool _onGround = false;
    public bool OnGround {get => _onGround;}
    public int Number {get => _number;}
    int _groundLayer;
    // Start is called before the first frame update
    void Start()
    {
        _groundLayer = LayerMask.NameToLayer("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other) 
    {
        if (other.gameObject.layer == _groundLayer)
        {
            _onGround = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.layer == _groundLayer)
        {
            _onGround = false;
        }
    }

#if UNITY_EDITOR
    protected override void OnBindSerializedField()
    {
        base.OnBindSerializedField();
        _number = int.Parse(gameObject.name);
    }
#endif
}
