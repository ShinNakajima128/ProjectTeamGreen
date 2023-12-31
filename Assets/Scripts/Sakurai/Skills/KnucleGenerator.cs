﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ナックルのプール機能
/// </summary>
public class KnucleGenerator : MonoBehaviour
{
    #region property
    public ObjectPool<Knuckle> KnucklePool => _knucklePool;
    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("初期の段階で持たせるプール数")]
    [SerializeField]
    private uint _reserveAmount = 10;

    [Tooltip("プールの最大値")]
    [SerializeField]
    private uint _limit = 100;

    [Tooltip("ナックルプレハブ")]
    [SerializeField]
    private Knuckle _knucklePrefab = default;

    [Tooltip("親オブジェクト")]
    [SerializeField]
    private Transform _parent = default;
    #endregion

    #region private
    private ObjectPool<Knuckle> _knucklePool;
    #endregion

    #region unity methods
    private void Awake()
    {
        _knucklePool = new ObjectPool<Knuckle>(_knucklePrefab, _parent);
    }
    #endregion
}