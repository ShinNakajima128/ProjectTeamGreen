using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SkillTest : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
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
        //Update内で行う処理を登録
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SkillManager.Instance.SetSkill(SkillType.Aura);
                }
                if (Input.GetKeyDown(KeyCode.F))
                {
                    SkillManager.Instance.SetSkill(SkillType.Fairy);
                }
            })
            .AddTo(this);
    }

    private void Update()
    {

    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
}
