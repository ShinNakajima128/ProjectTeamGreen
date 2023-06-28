using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各スキルのベースとなるクラス。スキル作成時はこれを必ず継承すること
/// </summary>
public abstract class SkillBase : MonoBehaviour
{
    #region property
    public SkillType Type => _skillData.Type;
    public bool IsSkillActived => _isSkillActived;
    #endregion

    #region serialize
    [Tooltip("スキルデータ")]
    [SerializeField]
    private SkillData _skillData = default;
    #endregion

    #region protected
    /// <summary>現在の攻撃力</summary>
    protected float _currentAttackAmount = 0;
    /// <summary>現在のスキルレベル</summary>
    protected int _currentSkillLebel = 1;
    /// <summary>スキルがアクティブかどうか</summary>
    protected bool _isSkillActived = false;
    #endregion

    #region private
    #endregion

    #region Constant
    /// <summary>スキルのレベルの最大値</summary>
    protected const int MAX_LEVEL = 5;
    #endregion

    #region Event
    #endregion

    #region unity methods
    protected virtual void Awake()
    {
        Setup();
    }

    protected virtual void Start()
    {

    }
    #endregion

    #region public method
    /// <summary>
    /// スキルの処理を停止する
    /// </summary>
    public void StopSkill()
    {
        _isSkillActived = false;
    }
    #endregion

    #region private method
    private void Setup()
    {
        _currentAttackAmount = _skillData.AttackAmount;
    }
    #endregion

    #region abstract method
    /// <summary>
    /// スキル発動時のアクション
    /// </summary>
    public abstract void OnSkillAction();
    /// <summary>
    /// スキルをレベルアップする
    /// </summary>
    public abstract void LebelUpSkill();
    /// <summary>
    /// スキルの攻撃力を上げる
    /// </summary>
    /// <param name="coefficient">係数</param>
    public abstract void AttackUpSkill(float coefficient);
    #endregion
}