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

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {

            })
            .AddTo(this);
    }
    #endregion

    #region public method
    #endregion

    #region protected
    protected override void OnAction()
    {
        if (!_isActionable)
        {
            return;
        }
    }
    #endregion

    #region private method
    #endregion
}
