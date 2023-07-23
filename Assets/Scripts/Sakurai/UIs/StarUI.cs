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
    /// <summary>アクティブ確認用</summary>
    private List<SkillType> _activeSkill = default;

    /// <summary>現在のスキルレベル</summary>
    private int _currentLevel;

    /// <summary>点滅させる星</summary>
    private Image _flashingImage;

    /// <summary>点滅アニメーション格納用</summary>
    private Tween _currentTween;

    /// <summary>リスタートするときのスプライト</summary>
    private Sprite _originStarSprite;
    #endregion

    #region unity methods
    private void Awake()
    {
        _originStarSprite = _stars[1].sprite;
    }

    private void Start()
    {
        //プレイヤーのレベルがあがったときに実行
        PlayerController.Instance.Status.CurrentPlayerLevel
                        .TakeUntilDestroy(this)
                        .Subscribe(_ => IncreaseStar());

        //ゲームリスタート時に実行
        StageManager.Instance.GameResetObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(_ => OnReset());
    }

    private void OnDisable()
    {
        //非表示になったときに点滅を終わらせる。
        if (_currentTween != null)
        {
            _currentTween.Kill();
            _currentTween = null;
        }
    }
    #endregion

    #region private method
    /// <summary>
    /// スキル獲得時に星を増やす
    /// </summary>
    private void IncreaseStar()
    {
        //アクティブ中のスキルを取得
        _activeSkill = SkillManager.Instance.Skills
                                   .Where(x => x.IsSkillActived)
                                   .Select(x => x.SkillType)
                                   .ToList();

        //typeと一致するものを探してスキルレベルを取得
        _currentLevel = SkillManager.Instance.Skills
                                    .FirstOrDefault(x => x.SkillType == _type) 
                                    .CurrentSkillLevel;

        //カレントレベルが最大でなければ点滅させる星を指定
        if (_currentLevel < 5)
        {
            _flashingImage = _stars[_currentLevel];
        }

        //スキルがアクティブ中であれば現在のレベルを点灯させて次のレベルを点滅させる。
        if (_activeSkill.Any(x => x == _type))
        {
            _flashingImage.sprite = _filledStarSprite;
            
            _stars[_currentLevel - 1].sprite = _filledStarSprite;
            _stars[_currentLevel - 1].DOKill();

            if (_currentLevel < _stars.Length)
            {
                Flashing(_flashingImage);
            }
        }
        else
        {
            //アクティブ中でなければ最初のスターを点滅させる。
            _flashingImage = _stars[0];
            _flashingImage.sprite = _filledStarSprite;

            Flashing(_flashingImage);
        }
    }
    #endregion

    /// <summary>
    /// 点滅させるアニメーション
    /// </summary>
    /// <param name="flashingImage"></param>
    private void Flashing(Image flashingImage)
    {
        flashingImage.DOFade(1f, 0f).SetUpdate(true);

        _currentTween = flashingImage.DOFade(0f, 1f)               //1秒かけてフェード
                                     .SetEase(Ease.InQuad)         //徐々に早くなる動き
                                     .SetLoops(-1, LoopType.Yoyo)  //行き来を繰り返す
                                     .SetUpdate(true)              //TimeScaleを無視
                                     .OnKill(() => flashingImage.DOFade(1f,0f).SetUpdate(true));   //非アクティブと同時にアルファ値を1に戻す。
    }

    /// <summary>
    /// リスタート時のスプライトに変更
    /// </summary>
    private void OnReset()
    {
        for (int i = 0; i < _stars.Length; i++)
        {
            if (i == 0)
            {
                continue;
            }
            _stars[i].sprite = _originStarSprite;
        }
    }
}
