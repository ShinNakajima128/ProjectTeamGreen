using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各アイテムのベースクラス
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class ItemBase : MonoBehaviour
{
    #region property
    public ItemType ItemType => _itemData.ItemType;
    public uint ReserveAmount => _itemData.ReserveAmount;
    public uint ActivationLimit => _itemData.ActivationLimit;
    #endregion

    #region serialize
    [Tooltip("アイテムデータ")]
    [SerializeField]
    private ItemData _itemData = default;
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

    }

    private void Start()
    {

    }
    #endregion

    #region public method
    #endregion

    #region protected method
    /// <summary>
    /// 使用する
    /// </summary>
    public abstract void Use(PlayerController player);
    /// <summary>
    /// ステージ上から無くなる
    /// </summary>
    public abstract void Return();
    #endregion

    #region private method
    #endregion
}