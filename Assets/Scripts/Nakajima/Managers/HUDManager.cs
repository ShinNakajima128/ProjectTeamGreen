using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

/// <summary>
/// HUDの機能全般を管理するManagerクラス
/// </summary>
[RequireComponent(typeof(GameStatusUI))]
[RequireComponent(typeof(PlayerStatusUI))]
public class HUDManager : MonoBehaviour
{
    #region property
    public static HUDManager Instance { get; private set; }
    #endregion

    #region serialize
    #endregion

    #region private
    GameStatusUI _gameStatus;
    PlayerStatusUI _playerStatus;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        Instance = this;
        _gameStatus = GetComponent<GameStatusUI>();
        _playerStatus = GetComponent<PlayerStatusUI>();
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
