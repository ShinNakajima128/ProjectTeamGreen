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
    #region property
    
    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("スキルセレクトUIを格納するリスト")]
    [SerializeField]
    List<Button> _skillSelectUIs = default;

    [Tooltip("")]
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
        PlayerController.Instance.Status.CurrentPlayerLevel
                                        .Skip(1)
                                        .Subscribe(_ => ActivateRondomSkillUIs())
                                        .AddTo(this);
        
        for (int i = 0; i < _skillSelectUIs.Count; i++)
        {
            SkillType type = (SkillType)i;
            _skillSelectUIs[i].onClick.AddListener(() => OnSkill(type));
        }
        _skillUpSelectGroup.alpha = 0;
    }
    #endregion

    #region public method

    /// <summary>
    /// スキルアップした時にUIをランダムで表示させる
    /// </summary>
    public void ActivateRondomSkillUIs()
    {
        IEnumerable randomIndices = Enumerable.Range(0, _skillSelectUIs.Count)
                                              .OrderBy(x => Random.value)
                                              .Take(_activeAmount);
        foreach (int index in randomIndices)
        {
            _skillSelectUIs[index].gameObject.SetActive(true);
        }
        _skillUpSelectGroup.alpha = 1.0f;
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
        SkillManager.Instance.SetSkill(type);

        foreach (Button skillUI in _skillSelectUIs)
        {
            skillUI.gameObject.SetActive(false); 
        }

        _skillUpSelectGroup.alpha = 0;
        Time.timeScale = 1f;
    }
    #endregion
}