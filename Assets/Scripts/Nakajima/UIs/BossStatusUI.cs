using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

/// <summary>
/// 現在出現しているボスのUIを管理するコンポーネント
/// </summary>
public class BossStatusUI : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [Tooltip("ボス名を表示するTMP")]
    [SerializeField]
    private TextMeshProUGUI _bossNameTMP = default;

    [Tooltip("ボスステータスのGroup")]
    [SerializeField]
    private CanvasGroup _bossStatusGroup = default;

    [Tooltip("HPゲージのFillArea")]
    [SerializeField]
    private Image _currentBossHPFillImage = default;
    #endregion

    #region private
    private float _currentBossMaxHP;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    #endregion

    #region public method
    /// <summary>
    /// ボスのHPを表示する
    /// </summary>
    public void OnBossHPView()
    {
        StartCoroutine(BossHPViewCoroutine());
    }

    /// <summary>
    /// ボスステータス画面の表示を切り替える
    /// </summary>
    /// <param name="value">ON/OFF</param>
    public void ChangeStatusView(bool value)
    {
        _bossStatusGroup.alpha = Convert.ToInt32(value);
        _bossStatusGroup.blocksRaycasts = value;
        _bossStatusGroup.interactable = value;
    }
    /// <summary>
    /// 現在のボスのデータをUIにセットする
    /// </summary>
    /// <param name="boss">ボスデータ</param>
    public void SetBossData(Enemy boss)
    {
        _bossNameTMP.text = boss.EnemyName;
        _currentBossMaxHP = boss.EnemyPrefab.MaxHP;

        Debug.Log($"現在のボス：{boss.EnemyName} 最大HP：{boss.EnemyPrefab.MaxHP}");
    }
    /// <summary>
    /// ボスのHPをゲージに反映させる
    /// </summary>
    /// <param name="current">現在のHP</param>
    public void ApplyCurrentHP(float current)
    { 
        _currentBossHPFillImage.fillAmount = current / _currentBossMaxHP;
    }
    #endregion

    #region private method
    #endregion

    #region coroutine method
    private IEnumerator BossHPViewCoroutine()
    {
        yield return new WaitForSeconds(5f);

        ChangeStatusView(true);
    }
    #endregion
}
