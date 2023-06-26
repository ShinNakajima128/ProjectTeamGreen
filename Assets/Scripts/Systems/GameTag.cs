using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム内で使用するタグを管理するクラス
/// </summary>
public class GameTag
{
    #region property
    public static string Player => _player;
    public static string Enemy => _enemy;
    #endregion

    #region Constant
    /// <summary>
    /// プレイヤー
    /// </summary>
    private static string _player = "Player";
    /// <summary>
    /// 敵
    /// </summary>
    private static string _enemy = "Enemy";
    #endregion
}
