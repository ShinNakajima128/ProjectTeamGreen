﻿using System.Collections;
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
    #endregion

    #region private
    /// <summary>現在のスキルの攻撃間隔</summary>
    private float _currentAttackInterval;
    /// <summary>弾を撃つ方向</summary>
    private Vector3 _targetDir;
    /// <summary>エネミーのTransformのリスト</summary>
    private List<Transform> _enemyList = new List<Transform>();
    private BombGenerator _generator;
    #endregion

    #region unity methods
    protected override void Awake()
    {
        base.Awake();
        _currentAttackInterval = _startAttackInterval;
        _generator = GetComponent<BombGenerator>();
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
        if (_currentSkillLebel >= MAX_LEVEL)
        {
            Debug.Log($"{SkillType}はレベル上限です");
            return;
        }
        _currentSkillLebel++;
        _currentAttackInterval /= _coefficient;

        Debug.Log($"レベルアップ!{_currentSkillLebel}に上がった！");
    }

    /// <summary>
    /// スキルの発動
    /// </summary>
    public override void OnSkillAction()
    {
        Debug.Log($"{SkillType}スキル発動");
        _isSkillActived = true;
        StartCoroutine(SkillActionCoroutine());
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
        Transform near = _enemyList.First();
        float distance = 9999;

        foreach (Transform enemyTransform in _enemyList)
        {
            float dist = Vector3.Distance(transform.position, enemyTransform.position);
            if (dist < distance)
            {
                near = enemyTransform;
                distance = dist;
            }
        }

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
        while (_isSkillActived)
        {
            if (_enemyList?.Count > 0)
            {
                SetTarget();

                GameObject skillObj = _generator.BombPool.Rent();
                if (skillObj != null)
                {
                    var bomb = skillObj.GetComponent<Bomb>();
                    bomb.gameObject.SetActive(true);
                    bomb.transform.position = transform.position;
                    bomb.gameObject.transform.SetParent(null);
                    bomb.SetAttackAmount(_currentAttackAmount);
                    bomb.SetVelocity(_targetDir);
                    bomb.GetCurrentLevel(_currentSkillLebel);
                }
            }
            yield return new WaitForSeconds(_currentAttackInterval);
        }
    }
    #endregion

}