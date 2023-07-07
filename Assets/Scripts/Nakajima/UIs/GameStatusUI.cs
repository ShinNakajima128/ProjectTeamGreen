using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

/// <summary>
/// ゲームの状態をUIに表示する機能を持つコンポーネント
/// </summary>
public class GameStatusUI : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [Tooltip("討伐数を表示するTMP")]
    [SerializeField]
    private TextMeshProUGUI _defeatTMP = default;

    [SerializeField]
    private CanvasGroup _gameStatusGroup = default;
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
        //討伐数が変化した時の処理を登録
        EnemyManager.Instance.DefeatedEnemyAmountViewObserver
                             .Subscribe(value => ViewDefeatedAmount(value))
                             .AddTo(this);
    }
    #endregion

    #region public method
    /// <summary>
    /// ゲームステータスの表示を切り替える
    /// </summary>
    /// <param name="value">ON(true)/OFF(false)</param>
    public void ChangeActivePanelView(bool value)
    {
        _gameStatusGroup.alpha = Convert.ToInt32(value);
    }
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
