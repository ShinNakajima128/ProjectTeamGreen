using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

/// <summary>
/// HUDの機能全般を管理するManagerクラス
/// </summary>
[RequireComponent(typeof(GameStatusUI))]
[RequireComponent(typeof(PlayerStatusUI))]
[RequireComponent(typeof(TitleUI))]
[RequireComponent(typeof(GamePauseUI))]
[RequireComponent(typeof(SkillUpSelect))]
[RequireComponent(typeof(ResultUI))]
public class HUDManager : MonoBehaviour
{
    #region property
    public static HUDManager Instance { get; private set; }
    public SkillUpSelect SkillUpSelect => _skillUpSelect;
    #endregion

    #region serialize
    #endregion

    #region private
    private GameStatusUI _gameStatus;
    private PlayerStatusUI _playerStatus;
    private TitleUI _title;
    private GamePauseUI _gamePause;
    private SkillUpSelect _skillUpSelect;
    private ResultUI _result;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        Setup();
    }

    private void Start()
    {
        StageManager.Instance.IsInGameObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(value => ChangeHUDPanelActive(value));

        //ゲーム終了時にリザルト画面を表示する処理を登録
        StageManager.Instance.GameEndObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(_ =>
                             {
                                 _result.OnResultView();
                             });
    }
    #endregion

    #region public method
    #endregion

    #region private method
    /// <summary>
    /// HUDの表示非表示を切り替える
    /// </summary>
    /// <param name="value">ON(true)/OFF(false)</param>
    private void ChangeHUDPanelActive(bool value)
    {
        _playerStatus.ChangeActivePanelView(value);
        _gameStatus.ChangeActivePanelView(value);
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Setup()
    {
        Instance = this;
        _gameStatus = GetComponent<GameStatusUI>();
        _playerStatus = GetComponent<PlayerStatusUI>();
        _title = GetComponent<TitleUI>();
        _gamePause = GetComponent<GamePauseUI>();
        _skillUpSelect = GetComponent<SkillUpSelect>();
        _result = GetComponent<ResultUI>();

        _playerStatus.ChangeActivePanelView(false);
        _gameStatus.ChangeActivePanelView(false);

        //タイトル画面のスタートボタンを押した時の処理を登録
        _title.PressStartButtonObserver
              .TakeUntilDestroy(this)
              .Subscribe(_ =>
              {
                  StageManager.Instance.OnGameStart();
              });
    }
    #endregion
}
