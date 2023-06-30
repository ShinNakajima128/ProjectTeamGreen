using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class MuscleThrowSkill : SkillBase
{
    #region property
    #endregion

    #region serialize
    [Tooltip("ダンベル（弾）")]
    [SerializeField]
    private Dumbbell _dumbbellPrefab = default;

    [Tooltip("スキルの攻撃間隔の初期値")]
    [SerializeField]
    private float _startAttackInterval = 1.0f;

    #endregion

    #region private
    /// <summary>現在のスキルの攻撃間隔</summary>
    private float _currentAttackInterval;

    List<Transform> _enemyList = new List<Transform>();
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == GameTag.Enemy)
        {
            _enemyList.Add(other.GetComponent<Transform>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == GameTag.Enemy)
        {
            _enemyList.Remove(other.GetComponent<Transform>());
        }
    }
    #endregion

    #region public method
    public override void LebelUpSkill()
    {
        //既にレベルが最大値の場合は処理を行わない
        if (_currentSkillLebel >= MAX_LEVEL)
        {
            Debug.Log($"{SkillType}はレベル上限です");
            return;
        }
        _currentSkillLebel++;
        Debug.Log($"レベルアップ!{_currentSkillLebel}に上がった！");
    }

    public override void OnSkillAction()
    {
        Debug.Log($"{SkillType}スキル発動");
        _isSkillActived = true;
        StartCoroutine(SkillActionCoroutine());
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
        while (_isSkillActived)
        {
            var dumbbell = Instantiate(_dumbbellPrefab, transform.position, transform.rotation);

            dumbbell.SetAttackAmount(_currentAttackAmount);

            yield return new WaitForSeconds(_currentAttackInterval);
        }
    }
    #endregion
}