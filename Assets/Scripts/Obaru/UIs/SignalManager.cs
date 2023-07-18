using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UniRx;

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

    [SerializeField]
    private float _skipFrame = 1020f;
    #endregion

    #region private
    private float _directorFps = 60f;
    /// <summary>PlayableDirectorコンポーネント格納用</summary>
    private PlayableDirector _playableDirector;
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
        _skipButton.onClick.AddListener(() => OnClickSkipButton());
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
        FadeManager.Fade(FadeType.Out, () =>
        {
            FadeManager.Fade(FadeType.In);
            _changeTitleActiveSubject.OnNext(Unit.Default);
        });
    }

    /// <summary>
    ///　スキップボタンを押した時の処理
    /// </summary>
    public void OnClickSkipButton()
    {
        _playableDirector.time = _skipFrame / _directorFps;
    }
    #endregion
}