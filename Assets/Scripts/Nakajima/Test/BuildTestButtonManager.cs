using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// ビルド時のテスト用ボタンの機能をまとめたManagerクラス
/// </summary>
public class BuildTestButtonManager : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private Button[] _skillTestButtons = default;
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
        for (int i = 0; i < _skillTestButtons.Length; i++)
        {
            SkillType type = (SkillType)i;

            _skillTestButtons[i].OnClickAsObservable()
                                .Subscribe(_ =>
                                {
                                    OnSkill(type);
                                })
                                .AddTo(this);
        }
    }
    #endregion

    #region public method
    #endregion

    #region private method
    /// <summary>
    /// スキルを発動する
    /// </summary>
    /// <param name="type">スキルの種類</param>
    private void OnSkill(SkillType type)
    {
        SkillManager.Instance.SetSkill(type);
    }
    #endregion
}
