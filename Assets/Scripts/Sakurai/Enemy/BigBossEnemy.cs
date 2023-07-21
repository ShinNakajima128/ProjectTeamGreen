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
    private float _bulletAttackAmount = 5.0f;

    [Tooltip("爆発プレハブ")]
    [SerializeField]
    private GameObject _explosionPrefab = default;

    [Tooltip("ターゲットイメージ")]
    [SerializeField]
    private GameObject _targetPrefab = default;
    #endregion

    #region private

    private float _waitTime = 1.0f;

    private bool _isFliped = false;

    EnemyBulletGenerater _generator;

    private Rigidbody2D _rd2D;
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

    private void Start()
    {
        base.Start();
        _generator = EnemyManager.Instance.MiddleBossPoolGenerator;

        _rd2D = GetComponent<Rigidbody2D>(); 
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
                    _waitTime = 1.0f;
                    yield return new WaitForSeconds(_waitTime);
                    _currentState = BossState.Attack;
                    break;
                //攻撃ステート
                case BossState.Attack:
                    yield return StartCoroutine(OnAttackCoroutine());
                    break;
                //ジャンプステート
                case BossState.Move:
                    yield return StartCoroutine(OnJumpCoroutine());
                    break;
                default:
                    break;
            }
        }
    }

    private IEnumerator OnAttackCoroutine()
    {
        _waitTime = 1.0f;
        EnemyFrip();

        for (int i = 0; i < 5; i++)
        {
            if (_generator != null)
            {
                EnemyBullet bulletobj = _generator.BulletPool.Rent();

                if (bulletobj != null)
                {
                    bulletobj.gameObject.SetActive(true);

                    bulletobj.transform.position = transform.position;

                    bulletobj.SetAttackAmount(_bulletAttackAmount);

                    bulletobj.SetVelocity((_playerTrans.position - transform.position).normalized);

                    yield return new WaitForSeconds(_waitTime);
                }
            }
            _currentState = BossState.Move;
        }
    }

    private IEnumerator OnJumpCoroutine()
    {
        _waitTime = 3f;
        yield return new WaitForSeconds(_waitTime);

        Vector2 targetPos = _playerTrans.position;
        float jumpForce = 8.0f;
        _targetPrefab.gameObject.SetActive(true);
        _targetPrefab.transform.position = _playerTrans.position;
        _rd2D.AddForce(Vector2.up * jumpForce,ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.3f);
        _rd2D.velocity = new Vector2(_rd2D.velocity.x, 0);

        while (Vector2.Distance(transform.localPosition,targetPos)>0.01f)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, targetPos, _moveSpeed * Time.deltaTime);
            yield return null;
        }
        _targetPrefab.gameObject.SetActive(false);


        _currentState = BossState.Idle;
    }
}
