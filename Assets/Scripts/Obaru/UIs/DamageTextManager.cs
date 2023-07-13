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

    #region private
    /// <summary>ダメージテキスト生成コンポーネント</summary>
    private DamageTextGenerator _generator;
    #endregion

    #region unity methods
    private void Awake()
    {
        Instance = this;
        _generator = GetComponent<DamageTextGenerator>();
    }
    #endregion
}