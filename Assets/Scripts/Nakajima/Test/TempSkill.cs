﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// テスト用の仮スキル
/// </summary>
public class TempSkill : SkillBase
{
    #region property
    #endregion

    #region serialize
    [Tooltip("テスト用の弾丸")]
    [SerializeField]
    private TempBullet _testBulletPrefab = default;

    [Tooltip("スキルの攻撃間隔の初期値")]
    [SerializeField]
    private float _startAttackInterval = 1.0f;
    #endregion

    #region private
    /// <summary>現在のスキルの攻撃間隔</summary>
    private float _currentAttackInterval;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    protected override void Awake()
    {
        base.Awake();
        _currentAttackInterval = _startAttackInterval;
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
        Debug.Log($"{SkillType}スキル発動");
        _isSkillActived = true;
        StartCoroutine(SkillActionCoroutine());
    }
    /// <summary>
    /// スキルをレベルアップする
    /// </summary>
    public override void LebelUpSkill()
    {
        //既にレベルが最大値の場合は処理を行わない
        if (_currentSkillLevel >= MAX_LEVEL)
        {
            Debug.Log($"{SkillType}はレベル上限です");
            return;
        }
        _currentSkillLevel++;
        Debug.Log($"レベルアップ!{_currentSkillLevel}に上がった！");
    }
    // <summary>
    /// スキルの攻撃力を上げる
    /// </summary>
    /// <param name="coefficient">係数</param>
    public override void AttackUpSkill(float coefficient)
    {
        //現在のスキル攻撃力に係数を掛け合わせる
        _currentAttackAmount *= coefficient;
    }
    #endregion

    #region private method
    #endregion

    #region coroutine method
    protected override IEnumerator SkillActionCoroutine()
    {
        while (_isSkillActived)
        {
            var bullet = Instantiate(_testBulletPrefab, transform.position, transform.rotation);

            bullet.SetAttackAmount(_currentAttackAmount);

            yield return new WaitForSeconds(_currentAttackInterval);
        }

        //コルーチンを使用使用しない場合は以下を記述する
        //yield return null;
    }
    #endregion
}
