using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
	public static VolumeManager ins;

	public AudioMixer musicMixer;
	public AudioMixer soundMixer;

	public float musicVolume = 1;
	public float soundVolume = 1;

    public AudioSource bgm;

    bool isChangingBgm = false;
    string bgmName;
    float t;


    void Start()
    {
    	ins = this;
    	Load();
    }

    void Update(){
        if(isChangingBgm){
            t-=Time.deltaTime;
            if(t<0){
                if(t<-1)isChangingBgm = false;
                bgm.volume =-t;
                if(t+Time.deltaTime > 0){
                    var clip = Resources.Load<AudioClip>("music/"+bgmName);
                    bgm.clip = clip;
                    bgm.Play();
                }
            }
            else{
                bgm.volume = t;
            }
        }
    }

    public void Load(){
        musicVolume = PlayerPrefs.GetFloat("musicVolume",0.7f);
        soundVolume = PlayerPrefs.GetFloat("soundVolume",0.7f);

        musicMixer.SetFloat("v",musicVolume<0.05f?-80:(musicVolume*50-50));
        soundMixer.SetFloat("v",soundVolume<0.05f?-80:(soundVolume*50-50));
    }

    public void SetMusicVolume(float volume){
    	musicVolume = volume;
        musicMixer.SetFloat("v",musicVolume<0.05f?-80:(musicVolume*50-50));
    }

    public void SetSoundVolume(float volume){
    	soundVolume = volume;
        soundMixer.SetFloat("v",soundVolume<0.05f?-80:(soundVolume*50-50));
    }

    public void Save(){
        PlayerPrefs.SetFloat("musicVolume",musicVolume);
        PlayerPrefs.SetFloat("soundVolume",soundVolume);
    }

    public void PlayBgm(string bgmName){
        if(bgm.clip!=null && bgm.clip.name == bgmName){
            if(!bgm.isPlaying)bgm.Play();
            return;
        }

        if(bgm.clip == null){
            var clip = Resources.Load<AudioClip>("music/"+bgmName);
            bgm.clip = clip;
            bgm.Play();
            return;
        }

        isChangingBgm = true;
        t = 1;
        this.bgmName = bgmName;
    }

    public void PauseBgm(){
        bgm.Pause();
    }

    public void ResumeBgm(){
        bgm.Play();
    }

    public void StopBgm(){
        bgm.Stop();
        bgm.clip = null;
    }
}
