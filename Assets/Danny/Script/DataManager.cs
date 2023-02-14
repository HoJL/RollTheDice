using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    public float AddDivePriceRatio = 1.1f;
    public float MergeDicePriceRatio = 1.1f;
    public float IncomePriceRatio = 1.1f;

    double _money = 0;
    int _addDiceLv = 1;
    int _mergeDiceLv = 1;
    int _incomeLv = 1;

    public double Money { get { return _addDiceLv; } }
    public int AddDiveLv { get { return _addDiceLv; } }
    public int MergeDiceLv { get { return _mergeDiceLv; } }
    public int IncomeLv { get { return _incomeLv; } }

}
