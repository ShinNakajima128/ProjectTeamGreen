using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

/// <summary>
/// 時間関連の機能を管理するManagerクラス
/// </summary>
public class TimeManager : MonoBehaviour
{
    #region property
    public static TimeManager Instance { get; private set; }
    #endregion

    #region serialize
    [Tooltip("制限時間(分)")]
    [SerializeField]
    private uint _limitTime = 15;
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
        Instance = this;
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
