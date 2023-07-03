using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : EnemyBase
{
    #region property
    #endregion

    #region serialize
    [Tooltip("敵の弾")]
    [SerializeField]
    private EnemyBullet _bullet = default;

    [Tooltip("攻撃間隔")]
    [SerializeField]
    private float _attackInterval = 3.0f;
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
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion

    protected override IEnumerator OnActionCoroutine()
    {
        throw new System.NotImplementedException();
    }
}