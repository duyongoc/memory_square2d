using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewInGame : View
{


    // inspector
    [Header("Text")]
    [SerializeField] private TMP_Text textLevel;
    [SerializeField] private TMP_Text textRemain;
    [SerializeField] private TMP_Text textWrong;

    [Space(10)]
    [SerializeField] private Slider sliderTimer;


    // private
    private bool _cancelCounting = false;



    #region  UNITY
    // private void Start()
    // {
    // }

    // private void Update()
    // {
    // }
    #endregion



    #region STATE
    public override void StartState()
    {
        base.StartState();
        StartView();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        UpdateView();
    }

    public override void EndState()
    {
        base.EndState();
        EndView();
    }
    #endregion



    private void StartView()
    {
    }

    private void UpdateView()
    {
    }

    private void EndView()
    {
    }



    public void StartCountTime(float value)
    {
        _cancelCounting = false;
        CountingTime(value, () => { GameScene.Instance.ShowLose($"Out of time!\n Do you wanna play again? "); });
    }


    public void CancelCounting()
    {
        _cancelCounting = true;
        SoundMgr.StopSFX(SoundMgr.SFX_TIMECOUNT);
    }


    public async void CountingTime(float value, Action callback)
    {
        float currentTimer = value;
        sliderTimer.maxValue = value;
        sliderTimer.value = value;
        bool playsound = false;

        while (currentTimer >= 0 && !_cancelCounting)
        {
            currentTimer -= .01f;
            sliderTimer.value = currentTimer;

            if(currentTimer <= 2 && !playsound)
            {
                SoundMgr.PlaySFXOneShot(SoundMgr.SFX_TIMECOUNT);
                playsound = true;
            }

            await UniTask.Delay(System.TimeSpan.FromSeconds(0.01f));
        }

        // out of time - lose game 
        if (currentTimer < 0)
        {
            callback?.Invoke();
        }
    }


    public void UpdateLevel(int level)
    {
        textLevel.text = $"Level: {level}";
    }


    public void UpdateWrongText(int wrong)
    {
        PlayWrongAnimation();
        textWrong.text = $"Wrong: {wrong}";
    }


    public void UpdateSquareRemain(int remain, int total)
    {
        textRemain.text = $"Squares remain: {remain}/{total}";
    }


    private void PlayWrongAnimation()
    {
        textWrong.transform.DOScale(Vector3.one * 1.2f, 1).SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                textWrong.transform.localScale = Vector3.one;
            });
    }


    public void ResetData()
    {
        textLevel.text = "00";
    }


}
