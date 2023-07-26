using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// 爆発
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
public class BombExplosion : MonoBehaviour, IPoolable
{
    #region serialize
    [Tooltip("スケールに対する係数")]
    [SerializeField]
    private float _scaleCoefficient = 1.2f;
    public IObservable<Unit> InactiveObserver => _inactiveSubject;
    #endregion

    #region private
    /// <summary>現在の攻撃力</summary>
    private float _currentAttackAmount = 0;
    /// <summary>もとの大きさ</summary>
    private Vector2 _baseScale;
    /// <summary>現在の大きさ</summary>
    private Vector2 _currentScale;
    /// <summary>親のTransform</summary>
    private Transform _parent;
    #endregion

    #region Event
    private Subject<Unit> _inactiveSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    private void Awake()
    {
        _baseScale = transform.localScale;
        _currentScale = _baseScale;
        _parent = transform.parent;
    }

    private void OnEnable()
    {
        _currentScale = _baseScale;
    }

    private void OnDisable()
    {
        _inactiveSubject.OnNext(Unit.Default);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTag.Enemy))
        {
            var target = collision.GetComponent<IDamagable>();
            target.Damage(_currentAttackAmount);
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
    /// 大きさアップ
    /// </summary>
    public void SetScale(int currentLevel)
    {
        //スキルレベル分係数をかける
        for(int i = 1; i < currentLevel; i++)
        {
            _currentScale *= _scaleCoefficient;
        }

        transform.localScale = _currentScale;
    }

    /// <summary>
    /// アニメーションが終了したら非アクティブにする
    /// </summary>
    public void OnAnimationFinish()
    {
        //親を設定しなおして非アクティブ化
        transform.SetParent(_parent);
        gameObject.SetActive(false);
    }

    public void ReturnPool()
    {
        gameObject.SetActive(false);
    }
    #endregion
}