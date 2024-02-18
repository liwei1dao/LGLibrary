﻿using UnityEngine;


namespace LG
{
    /// <summary>
    /// 音频播放器模块
    /// </summary>
    public class SoundModule : ModuleBase<SoundModule>
    {
        private SoundDataComp DataComp;
        public GameObject SoundPlayers { get; private set; }

        public override void LGLoad(params object[] _Agr)
        {
            SoundPlayers = new GameObject("SoundPlayers");
            SoundPlayers.AddComponent<AudioListener>();
            DataComp = AddComp<SoundDataComp>();
            SoundComp = AddComp<Module_SoundComp>();
            Object.DontDestroyOnLoad(SoundPlayers);
            base.LGLoad(_Agr);
        }

        public void InitModelSoundPlayer(string ModelName)
        {
            DataComp.InitModelMusicPLayers(ModelName);
        }
        /// <summary>
        /// 暂停 播放背景音乐
        /// </summary>
        /// <param name="ModelName"></param>
        public void PauseBackMusic(string ModelName)
        {
            DataComp.PauseBackMusic(ModelName);
        }
        /// <summary>
        /// 继续 播放背景音乐
        /// </summary>
        /// <param name="ModelName"></param>
        public void UnPauseBackMusic(string ModelName)
        {
            DataComp.UnPauseBackMusic(ModelName);
        }
        /// <summary>
        /// 设置 音乐 音量
        /// </summary>
        /// <param name="ModelName"></param>
        public void SetBackMusicValue(string ModelName, float soundValue)
        {
            DataComp.SetBackMusicValue(ModelName, soundValue);
        }

        public float GetBackMusicValue(string ModelName)
        {
            return DataComp.GetBackMusicValue(ModelName);
        }

        /// <summary>
        /// 设置 音效 音量
        /// </summary>
        /// <param name="ModelName"></param>
        public void SetEffectMusicValue(string ModelName, float soundValue)
        {
            DataComp.SetEffectMusicValue(ModelName, soundValue);

        }

        public float GetEffectMusicValue(string ModelName)
        {
            return DataComp.GetEffectMusicValue(ModelName);
        }

        /// <summary>
        /// 播放模块音乐/背景音乐
        /// </summary>
        /// <param name="ModelName">模块名称</param>
        /// <param name="Music">音效文件</param>
        /// <param name="IsBackMusic">是否是背景英语</param>
        public AudioSource PlayMusic(string ModelName, AudioClip Music, bool IsBackMusic = false)
        {
            return DataComp.PlayMusic(ModelName, Music, IsBackMusic);
        }

        /// <summary>
        /// 播放模块音乐/背景音乐
        /// </summary>
        /// <param name="ModelName">模块名称</param>
        /// <param name="Music">音效文件</param>
        /// <param name="MusicValue">音量大小</param>
        /// <param name="IsBackMusic">是否是背景英语</param>
        /// <returns></returns>
        public AudioSource PlayMusic(string ModelName, AudioClip Music, float MusicValue, bool IsBackMusic = false)
        {
            return DataComp.PlayMusic(ModelName, Music, MusicValue, IsBackMusic);
        }

        public void StopBackMusic(string ModelName)
        {
            DataComp.StopBackMusic(ModelName);
        }
    }
}