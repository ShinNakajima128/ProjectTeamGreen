﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 爆発
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
public class BombExplosion : MonoBehaviour
{
    #region serialize
    [Tooltip("スケールに対する係数")]
    [SerializeField]
    private float _scaleCoefficient = 1.2f;
    #endregion

    #region private
    /// <summary>現在の攻撃力</summary>
    private float _currentAttackAmount = 0;
    /// <summary>もとの大きさ</summary>
    private Vector2 _baseScale;
    /// <summary>現在の大きさ</summary>
    private Vector2 _currentScale;
    #endregion

    #region unity methods
    private void Awake()
    {
        _baseScale = transform.localScale;
        _currentScale = _baseScale;
    }

    private void OnEnable()
    {
        _currentScale = _baseScale;
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
        for(int i = 0; i < currentLevel; i++)
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
        gameObject.SetActive(false);
    }
    #endregion
}