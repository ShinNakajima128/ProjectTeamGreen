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
    public SkillType ThisSkillType => _skillType;
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
    private int[] _currentLevel;
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
        _currentLevel = SkillManager.Instance.Skills
                .Where(x => x.SkillType == _skillType)
                .Select(x => x.CurrentSkillLevel).ToArray();

        _currentLevelText.text = $"LV.{_currentLevel[0]}";
    }
    #endregion
}