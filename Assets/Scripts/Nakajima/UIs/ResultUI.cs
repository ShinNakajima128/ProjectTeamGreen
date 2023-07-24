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
    private uint _resultPlayerLevel;
    private uint _resultDefeatAmount;
    private bool _isLoading = false;
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
                      .Where(_ => !_isLoading)
                      .Subscribe(_ => OnRestart());

        _returnTitleButton.OnClickAsObservable()
                          .TakeUntilDestroy(this)
                          .Where(_ => !_isLoading)
                          .Subscribe(_ => OnReturnTitle());
    }
    #endregion

    #region public method
    /// <summary>
    /// リザルト画面を表示する
    /// </summary>
    public void OnResultView()
    {
        //StartCoroutine(OnResultCoroutine());
        OnResultAsync(this.GetCancellationTokenOnDestroy()).Preserve().Forget();
        //Debug.Log("call");
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
        try
        {
            _resultParent.SetActive(true);
            _resultDefeatAmount = EnemyManager.Instance.DefeatAmount.Value;
            _resultPlayerLevel = PlayerController.Instance.Status.CurrentPlayerLevelAmount;
            await UniTask.Delay(TimeSpan.FromSeconds(2.0f));

            FadeManager.Fade(FadeType.Out, () =>
            {
                FadeManager.Fade(FadeType.In);
                _gameEndTMP.enabled = false;
                ChangeResultView(true);
                StageManager.Instance.OnGameReset();
            });

            //フェード処理が終了するまで待機
            await UniTask.WaitUntil(() => !FadeManager.IsFading);

            await UniTask.Delay(1000);

            //現在のプレイヤーレベルを表示
            _playerLevelViewObj.SetActive(true);
            _playerLevelTMP.text = _resultPlayerLevel.ToString();

            await UniTask.Delay(1000);

            //討伐数を表示する処理
            _defeatAmountViewObj.SetActive(true);
            await DOTween.To(() =>
                          _currentDefeatAmount,
                          x => _currentDefeatAmount = x,
                          _resultDefeatAmount,
                          _viewAnimTime)
                         .OnUpdate(() =>
                         {
                             _defeatAmountTMP.text = _currentDefeatAmount.ToString();
                         })
                         .AsyncWaitForCompletion();


            await UniTask.Delay(1000);

            //もう一度遊ぶかタイトルに戻るか選択するボタンを表示
            _buttonsParent.SetActive(true);
        }
        catch (Exception e)
        {
            Debug.LogError($"エラーが発生しました:{e}");
        }
    }

    /// <summary>
    /// リザルト画面の各ボタンが押された時の処理を実行する
    /// </summary>
    /// <param name="action">フェード後の処理</param>
    private void OnPressResultButtonAction(Action action)
    {
        if (Time.timeScale < 1)
        {
            Time.timeScale = 1;
        }
        _isLoading = true;
        FadeManager.Fade(FadeType.Out, () =>
        {
            FadeManager.Fade(FadeType.In);
            _gameEndTMP.enabled = true;
            _currentDefeatAmount = 0;
            _playerLevelViewObj.SetActive(false);
            _defeatAmountViewObj.SetActive(false);
            _buttonsParent.SetActive(false);
            _resultParent.SetActive(false);
            ChangeResultView(false);
            _isLoading = false;
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
