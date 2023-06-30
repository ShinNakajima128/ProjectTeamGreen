using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ナックルスキルを扱うオブジェクト
/// </summary>
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
    #endregion

    #region private
    /// <summary>現在のスキルの攻撃間隔</summary>
    private float _currentAttackInterval;

    /// <summary>拳の生存時間</summary>
    private float _lifeTime = 5.0f;
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
        if (_currentSkillLebel >= MAX_LEVEL)
        {
            Debug.Log($"{SkillType}はレベル上限です");
            return;
        }
        _currentSkillLebel++;
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
    #endregion

    #region private method
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
             Knuckle knuckle = Instantiate(_knucklePrefab, transform.position, transform.rotation);
             knuckle.SetAttackAmount(_currentAttackAmount);

            if (_currentSkillLebel >= 3)
            {
                knuckle.RandomDirection = knuckle.RondomEightDirection;
                _coefficient = _coefficientUpdate;
            }
            else
            {
                knuckle.RandomDirection = knuckle.RondomFourDirection;
            }

            knuckle.RandomDirection.Invoke();

             yield return new WaitForSeconds(_currentAttackInterval);
        }

        //コルーチンを使用使用しない場合は以下を記述する
        //yield return null;
    }
    #endregion
}
