using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 跳ね返るボールを扱うスキル機能
/// </summary>
public class MuscleBoulSkill : SkillBase
{
    #region serialize
    [Header("変数")]
    [Tooltip("跳ね返るボール")]
    [SerializeField]
    private Boul _boudPrefab = default;

    [Tooltip("スキルアップ時の攻撃に対する係数")]
    [SerializeField]
    private float _attackCoefficient = 3.0f;
    #endregion

    #region private
    /// <summary>スキルアップ時のボールの速度に対する係数</summary>
    private float _speedCoefficient = 1.2f;

    /// <summary>ボールを格納するリスト</summary>
    private List<Boul> _currentBoulAmount = new List<Boul>();
    #endregion

    #region unity methods
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        //ゲーム開始時は跳ね返る壁を非アクティブ
        if (!_isSkillActived)
        {
            ChildActive(false);
        }
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

        //跳ね返る壁をアクティブにする。
        ChildActive(true);

        //メインカメラの位置を取得。
        Transform mainCameraTransform = Camera.main.transform;

        //メインカメラの子オブジェクトにする。
        foreach (Transform child in transform)
        {
            child.SetParent(mainCameraTransform);
        }

        _currentCoroutine = StartCoroutine(SkillActionCoroutine());
        CreateNewBoul();
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

        //レベルアップ
        _currentSkillLevel++;
        AttackUpSkill(_attackCoefficient);
        CreateNewBoul();

        //レベルアップしたら全ボールをスピードアップ
        foreach (Boul boul in _currentBoulAmount)
        {
            boul.MoveSpeedChange(_speedCoefficient);
        }

        Debug.Log($"レベルアップ!{_currentSkillLevel}にあがった!");
    }

    /// <summary>
    /// スキルの攻撃力を上げる
    /// </summary>
    /// <param name="coefficient">攻撃力に対する係数</param>
    public override void AttackUpSkill(float coefficient)
    {
        _currentAttackAmount *= coefficient;
    }

    /// <summary>
    /// スキルの状態をリセットする
    /// </summary>
    public override void ResetSkill()
    {
        base.ResetSkill();

        foreach (var boul in _currentBoulAmount)
        {
            Destroy(boul.gameObject);
        }
        _currentBoulAmount.Clear();
        Debug.Log("削除完了");
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
    /// <param name="">アクティブにするかどうか</param>
    private void ChildActive(bool change)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(change);
        }
    }
    #endregion

    #region coroutine method
    /// <summary>
    /// スキルのアクションで行うコルーチン(ボールスキルでは使用しない)
    /// </summary>
    protected override IEnumerator SkillActionCoroutine()
    {
        yield return null;
    }
    #endregion
}