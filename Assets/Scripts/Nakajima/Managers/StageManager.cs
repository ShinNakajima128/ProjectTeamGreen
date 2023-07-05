using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class StageManager : MonoBehaviour
{
    #region property
    public static StageManager Instance { get; private set; }
    public IObservable<bool> IsInGameSubject => _isInGameSubject;
    public IObservable<Unit> GameStartSubject => _gameStartSubject;
    public IObservable<Unit> GamePauseSubject => _gamePauseSubject;
    public IObservable<Unit> GameEndSubject => _gameEndSubject;
    #endregion

    #region serialize
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    /// <summary>インゲーム中かどうかを切り替えるSubject</summary>
    private Subject<bool> _isInGameSubject = new Subject<bool>();
    /// <summary>ゲーム開始時のSubject</summary>
    private Subject<Unit> _gameStartSubject = new Subject<Unit>();
    /// <summary>ゲーム中断時のSubject</summary>
    private Subject<Unit> _gamePauseSubject = new Subject<Unit>();
    /// <summary>ゲーム終了時のSubject</summary>
    private Subject<Unit> _gameEndSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    private void Awake()
    {
        Instance = this;
    }

    private async void Start()
    {
        await UniTask.Yield();
        _isInGameSubject.OnNext(true);
    }
    #endregion

    #region public method
    /// <summary>
    /// ゲームを終了する
    /// </summary>
    public void OnGameEnd()
    {
        _gameEndSubject.OnNext(Unit.Default);
    }
    #endregion

    #region private method
    #endregion

    #region unitask Method
    #endregion
}
