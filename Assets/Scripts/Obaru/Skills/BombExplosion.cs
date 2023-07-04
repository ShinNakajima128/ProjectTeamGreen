using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 爆発
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
public class BombExplosion : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    #endregion

    #region private
    /// <summary>現在の攻撃力</summary>
    private float _currentAttackAmount = 0;
    /// <summary>現在の半径</summary>
    private float _currentRadius = 0;
    private CircleCollider2D _coll;
    private Coroutine _inactiveCoroutine = default;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        _coll = GetComponent<CircleCollider2D>();
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

    #region private method
    #endregion
}