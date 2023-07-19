using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UniRx;
using DG.Tweening;

/// <summary>
/// スタースプライトを扱うクラス
/// </summary>
public class StarUI : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("スキルのレベルを表す星")]
    [SerializeField]
    private Image[] _stars;

    [Tooltip("レベル変更時の星")]
    [SerializeField]
    private Sprite _filledStarSprite;

    [Tooltip("スキルタイプ")]
    [SerializeField]
    private SkillType _type = default;
    #endregion

    #region private
    private List<SkillType> _activeSkill = default;

    private int _currentLevel;
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
        PlayerController.Instance.Status.CurrentPlayerLevel
                        .TakeUntilDestroy(this)
                        .Subscribe(_ => IncreaseStar());
    }

    private void Update()
    {

    }
    #endregion

    #region public method
    #endregion

    #region private method
    private void IncreaseStar()
    {
        //アクティブ中のスキルを取得
        _activeSkill = SkillManager.Instance.Skills
                                   .Where(x => x.IsSkillActived)
                                   .Select(x => x.SkillType)
                                   .ToList();

        //現在のレベルを取得
        _currentLevel = SkillManager.Instance.Skills
                                    .FirstOrDefault(x => x.SkillType == _type)
                                    .CurrentSkillLevel;

        Image flashingImage = _stars[_currentLevel];

        if (_activeSkill.Any(x => x == _type))
        {
            flashingImage.sprite = _filledStarSprite;
            
            _stars[_currentLevel - 1].sprite = _filledStarSprite;
            _stars[_currentLevel - 1].DOKill();
            _stars[_currentLevel - 1].color = new Color(flashingImage.color.r, flashingImage.color.g, flashingImage.color.b, 1f);

            if (_currentLevel < _stars.Length)
            {
                Flashing(flashingImage);
            }
        }
        else
        {
            flashingImage = _stars[0];
            flashingImage.sprite = _filledStarSprite;

            Flashing(flashingImage);
        }
    }
    #endregion

    private void Flashing(Image flashingImage)
    {
        _stars[_currentLevel].color = new Color(flashingImage.color.r, flashingImage.color.g, flashingImage.color.b, 1f);
        flashingImage.DOFade(0f, 1f)
                     .SetEase(Ease.InQuad)
                     .SetLoops(-1, LoopType.Yoyo)
                     .SetUpdate(true);
    }

}
