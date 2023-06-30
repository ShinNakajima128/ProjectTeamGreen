﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protein : ItemBase
{
    #region property
    #endregion

    #region serialize
    [Tooltip("倍率")]
    [SerializeField]
    private float _coefficient = 1.1f;
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

    /// <summary>
    /// アイテムの使用
    /// </summary>
    /// <param name="player"></param>
    public override void Use(PlayerController player)
    {
        Debug.Log($"{ItemType}を使用した");
        Destroy(gameObject);

        player.PowerUp(_coefficient);
    }

    /// <summary>
    /// 非アクティブ化
    /// </summary>
    public override void Return()
    {
        Debug.Log($"{ItemType}を非アクティブにした");

        //座標をリセットして非表示にする
        gameObject.transform.localPosition = Vector2.zero;
        gameObject.SetActive(false);
    }
    #endregion

    #region private method
    #endregion
}