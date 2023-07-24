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

    /// <summary>インゲームのフラグ</summary>
    private bool _isInGame = false;
    #endregion

    #region unity methods
    private void Start()
    {
        StageManager.Instance.IsInGameObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(value => _isInGame = value);

        //プレイヤーのレベルが変更されたら、UI表示の処理を行う(ゲームスタート時の1回はスキップ)
        PlayerController.Instance.Status.CurrentPlayerLevel
                                        .TakeUntilDestroy(this)
                                        .Skip(1)
                                        .Subscribe(_ => ActivateRondomSkillUIs());

        //UIのPlayerHealを登録する。
        float healAmount = 10.0f;
        _healUI.onClick.AddListener(() => PlayerHeal(healAmount));
                                                
        for (int i = 0; i < _skillSelectUIs.Count; i++)
        {
            //Enumにキャスト
            SkillType type = (SkillType)i;

            //UIにOnSkillを登録する。
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
        AudioManager.PlaySE(SEType.UIActive);
        if (!_isInGame)
        {
            return;
        }

        //スキルマックスを調べる
        int[] maxSkillIndices = SkillManager.Instance.Skills.Select((item,index) => new {Item = item , Index = index})  //intに格納するために要素に対してのインデックス番号が必要。
                                                            .Where(x => x.Item.CurrentSkillLevel >=5 )  //第一引数のカレントレベルを調べる。
                                                            .Select(c =>c.Index )　　//カレントレベル5以上のスキルの要素数を取得。
                                                            .ToArray();

        if (maxSkillIndices.Length == _skillSelectUIs.Count)
        {
            _healUI.gameObject.SetActive(true);
            CanvasGroupChange(true);
            Time.timeScale = 0;
            return;
        }
        else
        {
            //UIの数分を見てそこからOrderByでランダムの値を3つだけ値を取得する。LINQでシーケンス操作を行うのでIEnumerable<T>
            var randomIndices = Enumerable.Range(0, _skillSelectUIs.Count)  //6個のシーケンスを生成
                                          .Except(maxSkillIndices)          //上記配列の中身のシーケンスを削除
                                          .OrderBy(x => UnityEngine.Random.value)    //各シーケンスに対してRandom.valueで0~1の間の浮上小数点を与えてから昇順にする。
                                          .Take(_activeAmount);　　　　　　　　//OrderByからのランダム値から3つ取り出す


            //レベルマックスではないスキルが残り1or2個であればpaddingの値を変更。キレイな配置にする。
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

    /// <summary>
    /// プレイヤーを回復
    /// </summary>
    /// <param name="healAmount">回復する値</param>
    #region public method
    public void PlayerHeal(float healAmount)
    {
        _playerHealth.Heal(healAmount);

        _healUI.gameObject.SetActive(false);

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