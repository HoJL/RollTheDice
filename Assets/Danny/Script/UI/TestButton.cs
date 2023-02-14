using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    float num = 500;
    public void AddMoney()
    {

        GameManager.Instance.Data.Money += num;
        num = num * 1.5f;
    }

}
