using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 爆弾を投げるスキル
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(BombGenerator))]
public class MuscleBombSkill : SkillBase
{
    #region serialize
    [Tooltip("スキルの攻撃間隔の初期値")]
    [SerializeField]
    private float _startAttackInterval = 3.0f;

    [Tooltip("スキルの攻撃間隔に対する係数")]
    [SerializeField]
    private float _coefficient = 1.1f;

    [Tooltip("スキルの攻撃力に対する係数")]
    [SerializeField]
    private float _attackCoefficient = 1.2f;
    #endregion

    #region private
    /// <summary>現在のスキルの攻撃間隔</summary>
    private float _currentAttackInterval;
    /// <summary>弾を撃つ方向</summary>
    private Vector3 _targetDir;
    /// <summary>エネミーのTransformのリスト</summary>
    private List<Transform> _enemyList = new List<Transform>();
    /// <summary>ボム生成コンポーネント格納用</summary>
    private BombGenerator _bombGenerator;
    #endregion

    #region unity methods
    protected override void Awake()
    {
        base.Awake();
        _currentAttackInterval = _startAttackInterval;
        _bombGenerator = GetComponent<BombGenerator>();
    }

    protected override void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameTag.Enemy))
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
        if (_currentSkillLevel >= MAX_LEVEL)
        {
            Debug.Log($"{SkillType}はレベル上限です");
            return;
        }
        _currentSkillLevel++;
        //攻撃間隔を係数で割って短縮
        _currentAttackInterval /= _coefficient;
        //攻撃力アップ
        AttackUpSkill(_attackCoefficient);

        Debug.Log($"レベルアップ!{_currentSkillLevel}に上がった！");
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

        foreach (Transform enemyTransform in _enemyList)
        {
            //敵との距離を測る
            float dist = Vector3.Distance(transform.position, enemyTransform.position);
            //今測った距離のほうが近ければ
            if (dist < distance)
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

                //使うボムのオブジェクトを取得
                Bomb skillObj = _bombGenerator.BombPool.Rent();
                //ボムがnullでなければ
                if (skillObj != null)
                {
                    //ボムのオブジェクトをアクティブ化
                    skillObj.gameObject.SetActive(true);
                    //ボムをスキルの位置に移動
                    skillObj.transform.position = transform.position;
                    //親子関係を解除
                    skillObj.gameObject.transform.SetParent(null);
                    //攻撃力を設定
                    skillObj.SetAttackAmount(_currentAttackAmount);
                    //velocityを設定
                    skillObj.SetVelocity(_targetDir);
                    //スキルレベルを取得
                    skillObj.GetCurrentLevel(_currentSkillLevel);
                }
            }
            //攻撃間隔分待つ
            yield return new WaitForSeconds(_currentAttackInterval);
        }
    }
    #endregion

}