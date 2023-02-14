using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : BehaviourBase
{
    public enum DiceGrade
    {
        White,
        Red,
        Orange,
        Yellow,
        Green,
        Blue,
        Indigo,
        Violet
    }
    public struct RollInfo
    {
        public float _randomForce;
        public float _explosionRadius;
        public float _upForce;
        public Vector3 _pos;
        public RollInfo(float force, float radius, float upWard, Vector3 pos)
        {
            _randomForce = force;
            _explosionRadius = radius;
            _upForce = upWard;
            _pos = pos;
        }
    }
    [Serializable]
    struct MinMaxSt
    {
        public float min;
        public float max;
        public MinMaxSt(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
    
    [SerializeField] Dice[] _dice;
    [SerializeField] float _explosionRadius = 1.0f;
    [SerializeField] MinMaxSt _explosionForce = new MinMaxSt(250, 320);
    [SerializeField] MinMaxSt _minMaxRandPos = new MinMaxSt(-1.0f, 1.0f);
    [SerializeField] MinMaxSt _minMaxUpward = new MinMaxSt(1.5f, 2.0f);
    [SerializeField] float _autoTimeInterval = 1.0f;

    List<int> _diceNumList = new List<int>();
    float _time = 0.0f;
    void Init()
    {
        _diceNumList.Capacity = _dice.Length;
        for(int i = 0; i < _dice.Length; i++)
        {
            _dice[i].DoSetNumber += SetNumber;
        }
    }

    void Start() 
    {
        Init();
    }
    // Update is called once per frame
    void Update()
    {
        AutoRollTheDice();
    }

    void AutoRollTheDice()
    {
        //Debug.Log( _diceRb[0].velocity);
        if (Time.time - _time < _autoTimeInterval)
            return;
        _diceNumList.Clear();
        for(int i = 0; i < _dice.Length; i++)
        {
            RollInfo rollInfo = GetRandomRollInfo(_dice[i].transform.position);
            _dice[i].DoRoll(rollInfo);
        }
        _time = Time.time;
    }

    RollInfo GetRandomRollInfo(Vector3 orginPos)
    {
        var randX = UnityEngine.Random.Range(_minMaxRandPos.min, _minMaxRandPos.max);
        var randY = UnityEngine.Random.Range(_minMaxRandPos.min, _minMaxRandPos.max);
        var randZ = UnityEngine.Random.Range(_minMaxRandPos.min, _minMaxRandPos.max);
        var randUpward = UnityEngine.Random.Range(_minMaxUpward.min, _minMaxUpward.max);
        var force = UnityEngine.Random.Range(_explosionForce.min, _explosionForce.max);
        var pos = orginPos;
        pos.x += randX;
        pos.z += randZ;
        pos.y += randY;
        
        RollInfo rollInfo = new RollInfo(force, _explosionRadius, randUpward, pos);

        return rollInfo;
    }

    public void SetNumber(int num)
    {
        _diceNumList.Add(num);
        if (_diceNumList.Count == _dice.Length)
        {
            for (int i = 0; i < _diceNumList.Count; i++)
            {
                Debug.Log(_diceNumList[i]);
            }
        }
    }

#if UNITY_EDITOR
    protected override void OnBindSerializedField()
    {
        base.OnBindSerializedField();
        _dice = this.GetComponentsInChildren<Dice>();
    }
#endif
}
