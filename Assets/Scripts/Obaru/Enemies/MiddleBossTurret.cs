using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動しない中ボス
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
public class MiddleBossTurret : BossEnemyBase
{
    #region property
    #endregion

    #region serialize
    [Tooltip("弾の攻撃力")]
    [SerializeField]
    private float _bulletAttackAmount = 5.0f;

    [Tooltip("岩の攻撃力")]
    [SerializeField]
    private float _rockAttackAmount = 5.0f;

    [Tooltip("ミサイルの攻撃力")]
    [SerializeField]
    private float _missileAttackAmount = 5.0f;
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
    /// <summary>岩の生成用</summary>
    private RockGenerator _rockGenerator;
    /// <summary>ミサイルの生成用</summary>
    private MissileGenerator _missileGenerator;
    /// <summary>ステート</summary>
    private MiddleBossTurretState _turretState;
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
        _rockGenerator = EnemyManager.Instance.RockPoolGenerator;
        _missileGenerator = EnemyManager.Instance.MissilePoolGenerator;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _turretState = MiddleBossTurretState.Idle;
    }
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

            switch (_turretState)
            {
                //待機
                case MiddleBossTurretState.Idle:
                    yield return new WaitForSeconds(_waitTime);
                    _turretState = MiddleBossTurretState.Shot;
                    break;

                //弾発射
                case MiddleBossTurretState.Shot:
                    yield return StartCoroutine(OnShotBullet());
                    break;

                //岩投射
                case MiddleBossTurretState.Throw:
                    yield return StartCoroutine(OnThrowRock());
                    break;

                //ミサイル発射
                case MiddleBossTurretState.Missile:
                    yield return StartCoroutine(OnShotMissile());
                    break;

                default:
                    break;
            }
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
                    bulletObj.SetAttackAmount(_bulletAttackAmount);
                    //velocityをプレイヤーの方向に設定
                    bulletObj.SetVelocity((_playerTrans.position - transform.position).normalized);
                }
                yield return new WaitForSeconds(_attackInterval);
            }
            yield return new WaitForSeconds(_waitTime);
            _turretState = MiddleBossTurretState.Throw;
        }
    }

    /// <summary>
    /// ランダム3方向に岩を投射
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnThrowRock()
    {
        //攻撃可能なら
        if (_isCanAttack)
        {
            _attackInterval = 0.5f;
            for (int i = 0; i < 6; i++)
            {
                //使う岩を取得
                Rock rockObj = _rockGenerator.RockPool.Rent();
                //岩がnullでないなら
                if (rockObj != null)
                {
                    //岩のポジションを移動
                    rockObj.transform.position = transform.position;
                    //岩をアクティブ化
                    rockObj.gameObject.SetActive(true);
                    //岩の攻撃力を設定
                    rockObj.SetAttackAmount(_rockAttackAmount);
                }
                yield return new WaitForSeconds(_attackInterval);
            }
            yield return new WaitForSeconds(_waitTime);
            _turretState = MiddleBossTurretState.Missile;
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
                //使うミサイルを取得
                Missile missileObj = _missileGenerator.MissilePool.Rent();
                //ミサイルがnullでないなら
                if (missileObj != null)
                {
                    //ミサイルのポジションを移動
                    missileObj.transform.position = transform.position;
                    //ミサイルをアクティブ化
                    missileObj.gameObject.SetActive(true);
                    //ミサイルの攻撃力を設定
                    missileObj.SetAttackAmount(_missileAttackAmount);
                    //プレイヤーのTransformを設定
                    missileObj.SetTargetTransform(_playerTrans);
                }
                yield return new WaitForSeconds(_attackInterval);
            }
            yield return new WaitForSeconds(_waitTime);
            _turretState = MiddleBossTurretState.Idle;
        }
    }
    #endregion
}

/// <summary>
/// タレット型中ボスのステート
/// </summary>
enum MiddleBossTurretState
{
    /// <summary>待機</summary>
    Idle,
    /// <summary>弾発射</summary>
    Shot,
    /// <summary>岩投射</summary>
    Throw,
    /// <summary>ミサイル発射</summary>
    Missile
}