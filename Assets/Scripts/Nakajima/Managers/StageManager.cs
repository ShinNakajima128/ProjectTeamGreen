using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class StageManager : MonoBehaviour
{
    #region property
    public static StageManager Instance { get; private set; }
    public Subject<bool> IsInGameSubject => _isInGameSubject;
    public Subject<Unit> GameStartSubject => _gameStartSubject;
    public Subject<Unit> GamePauseSubject => _gamePauseSubject;
    public Subject<Unit> GameEndSubject => _gameEndSubject;
    #endregion

    #region serialize
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    private Subject<bool> _isInGameSubject = new Subject<bool>();
    private Subject<Unit> _gameStartSubject = new Subject<Unit>();
    private Subject<Unit> _gamePauseSubject = new Subject<Unit>();
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
    #endregion

    #region private method
    #endregion

    #region unitask Method
    #endregion
}
