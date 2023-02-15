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
    public DiceManager.DiceGrade Grade{get => _grade; set => _grade = value;}
    Poolable poolable;
    MeshRenderer _meshRenderer;
    SkinnedMeshRenderer [] _skinnedRenderer;
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
        poolable = GetComponent<Poolable>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isRolling) return;
        if (!_rb.IsSleeping()) return;
        var num = GetTopNumber();
        _doSetNumber?.Invoke(num);
        _isRolling = false;
        /*
        for (int i = 0; i < numbers.Length; i++)
        {
            if (!numbers[i].OnGround) continue;
            _isRolling = false;
            _currentNumber = _numberOffset - numbers[i].Number;
            _doSetNumber(_currentNumber);
            break;
        }
        */
    }

    int GetTopNumber()
    {
        float minDot = float.MaxValue;
        int num = 0;
        for (int i = 0; i < 6; i++)
        {
            Vector3 faceNormal = transform.TransformDirection(GetFaceNormal(i));
            float dot = Vector3.Dot(Vector3.down, faceNormal);
            if (dot < minDot) {
                minDot = dot;
                num = i + 1;
            }
        }
        return num;
    }

    Vector3 GetFaceNormal(int idx)
    {
        switch (idx) {
            case 0: return Vector3.up;
            case 1: return Vector3.forward;
            case 2: return Vector3.right;
            case 3: return Vector3.left;
            case 4: return Vector3.back;
            case 5: return Vector3.down;
            default: return Vector3.zero;
        }
    }

    public void DoRoll(DiceManager.RollInfo rollInfo)
    {
        _isRolling = true;
        if (_rb == null) return;
        //_rb.AddForce(UnityEngine.Random.onUnitSphere * 3.0f, ForceMode.Impulse);
        _rb.AddTorque(rollInfo._randomTorque, ForceMode.Impulse);
        _rb.AddExplosionForce(rollInfo._randomForce, rollInfo._pos, rollInfo._explosionRadius, rollInfo._upForce);
    }

    public void Init(Material mat)
    {
        if (_meshRenderer != null)
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshRenderer.material = mat;
        }
        _skinnedRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        if (_skinnedRenderer == null) return;
        for (int i = 0; i < _skinnedRenderer.Length; i++)
        {
            _skinnedRenderer[i].material = mat;
        }

    }
}
