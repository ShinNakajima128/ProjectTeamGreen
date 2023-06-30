using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵全般を管理するManagerクラス
/// </summary>
[RequireComponent(typeof(EnemyGenerator))]
public class EnemyManager : MonoBehaviour
{
    #region property
    public uint DefeatAmount => _defeatAmount;
    #endregion

    #region serialize
    #endregion

    #region private
    private EnemyGenerator _generator;
    /// <summary>討伐数</summary>
    private uint _defeatAmount = 0;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        _generator = GetComponent<EnemyGenerator>();
    }

    private void Start()
    {

    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
}
