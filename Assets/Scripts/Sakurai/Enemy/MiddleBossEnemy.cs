﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 中ボスの機能。
/// </summary>
public class MiddleBossEnemy : BossEnemyBase
{

    #region serialize
    [Header("変数")]
    [Tooltip("移動速度")]
    [SerializeField]
    private float _moveSpeed = 5.0f;

    [Tooltip("弾オブジェクトの速さ")]
    [SerializeField]
    private float _bulletAttackAmount = 3.0f;
    #endregion

    #region private
    //ステートマシンの待機時間
    private float _waitTime = 1.0f;

    //プレハブの反転
    private bool _isFliped = false;

    //弾のプール用のクラスを参照
    EnemyBulletGenerater _generator;
    #endregion

    #region unity methods
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        _generator = EnemyManager.Instance.MiddleBossPoolGenerator;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
    #endregion

    #region private method
    private void EnemyFrip()
    {
        //エネミーよりプレイヤーのx軸が大きい場合はエネミーを右向きにする
        if (!_isFliped && transform.localPosition.x < _playerTrans.position.x)
        {
            _enemyRenderer.flipX = true;
            _isFliped = true;
        }
        //プレイヤーのxの値は小さい場合は左向きにする
        else if (_isFliped && transform.localPosition.x >= _playerTrans.position.x)
        {
            _enemyRenderer.flipX = false;
            _isFliped = false;
        }
    }
    #endregion

    #region coroutine method
    /// <summary>
    /// エネミーの挙動を制御するステートマシン
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator OnActionCoroutine()
    {
        while(true)
        {
            switch(_currentState)
            {
                //アイドルステート
                case BossState.Idle:
                    _waitTime = 3.0f;
                    yield return new WaitForSeconds(_waitTime);
                    _currentState = BossState.Move;
                    break;

                //ムーブステート
                case BossState.Move:
                    yield return StartCoroutine(OnMoveCoroutine());
                    break;
                //攻撃ステート
                case BossState.Attack:
                    yield return StartCoroutine(OnAttackCoroutine());
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// プレイヤーに突進する。
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnMoveCoroutine()
    {
        _waitTime = 2.0f;

        //追いかける処理を3回繰り返す。
        for (int i = 0; i < 3; i++)
        {
            EnemyFrip();

            Vector2 targetPos = _playerTrans.position;

             //プレイヤーとの距離が0.01以上であれば追いかける。
            while (Vector2.Distance(transform.localPosition, targetPos) > 0.01f)
            {
                transform.localPosition = Vector2.MoveTowards(transform.localPosition, targetPos, _moveSpeed * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(_waitTime);
        }
        _currentState = BossState.Attack;
    }

    /// <summary>
    /// プレイヤーに向かって弾を発射
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnAttackCoroutine()
    {
        Debug.Log("OnAttackCoroutineスタート");
        _waitTime = 0.5f;
        EnemyFrip();
        for (int i = 0; i < 3; i++)
        {
            //プールからオブジェクトを取り出す。
            GameObject bulletObj = _generator.BulletPool.Rent();

            if (bulletObj != null)
            {
                EnemyBullet bullet = bulletObj.GetComponent<EnemyBullet>();

                //バレットをアクティブにする。
                bullet.gameObject.SetActive(true);
                //バレットのポジションをエネミーの位置に設定。
                bullet.transform.position = transform.position;
                //バレットに攻撃力を持たせる。
                bullet.SetAttackAmount(_bulletAttackAmount);
                //バレットに速度を持たせる。
                bullet.SetVelocity((_playerTrans.position - transform.position).normalized);
                yield return new WaitForSeconds(_waitTime);
            }
        }
        _currentState = BossState.Idle;
    }
    #endregion
}
