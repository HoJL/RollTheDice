using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyExpolsionManager : MonoBehaviour
{
    [SerializeField] Canvas _myCanvas;
    [SerializeField] Transform _destPos;
    [SerializeField] GameObject _moneyIcon;
    [SerializeField] int _count;
    [SerializeField] float _radius;

    Vector3 _localScale = new Vector3(1, 1, 1);

    void Start()
    {
        Vector3 pos = new Vector3(500, 500, 0);
        Show_Expolsion(pos);
    }

    void Show_Expolsion(Vector3 pos)
    {
        for (int i = 0; i < _count; i++)
        {
            var rad = Mathf.Deg2Rad * i * (360 / _count);
            var x = _radius * Mathf.Sin(rad);
            var y = _radius * Mathf.Cos(rad);

            GameObject go = Instantiate(_moneyIcon);
            go.transform.localScale = _localScale;
            go.transform.SetParent(_myCanvas.transform);
            go.transform.position = pos + new Vector3(x, y, 0);

        }




    }


}
