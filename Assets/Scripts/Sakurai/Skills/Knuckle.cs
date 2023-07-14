using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// テスト用の仮の弾丸コンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]

public class Knuckle : MonoBehaviour, IPoolable
{

    public IObservable<Unit> InactiveObserver => _inactiveSubject;


    #region serialize
    [Header("変数")]
    [Tooltip("拳が動く速さ")]
    [SerializeField]
    private float _moveSpeed = 3.0f;
    #endregion

    #region private
    private Rigidbody2D _rb;

    /// <summary>現在の攻撃力</summary>
    private float _currentAttackAmount = 0;

    /// <summary>拳の生存時間</summary>
    private float _lifeTime = 2.0f;

    private Coroutine _currentCoroutine;
    #endregion

    #region Event
    /// <summary>スキル機能変更用のデリゲート</summary>
    public Action RandomDirection { get; set; }

    private Subject<Unit> _inactiveSubject = new Subject<Unit>(); 

    #endregion

    #region unity methods
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
       _currentCoroutine = StartCoroutine(InActiveCoroutine());
    }

    private void OnDisable()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
        transform.localPosition = Vector2.zero;

        _inactiveSubject.OnNext(Unit.Default);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //当たったのがエネミーだった場合
        if (collision.CompareTag(GameTag.Enemy))
        {
            //インターフェースを取得
            var target = collision.GetComponent<IDamagable>();

            if (target != null)
            {
                //現在の攻撃力分ダメージを与える。
                target.Damage(_currentAttackAmount);
            }
        }
    }
    #endregion

    #region public method
    /// <summary>
    /// ナックルに攻撃力を持たせる。
    /// </summary>
    /// <param name="amount">スキルデータから受け取る攻撃力</param>
    public void SetAttackAmount(float amount)
    {
        _currentAttackAmount = amount;
    }

    /// <summary>
    /// 現在のレベル2までは4方向にランダム。
    /// </summary>
    public void RondomFourDirection()
    {
        //十字方向のランダムの値を持たせる。
        int dirType = UnityEngine.Random.Range(0, 4);

        switch (dirType)
        {
            case 0:
                _rb.velocity = new Vector2(0, _moveSpeed);
                break;
            case 1:
                _rb.velocity = new Vector2(_moveSpeed, 0);
                break;
            case 2:
                _rb.velocity = new Vector2(0, -_moveSpeed);
                break;
            case 3:
                _rb.velocity = new Vector2(-_moveSpeed, 0);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 現在のレベル3から8方向にランダム。
    /// </summary>
    public void RondomEightDirection()
    {
        //８方向にランダムの値に持たせる
        int dirType = UnityEngine.Random.Range(0, 8);

        switch (dirType)
        {
            case 0:
                _rb.velocity = new Vector2(0, _moveSpeed);
                break;
            case 1:
                _rb.velocity = new Vector2(_moveSpeed, 0);
                break;
            case 2:
                _rb.velocity = new Vector2(0, -_moveSpeed);
                break;
            case 3:
                _rb.velocity = new Vector2(-_moveSpeed, 0);
                break;
            case 4:
                _rb.velocity = new Vector2(_moveSpeed, _moveSpeed);
                break;
            case 5:
                _rb.velocity = new Vector2(_moveSpeed, -_moveSpeed);
                break;
            case 6:
                _rb.velocity = new Vector2(-_moveSpeed, _moveSpeed);
                break;
            case 7:
                _rb.velocity = new Vector2(-_moveSpeed, -_moveSpeed);
                break;
            default:
                break;
        }
    }
    #endregion

    #region private method
    /// <summary>
    /// プールで生成したオブジェクトを2秒後非アクティブにする。
    /// </summary>
    private IEnumerator InActiveCoroutine()
    {
        yield return new WaitForSeconds(_lifeTime);
        gameObject.SetActive(false);
    }

    public void ReturnPool()
    {
        throw new NotImplementedException();
    }
    #endregion

}
