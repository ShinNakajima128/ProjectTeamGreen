using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSkill : SkillBase
{
    #region property
    #endregion

    #region serialize
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {

    }
    #endregion

    #region public method
    /// <summary>
    /// スキル発動時のアクション
    /// </summary>
    public override void OnSkillAction()
    {
        Debug.Log($"{Type}スキル発動");
        _isSkillActived = true;
    }
    /// <summary>
    /// スキルをレベルアップする
    /// </summary>
    public override void LebelUpSkill()
    {
        //仮でレベルの最大値を「5」にしている
        if (_currentSkillLebel >= 5)
        {
            Debug.Log("レベル上限です");
        }
        _currentSkillLebel++;
        Debug.Log($"レベルアップ!{_currentSkillLebel}に上がった！");
    }
    // <summary>
    /// スキルの攻撃力を上げる
    /// </summary>
    /// <param name="coefficient">係数</param>
    public override void AttackUpSkill(float coefficient)
    {
        _currentAttackAmount *= coefficient;
    }
    #endregion

    #region private method
    #endregion
}
