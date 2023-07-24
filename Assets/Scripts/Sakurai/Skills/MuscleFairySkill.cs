using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 妖精を扱うスキル機能
/// </summary>
public class MuscleFairySkill : SkillBase
{
    #region serialize
    [Header("変数")]
    [Tooltip("回転する妖精")]
    [SerializeField]
    private Fairy _fairyPrefab = default;

    [Tooltip("回転する速度")]
    [SerializeField]
    private float _rotationSpeed = 1.0f;

    [Tooltip("妖精が回転する速度に対する係数")]
    [SerializeField]
    private float _speedCoefficient = 1.3f;

    [Tooltip("スキルアップ時の攻撃に対する係数")]
    [SerializeField]
    private float _attackCoefficient = 1.3f;
    #endregion

    #region private
    private float _currentRotationSpeed;
    /// <summary>妖精が回転する半径</summary>
    private float _fairyRadius = 1.0f;

    /// <summary>妖精の変更サイズ</summary>
    private float _scaleFactor = 2.0f;

    /// <summary>現在の妖精の数</summary>
    private List<Fairy> _currentFairyAmount = new List<Fairy>();

    /// <summary>妖精の開始角度</summary>
    private List<float> _currentFairyAngles = new List<float>();
    #endregion

    #region unity methods
    protected override void Awake()
    {
        base.Awake();
        _currentRotationSpeed = _rotationSpeed;
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
        _currentCoroutine = StartCoroutine(SkillActionCoroutine());
        CreateNewFairy();
        AudioManager.PlaySE(SEType.GetSkill_4);
    }
    /// <summary>
    /// スキルをレベルアップする
    /// </summary>
    public override void LebelUpSkill()
    {
        if (_currentSkillLebel >= MAX_LEVEL)
        {
            Debug.Log($"{SkillType}はレベル上限です");

            return;
        }
        _currentSkillLebel++;
        _currentRotationSpeed *= _speedCoefficient;
        AttackUpSkill(_attackCoefficient);
        CreateNewFairy();

        Debug.Log($"レベルアップ!{_currentSkillLebel}にあがった!");

        if (_currentSkillLebel >= 3)
        {
            foreach (Fairy fairy in _currentFairyAmount)
            {
                fairy.SizeChange(_scaleFactor);
            }
        }
        AudioManager.PlaySE(SEType.FairyUp);
    }

    /// <summary>
    /// スキルの攻撃力を上げる
    /// </summary>
    /// <param name="coefficient">係数</param>
    public override void AttackUpSkill(float coefficient)
    {
        //現在のスキル攻撃力に係数をかけ合わせる。
        _currentAttackAmount *= coefficient; 
    }

    /// <summary>
    /// スキルの状態をリセットする
    /// </summary>
    public override void ResetSkill()
    {
        base.ResetSkill();

        foreach (var fairy in _currentFairyAmount)
        {
            Destroy(fairy.gameObject);
        }
        _currentRotationSpeed = _rotationSpeed;
        _currentFairyAmount.Clear();
        _currentFairyAngles.Clear();
    }
    #endregion

    #region private method
    /// <summary>
    /// 妖精を生成
    /// </summary>
    private void CreateNewFairy()
    {
        //生成数は1度目のみ2体
        int instanceCount = _currentSkillLebel == 1 ? 2 : 1;

        for (int i = 0; i < instanceCount; i++)
        {
            Fairy newFairy = Instantiate(_fairyPrefab, transform);
            newFairy.SetAttackAmount(_currentAttackAmount);
            _currentFairyAmount.Add(newFairy);
        }
      
        //間隔が均等になるように360から現在の生成数を割る
        float angleStep = 360.0f / _currentFairyAmount.Count;

        //一度生成位置をリセット
        _currentFairyAngles.Clear();

        //floatの値更新。
        for (int i = 0; i < _currentFairyAmount.Count; i++)
        {
            _currentFairyAngles.Add(angleStep * i);
        }
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
            for (int i = 0; i < _currentFairyAmount.Count; i++)
            {
                //１体ずつの角度を常に更新
                _currentFairyAngles[i] += _currentRotationSpeed * Time.deltaTime;

                //更新した角度を常にかけあわせて位置を更新
                Vector2 fairyPosition = new Vector2(Mathf.Cos(_currentFairyAngles[i] * Mathf.Deg2Rad), Mathf.Sin(_currentFairyAngles[i] * Mathf.Deg2Rad));

                //プレイヤーからの半径
                fairyPosition *= _fairyRadius;

                //それぞれの妖精のポジションを更新
                _currentFairyAmount[i].transform.localPosition = fairyPosition;
            }
            yield return null;
        }
    }
    #endregion

}