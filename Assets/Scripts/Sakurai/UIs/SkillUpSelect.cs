using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UniRx;

/// <summary>
/// スキルアップ時のUI管理
/// </summary>
public class SkillUpSelect : MonoBehaviour
{

    #region serialize
    [Header("変数")]
    [Tooltip("スキルセレクトUIを格納するリスト")]
    [SerializeField]
    List<Button> _skillSelectUIs = default;

    [Tooltip("スキル獲得画面グループ")]
    [SerializeField]
    private CanvasGroup _skillUpSelectGroup = default;
    #endregion

    #region private
    /// <summary>表示させるUIの数</summary>
    private int _activeAmount = 3;
    #endregion

    #region Constant
    #endregion
    
    #region Event
    #endregion

    #region unity methods
    private void Start()
    {
        //プレイヤーのレベルが変更されたら、UI表示の処理を行う(ゲームスタート時の1回はスキップ)
        PlayerController.Instance.Status.CurrentPlayerLevel
                                        .TakeUntilDestroy(this)
                                        .Skip(1)
                                        .Subscribe(_ => ActivateRondomSkillUIs());
                                                
        for (int i = 0; i < _skillSelectUIs.Count; i++)
        {
            //Enumにキャスト
            SkillType type = (SkillType)i;

            //クリックしたらUIにスキルを登録する。
            _skillSelectUIs[i].onClick.AddListener(() => OnSkill(type));
        }
        _skillUpSelectGroup.alpha = 0;
        _skillUpSelectGroup.interactable = false;
        _skillUpSelectGroup.blocksRaycasts = false;
    }
    #endregion

    #region public method
    /// <summary>
    /// スキルアップした時にUIをランダムで表示させる
    /// </summary>
    public void ActivateRondomSkillUIs()
    {        
        int[] maxSkillIndices = SkillManager.Instance.Skills.Select((item,index) => new {Item = item , Index = index})  //Skillsの第一引数が要素、第二が要素のインデックス番号。
                                                            .Where(x => x.Item.CurrentSkillLevel >= 5)  //第一引数のカレントレベルを調べる。
                                                            .Select(c =>c.Index )　　//カレントレベル5以上のスキルの要素数を取得。
                                                            .ToArray();　　　　

        //UIの数分を見てそこからOrderByでランダムの値を3つだけ値を取得する。
        IEnumerable randomIndices = Enumerable.Range(0, _skillSelectUIs.Count)
                                              .Except(maxSkillIndices)
                                              .OrderBy(x => Random.value)
                                              .Take(_activeAmount);
        //ランダムで取得したUIをアクティブにする。
        foreach (int index in randomIndices)
        {
            _skillSelectUIs[index].gameObject.SetActive(true);
        }
        _skillUpSelectGroup.alpha = 1.0f;
        _skillUpSelectGroup.interactable = true;
        _skillUpSelectGroup.blocksRaycasts = true;

        //ゲーム画面を止める。
        Time.timeScale = 0f;
    }
    #endregion

    #region private method
    /// <summary>
    /// OnClickしたときの処理
    /// </summary>
    /// <param name="type">各ボタンのスキルタイプ</param>
    private void OnSkill(SkillType type)
    {
        //UI押したらスキルをセット。
        SkillManager.Instance.SetSkill(type);

        //クリックしたらUIを全部非アクティブにする。
        foreach (Button skillUI in _skillSelectUIs)
        {
            skillUI.gameObject.SetActive(false); 
        }
        
        _skillUpSelectGroup.alpha = 0;
        _skillUpSelectGroup.interactable = false;
        _skillUpSelectGroup.blocksRaycasts = false;

        //ゲーム画面を再開
        Time.timeScale = 1f;
    }
    #endregion
}