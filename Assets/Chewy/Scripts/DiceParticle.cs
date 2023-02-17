using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceParticle : MonoBehaviour
{
    [SerializeField] Material[] _diceHighlightMat;
    [SerializeField] GameObject _addParticle;
    [SerializeField] GameObject _mergeParticle;
    [SerializeField] ParticleSystem _straightParticle;
    [SerializeField] ParticleSystem _doubleParticle;
    [SerializeField] ParticleSystem _tripleParticle;
    [SerializeField] ParticleSystem _fourkindParticle;
    [SerializeField] ParticleSystem _pentaParticle;
    [SerializeField] GameObject _hightlightParticle;
    public void ShowAddParticle(Vector3 pos, Transform parent = null)
    {
        ShowParticle(_addParticle, pos, parent);
    }

    public void ShowMergeParticle(Vector3 pos, Transform parent = null)
    {
        ShowParticle(_mergeParticle, pos, parent);
    }

    void ShowParticle(GameObject particle, Vector3 pos, Transform parent = null)
    {
        Poolable pool = GameManager.Instance.Pool.Pop(particle, parent);
        pool.transform.position = pos;
        pool.GetComponent<ParticleSystem>().Play();
        pool.Distroy_Pool(3);
    }
    public void ShowCombineParticle(Scoring.DiceScore dc, int highNum, List<Dice> dice, int rollCnt)
    {
        switch(dc)
        {
            case Scoring.DiceScore.Double:
                _doubleParticle.Play();
                break;
            case Scoring.DiceScore.Triple:
                _tripleParticle.Play();
                break;
            case Scoring.DiceScore.FourOfKind:
                _fourkindParticle.Play();
                break;
            case Scoring.DiceScore.Straight:
                _straightParticle.Play();
                break;
            case Scoring.DiceScore.Penta:
                _pentaParticle.Play();
                break;
        }
        if (highNum <= 0) return;
        if (dc != Scoring.DiceScore.Straight)
        {
            for (int i = 0; i < rollCnt; i++)
            {
                if (dice[i].CurrentNum != highNum) continue;
                StartCoroutine(ShowGlowDiceCo(dice[i]));
            }
        }
        else
        {
            for (int i = highNum; i >= highNum - 4; i--)
            {
                var d = dice.Find(d => d.CurrentNum == i);
                StartCoroutine(ShowGlowDiceCo(d));
            }
        }
    }

    IEnumerator ShowGlowDiceCo(Dice dice)
    {
        float time = 0.0f;
        int matIdx = (int)dice.Grade - 1;
        if (matIdx < 0) throw new Exception("Grade is none");
        var originMat = dice.MeshRender.material;
        dice.MeshRender.material = _diceHighlightMat[matIdx];
        ShowHightlightParticle(dice.transform.position);
        while(time < 0.5f)
        {
            yield return null;
            time += Time.deltaTime;
        }
        dice.MeshRender.material = originMat;
    }

    void ShowHightlightParticle(Vector3 pos)
    {
        Poolable pool = GameManager.Instance.Pool.Pop(_hightlightParticle, gameObject.transform);
        pool.transform.position = pos;
        pool.GetComponent<ParticleSystem>().Play();
        pool.Distroy_Pool(1);
    }
}
