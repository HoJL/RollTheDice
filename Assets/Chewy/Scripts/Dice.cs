using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] float _velocityFactor = 0.01f;
    Rigidbody _rb = null;
    DiceNumber[] numbers = null;
    int _currentNumber = 0;
    bool _isRolling = false;
    readonly int _numberOffset = 7;
    DiceManager.DiceGrade _grade = default;
    Action<int> _doSetNumber;
    public event Action<int> DoSetNumber
    {
        add => _doSetNumber += value;
        remove => _doSetNumber -= value;
    }
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        numbers = GetComponentsInChildren<DiceNumber>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isRolling) return;
        if (!_rb.IsSleeping()) return;
        for (int i = 0; i < numbers.Length; i++)
        {
            if (!numbers[i].OnGround) continue;
            _isRolling = false;
            _currentNumber = _numberOffset - numbers[i].Number;
            _doSetNumber(_currentNumber);
            break;
        }
    }

    public void DoRoll(DiceManager.RollInfo rollInfo)
    {
        _isRolling = true;
        _rb.AddForce(Vector3.up * rollInfo._upForce);
        _rb.AddExplosionForce(rollInfo._randomForce, rollInfo._pos, rollInfo._explosionRadius, rollInfo._upForce);
    }
}
