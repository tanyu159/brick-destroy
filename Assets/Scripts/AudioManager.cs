using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType { BUTTON,WIN,FAILED,BRICK,BRICK_DESTROY,JUMP,FALL_DOWN,AWARD,PUNISHMENT,PREFECT }

public class AudioManager : MonoBehaviour {
    //该脚本管理声音播放
    private static AudioManager _instance;
    private AudioSource _soundAS;
    private AudioSource _bgmAS;
    public static AudioManager Instance
    {
        get
        {
            return _instance;
        }

    }
    public Dictionary<SoundType, AudioClip> SoundClipDic;
    [System.Serializable]
    public struct SoundClip {
        public SoundType soundType;
        public AudioClip audioClip;
    }
    public SoundClip[] soundClipsArray;
    private void Awake()
    {
        _instance = this;
    }
    void Start () {
        _soundAS = this.transform.Find("Sound").GetComponent<AudioSource>();
        _bgmAS = this.transform.Find("BGM").GetComponent<AudioSource>();
        SoundClipDic = new Dictionary<SoundType, AudioClip>();
        foreach (var temp in soundClipsArray)
        {
            SoundClipDic.Add(temp.soundType, temp.audioClip);
        }
        if (PlayerPrefs.GetInt("AudioSwitch",1)==0)
        {
            _soundAS.enabled = false;
            _bgmAS.enabled = false;
        }
        else
        {
            _soundAS.enabled = true;
            _bgmAS.enabled = true;

        }
    }
    public void PlaySound(SoundType soundType)
    {
        _soundAS.PlayOneShot(SoundClipDic[soundType]);
    }
    public void ControllBGM(bool play)
    {
        if (play)
        {
            _bgmAS.Play();
        }
        else {
            _bgmAS.Stop();
        }
    }
}
