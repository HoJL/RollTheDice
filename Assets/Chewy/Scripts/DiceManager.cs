using System.Text;
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
        Triple,
        Straight,
        FourOfKind
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
    [SerializeField] int _diceMax = 7;
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
    [Space(10)]
    [Header("DiceMaterial")]
    [SerializeField] Material[] _diceMat;
    [SerializeField] Material[] _diceHighlightMat;
    [Space(10)]
    [Header("Particle")]
    [SerializeField] GameObject _addParticle;
    [SerializeField] GameObject _mergeParticle;
    [SerializeField] ParticleSystem _straightParticle;
    [SerializeField] ParticleSystem _doubleParticle;
    [SerializeField] ParticleSystem _tripleParticle;
    [SerializeField] ParticleSystem _fourkindParticle;
    [SerializeField] GameObject _moneyText;
    [SerializeField] Vector3 _moneyTextOffset = Vector3.zero;
    List<int> _diceNumList = new List<int>();
    Dictionary<int, int> _diceNumDictionary = new Dictionary<int, int>();
    float _time = 0.0f;
    Coroutine speedCo = null;
    DiceGrade _mergeableGrade;
    bool _isMergeable;
    bool _isRoll = false;
    int _rollCnt = 0;
    public bool IsMergeable { get => _isMergeable; }
    public void Init()
    {
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
            var rand = UnityEngine.Random.insideUnitSphere * 2.0f + Vector3.up * 3.0f;
            AddDice(rand, Quaternion.identity);
            //ShowAddParticle(rand, gameObject.transform);
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
        //if (_isRoll) return;
        _isRoll = true;
        _diceNumList.Clear();
        _diceNumDictionary.Clear();
        _rollCnt = _dice.Count;
        for (int i = 0; i < _dice.Count; i++)
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

    public void AddDice(Vector3 pos, Quaternion rot, DiceGrade grade = DiceGrade.Red, bool isMerge = false)
    {
        Poolable newDice = GameManager.Instance.Pool.Pop(_dicePrefab, gameObject.transform);
        newDice.transform.position = pos;
        if (!isMerge) ShowAddParticle(pos, gameObject.transform);
        else ShowMergeParticle(pos, gameObject.transform);
        Dice d = newDice.GetComponent<Dice>();
        d.Grade = grade;
        d.Init(_diceMat[(int)grade - 1]);
        d.DoSetNumber += DoSetNumber;
        _dice.Add(d);
        _isMergeable = TryGetMergeableGrade(out _mergeableGrade);
        Debug.Log(_isMergeable);
    }

    public void ShowAddParticle(Vector3 pos, Transform parent = null)
    {
        ShowParticle(_addParticle, pos, parent);
    }

    public void ShowMergeParticle(Vector3 pos, Transform parent = null)
    {
        ShowParticle(_mergeParticle, pos, parent);
    }

    void ShowParticle(GameObject particle, Vector3 pos, Transform parent = null)
    {
        Poolable pool = GameManager.Instance.Pool.Pop(particle, parent);
        pool.transform.position = pos;
        pool.GetComponent<ParticleSystem>().Play();
        pool.Distroy_Pool(3);
    }

    public void RemoveDice(Dice dice)
    {
        dice.DoSetNumber -= DoSetNumber;
        Poolable poolable = dice.GetComponent<Poolable>();
        poolable.Distroy_Pool(0);
    }

    public void MergeDice()
    {
        if (_dice.Count < 2) return;
        if (!_isMergeable) return;
        StartCoroutine(MergeCo());
    }

    IEnumerator MergeCo()
    {
        Dice[] dice = PopTwoDiceByGrade(_mergeableGrade);
        if (dice == null) yield break;
        var mergePos = dice[0].transform.position;
        mergePos.y = _mergeHeight;
        var time = 0.0f;
        var d1Pos = dice[0].transform.position;
        var d2Pos = dice[1].transform.position;
        while (time < _mergeTime)
        {
            yield return null;
            dice[0].transform.position = Vector3.Lerp(d1Pos, mergePos, time / _mergeTime);
            dice[1].transform.position = Vector3.Lerp(d2Pos, mergePos, time / _mergeTime);
            time += Time.deltaTime;
        }
        dice[0].GetComponent<Rigidbody>().velocity = Vector3.zero;
        dice[1].GetComponent<Rigidbody>().velocity = Vector3.zero;
        //Merge
        RemoveDice(dice[0]);
        RemoveDice(dice[1]);
        if (_isRoll) _rollCnt -= 2;
        AddDice(mergePos, Quaternion.identity, (DiceGrade)(_mergeableGrade + 1));
        ShowMergeParticle(mergePos, gameObject.transform);
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

    Dice[] PopTwoDiceByGrade(DiceGrade grade)
    {
        List<Dice> list = new List<Dice>();
        if (_dice.Count < 2) return null;
        for (int i = 0; i < _dice.Count; i++)
        {
            if (_dice[i].Grade != grade) continue;
            list.Add(_dice[i]);
            if (list.Count == 2)
            {
                for (int j = 0; j < list.Count; j++)
                    _dice.Remove(list[j]);
                return list.ToArray();
            }
        }
        return null;
    }

    IEnumerator SpeedUpCo()
    {
        Time.timeScale = 2.5f;
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1;
    }

    DiceCombine CheckScore(out int highNum)
    {
        highNum = 0;
        if (_diceNumList.Count < 2) return DiceCombine.None;
        if (IsStraight()) return DiceCombine.Straight;
        _diceNumList.Reverse();
        return IsDouble(out highNum);
    }

    bool IsStraight()
    {
        int cnt = 0;
        if (_diceNumList.Count < 5) return false;
        _diceNumList.Sort();
        for (int i = 0; i < _diceNumList.Count - 1; i++)
        {
            var abst = _diceNumList[i + 1] - _diceNumList[i];
            if (abst == 0) continue;
            if (abst != 1) return false;
            cnt++;
        }
        if (cnt >= 4) return true;
        return false;
    }

    DiceCombine IsFourKind(out int highNum)
    {
        return FindCombine(out highNum, 4);
    }

    DiceCombine IsDouble(out int highNum)
    {
        return FindCombine(out highNum, 2);
    }

    DiceCombine FindCombine(out int highNum, int cnt)
    {
        for (int i = 0; i < _diceNumList.Count; i++)
        {
            int idx = _diceNumList[i];
            var num = _diceNumDictionary[idx];
            if (num != cnt) continue;
            highNum = idx;
            return DiceCombine.FourOfKind;
        }
        highNum = 0;
        return DiceCombine.None;
    }

    public void DoSetNumber(int num, Dice dice)
    {
        _diceNumList.Add(num);
        ShowMoneyText(num, dice);
        if (!_diceNumDictionary.ContainsKey(num))
        {
            _diceNumDictionary[num] = 0;
        }
        _diceNumDictionary[num]++;
        if (_diceNumList.Count == _rollCnt)
        {
            _isRoll = false;
            testPrint();
            int highNum;
            DiceCombine dc = CheckScore(out highNum);
            Debug.Log(dc);
            ShowCombineParticle(dc, highNum);
        }
    }

    void ShowCombineParticle(DiceCombine dc, int highNum)
    {
        switch(dc)
        {
            case DiceCombine.Double:
                _doubleParticle.Play();
                break;
            case DiceCombine.Triple:
                _tripleParticle.Play();
                break;
            case DiceCombine.FourOfKind:
                _fourkindParticle.Play();
                break;
            case DiceCombine.Straight:
                _straightParticle.Play();
                break;
        }
        if (highNum <= 0) return;
        Debug.Log(highNum);
        for (int i = 0; i < _rollCnt; i++)
        {
            if (_dice[i].CurrentNum != highNum) continue;
            StartCoroutine(ShowGlowDiceCo(_dice[i]));
        }
    }

    IEnumerator ShowGlowDiceCo(Dice dice)
    {
        float time = 0.0f;
        int matIdx = (int)dice.Grade - 1;
        if (matIdx < 0) throw new Exception("Grade is none");
        dice.MeshRender.material = _diceHighlightMat[matIdx];
        while(time < 0.5f)
        {
            yield return null;
            time += Time.deltaTime;
        }
        dice.MeshRender.material = _diceMat[matIdx];
    }

    void ShowMoneyText(int num, Dice dice)
    {
        Poolable pool = GameManager.Instance.Pool.Pop(_moneyText, gameObject.transform);
        var mt = pool.GetComponent<ShowUpMoneyText>();
        var moneyPos = dice.transform.position;
        moneyPos.x += _moneyTextOffset.x;
        moneyPos.y += _moneyTextOffset.y;
        moneyPos.z += _moneyTextOffset.z;
        mt.transform.position = moneyPos;
        //(주사위 숫자 * 3^(등급-1)) * (1 + 0.1 * 인컴레벨)
        var money = num * Mathf.Pow(3, ((int)dice.Grade - 1)) * (1 + GameManager.Instance.Data.IncomeLv);
        GameManager.Instance.Data.Money += money;
        mt.SetText(money);
        pool.Distroy_Pool(1.5f);
    }

    public void testPrint()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _diceNumList.Count; i++)
        {
            sb.Append(_diceNumList[i]);
            sb.Append(", ");
        }
        Debug.Log(sb.ToString());
    }

#if UNITY_EDITOR
    protected override void OnBindSerializedField()
    {
        base.OnBindSerializedField();
        _dice.AddRange(this.GetComponentsInChildren<Dice>());
    }
#endif
}
