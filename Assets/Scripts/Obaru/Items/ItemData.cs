using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテムデータ
/// </summary>
[CreateAssetMenu(menuName ="MyScriptable/ItemData")]
public class ItemData : ScriptableObject
{
    #region property
    public ItemType ItemType => _itemType;
    public uint ReserveAmount => _reserveAmount;
    public uint ActivationLimit => _activationLimit;
    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("アイテムの種類")]
    [SerializeField]
    private ItemType _itemType = default;

    [Tooltip("初期数")]
    [SerializeField]
    private uint _reserveAmount = 5;

    [Tooltip("フィールドに生成される限度数")]
    [SerializeField]
    private uint _activationLimit = 10;
    #endregion
}

/// <summary>
/// アイテムの種類
/// </summary>
public enum ItemType
{
    /// <summary>設定なし</summary>
    None,
    /// <summary>回復アイテム</summary>
    Heal,
    /// <summary>経験値（小）</summary>
    EXP_small,
    /// <summary>経験値（中）</summary>
    EXP_middle,
    /// <summary>経験値（大）</summary>
    EXP_Large,
    /// <summary>パワーアップアイテム</summary>
    PowerUp    
}