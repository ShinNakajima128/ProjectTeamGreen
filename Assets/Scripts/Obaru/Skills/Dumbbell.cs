using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// ダンベルコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Dumbbell : MonoBehaviour, IPoolable
{
    #region property
    public IObservable<Unit> InactiveObserver => _inactiveSubject;
    #endregion

    #region serialize
    [Tooltip("飛ぶ速さ")]
    [SerializeField]
    private float _moveSpeed = 3.0f;
    #endregion

    #region private
    /// <summary>現在の攻撃力</summary>
    private float _currentAttackAmount = 0;
    /// <summary>スキル持続時間</summary>
    private float _lifeTime = 5.0f;
    /// <summary>Rigidbody2Dコンポーネント格納用</summary>
    private Rigidbody2D _rb;
    /// <summary>コルーチン格納用</summary>
    private Coroutine _currentCoroutine = default;
    /// <summary>親のTransform格納用</summary>
    private Transform _parent;
    #endregion

    #region Event
    private Subject<Unit> _inactiveSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _parent = transform.parent;
    }

    private void OnEnable()
    {
        _currentCoroutine = StartCoroutine(InactiveCoroutine());
    }

    private void OnDisable()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
        _inactiveSubject.OnNext(Unit.Default);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTag.Enemy))
        {
            //敵にダメージを与える
            var target = collision.GetComponent<IDamagable>();
            target.Damage(_currentAttackAmount);
            target.Knockback(transform);
            //親を設定しなおしてから非アクティブ化
            transform.SetParent(_parent);
            gameObject.SetActive(false);
        }
    }
    #endregion

    #region public method
    /// <summary>
    /// 攻撃力の設定
    /// </summary>
    /// <param name="amount"></param>
    public void SetAttackAmount(float amount)
    {
        _currentAttackAmount = amount;
    }

    /// <summary>
    /// velocityの設定
    /// </summary>
    /// <param name="dir"></param>
    public void SetVelocity(Vector3 dir)
    {
        _rb.velocity = dir * _moveSpeed;
    }
    #endregion

    #region Coroutine method
    /// <summary>
    /// 一定時間後に非アクティブ化
    /// </summary>
    /// <returns></returns>
    private IEnumerator InactiveCoroutine()
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