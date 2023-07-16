using System;
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

    [Tooltip("回復ボタン")]
    [SerializeField]
    private Button _healUI = default;

    [Tooltip("スキル獲得画面グループ")]
    [SerializeField]
    private CanvasGroup _skillUpSelectGroup = default;

    [Tooltip("スキル獲得画面のグリッドレイアウトグループ")]
    [SerializeField]
    private GridLayoutGroup _skillUpSelectGrid = default;

    [Tooltip("プレイヤー回復用コンポーネント")]
    [SerializeField]
    private PlayerHealth _playerHealth = default;
    #endregion

    #region private
    /// <summary>表示させるUIの数</summary>
    private int _activeAmount = 3;

    /// <summary>キャンバスグループのアルファ値</summary>
    private int _alphaAmount = 1;
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

        float healAmount = 10.0f;
        _healUI.onClick.AddListener(() => PlayerHeal(healAmount));
                                                
        for (int i = 0; i < _skillSelectUIs.Count; i++)
        {
            //Enumにキャスト
            SkillType type = (SkillType)i;

            //クリックしたらUIにスキルを登録する。
            _skillSelectUIs[i].onClick.AddListener(() => OnSkill(type));
        }
        CanvasGroupChange(false);
    }
    #endregion

    #region public method
    /// <summary>
    /// スキルアップした時にUIをランダムで表示させる
    /// </summary>
    public void ActivateRondomSkillUIs()
    {
        int[] maxSkillIndices = SkillManager.Instance.Skills.Select((item,index) => new {Item = item , Index = index})  //Skillsの第一引数が要素、第二が要素のインデックス番号。
                                                            .Where(x => x.Item.CurrentSkillLevel >=5 )  //第一引数のカレントレベルを調べる。
                                                            .Select(c =>c.Index )　　//カレントレベル5以上のスキルの要素数を取得。
                                                            .ToArray();

        _alphaAmount = 1;
        if (maxSkillIndices.Length == _skillSelectUIs.Count)
        {
            _healUI.gameObject.SetActive(true);
            CanvasGroupChange(true);
            Time.timeScale = 0;
            return;
        }
        else
        {
            //UIの数分を見てそこからOrderByでランダムの値を3つだけ値を取得する。
            IEnumerable<int> randomIndices = Enumerable.Range(0, _skillSelectUIs.Count)
                                                       .Except(maxSkillIndices)
                                                       .OrderBy(x => UnityEngine.Random.value)
                                                       .Take(_activeAmount);

            //レベルマックスではないスキルが残り1or2個であればpaddingの値を変更。
            int gridLeftAmount = (randomIndices.Count() >= 3) ? -450:(randomIndices.Count() == 2) ? -270 : -100; 
            _skillUpSelectGrid.padding.left = gridLeftAmount;

            //ランダムで取得したUIをアクティブにする。
            foreach (int index in randomIndices)
            {
                _skillSelectUIs[index].gameObject.SetActive(true);
            }
            CanvasGroupChange(true);

            //ゲーム画面を止める。
            Time.timeScale = 0f;
        }
    }
    #endregion

    #region public method
    public void PlayerHeal(float healAmount)
    {
        _playerHealth.Heal(healAmount);

        _healUI.gameObject.SetActive(false);

        _alphaAmount = 0;
        CanvasGroupChange(false);
        //ゲーム画面を再開
        Time.timeScale = 1f;
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
        _alphaAmount = 0;

        CanvasGroupChange(false);

        //ゲーム画面を再開
        Time.timeScale = 1f;
    }

    private void CanvasGroupChange(bool change) 
    {
        _skillUpSelectGroup.alpha = Convert.ToInt32(change);
        _skillUpSelectGroup.interactable = change;
        _skillUpSelectGroup.blocksRaycasts = change;
    }
    #endregion
}