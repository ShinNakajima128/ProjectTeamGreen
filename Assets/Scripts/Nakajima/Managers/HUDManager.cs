using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

/// <summary>
/// HUDの機能全般を管理するManagerクラス
/// </summary>
public class HUDManager : MonoBehaviour
{
    #region property
    public static HUDManager Instance { get; private set; }
    #endregion

    #region serialize
    [Tooltip("討伐数を表示するTMP")]
    [SerializeField]
    private TextMeshProUGUI _defeatTMP = default;
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
        //討伐数が変化した時の処理を登録
        EnemyManager.Instance.DefeatedEnemyAmountViewObserver
                             .Subscribe(value => ViewDefeatedAmount(value))
                             .AddTo(this);
    }
    #endregion

    #region public method
    #endregion

    #region private method
    /// <summary>
    /// 討伐数を表示する
    /// </summary>
    /// <param name="amount">討伐数</param>
    private void ViewDefeatedAmount(uint amount)
    {
        _defeatTMP.text = $"{amount}";
    }
    #endregion
}
