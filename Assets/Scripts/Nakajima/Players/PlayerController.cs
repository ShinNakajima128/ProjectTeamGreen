﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

/// <summary>
/// プレイヤーの機能全般を持つコンポ―ネント
/// </summary>
[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerStatus))]
public class PlayerController : MonoBehaviour, IDamagable
{
    #region property
    public static PlayerController Instance { get; private set; }
    public float CurrentMaxHP => _health.CurrentMaxHP;
    public bool IsInvincible => _isInvincible;
    public PlayerHealth Health => _health;
    public PlayerStatus Status => _status;
    public IObservable<float> ChangeAttackCoefficientSubject => _changeAttackCoefficientSubject;
    #endregion

    #region serialize
    [SerializeField]
    private float _knockbackAmount = 10;
    [SerializeField]
    private bool _debugMode = false;
    #endregion

    #region private
    private PlayerInput _input;
    private PlayerMove _move;
    private PlayerHealth _health;
    private PlayerStatus _status;
    private SpriteRenderer _sr;
    private bool _isDead = false;
    private bool _isInvincible = false;
    private Tween _currentTween;
    #endregion

    #region Constant
    #endregion

    #region Event
    /// <summary>スキルに掛け合わせる係数の変更時のSubject</summary>
    private Subject<float> _changeAttackCoefficientSubject = new Subject<float>();
    #endregion

    #region unity methods
    private void Awake()
    {
        Instance = this;
        _input = GetComponent<PlayerInput>();
        _move = GetComponent<PlayerMove>();
        _health = GetComponent<PlayerHealth>();
        _status = GetComponent<PlayerStatus>();
        _sr = GetComponent<SpriteRenderer>();
        _move.DebugMode = _debugMode;
    }

    private void Start()
    {
        //プレイヤーの画像を右向きか左向きか切り替える機能を登録
        _move.IsFlipedProerty
             .TakeUntilDestroy(this)
             .Subscribe(value => FlipSprite(value));

        //プレイヤーのレベルが上がった時の処理を登録
        _status.CurrentPlayerLevel
               .TakeUntilDestroy(this)
               .Subscribe(_ => _health.PowerUpHealth());

        StageManager.Instance.IsInGameObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(value => ChangeIsDead(!value));

        StageManager.Instance.GameResetObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(_ => ResetPlayerStatus());
    }

    private void OnEnable()
    {
        _input.actions["Move"].performed += OnMove;
        _input.actions["Move"].canceled += StopMove;
    }

    private void OnDisable()
    {
        _input.actions["Move"].performed -= OnMove;
        _input.actions["Move"].canceled -= StopMove;
    }

    #endregion

    #region public method
    /// <summary>
    /// HPを回復する
    /// </summary>
    /// <param name="amount">回復量</param>
    public void Heal(float amount)
    {
        _health.Heal(amount);
        Debug.Log("PlayerのHPを回復");
    }
    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="amount">ダメージ量</param>
    public void Damage(float amount)
    {
        if (_isDead)
        {
            return;
        }

        Debug.Log("ダメージ");
        //ダメージアニメーション
        if (_currentTween == null)
        {
            _currentTween = _sr.DOColor(Color.red, 0.1f)
                               .SetLoops(2, LoopType.Yoyo)
                               .OnComplete(() =>
                               {
                                   _sr.color = Color.white;
                                   _currentTween = null;
                               });
        }
        //ダメージを受けた後、プレイヤーのHPが無くなったら
        if (_health.Damage(amount))
        {
            if (!_isDead)
            {
                StageManager.Instance.OnGameEnd();
                _isDead = true;
            }
        }
    }

    /// <summary>
    /// 攻撃力を上昇させる
    /// </summary>
    /// <param name="newValue">倍率（1.1倍、1.5倍といった元の攻撃力に掛け合わせる数字）</param>
    public void PowerUp(float coefficient)
    {
        _status.ChangeCoefficient(coefficient);
    }

    /// <summary>
    /// 経験値を獲得する
    /// </summary>
    /// <param name="value">獲得した経験値の値（正の値のみ）</param>
    public void GetExp(uint value)
    {
        if (!_isDead)
        {
            _status.AddExp(value);
        }
    }

    public void Knockback(Transform other)
    {
        Vector2 dir = (transform.position - other.position).normalized;
        transform.DOLocalMove(transform.localPosition + (Vector3)(dir * _knockbackAmount),
                              0.15f)
                 .SetEase(Ease.InQuad)
                 .SetLink(gameObject, LinkBehaviour.CompleteAndKillOnDisable);
    }
    #endregion

    #region private method
    /// <summary>
    /// 入力された方向をセットする
    /// </summary>
    /// <param name="obj">入力された値</param>
    private void OnMove(InputAction.CallbackContext obj)
    {
        var value = obj.ReadValue<Vector2>();
        _move.SetDirection(value);
    }
    
    /// <summary>
    /// 移動方向をリセットする
    /// </summary>
    /// <param name="obj">入力された値</param>
    private void StopMove(InputAction.CallbackContext obj)
    {
        _move.SetDirection(Vector2.zero);
    }

    /// <summary>
    /// プレイヤーのSpriteのFlipXを切り替える
    /// </summary>
    /// <param name="value">フリップさせるかどうかの値</param>
    private void FlipSprite(bool value)
    {
        _sr.flipX = value;
    }

    /// <summary>
    /// プレイヤーの各パラメーターをリセットする
    /// </summary>
    private void ResetPlayerStatus()
    {
        _health.ResetHealth();
        _status.ResetStatus();
    }

    /// <summary>
    /// ゲーム開始時の初期化処理
    /// </summary>
    private void ChangeIsDead(bool value)
    {
        _isDead = value;
    }
    #endregion
}
