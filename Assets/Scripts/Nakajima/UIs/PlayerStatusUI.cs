using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;
using DG.Tweening;

/// <summary>
/// プレイヤーのステータスを画面に表示するUI機能を持つコンポーネント
/// </summary>
public class PlayerStatusUI : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [Tooltip("FillAmountの数値のアニメーション時間")]
    [SerializeField]
    private float _fillAnimationTime = 0.1f;

    [Tooltip("現在のHPバーのFillArea")]
    [SerializeField]
    private Image _currentHPFillArea = default;

    [Tooltip("経験値バーのFillArea")]
    [SerializeField]
    private Image _bulkFillArea = default;

    [Tooltip("現在のレベルを表示するTMP")]
    [SerializeField]
    private TextMeshProUGUI _currentLevelTMP = default;

    [Tooltip("プレイヤーステータスのCanvasGroup")]
    [SerializeField]
    private CanvasGroup _playerStatusGroup = default;
    #endregion

    #region private
    Tween _currentTween;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Start()
    { 
        //プレイヤーのHPの値が変化した時の処理を登録
        PlayerController.Instance.Health.ChangeHPObserver
                                        .TakeUntilDestroy(this)
                                        .Subscribe(amount => CurrentHPView(amount));
        //レベルアップ時の処理を登録
        PlayerController.Instance.Status.CurrentPlayerLevel
                                        .TakeUntilDestroy(this)
                                        .Subscribe(amount => CurrentLevelView(amount));

        //経験値を取得した時の処理を登録
        PlayerController.Instance.Status.GetEXPObserver
                                        .TakeUntilDestroy(this)
                                        .Subscribe(amount => CurrentEXPView(amount));
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
        if (_currentTween != null)
        {
            _currentTween.Kill();
            _currentTween = null;
        }
        _currentTween = _bulkFillArea.DOFillAmount(amount, _fillAnimationTime);
    }
    /// <summary>
    /// レベルをUIに表示する
    /// </summary>
    /// <param name="currentLevel">現在のレベル</param>
    private void CurrentLevelView(uint currentLevel)
    {
        _currentLevelTMP.text = $"{currentLevel}";
    }
    /// <summary>
    /// ゲームステータスの表示を切り替える
    /// </summary>
    /// <param name="value">ON(true)/OFF(false)</param>
    public void ChangeActivePanelView(bool value)
    {
        _playerStatusGroup.alpha = Convert.ToInt32(value);
    }
    #endregion
}
