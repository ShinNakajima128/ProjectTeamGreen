using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージ表記を管理するManagerクラス
/// </summary>
[RequireComponent(typeof(DamageTextGenerator))]
public class DamageTextManager : MonoBehaviour
{
    #region property
    public static DamageTextManager Instance { get; private set; }
    public DamageTextGenerator TextGenerator => _generator;
    #endregion

    #region serialize
    #endregion

    #region private
    private DamageTextGenerator _generator;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        Instance = this;
        _generator = GetComponent<DamageTextGenerator>();
    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
}