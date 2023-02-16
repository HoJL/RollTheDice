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

    Vector3 _localScale = new Vector3(1, 1, 1);

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Show_Expolsion();
        }

    }

    void Show_Expolsion()
    {

        Vector3 pos = new Vector3(Random.Range(200.0f, 520.0f), Random.Range(400.0f, 900.0f), 0);
        for (int i = 0; i < _count; i++)
        {
            var rad = Mathf.Deg2Rad * i * (360 / _count);
            var x = _radius * Mathf.Sin(rad);
            var y = _radius * Mathf.Cos(rad);

            Vector3 startDes = pos + new Vector3(x + Random.Range(-20.0f, +20.0f), y + Random.Range(-20.0f, +20.0f), 0);

            Poolable go = GameManager.Instance.Pool.Pop(_moneyIcon);
            //go.transform.localScale = _localScale;
            go.transform.SetParent(_myCanvas.transform);
            go.transform.position = pos;

            MoneyIcon mi = go.GetComponent<MoneyIcon>();
            if (mi == null)
                return;

            mi.Init(startDes, _destPos.transform.position);

        }




    }


}
