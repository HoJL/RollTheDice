using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    [SerializeField] Rigidbody[] _diceRb;
    [SerializeField] float _explosionFoce = 1.0f;
    [SerializeField] float _explosionRadius = 1.0f;
    [SerializeField] float _minRandPos = -2.0f;
    [SerializeField] float _maxRandPos = 2.0f;
    [SerializeField] float _minRandUpward = 1.0f;
    [SerializeField] float _maxRandUpward = 5.0f;
    [SerializeField] float _autoTimeInterval = 1.0f;
    

    float _time = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AutoRollTheDice();
    }

    void AutoRollTheDice()
    {
        if (Time.time - _time < _autoTimeInterval)
            return;
        float randX;
        float randY;
        float randZ;
        float randUpward;
        for(int i = 0; i < _diceRb.Length; i++)
        {
            randX = Random.Range(_minRandPos, _maxRandPos);
            randY = Random.Range(_minRandPos, _maxRandPos);
            randZ = Random.Range(_minRandPos, _maxRandPos);
            randUpward = Random.Range(_minRandUpward, _maxRandUpward);
            var pos = _diceRb[i].transform.position;
            pos.x += randX;
            pos.z += randZ;
            pos.y += randY;
            _diceRb[i].AddExplosionForce(_explosionFoce, pos, _explosionRadius, randUpward);
        }
        _time = Time.time;
    }
}
