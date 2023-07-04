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
    public ItemGenerator Generator => _generator;
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
        Instance = this;
        _generator = GetComponent<ItemGenerator>();
    }

    private void Start()
    {
        _generator.RandoomGenerate(ItemType.Heal);
        //_generator.RandoomGenerate(ItemType.EXP);
    }
    #endregion

    #region public method
    /// <summary>
    /// アイテムを生成する
    /// </summary>
    /// <param name="type">アイテムの種類</param>
    /// <param name="pos">生成する座標</param>
    public void GenerateItem(ItemType type, Vector2 pos)
    {
        _generator.Generate(type, pos);
    }
    #endregion

    #region private method
    #endregion
}
