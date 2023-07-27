using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// 各エフェクトをコントロールするクラス
/// </summary>
public class EffectController : MonoBehaviour
{
    #region private
    /// <summary>パーティクルを格納する配列</summary>
    private ParticleSystem[] _particles;
    #endregion

    #region unity methods
    private void Awake()
    {
        //子オブジェクトのパーティクルシステムを取得
        _particles = GetComponentsInChildren<ParticleSystem>();
    }

    private void Start()
    {
        //各エフェクトが再生しているか確認する処理の登録。
        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                foreach (var particle in _particles)
                {
                    if (particle.isPlaying)
                    {
                        return;
                    }
                }
                //Particleが停止したら非アクティブ。
                gameObject.SetActive(false);
            });
    }
    #endregion

    #region public method
    /// <summary>
    /// エフェクトを再生する
    /// </summary>
    /// <param name="pos"></param>
    public void Play(Vector3 pos)
    {
        gameObject.SetActive(true);
        transform.localPosition = pos;
        foreach (var particle in _particles)
        {
            particle.Play();
        }
    }

    /// <summary>
    /// エフェクトの再生を止める
    /// </summary>
    public void Return()
    {
        foreach(var particle in _particles)
        {
            particle.Stop();
        }
    }

    /// <summary>
    /// 使用中か確認する
    /// </summary>
    public bool IsActive()
    {
        return gameObject.activeInHierarchy;
    }
    #endregion
}