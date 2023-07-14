using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ナックルを扱うスキル機能
/// </summary>
[RequireComponent(typeof(KnucleGenerator))]
public class MuscleKnuckleSkill : SkillBase
{
    #region property
    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("テスト用の弾丸")]
    [SerializeField]
    private Knuckle _knucklePrefab = default;

    [Tooltip("スキルの攻撃間隔の初期値")]
    [SerializeField]
    private float _startAttackInterval = 5.0f;

    [Tooltip("スキルの攻撃間隔に対する係数")]
    [SerializeField]
    private float _coefficient = 2.0f;

    [Tooltip("スキルの攻撃間隔に対する係数を増やす値")]
    [SerializeField]
    private float _coefficientUpdate = 4.0f;

    [Tooltip("スキルアップ時の攻撃に対する係数")]
    [SerializeField]
    private float _attackCoefficient = 2.0f;
    #endregion

    #region private
    /// <summary>現在のスキルの攻撃間隔</summary>
    private float _currentAttackInterval;

    /// <summary>ナックルプール用のコンポーネント</summary>
    private KnucleGenerator _knucleGenerator;
    #endregion

    #region unity methods
    protected override void Awake()
    {
        base.Awake();

        //攻撃の間隔の初期値を代入
        _currentAttackInterval = _startAttackInterval;

        _knucleGenerator = GetComponent<KnucleGenerator>();
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
        if (_currentSkillLebel >= MAX_LEVEL)
        {
            Debug.Log($"{SkillType}はレベル上限です");
            return;
        }

        //レベルアップ
        _currentSkillLebel++;
        AttackUpSkill(_attackCoefficient);

        //攻撃間隔を縮める
        _currentAttackInterval /= _coefficient;

        Debug.Log($"レベルアップ!{_currentSkillLebel}に上がった！");
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

    /// <summary>
    /// スキルの状態をリセットする
    /// </summary>
    public override void ResetSkill()
    {
        base.ResetSkill();
        _currentAttackInterval = _startAttackInterval;
    }
    #endregion

    #region coroutine method
    /// <summary>
    /// スキル実行時に行う処理のコルーチン
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator SkillActionCoroutine()
    {
        while (_isSkillActived)
        {
            Knuckle sklObj = _knucleGenerator.KnucklePool.Rent();

            if (sklObj != null)
            {
                sklObj.transform.position = transform.position;
                sklObj.gameObject.SetActive(true);
                sklObj.SetAttackAmount(_currentAttackAmount);

                //これは使いどころではない。関数に変更する。
                sklObj.RandomDirection = (_currentSkillLebel < 4) ? new Action(sklObj.RondomFourDirection) : new Action(sklObj.RondomEightDirection);
                sklObj.RandomDirection.Invoke();
            }

             yield return new WaitForSeconds(_currentAttackInterval);
        }
    }
    #endregion
}
