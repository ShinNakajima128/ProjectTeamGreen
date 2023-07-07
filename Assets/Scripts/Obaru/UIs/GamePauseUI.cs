using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// ポーズボタンを押した時の処理
/// </summary>
public class GamePauseUI : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [Tooltip("ポーズ画面をまとめたパネル")]
    [SerializeField]
    private CanvasGroup _panel = default;

    [Tooltip("Continueボタン")]
    [SerializeField]
    private Button _continueButton = default;

    [Tooltip("スキル情報")]
    [SerializeField]
    private SkillInfo[] _skillInfos = default;
    #endregion

    #region private
    private bool _isPause = false;
    private Button _pauseButton;
    private List<SkillType> _activeSkill;
    #endregion

    #region unity methods
    private void Awake()
    {
        _pauseButton = GetComponent<Button>();
    }

    private void Start()
    {
        //ボタン押下時の処理を設定
        _pauseButton.onClick.AddListener(() => GamePause());
        _continueButton.onClick.AddListener(() => PauseEnd());
    }
    #endregion

    #region public method
    #endregion

    #region private method
    /// <summary>
    /// 一時停止
    /// </summary>
    private void GamePause()
    {
        //ポーズ中ならreturn
        if (_isPause) return;

        //時間を止める
        Time.timeScale = 0;

        //ポーズ画面を表示
        _panel.alpha = 1;

        ActiveSkill();
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
        _isPause = false;
    }

    /// <summary>
    /// 現在持っているスキルをアクティブにする
    /// </summary>
    private void ActiveSkill()
    {
        //現在持っているスキルのスキルタイプを取得
        _activeSkill = SkillManager.Instance.Skills
            .Where(x => x.IsSkillActived)
            .Select(x => x.SkillType)
            .ToList();

        //現在持っているスキルのアイコンとレベルを表示
        foreach(SkillInfo info in _skillInfos)
        {
            if(_activeSkill.Any(x=>x == info.ThisSkillType))
            {
                info.gameObject.SetActive(true);
                info.RewriteCurrentLevelText();
            }
        }
    }
    #endregion
}