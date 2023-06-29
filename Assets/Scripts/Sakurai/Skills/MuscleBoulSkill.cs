using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 跳ね返るボールを扱うオブジェクト
/// </summary>
public class MuscleBoulSkill : SkillBase
{

    #region property
    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("跳ね返るボール")]
    [SerializeField]
    private Boul _boudPrefab = default;

    #endregion

    #region private

    /// <summary>スキルアップ時の速度に対する係数</summary>
    private float _coefficient = 1.2f;

    /// <summary>ボールを格納するリスト</summary>
    private List<Boul> _currentBoulAmount = new List<Boul>();

    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {

    }

    private void Start()
    {
        //ゲーム開始時は子オブジェクトを非アクティブc
        if (!_isSkillActived)
        {
            childActive(false);
        }
    }

    private void Update()
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
        childActive(true);
        StartCoroutine(SkillActionCoroutine());
        CreateNewBoul();
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
        CreateNewBoul();
        foreach (Boul boul in _currentBoulAmount)
        {
            boul.MoveSpeedChange(_coefficient);
        }

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

    #endregion

    #region private method
    /// <summary>
    /// ボールの生成
    /// </summary>
    private void CreateNewBoul()
    {
        Boul newBoul = Instantiate(_boudPrefab, transform);
        newBoul.SetAttackAmount(_currentAttackAmount);
        _currentBoulAmount.Add(newBoul);
    }

    /// <summary>
    /// 子オブジェクトはスキル発動中のみアクティブ状態
    /// </summary>
    /// <param name=""></param>
    private void childActive(bool change)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(change);
        }
    }
    #endregion

    #region coroutine method
    /// <summary>
    /// スキル発動中の処理
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator SkillActionCoroutine()
    {
        yield return null;
    }
    #endregion
}