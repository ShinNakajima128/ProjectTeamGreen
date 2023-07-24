using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public enum BGMType
{
    Title,         //タイトル画面
    InGame,        //インゲーム
    Result         //リザルト画面
}
public enum SEType
{
    Damage_Player, //Hit06-1
    Dead_Player,   //Hit06-6
    Damage_Enemy,  //Motion-Grab01-1
    Dead_Enemy,    //Motion-Pop20-1
    Heal,          //ゲージ回復1
    PowerUp,       //ゲージ回復2
    Transition,    //シーン切り替え2,
    GetSkill_1,    //パワーアップ
    GetSkill_2,    //パワーチャージ
    GetSkill_3,    //ロケットランチャー
    GetSkill_4,    //決定ボタンを押す10
    BecomeBoss_1,  //文字表示の衝撃音1
    BecomeBoss_2,  //文字表示の衝撃音3
    UIActive,      //UI表示
    BossDied,      //ボス撃破
    BombExplotion, //se_bomb5
    BossAlert,     //Warning-Siren
    AuraActive,     //重力魔法2
    FairyUp
}


public class AudioManager : MonoBehaviour
{
    #region property
    public int CurrentBGMVolume { get; set; } = 5;

    public int CurrentSEVolume { get; set; } = 5;

    public static AudioManager Instance {get; private set;}
    #endregion

    #region serialize
    [Header("各音量")]
    [Tooltip("全体の音量")]
    [SerializeField, Range(0f, 1f)]
    float _masterVolume = 1.0f;

    [Tooltip("BGMの音量")]
    [SerializeField,Range(0f,1f)]
    float _bgmVolume = 0.3f;

    [Tooltip("SEの音量")]
    [SerializeField, Range(0f,1f)]
    float _seVolume = 1.0f;

    [Tooltip("SEのAudioSourceの生成数")]
    [SerializeField]
    int _seAudioSourceNum = 5;

    [Header("各音源のリスト")]
    [Tooltip("BGMのリスト")]
    [SerializeField]
    List<BGM> _bgmList = new List<BGM>();

    [Tooltip("SEのリスト")]
    [SerializeField]
    List<SE> _seList = new List<SE>();

    [Header("使用する各オブジェクト")]
    [Tooltip("BGM用のオーディオソース")]
    [SerializeField]
    AudioSource _bgmSource = default;

    [Tooltip("SE用のAudioSourceをまとめるオブジェクト")]
    [SerializeField]
    Transform _seSorcesParent = default;

    [Tooltip("AudioMixer")]
    [SerializeField]
    AudioMixer _mixer = default;

    List<AudioSource> _seAudioSourceList = new List<AudioSource>();
    bool _isStoping = false;

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
        Instance = this;

