using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleBossEnemy : BossEnemyBase
{
    #region property
    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("移動速度")]
    [SerializeField]
    private float _moveSpeed = 3.0f;

    [Tooltip("チョコ弾オブジェクト")]
    [SerializeField]
    private BossBullet _bulletPrefab = default;

    [Tooltip("チョコ弾オブジェクトの速さ")]
    [SerializeField]
    private float _bulletSpeed = 5.0f;
    #endregion

    #region private

    //ステートマシンの待機時間
    private float _waitTime = 1.0f;

    //プレハブの反転
    private bool _isFliped = false;

    private Coroutine _currentBossCoroutine;
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
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Debug.Log("onEnable");
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
    #endregion

    #region public method
    #endregion

    #region private method
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
                case BossState.Idle:
                    _waitTime = 3.0f;
                    yield return new WaitForSeconds(_waitTime);
                    _currentState = BossState.Move;
                    break;
                case BossState.Move:
                    Debug.Log("aaa");
                    _currentBossCoroutine = StartCoroutine(OnMoveCoroutine());
                    break;
                case BossState.Attack:
                    _currentBossCoroutine = StartCoroutine(OnAttackCoroutine());
                    break;

                default:
                    break;
            }
            yield return _currentBossCoroutine;
        }
    }

    /// <summary>
    /// プレイヤーに突進する。
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnMoveCoroutine()
    {
        _waitTime = 2.0f;
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("OnMoveCoroutineスタート");
            Vector2 targetPos = _playerTrans.position;

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
        for (int i = 0; i < 3; i++)
        {
            BossBullet chocoBullet = Instantiate(_bulletPrefab, transform.position,Quaternion.identity);
            Vector2 direction = (_playerTrans.position - transform.position).normalized;
            chocoBullet.GetComponent<Rigidbody2D>().velocity = direction * _bulletSpeed;
            yield return new WaitForSeconds(_waitTime);
        }

        _currentState = BossState.Idle;
    }


    #endregion
}
