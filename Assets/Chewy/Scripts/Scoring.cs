using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    public enum DiceScore
    {
        None,
        Double,
        Triple,
        Straight,
        FourOfKind,
        Penta,
        Hexa,
        Hepta,
        Octa,
        Novem,
        Deca,
        Undeca,
        DoDeca,
    }

    List<int> _diceNumList;
    Dictionary<int, int> _diceNumDictionary;
    public DiceScore CheckScore(List<int> diceNumList, Dictionary<int, int> diceNumDictionary, out int highNum)
    {
        _diceNumList = diceNumList;
        _diceNumDictionary = diceNumDictionary;
        highNum = 0;
        if (_diceNumList.Count < 2) return DiceScore.None;
        _diceNumList.Sort();
        _diceNumList.Reverse();
        if (IsPenta(out highNum)) return DiceScore.Penta;
        else if (IsFourKind(out highNum)) return DiceScore.FourOfKind;
        else if (IsStraight(out highNum)) return DiceScore.Straight;
        else if (IsTriple(out highNum)) return DiceScore.Triple;
        else if (IsDouble(out highNum)) return DiceScore.Double;

        return DiceScore.None;
    }

    bool IsStraight(out int highNum)
    {
        highNum = 0;
        if (_diceNumList.Count < 5) return false;
        if (StraightKind(1, 5)) 
        {
            highNum = 5;
            return true;
        }
        highNum = 6;
        return StraightKind(2, 6);
    }

    public bool StraightKind(int start, int end)
    {
        for (int i = 1; i <= 5; i++)
        {
            if (!_diceNumDictionary.ContainsKey(i)) return false;
        }
        return true;
    }
    bool IsHexa(out int highNum)
    {
        return FindCombine(out highNum, 5);
    }
    bool IsPenta(out int highNum)
    {
        return FindCombine(out highNum, 5);
    }

    bool IsFourKind(out int highNum)
    {
        return FindCombine(out highNum, 4);
    }

    bool IsTriple(out int highNum)
    {
        return FindCombine(out highNum, 3);
    }

    bool IsDouble(out int highNum)
    {
        return FindCombine(out highNum, 2);
    }

    bool FindCombine(out int highNum, int cnt)
    {
        highNum = 0;
        if (_diceNumList.Count < cnt) return false;
        for (int i = 0; i < _diceNumList.Count; i++)
        {
            int idx = _diceNumList[i];
            var num = _diceNumDictionary[idx];
            if (num != cnt) continue;
            highNum = idx;
            return true;
        }
        highNum = 0;
        return false;
    }
}
