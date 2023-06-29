using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MascleBoulSkill : SkillBase
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
    private void Awake()
    {

    }

    private void Start()
    {

    }

    private void Update()
    {

    }
    #endregion

    #region public method
    public override void OnSkillAction()
    {
        Debug.Log($"{SkillType}スキル発動");
        _isSkillActived = true;
        StartCoroutine(SkillActionCoroutine());
    }

    public override void LebelUpSkill()
    {
        if (_currentSkillLebel >= MAX_LEVEL)
        {
            Debug.Log($"{SkillType}はレベル上限です");

            return;
        }
        _currentSkillLebel++;

        Debug.Log($"レベルアップ!{_currentSkillLebel}にあがった!");
    }

    public override void AttackUpSkill(float coefficient)
    {
        _currentAttackAmount *= coefficient;
    }

    #endregion

    #region private method
    #endregion

    #region coroutine method
    protected override IEnumerator SkillActionCoroutine()
    {
        yield return null;
    }
    #endregion
}