        //指定した数のSE用AudioSourceを生成
        for (int i = 0; i < _seAudioSourceNum; i++)
        {
            //SEAudioSourceのオブジェクトを生成し親オブジェクトにセット
            var obj = new GameObject($"SESource{i + 1}");
            obj.transform.SetParent(_seSorcesParent);

            //生成したオブジェクトにAudioSourceを追加
            var source = obj.AddComponent<AudioSource>();

            if (_mixer != null)
            {
                source.outputAudioMixerGroup = _mixer.FindMatchingGroups("Master")[2];
            }
            _seAudioSourceList.Add(source);
        }
    }
    #endregion

    #region play method
    /// <summary>
    /// BGMを再生
    /// </summary>
    /// <param name="type">BGMの種類</param>
    /// <param name="loopType"></param>
    public static void PlayBGM(BGMType type, bool loopType = true)
    {
        var bgm = GetBGM(type);

        if (bgm != null)
        {
            if (Instance._bgmSource.clip == null)
            {
                Instance._bgmSource.clip = bgm.Clip;
                Instance._bgmSource.loop = loopType;
                Instance._bgmSource.volume = Instance._bgmVolume * Instance._masterVolume * bgm.Volume;
                Instance._bgmSource.Play();
                Debug.Log($"{bgm.BGMName}を再生");
            }
            else
            {
                Instance.StartCoroutine(Instance.SwitchingBgm(bgm, loopType));
                Debug.Log($"{bgm.BGMName}を再生");
            }
        }
        else
        {
            Debug.LogError($"BGM:{type}を再生できませんでした");
        }
    }

    public static void PlaySE(SEType type)
    {
        var se = GetSE(type);

        if (se != null)
        {
            foreach (var s in Instance._seAudioSourceList)
            {
                if (!s.isPlaying)
                {
                    s.PlayOneShot(se.Clip, Instance._seVolume * Instance._masterVolume);
                    return;
                }
            }
        }
        else
        {
            Debug.LogError($"SE:{type}を再生できませんでした。");
        }
    }

    #endregion

    #region stop method

    /// <summary>
    /// 再生中のBGMを停止する
    /// </summary>
    public static void StopBGM()
    {
        Instance._bgmSource.Stop();
        Instance._bgmSource.clip = null; 
    }

    /// <summary>
    /// 再生中のBGMの音量を徐々に下げて停止する
    /// </summary>
    /// <param name="stopTime"></param>
    public static void StopBGM(float stopTime)
    {
        Instance.StartCoroutine(Instance.LowerVolume(stopTime));
    }

    /// <summary>
    /// 再生中のSEを停止する
    /// </summary>
    public static void StopSE()
    {
        foreach (var s in Instance._seAudioSourceList)
        {
            s.Stop();
            s.clip = null;
        }
    }
    #endregion


    #region volume method

    /// <summary>
    /// マスター音量を変更する
    /// </summary>
    /// <param name="masterValue"></param>
    public static void MasterVolChange(float masterValue)
    {
        Instance._masterVolume = masterValue;
        Instance._bgmSource.volume = Instance._bgmVolume * Instance._masterVolume;
    }

    /// <summary>
    /// BGM音量を変更する
    /// </summary>
    /// <param name="bgmVolume">音量</param>
    public static void BGMVolChange(float bgmVolume)
    {
        //-80~0に変換
        var volume = Mathf.Clamp(Mathf.Log10(Mathf.Clamp(bgmVolume, 0f, 1f)) * 20f, -80f, 0f);
        Debug.Log($"音量をdBに変換:{Mathf.Log10(bgmVolume) * 20f}");
        //audioMixerに代入
        Instance._mixer.SetFloat("BGM", volume);
    }

    /// <summary>
    /// SE音量を変更する
    /// </summary>
    /// <param name="seVolume">音量</param>
    public static void SeVolChange(float seVolume)
    {
        //80~0に変換
        var volume = Mathf.Clamp(Mathf.Log10(Mathf.Clamp(seVolume, 0f, 1f)) * 20f, -80f, 0f);
        Debug.Log($"音量をdBに変換:{Mathf.Log10(seVolume) *20f}");
        //audioMixerに大ニュ
        Instance._mixer.SetFloat("SE", volume);
    }
    #endregion

    #region private method
    #endregion

    #region coroutine method

    /// <summary>
    /// BGMを徐々に変更する
    /// </summary>
    /// <param name="afterBgm">変更後のBGM</param>
    /// <param name="loopType"></param>
    IEnumerator SwitchingBgm(BGM afterBgm, bool loopType = true)
    {
        _isStoping = false;
        float currentVol = _bgmSource.volume;

        while(_bgmSource.volume > 0) //現在の音量を0にする
        {
            _bgmSource.volume -= Time.deltaTime * 1.5f;
            yield return null;
        }

        _bgmSource.clip = afterBgm.Clip; //BGMの入れ替え
        _bgmSource.loop = loopType;
        _bgmSource.Play();
        //currentVol = Instance._bgmVolume * Instance._masterVolume;

        //音量を基に戻す
        while(_bgmSource.volume < currentVol)
        {
            _bgmSource.volume += Time.deltaTime * 1.5f;
            yield return null;
        }
        _bgmSource.volume = currentVol;
    }

    /// <summary>
    /// 音量を徐々に下げて停止するコルーチン
    /// </summary>
    /// <param name="time">停止するまでの時間</param>
    /// <returns></returns>
    IEnumerator LowerVolume(float time)
    {
        float currentVol = _bgmSource.volume;
        _isStoping = true;

        while(_bgmSource.volume > 0)//現在の音量を0にする
        {
            _bgmSource.volume -= Time.deltaTime * currentVol / time;

            //途中でBGM等が変更された場合は処理を中断
            if (!_isStoping)
            {
                yield break;
            }
            yield return null;
        }

        _isStoping = false;
        Instance._bgmSource.Stop();
        Instance._bgmSource.clip = null;
    }
    #endregion

    #region get method

    /// <summary>
    /// BGMを取得
    /// </summary>
    /// <param name="type">BGMの種類</param>
    /// <returns>指定したBGM</returns>
    private static BGM GetBGM(BGMType type)
    {
        var bgm = Instance._bgmList.FirstOrDefault(b => b.BGMType == type);
        return bgm;
    }

    private static SE GetSE(SEType type)
    {
        var se = Instance._seList.FirstOrDefault(s => s.SEType == type);
        return se;
    }
    #endregion

}

[Serializable]
public class BGM
{
    public string BGMName;
    public BGMType BGMType;
    public AudioClip Clip;
    [Range(0, 1)]
    public float Volume = 1f;
}

[Serializable]
public class SE
{
    public string SEName;
    public SEType SEType;
    public AudioClip Clip;
    [Range(0, 1)]
    public float Volume = 1f;
}
