using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerController : MonoBehaviour, IDamagable
{
    #region property
    public bool IsInvincible => throw new System.NotImplementedException();
    public Action<int> ChangeAttackAmountAction { get => _changeAttackAmountAction; set => _changeAttackAmountAction = value; }
    //public Subject<float>
    #endregion

    #region serialize
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    //private Subject<float> _changeAttackAmountSubject = new Subject<float>();
    private event Action<int> _changeAttackAmountAction;
    #endregion

    #region unity methods
    private void Awake()
    {

    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2.0f);


        _changeAttackAmountAction?.Invoke(2);
    }


    #endregion

    #region public method
    public void Damage(float amount)
    {
        Debug.Log("Playerがダメージを受けた");
    }
    #endregion

    #region private method
    #endregion
}
