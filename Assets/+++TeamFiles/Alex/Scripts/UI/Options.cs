using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [Header("Fullscreen")] 
    private const string FullScreenPlayerPrefs = "Fullscreen";
    private int fullScreenInt = 1;
    [SerializeField] private GameObject fullScreenCheck;
    
    [Header("Brightness")]
    [SerializeField] private Volume brightnessVolume;
    private int sameDirectionMinusBrightness = 1;
    private int sameDirectionPlusBrightness;
    private int currentBrightnessPoint = 4;
    private const string BrightnessPlayerPrefs = "brightness";
    private float brightness = .5f;
    [SerializeField] private List<GameObject> brightnessPoints;

    [Header("AudioMixer")] 
    public AudioMixer audioMixer;
    
    [Header("AudioMaster")]
    [SerializeField] private List<GameObject> masterPoints;
    private int currentVolumePointMaster = 4;
    private const string MasterVolumePlayerPrefs = "masterVolume";
    private float masterVolume = .5f;
    private int sameDirectionMinusMaster = 1;
    private int sameDirectionPlusMaster;
    
    [Header("AudioMusic")]
    [SerializeField] private List<GameObject> musicPoints;
    private int currentVolumePointMusic = 4;
    private const string MusicVolumePlayerPrefs = "musicVolume";
    private float musicVolume = .5f;
    private int sameDirectionMinusMusic = 1;
    private int sameDirectionPlusMusic;
    
    [Header("AudioSFX")]
    [SerializeField] private List<GameObject> sfxPoints;
    private int currentVolumePointSfx = 4;
    private const string SfxVolumePlayerPrefs = "sfxVolume";
    private float sfxVolume = .5f;
    private int sameDirectionMinusSfx = 1;
    private int sameDirectionPlusSfx;

    public void Start()
    {
        SetPlayerPrefs();
    }
    
    //Sets all the player prefs for the settings
    private void SetPlayerPrefs()
    {
        fullScreenInt = PlayerPrefs.GetInt(FullScreenPlayerPrefs, fullScreenInt);

        PlayerPrefs.GetFloat(MasterVolumePlayerPrefs, masterVolume);
        for (int i = 0; i < masterPoints.Count; i++)
        {
            if (masterVolume * 10 - 2 < i)
            {
                masterPoints[i].SetActive(true);
            }
        }
        
        PlayerPrefs.GetFloat(MusicVolumePlayerPrefs, musicVolume);
        for (int i = 0; i < musicPoints.Count; i++)
        {
            if (musicVolume * 10 - 2 < i)
            {
                musicPoints[i].SetActive(true);
            }
        }
        
        PlayerPrefs.GetFloat(SfxVolumePlayerPrefs, sfxVolume);
        for (int i = 0; i < sfxPoints.Count; i++)
        {
            if (sfxVolume * 10 - 2 < i)
            {
                sfxPoints[i].SetActive(true);
            }
        }

        Screen.fullScreen = fullScreenInt == 1;
        fullScreenCheck.SetActive(fullScreenInt == 1);
    }

    //converts The bool isFullScreenOn to int for player prefab
    public void ChangeFullScreenMode(bool isFullScreenOn)
    {
        AudioManager.Instance.Play("ButtonRegular");
        
        Screen.fullScreen = isFullScreenOn;
        
        fullScreenInt = isFullScreenOn ? 1 : 0;

        fullScreenCheck.SetActive(isFullScreenOn);

        PlayerPrefs.SetInt(FullScreenPlayerPrefs, fullScreenInt);
    }

    //Adds or subtracts value according to the parameter
    #region Plus and minus of sfx and graphics

        public void BrightnessMinus()
    {
        if (brightness <= 0)
            return;
        
        sameDirectionMinusBrightness++;
        
        if (sameDirectionPlusBrightness > 0)
        {
            currentBrightnessPoint++;
        }

        sameDirectionPlusBrightness = 0;
        
        if (!(currentBrightnessPoint <= 0.2f))
        {
            currentBrightnessPoint--;
            brightnessPoints[currentBrightnessPoint].GetComponent<Image>().color = new Color(0.39f, 0.39f, 0.39f);
            ChangeBrightness(.1f);
            AudioManager.Instance.Play("ButtonDown");
        }
    }
    
    public void BrightnessPlus()
    {
        if (brightness <= .1f)
            return;
        
        
        sameDirectionPlusBrightness++;
        
        if (sameDirectionMinusBrightness > 0)
        {
            currentBrightnessPoint--;
        }

        sameDirectionMinusBrightness = 0;
        
        if (!(brightness > 0.99f))
        {
            currentBrightnessPoint++;
            brightnessPoints[currentBrightnessPoint].GetComponent<Image>().color = new Color(1f, 1f, 1f);
            ChangeBrightness(-.1f);
            AudioManager.Instance.Play("ButtonUp");
        }
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
            AudioManager.Instance.Play("ButtonDown");
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
            AudioManager.Instance.Play("ButtonUp");
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
            AudioManager.Instance.Play("ButtonDown");
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
            AudioManager.Instance.Play("ButtonUp");
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
            AudioManager.Instance.Play("ButtonDown");
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
            AudioManager.Instance.Play("ButtonUp");
        }
    }

    #endregion

    //Applies the changes of the methods before in here
    #region sfx and graphics change

    private void ChangeMasterVolume(float value)
    {
        masterVolume += value;
        
        audioMixer.SetFloat("masterVolume", Mathf.Log10(masterVolume) * 20);
        
        PlayerPrefs.SetFloat(MasterVolumePlayerPrefs, masterVolume);
    }
    
    private void ChangeMusicVolume(float value)
    {
        musicVolume += value;
        
        audioMixer.SetFloat("musicVolume", Mathf.Log10(musicVolume) * 20);
        
        PlayerPrefs.SetFloat(MusicVolumePlayerPrefs, musicVolume);
    }
    
    private void ChangeSfxVolume(float value)
    {
        sfxVolume += value;

        audioMixer.SetFloat("sfxVolume", Mathf.Log10(sfxVolume) * 20);
        
        PlayerPrefs.SetFloat(SfxVolumePlayerPrefs, sfxVolume);
    }
    
    private void ChangeBrightness(float value)
    {
        brightness += value;

        brightnessVolume.weight = brightness;
        
        PlayerPrefs.SetFloat(BrightnessPlayerPrefs, brightness);
    }

    #endregion
}
