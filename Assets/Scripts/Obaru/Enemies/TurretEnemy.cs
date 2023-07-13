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
    /// <summary>反転しているかどうか</summary>
    private bool _isFliped = false;
    /// <summary>弾を撃つかどうか</summary>
    private bool _isCanShot = true;
    /// <summary>フリップを行うコルーチン格納用</summary>
    private Coroutine _flipCoroutine;
    /// <summary>弾生成コンポーネント格納用</summary>
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
        _isCanShot = false;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (_flipCoroutine != null)
        {
            StopCoroutine(_flipCoroutine);
            _flipCoroutine = null;
        }
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

    #region Coroutine method
    /// <summary>
    /// 敵ごとの行動処理を行うコルーチン
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator OnActionCoroutine()
    {
        //行動可能なら
        while (_isActionable)
        {
            //弾を発射可能なら
            if (_isCanShot)
            {
                //使う弾を取得
                GameObject bulletObj = _generator.BulletPool.Rent();
                //弾がnullでないなら
                if (bulletObj != null)
                {
                    //使用する弾のコンポーネントを取得
                    var bullet = bulletObj.GetComponent<EnemyBullet>();
                    //弾をアクティブ化
                    bullet.gameObject.SetActive(true);
                    //弾のポジションを移動
                    bullet.transform.position = transform.position;
                    //親子関係を解除
                    bullet.gameObject.transform.SetParent(null);
                    //弾の攻撃力を設定
                    bullet.SetAttackAmount(_currentAttackAmount);
                    //velocityをプレイヤーの方向に設定
                    bullet.SetVelocity((_playerTrans.position - transform.position).normalized);
                }
            }
            //発射間隔分待つ
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
    #endregion
}