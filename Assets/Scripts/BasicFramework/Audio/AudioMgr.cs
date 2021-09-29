using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 音效管理器
/// 1.播放，暂停，停止背景音乐
/// 2.播放，停止 音效
/// 3.可以设置背景音乐和音效的音量
/// </summary>
public class AudioMgr : SingleMgr<AudioMgr>
{
    private AudioSource bGAudio;

    private GameObject effectAudioObj;

    private List<AudioSource> effectList;
    private float bGAudioValue;
    private float effectAudioValue;

    List<float> RandomHitPitch = new List<float> { 1.5f, 1.7f, 1.5f, 1.35f, 1.9f, 1f, 1.7f, 1.5f, 1.35f, 1.9f, 1f, 1.7f, 1.5f, 1.35f, 1.9f, 1f, 1.7f, 1.5f, 1.35f, 1.9f };
    List<float> RandomHitVolume = new List<float> { 0.3f, 0.3f, 0.4f, 0.5f, 0.5f, 0.4f, 0.3f, 0.4f, 0.5f, 0.4f, 0.1f, 0.5f, 0.4f, 0.5f, 0.2f, 0.4f, 0.3f, 0.4f, 0.5f, 0.2f };

    int RandomPlayIndex = 0;

    public AudioMgr()
    {
        effectList = new List<AudioSource>();
        bGAudioValue = 1;
        effectAudioValue = 1;

        MonoMgr.GetInstance().AddUpdateListener(Update);
    }

    private void Update()
    {
        for (int i = 0; i < effectList.Count; i++)
        {
            if (effectList[i] != null && !effectList[i].isPlaying)
            {
                GameObject.Destroy(effectList[i]);
                effectList.Remove(effectList[i]);
            }
        }
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="audioName">背景音乐名</param>
    public void playBGAudio(string audioName)
    {
        if (bGAudio == null)
        {
            GameObject BGAudioObj = new GameObject("BGAudio");
            bGAudio = BGAudioObj.AddComponent<AudioSource>();
        }
        if (bGAudio != null && audioName != bGAudio.clip.name)
        {
            ResMgr.GetInstance().ResourceLoadAsync<AudioClip>("Music/" + audioName, (clip) =>
            {
                bGAudio.clip = clip;
                bGAudio.loop = true;
                bGAudio.volume = bGAudioValue;
                bGAudio.Play();
            });
        }
        else
        {
            bGAudio.loop = true;
            bGAudio.volume = bGAudioValue;
            bGAudio.Play();
        }
    }

    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void PauseBGAudio()
    {
        if (bGAudio != null)
        {
            bGAudio.Pause();
        }
    }

    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBGAudio()
    {
        if (bGAudio != null)
        {
            bGAudio.Stop();
        }
    }

    /// <summary>
    /// 设置背景音乐的音量
    /// </summary>
    /// <param name="value">0~1之间的音量值</param>
    public void SetBGAudioValue(float value)
    {
        bGAudioValue = value;

        if (bGAudio != null)
            bGAudio.volume = value;
    }

    /// <summary>
    /// 播放音效 可以回调播放的音效
    /// </summary>
    /// <param name="name">音效名</param>
    /// <param name="isLoop">是否循环</param>
    /// <param name="callBack">播放音效的回调函数</param>
    public void PlayEffectAudio(string name, bool isLoop = false, UnityAction<AudioSource> callBack = null)
    {
        if (effectAudioObj == null)
            effectAudioObj = new GameObject("EffectAudio"); 

        ResMgr.GetInstance().ResourceLoadAsync<AudioClip>("Music/Sound/" + name, (clip) =>
        {
            if (effectAudioObj != null)
            {
                AudioSource source = effectAudioObj.AddComponent<AudioSource>();
                source.volume = CountIndexForRandomVolume();
                source.pitch = CountIndexForRandomPitch();
                if (name.Contains("Kill"))
                {
                    source.volume += 0.4f;
                    //source.pitch = 1f;
                }
                source.clip = clip;
                source.loop = isLoop;
                //source.volume = effectAudioValue;
                source.Play();

                effectList.Add(source);

                if (callBack != null)
                    callBack(source);
            }
        });
    }
    
    /// <summary>
    /// 停止指定音效
    /// </summary>
    /// <param name="source"></param>
    public void StopEffectAudio(AudioSource source)
    {
        if (effectList.Contains(source))
        {
            source.Stop();
            GameObject.Destroy(source);
            effectList.Remove(source);
        }
    }

    /// <summary>
    /// 停止所有音效
    /// </summary>
    public void StopAllEffectAudio()
    {
        for (int i = 0; i < effectList.Count; i++)
            GameObject.Destroy(effectList[i]);

        effectList.Clear();
    }


    /// <summary>
    /// 设置所有音效的音量大小
    /// </summary>
    /// <param name="value"></param>
    public void SetEffectAudioValue(float value)
    {
        effectAudioValue = value;

        for (int i = 0; i < effectList.Count; i++)
        {
            effectList[i].volume = value;
        }
    }

    public float CountIndexForRandomPitch()
    {
        RandomPlayIndex += 1;
        if (RandomPlayIndex + 1 >= RandomHitPitch.Count)
            RandomPlayIndex = 0;

        return RandomHitPitch[RandomPlayIndex];
    }
    public float CountIndexForRandomVolume()
    {
        RandomPlayIndex += 1;
        if (RandomPlayIndex + 1 >= RandomHitPitch.Count)
            RandomPlayIndex = 0;

        return RandomHitVolume[RandomPlayIndex];
    }
}
