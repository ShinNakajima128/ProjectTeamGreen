using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 不動で弾を撃ってくる敵
/// </summary>
public class TurretEnemy : EnemyBase
{
    #region serialize
    [Tooltip("攻撃間隔")]
    [SerializeField]
    private float _attackInterval = 3.0f;
    #endregion

    #region private
    private bool _isFliped = false;
    private bool _isCanShot = true;
    private Coroutine _flipCoroutine;
    private EnemyBulletGenerater _generator;
    #endregion

    #region unity methods
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        _generator = EnemyManager.Instance.TurretPoolGenerator;
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

    private void OnBecameVisible()
    {
        _isCanShot = true;
    }

    private void OnBecameInvisible()
    {
        _isCanShot = false;
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
            if (_isCanShot)
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