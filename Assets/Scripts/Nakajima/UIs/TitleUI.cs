﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using TMPro;
using DG.Tweening;

/// <summary>
/// タイトル画面の機能を持つコンポーネント
/// </summary>
public class TitleUI : MonoBehaviour
{
    #region property
    public IObservable<Unit> PressStartButtonObserver => _pressStartButtonSubject;
    #endregion

    #region serialize
    [Header("UIObjects")]
    [Tooltip("タイトル画面のPanel")]
    [SerializeField]
    private GameObject _titlePanel = default;

    [Tooltip("スタートボタン")]
    [SerializeField]
    private Button _startButton = default;
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    /// <summary>スタートボタンが押された時のSubject</summary>
    private Subject<Unit> _pressStartButtonSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    private void Awake()
    {
        Setup();
    }

    private void Start()
    {
        //アクティブ状態の切り替え処理を登録
        SignalManager.Instance.ChangeTitleActiveObserver
                     .TakeUntilDestroy(this)
                     .Subscribe(_ => ChangeActive());
    }
    #endregion

    #region public method
    #endregion

    #region private method
    /// <summary>
    /// 初期化
    /// </summary>
    private void Setup()
    {
        _titlePanel.SetActive(true);
        
        //スタートボタンが押された時の処理を登録
        _startButton.OnClickAsObservable()
                    .TakeUntilDestroy(this)
                    //連打防止処理。一度押されてから「5」秒経つまでは次の処理を実行しない
                    .ThrottleFirst(TimeSpan.FromMilliseconds(5000))
                    .Subscribe(_ =>PressStartButton());
    }

    /// <summary>
    /// スタートボタンが押された時の処理
    /// </summary>
    private void PressStartButton()
    {
        FadeManager.Fade(FadeType.Out, () =>
        {
            FadeManager.Fade(FadeType.In);
            _titlePanel.SetActive(false);
            _pressStartButtonSubject.OnNext(Unit.Default);
            AudioManager.PlayBGM(BGMType.InGame);
        });
    }

    /// <summary>
    /// タイトルパネルのアクティブ状態の切り替え
    /// </summary>
    private void ChangeActive()
    {
        //タイトルパネルの親のキャンバス
        var parentCanvas = _titlePanel.transform.parent.gameObject;

        if (parentCanvas.activeSelf)
        {
            parentCanvas.SetActive(false);
        }
        else
        {
            parentCanvas.SetActive(true);
        }
    }
    #endregion
}
