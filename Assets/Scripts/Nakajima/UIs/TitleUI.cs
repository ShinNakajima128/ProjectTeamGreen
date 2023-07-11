using System;
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
                    //連打防止処理。一度押されてから「5」秒経つまでは次の処理を実行しない
                    .ThrottleFirst(TimeSpan.FromMilliseconds(5000))
                    .Subscribe(_ =>PressStartButton())
                    .AddTo(this);
    }

    /// <summary>
    /// スタートボタンが押された時の処理
    /// </summary>
    private void PressStartButton()
    {
        _titlePanel.SetActive(false);
        _pressStartButtonSubject.OnNext(Unit.Default);
    }
    #endregion
}
