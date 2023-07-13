using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TestEnemy : EnemyBase
{
    #region property
    #endregion

    #region serialize
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    protected override void Awake()
    {
        base.Awake();
        Debug.Log($"HP:{_currentHP} 攻撃力:{_currentAttackAmount}");
    }

    protected override void Start()
    {
        base.Start();
    }
    #endregion

    #region public method
    #endregion

    #region protected
    #endregion

    #region private method
    #endregion

    #region coroutine method
    protected override IEnumerator OnActionCoroutine()
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
