using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Debug.Log($"HP:{_currentHp} 攻撃力:{_currentAttackAmount}");
    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
}
