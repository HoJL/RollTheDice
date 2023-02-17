using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyExpolsionManager : MonoBehaviour
{
    [SerializeField] Canvas _myCanvas;
    [SerializeField] GameObject _destPos;
    [SerializeField] GameObject _moneyIcon;
    [SerializeField] int _count;
    [SerializeField] float _radius;

    float screenXmin = 150.0f;
    float screenXmax = 650.0f;
    float screenYmin = 450.0f;
    float screenYmax = 950.0f;
    float posRange = 20.0f;

    Vector3 _localScale = Vector3.one;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Show_Expolsion();
        }
    }

    public void Show_Expolsion()
    {

        Vector3 pos = new Vector3(Random.Range(screenXmin, screenYmin), Random.Range(screenYmin, screenYmax), 0);
        for (int i = 0; i < _count; i++)
        {
            var rad = Mathf.Deg2Rad * i * (360 / _count);
            var x = _radius * Mathf.Sin(rad);
            var y = _radius * Mathf.Cos(rad);

            Vector3 startDes = pos + new Vector3(x + Random.Range(-posRange, posRange), y + Random.Range(-posRange, posRange), 0);

            Poolable go = GameManager.Instance.Pool.Pop(_moneyIcon);
            go.transform.localScale = _localScale;
            go.transform.SetParent(_myCanvas.transform);
            go.transform.position = pos;

            MoneyIcon mi = go.GetComponent<MoneyIcon>();
            if (mi == null)
                return;

            mi.Init(startDes, _destPos.transform.position);
        }




    }


}
