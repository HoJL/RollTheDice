using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
        public Vector3 _randomTorque;
        public RollInfo(float force, float radius, float upWard, Vector3 pos, Vector3 torque)
        {
            _randomForce = force;
            _explosionRadius = radius;
            _upForce = upWard;
            _pos = pos;
            _randomTorque = torque;
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
    [SerializeField] MinMaxSt _minMaxTorque = new MinMaxSt(-300.0f, 300.0f);
    [SerializeField] float _autoTimeInterval = 1.0f;
    [SerializeField] float _offset = 0.5f;
    [SerializeField] GameObject _dicePrefab;
    List<int> _diceNumList = new List<int>();
    float _time = 0.0f;
    Coroutine speedCo = null;
    public void Init()
    {
        _diceNumList.Capacity = _dice.Length;
        for(int i = 0; i < _dice.Length; i++)
        {
            _dice[i].DoSetNumber += SetNumber;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (speedCo != null)
                StopCoroutine(speedCo);
            speedCo = StartCoroutine(SpeedUpCo());
        }
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
        //var randY = UnityEngine.Random.Range(_minMaxRandPos.min, _minMaxRandPos.max);
        var randZ = UnityEngine.Random.Range(_minMaxRandPos.min, _minMaxRandPos.max);
        var randUpward = UnityEngine.Random.Range(_minMaxUpward.min, _minMaxUpward.max);
        var force = UnityEngine.Random.Range(_explosionForce.min, _explosionForce.max);
        var tX = UnityEngine.Random.Range(_minMaxTorque.min, _minMaxTorque.max);
        var tY = UnityEngine.Random.Range(_minMaxTorque.min, _minMaxTorque.max);
        var tZ = UnityEngine.Random.Range(_minMaxTorque.min, _minMaxTorque.max);

        var pos = orginPos;
        if (randX < 0) pos.x += (-_offset + randX);
        else pos.x += (_offset + randX);
        if (randZ < 0) pos.z += (-_offset + randZ);
        else pos.z += (_offset + randZ);

        RollInfo rollInfo = new RollInfo(force, _explosionRadius, randUpward, pos, new Vector3(tX, tY, tZ));

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

    void CheckScore()
    {
        for (int i = 0; i < _diceNumList.Count; i++)
        {
            
        }
    }

    public void AddDice()
    {
        Poolable newDice = GameManager.Instance.Pool.Pop(_dicePrefab, gameObject.transform);
        newDice.transform.position = Vector3.zero + Vector3.up * 2;
    }
    public void RemoveDice(Poolable poolable)
    {
        GameManager.Instance.Pool.Push(poolable);
    }
    void MergeDice()
    {

    }

    IEnumerator SpeedUpCo()
    {
        Time.timeScale = 2.5f;
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1;
    }

#if UNITY_EDITOR
    protected override void OnBindSerializedField()
    {
        base.OnBindSerializedField();
        _dice = this.GetComponentsInChildren<Dice>();
    }
#endif
}
