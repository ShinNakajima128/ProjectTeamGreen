using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 一番近い敵に攻撃するスキル
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(DumbbellGenerator))]
public class MuscleThrowSkill : SkillBase
{
    #region serialize
    [Tooltip("スキルの攻撃間隔の初期値")]
    [SerializeField]
    private float _startAttackInterval = 1.0f;

    [Tooltip("スキルの攻撃間隔に対する係数")]
    [SerializeField]
    private float _coefficient = 1.2f;

    [Tooltip("スキルの攻撃力に対する係数")]
    [SerializeField]
    private float _attackCoefficient = 1.2f;
    #endregion

    #region private
    /// <summary>現在のスキルの攻撃間隔</summary>
    private float _currentAttackInterval;
    /// <summary>弾を撃つ方向</summary>
    private Vector3 _targetDir = Vector3.zero;
    /// <summary>エネミーのTransformのリスト</summary>
    private List<Transform> _enemyList = new List<Transform>();
    /// <summary>ダンベル生成コンポーネント格納用</summary>
    private DumbbellGenerator _dumbbellGenerator;
    #endregion

    #region unity methods
    protected override void Awake()
    {
        base.Awake();
        _currentAttackInterval = _startAttackInterval;
        _dumbbellGenerator = GetComponent<DumbbellGenerator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(GameTag.Enemy))
        {
            _enemyList.Add(other.GetComponent<Transform>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(GameTag.Enemy))
        {
            _enemyList.Remove(other.GetComponent<Transform>());
        }
    }
    #endregion

    #region public method
    /// <summary>
    /// スキルのレベルアップ
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
        //攻撃間隔を係数で割って短縮
        _currentAttackInterval /= _coefficient;
        //攻撃力をアップ
        AttackUpSkill(_attackCoefficient);

        Debug.Log($"レベルアップ!{_currentSkillLebel}に上がった！");
    }

    /// <summary>
    /// スキルの発動
    /// </summary>
    public override void OnSkillAction()
    {
        Debug.Log($"{SkillType}スキル発動");
        _isSkillActived = true;
        _currentCoroutine = StartCoroutine(SkillActionCoroutine());
        AudioManager.PlaySE(SEType.GetSkill_4);
    }

    /// <summary>
    /// スキルの攻撃力アップ
    /// </summary>
    /// <param name="coefficient"></param>
    public override void AttackUpSkill(float coefficient)
    {
        _currentAttackAmount *= coefficient;
    }
    #endregion

    #region private method
    /// <summary>
    /// 一番近い敵を探す
    /// </summary>
    private void SetTarget()
    {
        //一番近い敵のTransform
        Transform near = _enemyList.First();
        //現状一番近い敵との距離
        float distance = float.MaxValue;

        //範囲内の敵とプレイヤーの距離を調べる
        foreach (Transform enemyTransform in _enemyList)
        {
            float dist = Vector3.Distance(transform.position, enemyTransform.position);
            //今調べた距離のほうが近いなら
            if(dist < distance)
            {
                near = enemyTransform;
                distance = dist;
            }
        }
        //一番近い敵への単位ベクトル
        _targetDir = (near.position - transform.position).normalized;
    }
    #endregion

    #region coroutine method
    /// <summary>
    /// スキル実行時の処理を行うコルーチン
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator SkillActionCoroutine()
    {
        //スキルがアクティブなら
        while (_isSkillActived)
        {
            //enemyListはnullではなく要素数は1以上
            if (_enemyList?.Count > 0)
            {
                //一番近い敵を探す
                SetTarget();

                //使うダンベルを取得
                Dumbbell skillObj = _dumbbellGenerator.DumbbellPool.Rent();
                //ダンベルがnullでないなら
                if (skillObj != null)
                {
                    //オブジェクトをアクティブ化
                    skillObj.gameObject.SetActive(true);
                    //ダンベルの位置をスキルの位置に
                    skillObj.transform.position = transform.position;
                    //親子関係を解除
                    skillObj.gameObject.transform.SetParent(null);
                    //攻撃力を設定
                    skillObj.SetAttackAmount(_currentAttackAmount);
                    //verocityを設定
                    skillObj.SetVelocity(_targetDir);
                }
            }
            //攻撃間隔分待つ
            yield return new WaitForSeconds(_currentAttackInterval);
        }
    }
    #endregion
}