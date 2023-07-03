using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

/// <summary>
/// プレイヤーのステータスを画面に表示するUI機能を持つコンポーネント
/// </summary>
public class PlayerStatusUI : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [Tooltip("現在のHPバーのFillArea")]
    [SerializeField]
    private Image _currentHPFillArea = default;

    [Tooltip("経験値バーのFillArea")]
    [SerializeField]
    private Image _bulkFillArea = default;

    [Tooltip("現在のレベルを表示するTMP")]
    [SerializeField]
    private TextMeshProUGUI _currentLevelTMP = default; 
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
        //レベルアップ時の処理を登録
        PlayerController.Instance.Status.CurrentPlayerLevel
                                        .Subscribe(amount => CurrentLevelView(amount))
                                        .AddTo(this);
    }
    #endregion

    #region public method
    #endregion

    #region private method
    /// <summary>
    /// 現在の残りHPを表示する
    /// </summary>
    /// <param name="amount">残りHPの割合</param>
    private void CurrentHPView(float amount)
    {
        _currentHPFillArea.fillAmount = amount;
    }
    /// <summary>
    /// 現在の取得経験値を表示する
    /// </summary>
    /// <param name="amount">進行度</param>
    private void CurrentEXPView(float amount)
    {
        _bulkFillArea.fillAmount = amount;
    }
    /// <summary>
    /// レベルをUIに表示する
    /// </summary>
    /// <param name="currentLevel">現在のレベル</param>
    private void CurrentLevelView(uint currentLevel)
    {
        _currentLevelTMP.text = $"{currentLevel}";
    }
    #endregion
}
