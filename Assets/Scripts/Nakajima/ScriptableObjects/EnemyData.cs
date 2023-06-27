using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵のデータをまとめたScriptableObjectクラス
/// </summary>
[CreateAssetMenu(menuName = "MyScriptable/EnemyData")]
public class EnemyData : ScriptableObject
{
    #region property
    public EnemyActionType Type => _actionType;
    public int HP => _hp;
    public int AttackAmount => _attackAmount;
    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("敵の行動の種類")]
    [SerializeField]
    private EnemyActionType _actionType = default;

    [Tooltip("敵のHP")]
    [SerializeField]
    private int _hp = 10;

    [Tooltip("攻撃力")]
    [SerializeField]
    private int _attackAmount = 1;

    [Tooltip("プレイヤーに接触するまでの距離")]
    [SerializeField]
    private float _approachDistance = 0.1f;
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
}

/// <summary>
/// 敵の行動の種類
/// </summary>
public enum EnemyActionType
{
    /// <summary>動かない</summary>
    None,
    /// <summary>プレイヤーを追跡する</summary>
    Chase,
    /// <summary>各ボスの行動を行う</summary>
    Boss
}
