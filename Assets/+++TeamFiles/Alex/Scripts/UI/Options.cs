using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [Header("Graphics")] 
    private string fullScreenPlayerPrefs = "Fullscreen";
    private int fullScreenInt = 1;
    [SerializeField] private GameObject fullScreenCheck;
    [SerializeField] private Volume brightnessVolume;
    private int sameDirectionMinusBrightness = 1;
    private int sameDirectionPlusBrightness;
    private int currentBrightnessPoint = 4;
    private string brightnessPlayerPrefs = "brightness";
    private float brightness = .5f;
    [SerializeField] private List<GameObject> brightnessPoints;

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
        for (int i = 0; i < masterPoints.Count; i++)
        {
            if (masterVolume * 10 - 2 < i)
            {
                masterPoints[i].SetActive(true);
            }
        }
        
        PlayerPrefs.GetFloat(musicVolumePlayerPrefs, musicVolume);
        for (int i = 0; i < musicPoints.Count; i++)
        {
            if (musicVolume * 10 - 2 < i)
            {
                musicPoints[i].SetActive(true);
            }
        }
        
        PlayerPrefs.GetFloat(sfxVolumePlayerPrefs, sfxVolume);
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

        PlayerPrefs.SetInt(fullScreenPlayerPrefs, fullScreenInt);
    }

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
        }
    }
    
    public void BrightnessPlus()
    {
        if (brightness <= .1f)
            return;
        
        Debug.Log(brightness);
        
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
        
        AudioManager.Instance.Play("ButtonDown");
    }
    
    private void ChangeMusicVolume(float value)
    {
        musicVolume += value;
        
        audioMixer.SetFloat("musicVolume", Mathf.Log10(musicVolume) * 20);
        
        PlayerPrefs.SetFloat(musicVolumePlayerPrefs, musicVolume);
        
        AudioManager.Instance.Play("ButtonDown");
    }
    
    private void ChangeSfxVolume(float value)
    {
        sfxVolume += value;

        audioMixer.SetFloat("sfxVolume", Mathf.Log10(sfxVolume) * 20);
        
        PlayerPrefs.SetFloat(sfxVolumePlayerPrefs, sfxVolume);
        
        AudioManager.Instance.Play("ButtonDown");
    }
    
    private void ChangeBrightness(float value)
    {
        brightness += value;

        brightnessVolume.weight = brightness;
        
        PlayerPrefs.SetFloat(brightnessPlayerPrefs, brightness);
        
        AudioManager.Instance.Play("ButtonDown");
    }
}
