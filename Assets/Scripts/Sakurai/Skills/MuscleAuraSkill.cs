using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// オーラを扱うオブジェクト
/// </summary>
public class MuscleAuraSkill : SkillBase
{
    #region property
    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("1~3レベルまでのオーラ")]
    [SerializeField]
    private Aura _auraPrefab = default;

    [Tooltip("4~5レベルまでのオーラ")]
    [SerializeField]
    private Aura _highAuraPrefab = default;
    #endregion

    #region private

    /// <summary>スキルアップ時のスケールに対する数値</summary>
    private float _sumAmount = 1.0f;

    /// <summary>現在のオーラ</summary>
    Aura _currentAura = default;
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
    #endregion

    #region public method

    /// <summary>
    /// スキル発動時のアクション
    /// </summary>
    public override void OnSkillAction()
    {
        Debug.Log($"{SkillType}スキル発動");
        _isSkillActived = true;
        CreateNewAura(_auraPrefab);
    }

    /// <summary>
    /// スキルをレベルアップさせる
    /// </summary>
    public override void LebelUpSkill()
    {
        //既にレベルが最大値の場合は処理を行わない
        if (_currentSkillLebel >= MAX_LEVEL)
        {
            Debug.Log($"{SkillType}はレベル上限です");

            return;
        }
        _currentSkillLebel++;
        
        if (_currentSkillLebel <= 3 || _currentSkillLebel == 5)
        {
            _currentAura.SizeChange(_sumAmount);
        }

        if (_currentSkillLebel == 4)
        {
           CreateNewAura(_highAuraPrefab);
        }

        Debug.Log($"レベルアップ!{_currentSkillLebel}にあがった!");
    }

    /// <summary>
    /// スキルの攻撃力を上げる
    /// </summary>
    /// <param name="coefficient"></param>
    public override void AttackUpSkill(float coefficient)
    {
        _currentAttackAmount *= coefficient;
    }
    #endregion

    #region private method

    /// <summary>
    /// オーラを生成する
    /// </summary>
    /// <param name="aura">3レベルまではAura。4レベルからHighAura</param>
    private void CreateNewAura(Aura aura)
    {
        if (_currentAura != null)
        {
            _currentAura.gameObject.SetActive(false);
        }
        _currentAura = Instantiate(aura, transform);
        _currentAura.SetAttackAmount(_currentAttackAmount);
    }
    #endregion

    #region coroutine method
    protected override IEnumerator SkillActionCoroutine()
    {
        yield return null;
    }
    #endregion

}