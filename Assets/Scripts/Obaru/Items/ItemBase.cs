using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    #region property
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

    private void Update()
    {

    }
    #endregion

    #region public method
    #endregion

    #region protected method
    protected abstract void ItemUse();
    #endregion

    #region private method
    #endregion
}