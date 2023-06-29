using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// アイテム全般を管理するManagerクラス
/// </summary>
[RequireComponent(typeof(ItemGenerator))]
public class ItemManager : MonoBehaviour
{
    #region property
    public static ItemManager Instance { get; private set; }
    #endregion

    #region serialize
    #endregion

    #region private
    private ItemGenerator _generator;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        _generator = GetComponent<ItemGenerator>();
    }

    private void Start()
    {
        _generator.RandoomGenerate();
    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
}
