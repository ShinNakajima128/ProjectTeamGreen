using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 各スキルのベースとなるクラス。スキル作成時はこれを必ず継承すること
/// </summary>
public abstract class SkillBase : MonoBehaviour
{
    #region property
    public SkillType SkillType => _skillData.SkillType;
    public bool IsSkillActived => _isSkillActived;
    public int CurrentSkillLevel => _currentSkillLebel;
    #endregion

    #region serialize
    [Tooltip("スキルデータ")]
    [SerializeField]
    private SkillData _skillData = default;
    #endregion

    #region protected
    /// <summary>現在の攻撃力</summary>
    protected float _currentAttackAmount = 0;
    /// <summary>現在のスキルレベル</summary>
    protected int _currentSkillLebel = 1;
    /// <summary>スキルがアクティブかどうか</summary>
    protected bool _isSkillActived = false;
    /// <summary>現在稼働中のコルーチン</summary>
    protected Coroutine _currentCoroutine;
    #endregion

    #region private
    #endregion

    #region Constant
    /// <summary>スキルのレベルの最大値</summary>
    protected const int MAX_LEVEL = 5;
    #endregion

    #region Event
    #endregion

    #region unity methods
    protected virtual void Awake()
    {
        Setup();
    }

    protected virtual void Start()
    {
        //ゲーム終了時にスキルをリセットする処理を登録
        StageManager.Instance.GameEndObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(_ => ResetSkill());
    }
    #endregion

    #region public method
    /// <summary>
    /// スキルの処理を停止する
    /// </summary>
    public void StopSkill()
    {
        _isSkillActived = false;
    }
    #endregion

    #region private method
    private void Setup()
    {
        _currentAttackAmount = _skillData.AttackAmount;
    }
    #endregion

    #region abstract method
    /// <summary>
    /// スキル発動時のアクション
    /// </summary>
    public abstract void OnSkillAction();
    /// <summary>
    /// スキルをレベルアップする
    /// </summary>
    public abstract void LebelUpSkill();
    /// <summary>
    /// スキルの攻撃力を上げる
    /// </summary>
    /// <param name="coefficient">係数</param>
    public abstract void AttackUpSkill(float coefficient);

    /// <summary>
    /// スキルの状態をリセットする
    /// </summary>
    public virtual void ResetSkill()
    {
        _isSkillActived = false;
        _currentSkillLebel = 1;
        _currentAttackAmount = _skillData.AttackAmount;

        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
    }

    /// <summary>
    /// スキル実行時の処理を行うコルーチン
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerator SkillActionCoroutine();
    #endregion
}