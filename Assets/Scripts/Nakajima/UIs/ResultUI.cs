using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using DG.Tweening;

/// <summary>
/// リザルト画面のUIを管理するコンポーネント
/// </summary>
public class ResultUI : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [Header("Variables")]
    [Tooltip("各アニメーションの時間")]
    [SerializeField]
    private float _viewAnimTime = 1.5f;

    [Header("Objects_UI")]
    [Tooltip("リザルト画面全体の親オブジェクト")]
    [SerializeField]
    private GameObject _resultParent = default;

    [Tooltip("リザルト画面のGroup")]
    [SerializeField]
    private CanvasGroup _resultGroup = default;

    [Tooltip("ゲーム終了時に表示するTMP")]
    [SerializeField]
    private TextMeshProUGUI _gameEndTMP = default;

    [Tooltip("プレイヤーレベルを表示するオブジェクト")]
    [SerializeField]
    private GameObject _playerLevelViewObj = default;

    [Tooltip("ゲーム終了時点のプレイヤーのレベルを表示するTMP")]
    [SerializeField]
    private TextMeshProUGUI _playerLevelTMP = default;

    [Tooltip("討伐数を表示するオブジェクト")]
    [SerializeField]
    private GameObject _defeatAmountViewObj = default;

    [Tooltip("討伐数を表示するTMP")]
    [SerializeField]
    private TextMeshProUGUI _defeatAmountTMP = default;

    [Tooltip("各ボタンの親オブジェクト")]
    [SerializeField]
    private GameObject _buttonsParent = default;

    [Tooltip("リスタートボタン")]
    [SerializeField]
    private Button _restartButton = default;

    [Tooltip("タイトルに戻るボタン")]
    [SerializeField]
    private Button _returnTitleButton = default;
    #endregion

    #region private
    /// <summary>倒した数</summary>
    private uint _currentDefeatAmount;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        ChangeResultView(false);

        _restartButton.OnClickAsObservable()
                      .TakeUntilDestroy(this)
                      .Subscribe(_ => OnRestart());

        _returnTitleButton.OnClickAsObservable()
                          .TakeUntilDestroy(this)
                          .Subscribe(_ => OnReturnTitle());
    }
    #endregion

    #region public method
    /// <summary>
    /// リザルト画面を表示する
    /// </summary>
    public void OnResultView()
    {
        OnResultAsync(this.GetCancellationTokenOnDestroy()).Forget();
        Debug.Log("call");
    }
    #endregion

    #region private method
    /// <summary>
    /// ゲームをリスタートする
    /// </summary>
    private void OnRestart()
    {
        OnPressResultButtonAction(() =>
        {
            StageManager.Instance.OnGameStart();
            AudioManager.PlayBGM(BGMType.InGame);
        });
    }
    /// <summary>
    /// タイトル画面に戻る
    /// </summary>
    private void OnReturnTitle()
    {
        OnPressResultButtonAction(() =>
        {
            HUDManager.Instance.OnTitleView();
        });
    }

    /// <summary>
    /// リザルト画面の表示を切り替える
    /// </summary>
    /// <param name="value">ON/OFF</param>
    private void ChangeResultView(bool value)
    {
        _resultGroup.alpha = Convert.ToInt32(value);
        _resultGroup.blocksRaycasts = value;
        _resultGroup.interactable = value;
    }
    #endregion

    #region UniTask method
    /// <summary>
    /// リザルト画面を表示するUniTask
    /// </summary>
    private async UniTask OnResultAsync(CancellationToken token)
    {
        _resultParent.SetActive(true);

        await UniTask.Delay(TimeSpan.FromSeconds(2.0f));

        FadeManager.Fade(FadeType.Out, () =>
        {
            FadeManager.Fade(FadeType.In);
            _gameEndTMP.enabled = false;
            ChangeResultView(true);
        });

        //フェード処理が終了するまで待機
        await UniTask.WaitUntil(() => !FadeManager.IsFading, cancellationToken: token);

        await UniTask.Delay(1000, cancellationToken: token);

        //現在のプレイヤーレベルを表示
        _playerLevelViewObj.SetActive(true);
        _playerLevelTMP.text = PlayerController.Instance.Status.CurrentPlayerLevelAmount.ToString();

        await UniTask.Delay(1000, cancellationToken: token);

        //討伐数を表示する処理
        _defeatAmountViewObj.SetActive(true);
        await DOTween.To(() =>
                      _currentDefeatAmount,
                      x => _currentDefeatAmount = x,
                      EnemyManager.Instance.DefeatAmount.Value,
                      _viewAnimTime)
                     .OnUpdate(() =>
                     {
                         _defeatAmountTMP.text = _currentDefeatAmount.ToString();
                     })
                     .AsyncWaitForCompletion();

        await UniTask.Delay(1000, cancellationToken: token);

        //もう一度遊ぶかタイトルに戻るか選択するボタンを表示
        _buttonsParent.SetActive(true);
    }

    /// <summary>
    /// リザルト画面の各ボタンが押された時の処理を実行する
    /// </summary>
    /// <param name="action">フェード後の処理</param>
    private void OnPressResultButtonAction(Action action)
    {
        FadeManager.Fade(FadeType.Out, () =>
        {
            FadeManager.Fade(FadeType.In);
            _resultParent.SetActive(false);
            StageManager.Instance.OnGameReset();
            action?.Invoke();
        });
    }
    #endregion

    /// <summary>
    /// ボタンの種類
    /// </summary>
    enum ButtonType
    {
        Restart,
        ReturnTitle
    }
}
