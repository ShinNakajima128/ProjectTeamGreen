using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// プレイヤーの機能全般を持つコンポ―ネント
/// </summary>
[RequireComponent(typeof(PlayerMove))]
public class PlayerController : MonoBehaviour, IDamagable
{
    #region property
    public static PlayerController Instance { get; private set; }
    public bool IsInvincible => _isInvincible;
    public Subject<float> ChangeAttackCoefficientSubject => _changeAttackCoefficientSubject;
    #endregion

    #region serialize
    #endregion

    #region private
    private PlayerInput _input;
    private PlayerMove _move;
    private SpriteRenderer _sr;
    private bool _isCanControl = false;
    private bool _isInvincible = false;
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
        _input = GetComponent<PlayerInput>();
        _move = GetComponent<PlayerMove>();
        _sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        StageManager.Instance.IsInGameSubject
                             .Subscribe(value => ChangeIsCanControl(value))
                             .AddTo(this);
        
        //プレイヤーの画像を右向きか左向きか切り替える機能を登録
        _move.IsFlipedProerty
             .Subscribe(value => FlipSprite(value))
             .AddTo(this);

        //Update内で行う処理を登録
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SkillManager.Instance.SetSkill(SkillType.Aura);
                }
            })
            .AddTo(this);
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
    public void Damage(float amount)
    {
        Debug.Log("Playerがダメージを受けた");
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

    private void ChangeIsCanControl(bool value)
    {
        _isCanControl = value;
    }

    /// <summary>
    /// プレイヤーのSpriteのFlipXを切り替える
    /// </summary>
    /// <param name="value">フリップさせるかどうかの値</param>
    private void FlipSprite(bool value)
    {
        _sr.flipX = value;
    }
    #endregion
}
