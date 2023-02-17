using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyIcon : MonoBehaviour
{
    Vector3 _startDes;
    Vector3 _endDes;
    Vector3 _endScale = new Vector3(0.7f, 0.7f, 1);

    [SerializeField] float _startSpeed;
    [SerializeField] float _endSpeed;

    public void Init(Vector3 startDes, Vector3 endDes)
    {
        _startDes = startDes;
        _endDes = endDes;
        StartCoroutine(Co_MoveIconToStartPos());
    }

    IEnumerator Co_MoveIconToEndDes()
    {
        float time = 0;
        Vector3 originPos = transform.position;
        Vector3 originSacle = transform.localScale;
        while (time <= _endSpeed)
        {
            yield return null;
            transform.position = Vector3.Lerp(originPos, _endDes, time / _endSpeed);
            transform.localScale = Vector3.Lerp(originSacle, _endScale, time / _endSpeed);
            time += Time.deltaTime;
        }

        GetComponent<Poolable>().Distroy_Pool(0.1f);
    }

    IEnumerator Co_MoveIconToStartPos()
    {
        float time = 0;
        Vector3 origin = transform.position;
        while (time <= 0.5f)
        {
            yield return null;
            transform.position = Vector3.Lerp(origin, _startDes, time / _startSpeed);
            time += Time.deltaTime;
        }
        StartCoroutine(Co_MoveIconToEndDes());

    }


}
