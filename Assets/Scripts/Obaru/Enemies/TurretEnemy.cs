﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 不動で弾を撃ってくる敵
/// </summary>
public class TurretEnemy : EnemyBase
{
    #region serialize
    [Tooltip("弾")]
    [SerializeField]
    private EnemyBullet _bullet = default;

    [Tooltip("攻撃間隔")]
    [SerializeField]
    private float _attackInterval = 3.0f;
    #endregion

    #region private
    private bool _isFliped = false;
    private EnemyBulletGenerater _generator;
    private Coroutine _flipCoroutine;
    #endregion

    #region unity methods
    protected override void Awake()
    {
        base.Awake();
        _generator = GetComponent<EnemyBulletGenerater>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _flipCoroutine = StartCoroutine(FlipCoroutine());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StopCoroutine(_flipCoroutine);
        _flipCoroutine = null;
    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion

    /// <summary>
    /// 敵ごとの行動処理を行うコルーチン
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator OnActionCoroutine()
    {
        while (_isActionable)
        {
            GameObject bulletObj = _generator.BulletPool.Rent();
            if (bulletObj != null)
            {
                var bullet = bulletObj.GetComponent<EnemyBullet>();
                bullet.gameObject.SetActive(true);
                bullet.transform.position = transform.position;
                bullet.gameObject.transform.SetParent(null);
                bullet.SetAttackAmount(_currentAttackAmount);
                bullet.SetVelocity((_playerTrans.position - transform.position).normalized);
            }
            yield return new WaitForSeconds(_attackInterval);
        }
    }

    /// <summary>
    /// 画像の反転
    /// </summary>
    /// <returns></returns>
    private IEnumerator FlipCoroutine()
    {
        while (_isActionable)
        {
            //プレイヤーより左の位置にいたら画像を反転
            if (!_isFliped && transform.localPosition.x < _playerTrans.position.x)
            {
                _enemyRenderer.flipX = true;
                _isFliped = true;
            }
            //プレイヤーより右の位置にいたら画像を元に戻す
            else if (_isFliped && transform.localPosition.x >= _playerTrans.position.x)
            {
                _enemyRenderer.flipX = false;
                _isFliped = false;
            }

            yield return null;
        }
    }
}