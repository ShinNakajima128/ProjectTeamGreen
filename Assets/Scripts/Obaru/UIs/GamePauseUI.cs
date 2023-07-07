using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
    private GameObject[] _skillInfos = default;
    #endregion

    #region private
    private Button _pauseButton;
    #endregion

    #region unity methods
    private void Awake()
    {
        _pauseButton = GetComponent<Button>();
    }

    private void Start()
    {
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
        Debug.Log("Pause");
        Time.timeScale = 0;
        _panel.alpha = 1;
    }

    /// <summary>
    /// 一時停止を解除する
    /// </summary>
    private void PauseEnd()
    {
        Debug.Log("Continue");
        Time.timeScale = 1;
        _panel.alpha = 0;
    }

    /// <summary>
    /// 現在アクティブのスキル
    /// </summary>
    private void CurrentAktiveSkill()
    {
        var activeSkill = SkillManager.Instance.Skills
            .Where(x => x.IsSkillActived)
            .Select(x => x.SkillType)
            .ToList();
    }
    #endregion
}