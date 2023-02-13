using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceNumber : MonoBehaviour
{
    [SerializeField] int number;
    bool onGround = false;
    int _groundLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        _groundLayerMask = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other) 
    {
        if (other.gameObject.layer != _groundLayerMask) return;
        Debug.Log(number);
        onGround = true;
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.layer != _groundLayerMask) return;
        onGround = false;
    }
}
