using System;
using System.Collections.Generic;
using UnityEngine;

namespace LG
{
  /// <summary>
  /// 模块音频组件
  /// </summary>
  public class Module_SoundComp : ModelCompBase
  {
    #region 框架构造
    public override void LGLoad(ModuleBase module, params object[] _Agr)
    {
      if (SoundModule.Instance == null)
      {
        Debug.LogError("SoundModule User but No Load");
        return;
      }
      base.LGLoad(module);
      base.LoadEnd();
    }

    public override void LGStart()
    {
      SoundModule.Instance.InitModelSoundPlayer(Module.ModuleName);
      base.LGStart();
    }

    #endregion
    public void PauseBackMusic()
    {
      SoundModule.Instance.PauseBackMusic(Module.ModuleName);
    }
    public void UnPauseBackMusic()
    {
      SoundModule.Instance.UnPauseBackMusic(Module.ModuleName);
    }

    public void SetBackMusicValue(string ModelName, float soundValue)
    {
      SoundModule.Instance.SetBackMusicValue(Module.ModuleName, soundValue);
    }

    public void SetEffectMusicValue(string ModelName, float soundValue)
    {
      SoundModule.Instance.SetEffectMusicValue(Module.ModuleName, soundValue);
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="ModelName"></param>
    /// <param name="Music"></param>
    /// <param name="MusicValue"></param>
    /// <param name="IsBackMusic"></param>
    /// <returns></returns>
    public AudioSource PlayMusic(string Music, bool IsBackMusic = false)
    {
      AudioClip music = Module.LoadAsset<AudioClip>("Sound", Music);
      return SoundModule.Instance.PlayMusic(Module.ModuleName, music, IsBackMusic);
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="ModelName"></param>
    /// <param name="Music"></param>
    /// <param name="MusicValue"></param>
    /// <param name="IsBackMusic"></param>
    /// <returns></returns>
    public AudioSource PlayMusic(AudioClip Music, bool IsBackMusic = false)
    {
      return SoundModule.Instance.PlayMusic(Module.ModuleName, Music, IsBackMusic);
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="ModelName"></param>
    /// <param name="Music"></param>
    /// <param name="MusicValue"></param>
    /// <param name="IsBackMusic"></param>
    /// <returns></returns>
    public AudioSource PlayMusic(string Music, float MusicValue, bool IsBackMusic = false)
    {
      AudioClip music = Module.LoadAsset<AudioClip>("Sound", Music);
      return SoundModule.Instance.PlayMusic(Module.ModuleName, music, MusicValue, IsBackMusic);
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="ModelName"></param>
    /// <param name="Music"></param>
    /// <param name="MusicValue"></param>
    /// <param name="IsBackMusic"></param>
    /// <returns></returns>
    public AudioSource PlayMusic(AudioClip Music, float MusicValue, bool IsBackMusic = false)
    {
      return SoundModule.Instance.PlayMusic(Module.ModuleName, Music, MusicValue, IsBackMusic);
    }
    public void StopBackMusic()
    {
      SoundModule.Instance.StopBackMusic(Module.ModuleName);
    }

  }
}