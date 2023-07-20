using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using TMPro;

/// <summary>
/// ボスイベント発生時のUIを表示するコンポーネント
/// </summary>
public class BossEventUI : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [Tooltip("イベント発生時のGroup")]
    [SerializeField]
    private CanvasGroup _bossEventGroup = default;

    [Tooltip("画面全体を赤く点滅させるImage")]
    [SerializeField]
    private Image _backgroundImage = default;

    [SerializeField]
    private TextMeshProUGUI _eventTMP = default;
    #endregion

    #region private
    Tween _currentTween;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        ActivateGroup(false);
    }
    #endregion

    private void Start()
    {
        //ゲームリセット時の処理を登録
        StageManager.Instance.GameResetObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(_ => OnReset());
    }

    #region public method
    public void OnBossEventAction()
    {
        ActivateGroup(true);

        _currentTween = _backgroundImage.DOFade(0.25f, 1f)
                                        .SetEase(Ease.InOutQuad)
                                        .SetLoops(4, LoopType.Yoyo)
                                        .OnComplete(() => 
                                        {
                                            _backgroundImage.DOFade(0f, 0f);
                                            ActivateGroup(false);
                                        });

        _eventTMP.transform.DOScale(1.1f, 0.5f)
                           .SetLoops(-1, LoopType.Yoyo)
                           .SetLink(gameObject, LinkBehaviour.CompleteAndKillOnDisable);
    }
    #endregion

    #region private method
    private void ActivateGroup(bool isActived)
    {
        _bossEventGroup.alpha = Convert.ToInt32(isActived);
    }
    private void OnReset()
    {
        ActivateGroup(false);
        
        if (_currentTween != null)
        {
            _currentTween.Kill();
            _currentTween = null;
        }
        _eventTMP.transform.DOScale(Vector2.one, 0f);
        _backgroundImage.DOFade(0f, 0f);
    }
    #endregion
}
