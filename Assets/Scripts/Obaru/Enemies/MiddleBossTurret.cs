using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動しない中ボス
/// </summary>
public class MiddleBossTurret : BossEnemyBase
{
    #region property
    #endregion

    #region serialize
    [Tooltip("弾の攻撃力")]
    [SerializeField]
    private float _bulletAttackAmount = 5.0f;

    [Tooltip("ポテトの攻撃力")]
    [SerializeField]
    private float _potatoAttackAmount = 5.0f;

    [Tooltip("ミサイルの攻撃力")]
    [SerializeField]
    private float _missileAttackAmount = 5.0f;

    [SerializeField]
    private PotatoGenerator _potatoGenerator = default;
    #endregion

    #region private
    /// <summary>待ち時間</summary>
    private float _waitTime = 1.0f;
    /// <summary>攻撃間隔</summary>
    private float _attackInterval = 0f;
    /// <summary>反転のフラグ</summary>
    private bool _isFliped = false;
    /// <summary>弾を発射可能かどうか</summary>
    private bool _isCanAttack = true;
    /// <summary>弾の生成用</summary>
    private EnemyBulletGenerater _bulletGenerator;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        _bulletGenerator = EnemyManager.Instance.MiddleBossPoolGenerator;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(OnThrowPotato());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
    #endregion

    #region public method
    #endregion

    #region private method
    private void EnemyFlip()
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

    #region Coroutine method
    protected override IEnumerator OnActionCoroutine()
    {
        //行動可能なら
        while (_isActionable)
        {
            EnemyFlip();
            
            yield return null;
        }
    }

    /// <summary>
    /// プレイヤーに向かって弾を発射
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnShotBullet()
    {
        //攻撃可能なら
        if (_isCanAttack)
        {
            _attackInterval = 1.0f;
            for (int i = 0; i < 5; i++)
            {
                //使う弾を取得
                EnemyBullet bulletObj = _bulletGenerator.BulletPool.Rent();
                //弾がnullでないなら
                if (bulletObj != null)
                {
                    //弾をアクティブ化
                    bulletObj.gameObject.SetActive(true);
                    //弾のポジションを移動
                    bulletObj.transform.position = transform.position;
                    //弾の攻撃力を設定
                    bulletObj.SetAttackAmount(_currentAttackAmount);
                    //velocityをプレイヤーの方向に設定
                    bulletObj.SetVelocity((_playerTrans.position - transform.position).normalized);
                }
                yield return new WaitForSeconds(_attackInterval);
            }
            yield return new WaitForSeconds(_waitTime);
        }
    }

    /// <summary>
    /// ランダム3方向に物体を投射
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnThrowPotato()
    {
        //攻撃可能なら
        if (_isCanAttack)
        {
            _attackInterval = 0.5f;
            for (int i = 0; i < 6; i++)
            {
                //使う弾を取得
                Potato potatoObj = _potatoGenerator.PotatoPool.Rent();
                //弾がnullでないなら
                if (potatoObj != null)
                {
                    //弾のポジションを移動
                    potatoObj.transform.position = transform.position;
                    //弾をアクティブ化
                    potatoObj.gameObject.SetActive(true);
                }
                yield return new WaitForSeconds(_attackInterval);
            }
            yield return new WaitForSeconds(_waitTime);
        }
    }

    /// <summary>
    /// 追尾ミサイル発射
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnShotMissile()
    {
        //攻撃可能なら
        if (_isCanAttack)
        {
            _attackInterval = 2.0f;
            for (int i = 0; i < 2; i++)
            {
                yield return new WaitForSeconds(_attackInterval);
            }
            yield return new WaitForSeconds(_waitTime);
        }
    }
    #endregion
}