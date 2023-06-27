using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="MyScriptable/ItemData")]
public class ItemData : ScriptableObject
{
    #region property
    public ItemType ItemType => _itemType;
    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("アイテムの種類")]
    [SerializeField]
    private ItemType _itemType = default;
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
/// アイテムの種類
/// </summary>
public enum ItemType
{
    /// <summary>回復アイテム</summary>
    Heal,
    /// <summary>パワーアップアイテム</summary>
    PowerUp,
    /// <summary>経験値アイテム</summary>
    EXP
}