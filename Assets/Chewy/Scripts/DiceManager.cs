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
        None,
        Red,
        Orange,
        Yellow,
        Green,
        Blue,
        Indigo,
        Violet
    }

    public enum DiceCombine
    {
        None,
        Double,
        Straight
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
    
    [SerializeField] List<Dice> _dice = new List<Dice>();
    [SerializeField] float _explosionRadius = 1.0f;
    [SerializeField] MinMaxSt _explosionForce = new MinMaxSt(250, 320);
    [SerializeField] MinMaxSt _minMaxRandPos = new MinMaxSt(-1.0f, 1.0f);
    [SerializeField] MinMaxSt _minMaxUpward = new MinMaxSt(1.5f, 2.0f);
    [SerializeField] MinMaxSt _minMaxTorque = new MinMaxSt(-300.0f, 300.0f);
    [SerializeField] float _autoTimeInterval = 1.0f;
    [SerializeField] float _offset = 0.5f;
    [SerializeField] GameObject _dicePrefab;
    [SerializeField] float _mergeHeight = 4.5f;
    [SerializeField] float _mergeTime = 1.0f;
    [SerializeField] Material[] _diceMat;
    List<int> _diceNumList = new List<int>();
    Dictionary<int ,int> _diceNumDictionary = new Dictionary<int, int>();
    float _time = 0.0f;
    Coroutine speedCo = null;
    DiceGrade _mergeableGrade;
    bool _isMergeable;
    public bool IsMergeable {get => _isMergeable;}
    public void Init()
    {
        _diceNumList.Capacity = _dice.Count;
        for(int i = 0; i < _dice.Count; i++)
        {
            _dice[i].DoSetNumber += DoSetNumber;
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
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddDice(UnityEngine.Random.insideUnitSphere * 2.0f + Vector3.up * 3.0f);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            MergeDice();
        }
        AutoRollTheDice();
    }

    void AutoRollTheDice()
    {
        if (_dice == null) return;
        if (_dice.Count == 0) return;
        if (Time.time - _time < _autoTimeInterval)
            return;
        _diceNumList.Clear();
        _diceNumDictionary.Clear();
        for(int i = 0; i < _dice.Count; i++)
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

    DiceCombine CheckScore()
    {
        if (_diceNumList.Count < 2) return DiceCombine.None;
        if (IsStraight()) return DiceCombine.Straight;

        if (IsDouble()) return DiceCombine.Double;

        return DiceCombine.None;
    }

    bool IsStraight()
    {
        if (_diceNumList.Count < 5) return false;
        _diceNumList.Sort();
        for (int i = 0; i < _diceNumList.Count - 1; i++)
        {
            var abst = _diceNumList[i + 1] - _diceNumList[i];
            if (abst == 0) continue;
            if (abst != 1) return false;
        }
        return true;
    }
    
    bool IsDouble()
    {
        for (int i = 0; i < _diceNumList.Count; i++)
        {
            int idx = _diceNumList[i];
            if (_diceNumDictionary[idx] >= 2) return true;
        }
        return false;
    }

    public void AddDice(Vector3 pos, DiceGrade grade = DiceGrade.Red)
    {
        Poolable newDice = GameManager.Instance.Pool.Pop(_dicePrefab, gameObject.transform);
        newDice.transform.position = pos;
        Dice d = newDice.GetComponent<Dice>();
        d.Grade = grade;
        d.Init(_diceMat[(int)grade - 1]);
        d.DoSetNumber += DoSetNumber;
        _dice.Add(d);
        _isMergeable = TryGetMergeableGrade(out _mergeableGrade);
        Debug.Log(_isMergeable);
    }

    public void RemoveDice(Poolable poolable)
    {
        poolable.Distroy_Pool(0);
    }

    void MergeDice()
    {
        if (_dice.Count < 2) return;
        if (!_isMergeable) return;
        StartCoroutine(MergeCo());
    }

    IEnumerator MergeCo()
    {
        Dice d1 = PopDiceByGrade(_mergeableGrade);
        Dice d2 = PopDiceByGrade(_mergeableGrade);
        var mergePos = d1.transform.position;
        mergePos.y = _mergeHeight;
        var time = 0.0f;
        var d1Pos = d1.transform.position;
        var d2Pos = d2.transform.position;
        while(time < _mergeTime)
        {
            yield return null;
            d1.transform.position = Vector3.Lerp(d1Pos, mergePos, time / _mergeTime);
            d2.transform.position = Vector3.Lerp(d2Pos, mergePos, time / _mergeTime);
            time += Time.deltaTime;
        }
        //Merge
        GameManager.Instance.Pool.Push(d1.GetComponent<Poolable>());
        GameManager.Instance.Pool.Push(d2.GetComponent<Poolable>());
        AddDice(mergePos, (DiceGrade)(_mergeableGrade + 1));
        //effect
    }

    bool TryGetMergeableGrade(out DiceGrade grade)
    {
        grade = (DiceGrade)0;
        if (_dice == null) return false;
        if (_dice.Count < 2) return false;
        var enumLength = Enum.GetValues(typeof(DiceGrade)).Length;
        for (int j = 1; j < enumLength - 1; j++)
        {
            var list = _dice.FindAll(d => d.Grade == (DiceGrade)j);
            if (list.Count < 2) continue;
            grade = list[0].Grade;
            return true;
        }
        return false;
    }

    int FindDiceIndexByGrade(DiceGrade grade)
    {
        if (_dice == null || _dice.Count == 0) return -1;
        for (int i = 0; i < _dice.Count; i++)
        {
            if (_dice[i].Grade != grade) continue;
            return i;
        }
        return -1;
    }

    Dice PopDiceByGrade(DiceGrade grade)
    {
        int idx = FindDiceIndexByGrade(grade);
        if (idx < 0) return null;
        Dice d = _dice[idx];
        _dice.RemoveAt(idx);
        return d;
    }

    IEnumerator SpeedUpCo()
    {
        Time.timeScale = 2.5f;
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1;
    }


    public void DoSetNumber(int num)
    {
        _diceNumList.Add(num);
        if (!_diceNumDictionary.ContainsKey(num))
        {
            _diceNumDictionary[num] = 0;
        }
        _diceNumDictionary[num]++;
        if (_diceNumList.Count == _dice.Count)
        {
            Debug.Log(CheckScore());
        }
    }


#if UNITY_EDITOR
    protected override void OnBindSerializedField()
    {
        base.OnBindSerializedField();
        _dice.AddRange(this.GetComponentsInChildren<Dice>());
    }
#endif
}
