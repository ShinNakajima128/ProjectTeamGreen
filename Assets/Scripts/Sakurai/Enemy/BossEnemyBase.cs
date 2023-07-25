using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 各ボスが継承する基底クラス
/// </summary>
public abstract class BossEnemyBase : EnemyBase
{
    #region property
    public Action<float> OnDamageAction { get => _onDamageAction; set => _onDamageAction = value; }
    #endregion

    #region protected
    protected BossState _currentState = BossState.Idle;
    #endregion

    #region events
    private event Action<float> _onDamageAction;
    #endregion

    #region public method
    public override void Damage(float amount)
    {
        base.Damage(amount);
        _onDamageAction?.Invoke(_currentHP);
        Debug.Log($"ボスダメージ:{_currentHP} アクション：{_onDamageAction}");
    }
    #endregion

    #region protected method
    protected override void ResetParameter()
    {
        base.ResetParameter();

        if (_onDamageAction != null)
        {
            _onDamageAction = null;
        }
    }
    #endregion
}

//ボスのステート
public enum BossState
{
    Idle,    //アイドル
    FirstMove,    //動き1
    SecondMove,  //動き2
    FirstAttack,　 //攻撃1  
    SecondAttack   //攻撃2
}