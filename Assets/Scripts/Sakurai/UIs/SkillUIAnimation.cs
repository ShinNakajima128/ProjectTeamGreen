using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

/// <summary>
/// UIをアニメーションさせるクラス
/// </summary>
public class SkillUIAnimation : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private Image _animationImage;
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
        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                 //TouchAnimation();
            });
    }

    private void Update()
    {

    }

    private void OnEnable()
    {
        _animationImage.transform.localScale = Vector3.zero;
        DisplayAnimation();
    }



    #endregion

    #region public method
    #endregion

    #region private method
    //private void TouchAnimation()
    //{
    //    if (Input.touchCount > 0) 
    //    {
    //        Touch touch = Input.GetTouch(0);

    //        //押した瞬間のアニメーション
    //        if (touch.phase == TouchPhase.Began)
    //        {

    //            Vector3 touchScale = new Vector3(0.95f, 0.95f, transform.localScale.z);
    //            _animationImage.transform.DOScale(touchScale, 0.2f)
    //                                     .SetEase(Ease.Linear)
    //                                     .SetUpdate(true);
    //        }
    //        //離した瞬間のアニメーション
    //        if (touch.phase == TouchPhase.Ended)
    //        {
    //            _animationImage.transform.DOScale(Vector3.one, 0.15f)
    //                                     .SetEase(Ease.OutBack)
    //                                     .SetUpdate(true);
    //        }
    //    }
    //}

    private void DisplayAnimation()
    {
         _animationImage.transform.DOScale(Vector3.one, 0.15f)
                                  .SetEase(Ease.OutBack)
                                  .SetUpdate(true);
    }
    #endregion
}