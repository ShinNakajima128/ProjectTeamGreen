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
    public EnemyType EnemyType => _enemyType;
    public EnemyActionType ActionType => _actionType;
    public int HP => _hp;
    public float AttackAmount => _attackAmount;
    public float ApproachDistance => _approachDistance;
    public ItemType DropItemType => _dropItemType;
    public float KnockbackAmount => _knockbackAmount;
    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("敵の種類")]
    [SerializeField]
    private EnemyType _enemyType = default;

    [Tooltip("敵の行動の種類")]
    [SerializeField]
    private EnemyActionType _actionType = default;

    [Tooltip("敵のHP")]
    [SerializeField]
    private int _hp = 10;

    [Tooltip("攻撃力")]
    [SerializeField]
    private float _attackAmount = 1;

    [Tooltip("プレイヤーに接触するまでの距離")]
    [SerializeField]
    private float _approachDistance = 0.1f;

    [Tooltip("ドロップするアイテムの種類")]
    [SerializeField]
    private ItemType _dropItemType = default;

    [Tooltip("攻撃が当たった時の吹き飛ぶ力")]
    [SerializeField]
    private float _knockbackAmount = 0;
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
/// 敵の種類
/// </summary>
public enum EnemyType
{
    Wave1_Chase1,
    Wave1_Chase2,
    Wave1_Point1,
    Wave1_Boss,
    Wave2_Chase1,
    Wave2_Chase2,
    Wave2_Point1,
    Wave2_Boss,
    Wave3_Chase1,
    Wave3_Chase2,
    Wave3_Point1,
    Wave3_Boss,
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
