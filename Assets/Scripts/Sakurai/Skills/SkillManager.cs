using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SkillManager : MonoBehaviour
{
    #region property
    public static SkillManager Instance { get; private set; }
    #endregion

    #region serialize
    [Tooltip("プレイヤーの各スキル")]
    [SerializeField]
    private SkillBase[] _skills = default;
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
        Instance = this;
    }

    private void Start()
    {

    }
    #endregion

    #region public method
    /// <summary>
    /// スキルをセットする
    /// </summary>
    /// <param name="type">スキルの種類</param>
    public void SetSkill(SkillType type)
    {
        //スキル一覧から指定されたスキルを探索
        var skill = _skills.FirstOrDefault(x => x.Type == type);

        //スキルが非アクティブの場合はアクティブにする
        if (!skill.IsSkillActived)
        {
            skill.OnSkillAction();
        }
        //既にアクティブの場合はスキルのレベルを上げる
        else
        {
            skill.LebelUpSkill();
        }
    }
    #endregion

    #region private method
    #endregion
}
