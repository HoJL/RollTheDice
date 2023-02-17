using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceParticle : MonoBehaviour
{
    [SerializeField] Material[] _diceHighlightMat;
    [SerializeField] GameObject _addParticle;
    [SerializeField] GameObject _mergeParticle;
    [SerializeField] ParticleSystem _textParticle;
    [SerializeField] ParticleSystem _straightParticle;
    [SerializeField] GameObject _hightlightParticle;
    [SerializeField] GameObject _hightlightParticle2;
    [SerializeField] GameObject _hightlightParticle3;
    [SerializeField] GameObject _moneyText;
    [SerializeField] GameObject _scoringMoneyText;
    [SerializeField] Vector3 _moneyTextOffset = Vector3.zero;
    Color _textColor = new Color(53/255.0f, 169/255.0f, 242/255.0f);
    CartoonFX.CFXR_ParticleText _particleText;

    void Start()
    {
        _particleText = _textParticle.GetComponent<CartoonFX.CFXR_ParticleText>();
    }
    public void ShowAddParticle(Vector3 pos, Transform parent = null)
    {
        ShowParticle(_addParticle, pos, parent, 3);
    }

    public void ShowMergeParticle(Vector3 pos, Transform parent = null)
    {
        ShowParticle(_mergeParticle, pos, parent, 3);
    }

    void ShowParticle(GameObject particle, Vector3 pos, Transform parent = null, float destroyTime = 0)
    {
        Poolable pool = GameManager.Instance.Pool.Pop(particle, parent);
        pool.transform.position = pos;
        pool.GetComponent<ParticleSystem>().Play();
        pool.Distroy_Pool(destroyTime);
    }
    public void ShowCombineParticle(Scoring.DiceScore dc, int highNum, List<Dice> dice, int rollCnt)
    {
        if (dc != Scoring.DiceScore.Straight)
            ShowTextParticle(dc);
        else 
            _straightParticle.Play();
        if (highNum <= 0) return;
        if (dc != Scoring.DiceScore.Straight)
        {
            for (int i = 0; i < rollCnt; i++)
            {
                if (dice[i].CurrentNum != highNum) continue;
                StartCoroutine(ShowGlowDiceCo(dice[i], dc));
            }
        }
        else
        {
            for (int i = highNum; i >= highNum - 4; i--)
            {
                var d = dice.Find(d => d.CurrentNum == i);
                StartCoroutine(ShowGlowDiceCo(d, dc));
            }
        }
    }

    void ShowTextParticle(string txt)
    {
        _textParticle.Stop();
        _particleText.UpdateText(txt, 1, Color.white, _textColor, Color.white, 1f);
        _textParticle.Play();
    }
    void ShowTextParticle(Scoring.DiceScore diceScore)
    {
        if (diceScore == Scoring.DiceScore.None) return;
        string s = string.Concat(diceScore.ToString(),"!");
        ShowTextParticle(s);
    }

    IEnumerator ShowGlowDiceCo(Dice dice, Scoring.DiceScore score)
    {
        float time = 0.0f;
        int matIdx = (int)dice.Grade - 1;
        if (matIdx < 0) throw new Exception("Grade is none");
        var originMat = dice.MeshRender.material;
        dice.MeshRender.material = _diceHighlightMat[matIdx];
        ShowHightlightParticle(dice.transform.position);
        if (score == Scoring.DiceScore.Triple)
        {
            ShowHightlightParticle2(dice.transform.position);
        }
        else if (score > Scoring.DiceScore.Triple)
        {
            ShowHightlightParticle3(dice.transform.position);
        }
        ShowScoringMoneyText(dice.CurrentNum, dice, (int)score + 1);
        while(time < 0.5f)
        {
            yield return null;
            time += Time.deltaTime;
        }
        dice.MeshRender.material = originMat;
    }

    public void ShowMoneyText(int num, Dice dice)
    {
        SetMoneyText(_moneyText, dice, num);
    }

    public void ShowScoringMoneyText(int num, Dice dice, int multiple)
    {
        SetMoneyText(_scoringMoneyText, dice, num, multiple);
    }

    void SetMoneyText(GameObject textObj, Dice dice, int num, int multiple = 1)
    {
        Poolable pool = GameManager.Instance.Pool.Pop(textObj, gameObject.transform);
        var mt = pool.GetComponent<ShowUpMoneyText>();
        var moneyPos = dice.transform.position;
        moneyPos.x += _moneyTextOffset.x;
        moneyPos.y += _moneyTextOffset.y;
        moneyPos.z += _moneyTextOffset.z;
        mt.transform.position = moneyPos;
        //(주사위 숫자 * 3^(등급-1)) * (1 + 0.1 * 인컴레벨)
        var money = num * Mathf.Pow(3, ((int)dice.Grade - 1)) * (1 + GameManager.Instance.Data.IncomeLv);
        money *= multiple;
        GameManager.Instance.Data.Money += money;
        mt.SetText(money);
        pool.Distroy_Pool(1.5f);
    }

    void ShowHightlightParticle(Vector3 pos)
    {
        ShowParticle(_hightlightParticle, pos, gameObject.transform, 1);
    }

    void ShowHightlightParticle2(Vector3 pos)
    {
        ShowParticle(_hightlightParticle2, pos, gameObject.transform, 1);
    }

    void ShowHightlightParticle3(Vector3 pos)
    {
        ShowParticle(_hightlightParticle3, pos, gameObject.transform, 1);
    }
}
