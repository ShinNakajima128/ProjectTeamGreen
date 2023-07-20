using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UniRx;

/// <summary>
/// シグナル関連のManager
/// </summary>
[RequireComponent(typeof(PlayableDirector))]
public class SignalManager : MonoBehaviour
{
    #region property
    public static SignalManager Instance { get; private set; }
    public IObservable<Unit> ChangeTitleActiveObserver => _changeTitleActiveSubject;
    #endregion

    #region serialize
    [Tooltip("スキップボタン")]
    [SerializeField]
    private Button _skipButton = default;

    [Tooltip("スキップ先のフレーム")]
    [SerializeField]
    private float _skipFrame = 1020f;
    #endregion

    #region private
    /// <summary>timelineのfps</summary>
    private float _directorFps = 60f;
    /// <summary>PlayableDirectorコンポーネント格納用</summary>
    private PlayableDirector _playableDirector;
    /// <summary>スキップ時のフラグ</summary>
    private bool _isSkipped = false;
    #endregion

    #region Event
    /// <summary>タイトルパネルのアクティブを切り替えるSubject</summary>
    private Subject<Unit> _changeTitleActiveSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    private void Awake()
    {
        Instance = this;
        _playableDirector = GetComponent<PlayableDirector>();

        //スキップボタンを押した時の処理を追加
        _skipButton.OnClickAsObservable()
            .TakeUntilDestroy(this)
            //連打防止処理。一度押されてから「5」秒経つまでは次の処理を実行しない
            .ThrottleFirst(TimeSpan.FromMilliseconds(5000))
            .Subscribe(_ => OnClickSkipButton());
    }
    #endregion

    #region public method
    /// <summary>
    /// タイトルパネルを非アクティブにする
    /// </summary>
    public void TitlePanelInactive()
    {
        _changeTitleActiveSubject.OnNext(Unit.Default);
    }

    /// <summary>
    /// オープニングがフェードアウトしてタイトルにフェードイン
    /// </summary>
    public void FadeOpening()
    {
        if (!_isSkipped)
        {
            FadeManager.Fade(FadeType.Out, () =>
            {
                FadeManager.Fade(FadeType.In);
                gameObject.SetActive(false);
                _changeTitleActiveSubject.OnNext(Unit.Default);
                AudioManager.PlayBGM(BGMType.Title);
            });
        }
    }

        /// <summary>
        ///　スキップボタンを押した時の処理
        /// </summary>
        public void OnClickSkipButton()
    {
        FadeManager.Fade(FadeType.Out, () =>
        {
            FadeManager.Fade(FadeType.In);
            _playableDirector.time = _skipFrame / _directorFps;
            gameObject.SetActive(false);
            _changeTitleActiveSubject.OnNext(Unit.Default);
            AudioManager.PlayBGM(BGMType.Title);
            _isSkipped = true;
        });
    }
    #endregion
}