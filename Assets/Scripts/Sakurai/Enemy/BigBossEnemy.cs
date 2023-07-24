using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ラスボスの機能
/// </summary>
public class BigBossEnemy : BossEnemyBase
{
    #region serialize
    [Header("変数")]
    [Tooltip("移動速度")]
    [SerializeField]
    private float _moveSpeed = 7.0f;

    [Tooltip("弾のオブジェクトの攻撃力")]
    [SerializeField]
    private float _bulletAttackAmount = 30.0f;

    [Tooltip("爆発プレハブ")]
    [SerializeField]
    private GameObject _smokePrefab = default;

    [Tooltip("ターゲットイメージ")]
    [SerializeField]
    private GameObject _targetPrefab = default;

    [Tooltip("弾のジェネレーター")]
    [SerializeField]
    private BigBossBulletGenerator _generator = default;
    #endregion

    #region private
    /// <summary>コルーチンの待機時間</summary>
    private float _waitTime = 1.0f;

    /// <summary>プレイヤーの位置の応じて反転</summary>
    private bool _isFliped = false;

    /// <summary>攻撃時のバレットの数</summary>
    private float _bulletCount = 7.0f;

    private Rigidbody2D _rd2D;

    private Collider2D _col2D;
    #endregion

    #region unity methods
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        _rd2D = GetComponent<Rigidbody2D>();
        _col2D = GetComponent<Collider2D>();
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
    /// <summary>
    /// プレイヤーの向きに応じて反転
    /// </summary>
    private void EnemyFrip()
    {
        if (!_isFliped && transform.localPosition.x < _playerTrans.position.x)
        {
            _enemyRenderer.flipX = false;
            _isFliped = true;
        }
        else if(_isFliped && transform.localPosition.x >= _playerTrans.position.x)
        {
            _enemyRenderer.flipX = true;
            _isFliped = false;
        }
    }
    #endregion

    /// <summary>
    /// 大ボスのステートマシン
    /// </summary>
    protected override IEnumerator OnActionCoroutine()
    {
        while (true)
        {
            switch (_currentState)
            {
                //アイドルステート
                case BossState.Idle:
                    _waitTime = 3.0f;
                    yield return new WaitForSeconds(_waitTime);
                    _currentState = BossState.FirstMove;
                    break;
                //突進ステート
                case BossState.FirstMove:
                    yield return StartCoroutine(OnMoveCoroutine());
                    break;
                //攻撃ステート1
                case BossState.FirstAttack:
                    yield return StartCoroutine(OnFirstAttackCoroutine());
                    break;
                //ジャンプステート
                case BossState.SecondMove:
                    yield return StartCoroutine(OnJumpCoroutine());
                    break;
                //攻撃ステート2
                case BossState.SecondAttack:
                    yield return StartCoroutine(OnSecoundAttackCoroutine());
                    break;

                default:
                    Debug.Log($"{_currentState}はありません");
                    break;
            }
        }
    }

    /// <summary>
    /// プレイヤーに突進する。
    /// </summary>
    private IEnumerator OnMoveCoroutine()
    {
        _waitTime = 2.0f;

        //プレイヤーの向きに応じてエネミーの向き変更
        EnemyFrip();

        //プレイヤーの位置
        Vector2 targetPos = _playerTrans.position;

        //プレイヤーとの距離が0.01以上であれば追いかける。
         while (Vector2.Distance(transform.localPosition, targetPos) > 0.01f)
         {
             transform.localPosition = Vector2.MoveTowards(transform.localPosition, targetPos, _moveSpeed * Time.deltaTime);
             yield return null;
         }
         yield return new WaitForSeconds(_waitTime);

        _currentState = BossState.FirstAttack;
    }

    /// <summary>
    /// 1度目の攻撃
    /// </summary>
    private IEnumerator OnFirstAttackCoroutine()
    {
        _waitTime = 0.2f;
        
        //プレイヤーの向きに応じてエネミーの向き変更
        EnemyFrip();

        _bulletCount = 7.0f;

        //弾を7方向に飛ばすため弾のカウントで割る
        float angleStep = 360.0f / _bulletCount;

        for (int i = 0; i < _bulletCount; i++)
        {
            //プールから弾を取り出す
            BigBossBullet bulletobj = _generator.BigBossBulletPool.Rent();

            if (bulletobj != null)
            {
                bulletobj.gameObject.SetActive(true);
                bulletobj.transform.position = transform.position;
                bulletobj.SetAttackAmount(_bulletAttackAmount);

                //円形に均等に飛ばすため計算
                float angle = i * angleStep;

                //度をラジアンに変換して角度を求める。
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                bulletobj.SetVelocity(direction);
            }
            yield return new WaitForSeconds(_waitTime);
            _currentState = BossState.SecondMove;
        }
    }

    /// <summary>
    /// プレイヤーの位置にジャンプ
    /// </summary>
    private IEnumerator OnJumpCoroutine()
    {
        _waitTime = 3f;
        yield return new WaitForSeconds(_waitTime);
        
        Vector2 targetPos = _playerTrans.position;
        float jumpForce = 8.0f;

        //照準定める表現
        _targetPrefab.gameObject.SetActive(true);
        _targetPrefab.transform.position = targetPos;

        //0.3秒,上方向に力を加える
        _rd2D.AddForce(Vector2.up * jumpForce,ForceMode2D.Impulse);

        //ジャンプ中はコライダーをきる
        _col2D.enabled = false;
        _waitTime = 0.3f;
        yield return new WaitForSeconds(_waitTime);

        //速度を戻す
        _rd2D.velocity = new Vector2(_rd2D.velocity.x, 0);

        //プレイヤーの位置に向かって落ちる
        while (Vector2.Distance(transform.localPosition,targetPos)>0.01f)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, targetPos, _moveSpeed * Time.deltaTime);
            yield return null;
        }
        //着地とどうじにコライダーを戻して煙を出して照準はきる。
        _col2D.enabled = true;
        _smokePrefab.transform.position = transform.position;
        _smokePrefab.SetActive(true);
        _targetPrefab.gameObject.SetActive(false);

        _waitTime = 0.6f;
        yield return new WaitForSeconds(_waitTime);
        _smokePrefab.SetActive(false);
        _currentState = BossState.SecondAttack;
    }

    /// <summary>
    /// 2度目の攻撃
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnSecoundAttackCoroutine()
    {
        _waitTime = 0.2f;
        EnemyFrip();

        //弾の数は10個
        _bulletCount = 10.0f;

        for (int i = 0; i < _bulletCount; i++)
        {
            if (_generator != null)
            {
                BigBossBullet bulletobj = _generator.BigBossBulletPool.Rent();

                if (bulletobj != null)
                {
                    bulletobj.gameObject.SetActive(true);
                    bulletobj.transform.position = transform.position;
                    bulletobj.SetAttackAmount(_bulletAttackAmount);

                    //プレイヤーに向かって10発連射。
                    bulletobj.SetVelocity((_playerTrans.position - transform.position).normalized);

                    yield return new WaitForSeconds(_waitTime);
                }
            }
            _currentState = BossState.Idle;
        }
    }
}
