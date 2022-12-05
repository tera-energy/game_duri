using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TrUI_PuzzleDooRie : TrUI_PuzzleManager
{
    static TrUI_PuzzleDooRie _instance;
    public static TrUI_PuzzleDooRie xInstance { get { return _instance; } }
    [SerializeField] CanvasGroup _imgTimeTense;

    IEnumerator yTimerTenseExec()
    {
        int reach = 1;
        while (true)
        {
            _imgTimeTense.DOFade(reach, 0.5f);
            yield return TT.WaitForSeconds(0.5f);
            reach = reach == 1 ? 0 : 1;

        }
        _imgTimeTense.alpha = 0;
    }
    public void zSetTimerTenseEffect()
    {
        StartCoroutine(yTimerTenseExec());
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
