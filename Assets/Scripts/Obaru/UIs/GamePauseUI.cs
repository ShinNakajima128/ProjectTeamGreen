using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// ポーズボタンを押した時の処理
/// </summary>
public class GamePauseUI : MonoBehaviour
{
    #region serialize
    [Tooltip("ポーズ画面をまとめたパネル")]
    [SerializeField]
    private CanvasGroup _panel = default;

    [Tooltip("Continueボタン")]
    [SerializeField]
    private Button _continueButton = default;

    [Tooltip("Pauseボタン")]
    [SerializeField]
    private Button _pauseButton = default;

    [Tooltip("スキル情報")]
    [SerializeField]
    private SkillInfo[] _skillInfos = default;
    #endregion

    #region private
    /// <summary>ポーズ中かどうか</summary>
    private bool _isPause = false;
    /// <summary>現在アクティブのスキルのリスト</summary>
    private List<SkillType> _activeSkill;
    #endregion

    #region unity methods
    private void Awake()
    {
        //ポーズボタンを押した時の処理を追加
        _pauseButton.OnClickAsObservable()
            .TakeUntilDestroy(this)
            //連打防止処理。一度押されてから「5」秒経つまでは次の処理を実行しない
            .ThrottleFirst(TimeSpan.FromMilliseconds(5000))
            .Subscribe(_ => PauseGame());

        //コンティニューボタンを押した時の処理を追加
        _continueButton.OnClickAsObservable()
            .TakeUntilDestroy(this)
            //連打防止処理。一度押されてから「5」秒経つまでは次の処理を実行しない
            .ThrottleFirst(TimeSpan.FromMilliseconds(5000))
            .Subscribe(_ => PauseEnd());
    }
    #endregion

    #region private method
    /// <summary>
    /// 一時停止
    /// </summary>
    private void PauseGame()
    {
        //ポーズ中ならreturn
        if (_isPause) return;

        //時間を止める
        Time.timeScale = 0;

        //ポーズ画面を表示
        _panel.alpha = 1;
        _panel.interactable = true;

        ActiveSkillInfo();
        _isPause = true;
    }

    /// <summary>
    /// 一時停止を解除する
    /// </summary>
    private void PauseEnd()
    {
        //ポーズ中でなければreturn
        if (!_isPause) return;

        //時を動かす
        Time.timeScale = 1;

        //ポーズ画面を非表示
        _panel.alpha = 0;
        _panel.interactable = false;
        _isPause = false;
    }

    /// <summary>
    /// 現在持っているスキル情報をアクティブにする
    /// </summary>
    private void ActiveSkillInfo()
    {
        //現在持っているスキルのスキルタイプを取得
        _activeSkill = SkillManager.Instance.Skills
                                   .Where(x => x.IsSkillActived)
                                   .Select(x => x.SkillType)
                                   .ToList();

        //現在持っているスキルのアイコンとレベルを表示
        foreach (SkillInfo info in _skillInfos)
        {
            if (_activeSkill.Any(x => x == info.SkillType))
            {
                info.gameObject.SetActive(true);
                info.RewriteCurrentLevelText();
            }
        }
    }
    #endregion
}