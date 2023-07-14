using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オーラを扱うスキル機能
/// </summary>
public class MuscleAuraSkill : SkillBase
{
    #region serialize
    [Header("変数")]
    [Tooltip("1~3レベルまでのオーラ")]
    [SerializeField]
    private Aura _auraPrefab = default;

    [Tooltip("4レベルからのオーラ")]
    [SerializeField]
    private Aura _highAuraPrefab = default;

    [Tooltip("スキルアップ時の攻撃に対する係数")]
    [SerializeField]
    private float _attackCoefficient = 1.5f;

    [Tooltip("オーラの見た目が変わった時(スキルレベル4)の攻撃に対する係数")]
    [SerializeField]
    private float _attackHighCoefficient = 3.0f;
    #endregion

    #region private
    /// <summary>スキルアップ時のスケールに対する数値</summary>
    private float _sumAmount = 1.0f;

    /// <summary>現在のオーラ</summary>
    private Aura _currentAura = default;
    #endregion

    #region unity methods
    protected override void Awake()
    {
        base.Awake();
    }
    #endregion

    #region public method
    /// <summary>
    /// スキル発動時にオーラを生成
    /// </summary>
    public override void OnSkillAction()
    {
        Debug.Log($"{SkillType}スキル発動");
        _isSkillActived = true;
        CreateNewAura(_auraPrefab);
        _currentAura.SetAttackAmount(_currentAttackAmount);
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

        //レベルアップ
        _currentSkillLebel++;

        //3レベルまでと5レベルになった際はサイズを変更。
        if (_currentSkillLebel <= 3 || _currentSkillLebel == 5)
        {
            AttackUpSkill(_attackCoefficient);
            _currentAura.SizeChange(_sumAmount);
            
        }

        //4レベルになったらオーラを変更
        if (_currentSkillLebel == 4)
        {
            AttackUpSkill(_attackHighCoefficient);
            CreateNewAura(_highAuraPrefab);
        }

        _currentAura.SetAttackAmount(_currentAttackAmount);
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

    /// <summary>
    /// スキルの状態をリセットする
    /// </summary>
    public override void ResetSkill()
    {
        base.ResetSkill();
        Destroy(_currentAura.gameObject);
        _currentAura = null;
    }
    #endregion

    #region private method
    /// <summary>
    /// オーラを生成する
    /// </summary>
    /// <param name="aura">3レベルまでは通常オーラ。4レベルから強オーラ</param>
    private void CreateNewAura(Aura aura)
    {
        //_currentAuraがあれば非アクティブにして新たにauraを生成。
        if (_currentAura != null)
        {
            _currentAura.gameObject.SetActive(false);
        }
        _currentAura = Instantiate(aura, transform);
    }
    #endregion

    #region coroutine method
    /// <summary>
    /// スキルのアクションで行うコルーチン(オーラスキルでは使用しない)
    /// </summary>
    protected override IEnumerator SkillActionCoroutine()
    {
        yield return null;
    }
    #endregion
}