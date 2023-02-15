using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    public void Distroy_Pool(float time)
    {
        StartCoroutine(Co_Distroy_Pool(time));
    }

    IEnumerator Co_Distroy_Pool(float time)
    {
        yield return new WaitForSeconds(time);
        GameManager.Instance.Pool.Push(this);
    }

}
