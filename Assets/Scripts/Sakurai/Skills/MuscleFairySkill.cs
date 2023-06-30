using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 妖精スキルを扱うオブジェクト
/// </summary>
public class MuscleFairySkill : SkillBase
{
    #region property
    #endregion

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
    private float _coefficient = 1.3f;

    #endregion

    #region private
    /// <summary>妖精が回転する半径</summary>
    private float _fairyRadius = 1.0f;

    /// <summary>妖精の変更サイズ</summary>
    private float _scaleFactor = 2.0f;

    /// <summary>現在の妖精の数</summary>
    private List<Fairy> _currentFairyAmount = new List<Fairy>();

    /// <summary>妖精の開始角度</summary>
    private List<float> _currentFairyAngles = new List<float>();

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
        StartCoroutine(SkillActionCoroutine());
        CreateNewFairy();
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
        _rotationSpeed *= _coefficient;
        CreateNewFairy();

        Debug.Log($"レベルアップ!{_currentSkillLebel}にあがった!");

        if (_currentSkillLebel >= 3)
        {
            foreach (Fairy fairy in _currentFairyAmount)
            {
                fairy.SizaChange(_scaleFactor);
            }
        }
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
    #endregion

    #region private method
    /// <summary>
    /// 妖精を生成
    /// </summary>
    private void CreateNewFairy()
    {
        Fairy newFairy = Instantiate(_fairyPrefab,transform);
        newFairy.SetAttackAmount(_currentAttackAmount);
        _currentFairyAmount.Add(newFairy);

        float angleStep = 360.0f / _currentFairyAmount.Count;
        _currentFairyAngles.Clear();
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
                _currentFairyAngles[i] += _rotationSpeed * Time.deltaTime;
                Vector2 fairyPosition = new Vector2(Mathf.Cos(_currentFairyAngles[i] * Mathf.Deg2Rad), Mathf.Sin(_currentFairyAngles[i] * Mathf.Deg2Rad));
                fairyPosition *= _fairyRadius;
                _currentFairyAmount[i].transform.localPosition = fairyPosition;
            }
            yield return null;
        }
    }
    #endregion

}