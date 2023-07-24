using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 追尾するミサイル
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Missile : MonoBehaviour,IPoolable
{
    #region property
    public IObservable<Unit> InactiveObserver => _inactiveSubject;
    #endregion

    #region serialize
    [Tooltip("スピード")]
    [SerializeField]
    private float _moveSpeed = 80.0f;

    [Tooltip("生存時間")]
    [SerializeField]
    private float _lifeTime = 5.0f;
    #endregion

    #region private
    /// <summary>現在の攻撃力</summary>
    private float _currentAttackAmount = 0;
    /// <summary>Rigidbody2Dコンポーネント格納用</summary>
    private Rigidbody2D _rb;
    /// <summary>コルーチン格納用</summary>
    private Coroutine _currentCoroutine;
    #endregion

    #region Event
    private Subject<Unit> _inactiveSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnDisable()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTag.Player))
        {
            //プレイヤーにダメージを与える
            var target = collision.GetComponent<IDamagable>();
            target.Damage(_currentAttackAmount);
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
    /// プレイヤーのTransformを設定する
    /// </summary>
    /// <param name="target"></param>
    public void SetTargetTransform(Transform target)
    {
        _currentCoroutine = StartCoroutine(OnActionCoroutine(target));
    }

    public void ReturnPool()
    {
        throw new NotImplementedException();
    }
    #endregion

    #region private method
    #endregion

    #region Coroutine method
    /// <summary>
    /// ミサイルのアクション
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private IEnumerator OnActionCoroutine(Transform target)
    {
        //ライフタイムの間プレイヤーに向かって移動
        float time = 0;
        while (_lifeTime - time > 0)
        {
            time += Time.deltaTime;

            //up方向が常にプレイヤーに向くように
            Vector3 dir = (target.position - transform.position).normalized;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);

            _rb.velocity = transform.up * _moveSpeed;
            yield return null;
        }

        gameObject.SetActive(false);
    }
    #endregion
}