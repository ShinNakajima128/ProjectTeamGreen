using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [Tooltip("スキルレベル表示用の星")]
    [SerializeField]
    private Image[] _stars;

    [Tooltip("スキルレベル表示用のスプライト")]
    [SerializeField]
    private Sprite _startSprite;

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
        ChangeStartSprite();

        gameObject.SetActive(false);
    }
    #endregion

    #region public method
    /// <summary>
    /// テキストの更新
    /// </summary>
    public void ChangeStartSprite()
    {
        //現在のスキルレベルを取得
        _currentLevel = SkillManager.Instance.Skills
                                    .FirstOrDefault(x => x.SkillType == _skillType)
                                    .CurrentSkillLevel;

        for(int i = 0; i < _currentLevel; i++)
        {
            _stars[i].sprite = _startSprite;
        }
    }
    #endregion
}