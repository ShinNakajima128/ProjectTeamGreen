using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各ボスが継承する基底クラス
/// </summary>
public abstract class BossEnemyBase : EnemyBase
{
    #region protected
    protected BossState _currentState = BossState.Idle;
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