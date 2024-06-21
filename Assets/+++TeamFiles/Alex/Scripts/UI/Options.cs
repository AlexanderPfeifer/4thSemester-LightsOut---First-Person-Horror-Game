using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [Header("Fullscreen")] 
    private string fullScreenPlayerPrefs = "Fullscreen";
    private int fullScreenInt = 1;

    [Header("Audio")] 
    public AudioMixer audioMixer;
    
    [SerializeField] private List<GameObject> masterPoints;
    [SerializeField] private List<GameObject> musicPoints;
    [SerializeField] private List<GameObject> sfxPoints;
    private int currentVolumePointMusic = 4; 
    private int currentVolumePointMaster = 4; 
    private int currentVolumePointSfx = 4; 
    
    private string masterVolumePlayerPrefs = "masterVolume";
    private float masterVolume = .5f;

    private string musicVolumePlayerPrefs = "musicVolume";
    private float musicVolume = .5f;
    
    private string sfxVolumePlayerPrefs = "sfxVolume";
    private float sfxVolume = .5f;

    private int sameDirectionMinusMusic = 1;
    private int sameDirectionPlusMusic;
    
    private int sameDirectionMinusMaster = 1;
    private int sameDirectionPlusMaster;
    
    private int sameDirectionMinusSfx = 1;
    private int sameDirectionPlusSfx;

    public void Start()
    {
        SetPlayerPrefs();
    }

    private void SetPlayerPrefs()
    {
        fullScreenInt = PlayerPrefs.GetInt(fullScreenPlayerPrefs, fullScreenInt);

        PlayerPrefs.GetFloat(masterVolumePlayerPrefs, masterVolume);
        PlayerPrefs.GetFloat(musicVolumePlayerPrefs, musicVolume);
        PlayerPrefs.GetFloat(sfxVolumePlayerPrefs, sfxVolume);
        
        ChangeFullScreenMode(fullScreenInt == 1);
    }

    //converts The bool isFullScreenOn to int for player prefab
    public void ChangeFullScreenMode(bool isFullScreenOn)
    {
        Screen.fullScreen = isFullScreenOn;

        fullScreenInt = isFullScreenOn ? 1 : 0;
        
        PlayerPrefs.SetInt(fullScreenPlayerPrefs, fullScreenInt);
    }

    public void MusicVolumeMinus()
    {
        if (musicVolume <= 0)
            return;

        sameDirectionMinusMusic++;
        
        if (sameDirectionPlusMusic > 0)
        {
            currentVolumePointMusic++;
        }

        sameDirectionPlusMusic = 0;
        
        if (!(currentVolumePointMusic <= 0.2f))
        {
            currentVolumePointMusic--;
            musicPoints[currentVolumePointMusic].GetComponent<Image>().color = new Color(0.39f, 0.39f, 0.39f);
            ChangeMusicVolume(-.1f);
        }
    }
    
    public void MusicVolumePlus()
    {
        if (musicVolume >= 1)
            return;

        sameDirectionPlusMusic++;
        
        if (sameDirectionMinusMusic > 0)
        {
            currentVolumePointMusic--;
        }

        sameDirectionMinusMusic = 0;
        
        if (!(musicVolume > 0.99f))
        {
            currentVolumePointMusic++;
            musicPoints[currentVolumePointMusic].GetComponent<Image>().color = new Color(1f, 1f, 1f);
            ChangeMusicVolume(.1f);
        }
    }
    
    public void MasterVolumeMinus()
    {
        if (masterVolume <= 0)
            return;

        sameDirectionMinusMaster++;
        
        if (sameDirectionPlusMaster > 0)
        {
            currentVolumePointMaster++;
        }

        sameDirectionPlusMaster = 0;

        if (!(currentVolumePointMaster <= 0.2f))
        {
            currentVolumePointMaster--;
            masterPoints[currentVolumePointMaster].GetComponent<Image>().color = new Color(0.39f, 0.39f, 0.39f);
            ChangeMasterVolume(-.1f);
        }
    }
    
    public void MasterVolumePlus()
    {
        if (masterVolume >= 1)
            return;

        sameDirectionPlusMaster++;
        
        if (sameDirectionMinusMaster > 0)
        {
            currentVolumePointMaster--;
        }

        sameDirectionMinusMaster = 0;
        
        if (!(masterVolume > 0.99f))
        {
            currentVolumePointMaster++;
            masterPoints[currentVolumePointMaster].GetComponent<Image>().color = new Color(1f, 1f, 1f);
            ChangeMasterVolume(.1f);
        }
    }
    
    public void SfxVolumeMinus()
    {
        if (sfxVolume <= 0)
            return;

        sameDirectionMinusSfx++;
        
        if (sameDirectionPlusSfx > 0)
        {
            currentVolumePointSfx++;
        }

        sameDirectionPlusSfx = 0;

        if (!(currentVolumePointSfx <= 0.2f))
        {
            currentVolumePointSfx--;
            sfxPoints[currentVolumePointSfx].GetComponent<Image>().color = new Color(0.39f, 0.39f, 0.39f);
            ChangeSfxVolume(-.1f);
        }
    }
    
    public void SfxVolumePlus()
    {
        if (sfxVolume >= 1)
            return;

        sameDirectionPlusSfx++;
        
        if (sameDirectionMinusSfx > 0)
        {
            currentVolumePointSfx--;
        }

        sameDirectionMinusSfx = 0;

        if (!(sfxVolume > 0.99f))
        {
            currentVolumePointSfx++;
            sfxPoints[currentVolumePointSfx].GetComponent<Image>().color = new Color(1f, 1f, 1f);
            ChangeSfxVolume(.1f);
        }
    }
    
    private void ChangeMasterVolume(float value)
    {
        masterVolume += value;
        
        audioMixer.SetFloat("masterVolume", Mathf.Log10(masterVolume) * 20);
        
        PlayerPrefs.SetFloat(masterVolumePlayerPrefs, masterVolume);
    }
    
    private void ChangeMusicVolume(float value)
    {
        musicVolume += value;
        
        audioMixer.SetFloat("musicVolume", Mathf.Log10(musicVolume) * 20);
        
        PlayerPrefs.SetFloat(musicVolumePlayerPrefs, musicVolume);
    }
    
    private void ChangeSfxVolume(float value)
    {
        sfxVolume += value;

        audioMixer.SetFloat("sfxVolume", Mathf.Log10(sfxVolume) * 20);
        
        PlayerPrefs.SetFloat(sfxVolumePlayerPrefs, sfxVolume);
    }
}
