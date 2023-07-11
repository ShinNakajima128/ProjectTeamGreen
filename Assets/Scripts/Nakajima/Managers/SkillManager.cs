using System.Linq;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

/// <summary>
/// スキル全般の管理を行うManagerクラス
/// </summary>
public class SkillManager : MonoBehaviour
{
    #region property
    public static SkillManager Instance { get; private set; }
    public SkillBase[] Skills => _skills;
    #endregion

    #region serialize
    [Tooltip("プレイヤーの各スキル")]
    [SerializeField]
    private SkillBase[] _skills = default;
    #endregion

    #region private
    private Transform _playerTrans;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        Instance = this;
        _playerTrans = GameObject.FindGameObjectWithTag(GameTag.Player).transform;
    }

    private void Start()
    {
        StageManager.Instance.GameStartObserver
                             .Subscribe(_ =>
                             {
                                 //「Forget()」は「StartCoroutine()」と類似しているが、
                                 //書かないと動作しない「StartCoroutine()」とは違い、
                                 //「Forget()」無しでも動作する。(ただし警告が表示されるので、書く方が無難)
                                 OnStartSkillSelectAsync(this.GetCancellationTokenOnDestroy()).Forget();
                             })
                             .AddTo(this);

        //ゲーム終了時に実行する処理を登録
        StageManager.Instance.GameEndObserver
                             .Subscribe(_ => InActiveAllSkill())
                             .AddTo(this);
    }
    #endregion

    #region public method
    /// <summary>
    /// スキルをセットする
    /// </summary>
    /// <param name="type">スキルの種類</param>
    public void SetSkill(SkillType type)
    {
        //スキル一覧から指定されたスキルを探索
        var skill = _skills.FirstOrDefault(x => x.SkillType == type);

        //スキルが非アクティブの場合はアクティブにする
        if (!skill.IsSkillActived)
        {
            //スキルをプレイヤーの子オブジェクトにする
            skill.gameObject.transform.SetParent(_playerTrans);
            skill.gameObject.transform.localPosition = Vector2.zero;
            skill.OnSkillAction();
        }
        //既にアクティブの場合はスキルのレベルを上げる
        else
        {
            skill.LebelUpSkill();
        }
    }

    /// <summary>
    /// スキルの処理を停止する。ゲーム終了時などに呼び出す
    /// </summary>
    public void InActiveAllSkill()
    {
        for (int i = 0; i < _skills.Length; i++)
        {
            _skills[i].StopSkill();
        }
    }
    #endregion

    #region private method
    #endregion

    #region UniTask method
    /// <summary>
    /// ゲーム開始時にランダムなスキルを獲得する画面を表示する
    /// </summary>
    private async UniTask OnStartSkillSelectAsync(CancellationToken token)
    {
        await UniTask.Delay(1000, cancellationToken: token);

        HUDManager.Instance.SkillUpSelect.ActivateRondomSkillUIs();
    }
    #endregion
}
