using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 画面のフェード機能を管理するManagerクラス
/// </summary>
public class FadeManager : MonoBehaviour
{
    #region property
    /// <summary>フェードマネージャーのプロパティ</summary>
    public static FadeManager Instance { get; private set; }
    public static bool IsFading => Instance._isFading;
    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("フェードにかける時間")]
    [SerializeField]
    private float _fadeTime = 1.0f;

    [Tooltip("フェード用のマテリアル")]
    [SerializeField]
    private Material _effectMaterial = null;
    #endregion

    #region private
    /// <summary>シェーダープロパティにアクセスするためのIDを格納。読み取り専用</summary>
    private readonly int _progressId = Shader.PropertyToID("_progress");
    /// <summary>フェード中かどうかのフラグ</summary>
    private bool _isFading = false;
    /// <summary>フェードアウトの時に途中で待ち合わせる時間</summary>
    private float _fadeWaitTime = 1.0f;
    #endregion

    #region unity methods
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region public method
    /// <summary>
    /// 他クラスからFade機能を扱うためのメソッド
    /// </summary>
    /// <param name="type">フェードインかフェードアウト</param>
    /// <param name="callback">フェード完了後のイベント</param>
    public static void Fade(FadeType type, Action callback = null)
    {
        if (Instance._isFading)
        {
            return;
        }
        Instance.StartCoroutine(Instance.Transition(type, callback));
        Instance._isFading = true;
    }
    #endregion

    #region coroutine method
    /// <summary>
    /// フェードを扱うコルーチン
    /// </summary>
    /// <param name="type">フェードインかフェードアウト</param>
    /// <param name="callback">フェード完了後のイベント</param>
    /// <returns></returns>
    IEnumerator Transition(FadeType type, Action callback = null)
    {
        //フェードアウトの時のみ待ち合わせのフラグ。
        bool hasWaiting = false;

        yield return ShaderFade(type);

        ////フェードの処理。
        //while ((type == FadeType.In && currentValue < _fadeTime) || (type == FadeType.Out && currentValue > 0))
        //{
        //    //スライダーを動かすための値をprogressに代入。
        //    float progress = currentValue / _fadeTime;

        //    //第一引数はマテリアルのプロパティの値(Slider),第二引数は第一引数を変化させるための値。
        //    _effectMaterial.SetFloat(_progressId, progress);

        //    yield return null;

        //    //フェードアウト中にprogressの値は0.1を下回ったら1秒だけフェードを待つ。
        //    if (type == FadeType.Out && progress < 0.1f && !hasWaiting)
        //    {
        //        yield return new WaitForSeconds(_fadeWaitTime);
        //        hasWaiting = true;
        //    }
        //    //フェードインであれば値を足して、フェードアウトであれな値を減らす。
        //    currentValue += (type == FadeType.In ? Time.deltaTime : -Time.deltaTime);
        //}
        //フェードさせるマテリアルの値を指定の数字にセット。
        //_effectMaterial.SetFloat(_progressId, (type == FadeType.In) ? 1f : 0f);

        //Actionに登録されていれば実行。
        callback?.Invoke();
    }
    #endregion

    IEnumerator ShaderFade(FadeType type)
    {
        (float currentValue, float endValue) = type == FadeType.In ? (0f, 1f) : (1f, 0f);

        yield return DOTween.To(() => currentValue,
                                 x => currentValue = x,
                                 endValue,
                                 _fadeTime)
                                .SetEase(Ease.Linear)
                                .OnUpdate(() =>
                                {
                                    //第一引数はマテリアルのプロパティの値(Slider),第二引数は第一引数を変化させるための値。
                                    _effectMaterial.SetFloat(_progressId, currentValue);
                                })
                                .WaitForCompletion();

        _effectMaterial.SetFloat(_progressId, (type == FadeType.In) ? 1f : 0f);
        Instance._isFading = false;
    }
}

/// <summary>
/// フェードの種類
/// </summary>
public enum FadeType
{
    In,
    Out  
}