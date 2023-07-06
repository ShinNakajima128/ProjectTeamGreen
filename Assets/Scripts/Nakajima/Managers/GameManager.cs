using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム全体を管理するManagerクラス
/// </summary>
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    #region property
    #endregion

    #region serialize
    #endregion

    #region private
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

    }
    #endregion

    #region public method
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
    GameEnd,
    Result
}
