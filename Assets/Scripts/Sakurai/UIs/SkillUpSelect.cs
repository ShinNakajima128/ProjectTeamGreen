using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SkillUpSelect : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("スキルセレクトUIを格納するリスト")]
    [SerializeField]
    List<Button> _skillSelectUis = default;

    [Tooltip("経験値の値を取得するための参照")]
    [SerializeField]
    PlayerStatus playerStatus = default; 
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
    private void Awake()
    {
       
    }

    private void Start()
    {
        if (playerStatus.CurrentPlayerLevel.Value == 1)
        {
            ActivateRondomSkillUIs();
        }
        for (int i = 0; i < _skillSelectUis.Count; i++)
        {
            SkillType type = (SkillType)i;
            _skillSelectUis[i].onClick.AddListener(() => OnSkill(type));
        }
    }

    private void Update()
    {
        //if (playerStatus.CurrentRequireExp.Value <= playerStatus.CurrentExp.Value)
        //{
        //    ActivateRondomSkillUIs();
        //}

    }
    #endregion

    #region public method

    public void ActivateRondomSkillUIs()
    {

        IEnumerable randomIndices = Enumerable.Range(0, _skillSelectUis.Count)
                                    .OrderBy(x => Random.value)
                                    .Take(_activeAmount);

        foreach (int index in randomIndices)
        {
            _skillSelectUis[index].gameObject.SetActive(true);
        }

        Time.timeScale = 0f;
    }
    #endregion

    #region private method

    private void OnSkill(SkillType type)
    {
        SkillManager.Instance.SetSkill(type);

        foreach (Button skillUI in _skillSelectUis)
        {
            skillUI.gameObject.SetActive(false); 
        }

        Time.timeScale = 1f;
    }
    #endregion
}