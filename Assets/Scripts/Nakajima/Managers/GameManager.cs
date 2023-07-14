using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// ゲーム全体を管理するManagerクラス
/// </summary>
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    #region property
    public GameState CurrentState => _currentState;
    #endregion

    #region serialize
    #endregion

    #region private
    private GameState _currentState = default;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        FadeManager.Fade(FadeType.In);
        AudioManager.PlayBGM(BGMType.Title);

        //ゲーム終了時にBGMを変更する処理を登録
        StageManager.Instance.GameEndObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(_ =>
                             {
                                 AudioManager.PlayBGM(BGMType.Result);
                             });
    }
    #endregion

    #region public method
    /// <summary>
    /// 現在のゲームの状況を変更する
    /// </summary>
    /// <param name="newState">次のゲームの状況</param>
    public static void ChangeGameState(GameState newState)
    {
        Instance._currentState = newState;
    }
    #endregion

    #region private method
    #endregion
}

/// <summary>
/// ゲームの状態
/// </summary>
public enum GameState
{
    Title,
    StageSelect,
    InGame,
    Result
}
