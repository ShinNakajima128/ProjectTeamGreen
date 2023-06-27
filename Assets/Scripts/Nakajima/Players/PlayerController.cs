using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
    
    
    #region property
    public bool IsInvincible => throw new System.NotImplementedException();
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
    private void Awake()
    {

    }

    private void Start()
    {

    }


    #endregion

    #region public method
    public void Damage(int amount)
    {
        Debug.Log("Playerがダメージを受けた");
    }
    #endregion

    #region private method
    #endregion
}
