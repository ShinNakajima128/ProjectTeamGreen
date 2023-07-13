using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

/// <summary>
/// ポーズ画面中に表示するスキルの情報
/// </summary>
public class SkillInfo : MonoBehaviour
{
    #region property
    public SkillType SkillType => _skillType;
    #endregion

    #region serialize
    [Tooltip("スキルレベル表示用のテキスト")]
    [SerializeField]
    private TextMeshProUGUI _currentLevelText = default;

    [Tooltip("スキルタイプ")]
    [SerializeField]
    private SkillType _skillType = default;
    #endregion

    #region private
    /// <summary>現在のレベル</summary>
    private int _currentLevel;
    #endregion

    #region unity methods
    private void Start()
    {
        RewriteCurrentLevelText();

        gameObject.SetActive(false);
    }
    #endregion

    #region public method
    /// <summary>
    /// テキストの更新
    /// </summary>
    public void RewriteCurrentLevelText()
    {
        //現在のスキルレベルを取得
        _currentLevel = SkillManager.Instance.Skills
                                    .FirstOrDefault(x => x.SkillType == _skillType)
                                    .CurrentSkillLevel;

        _currentLevelText.text = $"LV.{_currentLevel}";
    }
    #endregion
}