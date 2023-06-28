using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MascleFairySkill : SkillBase
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

    [Tooltip("現在存在する妖精")]
    [SerializeField]
    private List<GameObject> _currentFairyAmount = new List<GameObject>();

    [Tooltip("妖精が回転する半径")]
    [SerializeField]
    private float _fairyRadius = 1.0f;

    #endregion

    #region private
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
       // _isSkillActived = true;
       // StartCoroutine(SkillActionCoroutine());

       // GameObject newFairy = Instantiate(_fairyPrefab.gameObject);
       // _currentFairyAmount.Add(newFairy);
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
        GameObject newFairy = Instantiate(_fairyPrefab.gameObject);
        _currentFairyAmount.Add(newFairy);
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
        Debug.Log($"レベルアップ!{_currentSkillLebel}にあがった!");

        GameObject newFairy = Instantiate(_fairyPrefab.gameObject);
        _currentFairyAmount.Add(newFairy);
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
    #endregion

    #region private method
    /// <summary>
    /// スキル実行時の処理を行うコルーチン
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator SkillActionCoroutine()
    {
        while (_isSkillActived)
        {
            Debug.Log("妖精発動");
            float angleStep = 360.0f / _currentFairyAmount.Count;
            for (int i = 0; i < _currentFairyAmount.Count; i++)
            {
                float angle = angleStep * i;

                Vector2 fairyPosition = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                fairyPosition *= _fairyRadius;
                //fairyPosition += (Vector2)transform.localPosition;
                //_currentFairyAmount[i].transform.localPosition = fairyPosition;
                fairyPosition += (Vector2)transform.position;
                _currentFairyAmount[i].transform.position = fairyPosition;

                _currentFairyAmount[i].transform.RotateAround(transform.position, Vector3.up, _rotationSpeed * Time.deltaTime);
            }
            yield return null;
        }
    }
    #endregion

}