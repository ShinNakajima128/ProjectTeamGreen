﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

/// <summary>
/// 時間関連の機能を管理するManagerクラス
/// </summary>
public class TimeManager : MonoBehaviour
{
    #region property
    public static TimeManager Instance { get; private set; }
    public IObservable<EnemyType> BossEventObserver => _bossEventSubject;
    #endregion

    #region serialize
    [Header("variables")]
    [Tooltip("制限時間(分)")]
    [SerializeField]
    private uint _limitTime = 15;

    [Tooltip("各ボスイベント")]
    [SerializeField]
    private BossEvent[] _bossEvents;

    [Tooltip("制限時間を表示するTMP")]
    [SerializeField]
    private TextMeshProUGUI _limitTimeTMP = default;
    #endregion

    #region private
    private ReactiveProperty<uint> _currentLimitTime = new ReactiveProperty<uint>();
    #endregion

    #region Constant
    private const uint TIME_SECOND = 1;
    #endregion

    #region Event
    private Subject<EnemyType> _bossEventSubject = new Subject<EnemyType>();
    private IObservable<long> _currentTimeEvent;
    private IDisposable _resetEvent;
    private Coroutine _currentCoroutine;
    #endregion

    #region unity methods
    private void Awake()
    {
        Instance = this;
        _currentLimitTime.Value = _limitTime * 60;
    }

    private void Start()
    {
        //ゲーム開始時の処理を登録
        StageManager.Instance.GameStartObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(_ => OnLimitAndEventTimer());

        _currentLimitTime.TakeUntilDestroy(this)
                         .Where(value => value >= 0)
                         .Subscribe(value => 
                         {
                             _limitTimeTMP.text = $"{value / 60:00} : {(value % 60):00}";

                             if (value <= 0)
                             {
                                 _limitTimeTMP.enabled = false;
                             }
                         });

        //ゲームリセット時の処理を登録
        StageManager.Instance.GameResetObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(_ => 
                             {
                                 OnReset();
                             });
    }
    #endregion

    #region public method
    #endregion

    #region private method
    /// <summary>
    /// 指定した時間にイベントを行うタイマーを起動する
    /// </summary>
    private void OnLimitAndEventTimer()
    {
        //指定された時間毎にイベントを発行するタイマーを起動
        _currentTimeEvent = _bossEvents.Select(e => Observable.Timer(TimeSpan.FromSeconds(e.InvokeTime)))
                                       .Merge();
        
        uint currentBossEventIndex = 0;

        //ボスイベント発光の処理を登録
        _resetEvent = _currentTimeEvent.TakeUntilDestroy(this)
                         .Subscribe(_ =>
                         {
                             _bossEventSubject.OnNext(_bossEvents[currentBossEventIndex].BossType);
                             currentBossEventIndex++;
                         });

        _currentCoroutine = StartCoroutine(OnLimitTimerCoroutine());
    }
    #endregion

    #region coroutine method
    private IEnumerator OnLimitTimerCoroutine()
    {
        var interval = new WaitForSeconds(1.0f);
        Debug.Log("開始");
        yield return interval;

        while (_currentLimitTime.Value > 0)
        {
            _currentLimitTime.Value -= TIME_SECOND;
            yield return interval;
        }
    }

    private void OnReset()
    {
        if (_resetEvent != null)
        {
            _resetEvent.Dispose();
            _resetEvent = null;
        }

        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }

        _currentLimitTime.Value = _limitTime * 60;
        _limitTimeTMP.enabled = true;
    }
    #endregion
}

/// <summary>
/// ボスイベントのデータ
/// </summary>
[Serializable]
public struct BossEvent
{
    public string EventName;
    public uint InvokeTime;
    public EnemyType BossType;
}
