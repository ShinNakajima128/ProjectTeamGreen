using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigBossEnemy : BossEnemyBase
{
    #region property
    #endregion

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

    private float _waitTime = 1.0f;

    private bool _isFliped = false;

    private Rigidbody2D _rd2D;

    private Collider2D _col2D;

    private float _bulletCount = 7.0f;
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

    private void Update()
    {
        Debug.Log(_rd2D.velocity.y);
    }
    #endregion

    #region public method
    #endregion

    #region private method
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

    protected override IEnumerator OnActionCoroutine()
    {
        while (true)
        {
            switch (_currentState)
            {
                case BossState.Idle:
                    _waitTime = 3.0f;
                    yield return new WaitForSeconds(_waitTime);
                    _currentState = BossState.FirstMove;
                    break;
                case BossState.FirstMove:
                    yield return StartCoroutine(OnMoveCoroutine());
                    break;
                //攻撃ステート
                case BossState.FirstAttack:
                    yield return StartCoroutine(OnFirstAttackCoroutine());
                    break;
                //ジャンプステート
                case BossState.SecondMove:
                    yield return StartCoroutine(OnJumpCoroutine());
                    break;
                case BossState.SecondAttack:
                    yield return StartCoroutine(OnSecoundAttackCoroutine());
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

        EnemyFrip();

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

    private IEnumerator OnFirstAttackCoroutine()
    {
        _waitTime = 0.2f;
        EnemyFrip();

        _bulletCount = 7.0f;
        
        float angleStep = 360.0f / _bulletCount;

        for (int i = 0; i < _bulletCount; i++)
        {
            BigBossBullet bulletobj = _generator.BigBossBulletPool.Rent();

            if (bulletobj != null)
            {
                bulletobj.gameObject.SetActive(true);

                bulletobj.transform.position = transform.position;

                bulletobj.SetAttackAmount(_bulletAttackAmount);

                float angle = i * angleStep;

                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

                bulletobj.SetVelocity(direction);
            }
            yield return new WaitForSeconds(_waitTime);
            _currentState = BossState.SecondMove;
        }
    }

    private IEnumerator OnJumpCoroutine()
    {
        _waitTime = 3f;
        yield return new WaitForSeconds(_waitTime);

        Vector2 targetPos = _playerTrans.position;
        float jumpForce = 8.0f;
        _targetPrefab.gameObject.SetActive(true);
        _targetPrefab.transform.position = targetPos;
        _rd2D.AddForce(Vector2.up * jumpForce,ForceMode2D.Impulse);
        _col2D.enabled = false;
        _waitTime = 0.3f;
        yield return new WaitForSeconds(_waitTime);
        _rd2D.velocity = new Vector2(_rd2D.velocity.x, 0);

        while (Vector2.Distance(transform.localPosition,targetPos)>0.01f)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, targetPos, _moveSpeed * Time.deltaTime);
            yield return null;
        }
        _smokePrefab.transform.position = transform.position;
        _smokePrefab.SetActive(true);
        _col2D.enabled = true;
        _targetPrefab.gameObject.SetActive(false);

        _waitTime = 0.6f;
        yield return new WaitForSeconds(_waitTime);

        _smokePrefab.SetActive(false);
        _currentState = BossState.SecondAttack;
    }

    private IEnumerator OnSecoundAttackCoroutine()
    {
        _waitTime = 0.2f;
        EnemyFrip();

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

                    bulletobj.SetVelocity((_playerTrans.position - transform.position).normalized);

                    yield return new WaitForSeconds(_waitTime);
                }
            }
            _currentState = BossState.Idle;
        }
    }
}
