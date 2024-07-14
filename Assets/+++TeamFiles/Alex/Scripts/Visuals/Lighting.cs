using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Lighting : MonoBehaviour
{
    [Header("Light")]
    [SerializeField] public GameObject roomLight;
    [SerializeField] public GameObject otherRoomLight;
    [SerializeField] private GameObject lightningObject;
    [SerializeField] private Light lightningLight;

    [Header("Material")]
    [SerializeField] private Material whiteMat;
    [SerializeField] public Material blackMat;
    
    [Header("Time")]
    private float randomTimeBetweenLightning;
    [SerializeField] private float minLightningTime;
    [SerializeField] private float maxLightningTime;
    private bool lerpLightning;
    
    private void Start()
    {
        AudioManager.Instance.Play("Rain");
        randomTimeBetweenLightning = Random.Range(minLightningTime, maxLightningTime);
    }

    private void Update()
    {
        RandomThunder();
    }

    //Plays a random thunder sound after a random time
    private void RandomThunder()
    {
        if(MotherTimerManager.Instance.diedInScene)
            return;
        
        randomTimeBetweenLightning -= Time.deltaTime;

        if (randomTimeBetweenLightning <= 0 && PlayerInputs.Instance.canInteract)
        {
            var randomLightning = Random.Range(1, 4);

            switch (randomLightning)
            {
                case 1:
                    FirstThunder();
                    break;
                case 2:
                    SecondThunder();
                    break;
                case 3:
                    ThirdThunder();
                    break;
                case 4:
                    FourthThunder();
                    break;
            }
            
            randomTimeBetweenLightning = Random.Range(minLightningTime, maxLightningTime);
        }
    }

    //All the thunders are all manually made by lerping the light intensity up and down again and this very often in succession 
    
    #region FirstThunder

    private void FirstThunder()
    {
        StartCoroutine(RandomLightningOne());
    }

    private IEnumerator RandomLightningOne()
    {
        lightningObject.SetActive(true);

        StartCoroutine(LerpLightningUp(.5f, 500));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));

        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningUp(.5f, 500));

        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningUp(.2f, 150));

        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));
        
        yield return new WaitUntil(() => lerpLightning == false);

        StartCoroutine(LerpLightningUp(.9f, 1000));
        
        yield return new WaitUntil(() => lerpLightning == false);

        StartCoroutine(LerpLightningDown(0, 0));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        lightningObject.SetActive(false);

        yield return new WaitForSeconds(Random.Range(4, 7));
        
        AudioManager.Instance.Play("ThunderOne");
    }
    
    #endregion

    #region SecondThunder

    private void SecondThunder()
    {
        StartCoroutine(RandomLightningTwo());
    }
    
        private IEnumerator RandomLightningTwo()
    {
        lightningObject.SetActive(true);

        StartCoroutine(LerpLightningUp(.3f, 250));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));

        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningUp(.3f, 250));

        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningUp(.6f, 450));

        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        
        StartCoroutine(LerpLightningUp(1f, 2000));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));

        yield return new WaitUntil(() => lerpLightning == false);

        StartCoroutine(LerpLightningUp(.3f, 250));
        
        yield return new WaitUntil(() => lerpLightning == false);

        StartCoroutine(LerpLightningDown(0f, 0));
        
        yield return new WaitUntil(() => lerpLightning == false);

        lightningObject.SetActive(false);

        yield return new WaitForSeconds(Random.Range(4, 7));
        
        AudioManager.Instance.Play("ThunderTwo");
    }

    #endregion

    #region ThirdThunder

    private void ThirdThunder()
    {
        StartCoroutine(RandomLightningThree());
    }

    private IEnumerator RandomLightningThree()
    {
        lightningObject.SetActive(true);

        StartCoroutine(LerpLightningUp(1f, 2500));

        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));

        yield return new WaitForSeconds(.8f);
        
        StartCoroutine(LerpLightningUp(.4f, 250));
        
        yield return new WaitUntil(() => lerpLightning == false);

        StartCoroutine(LerpLightningDown(0, 0));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningUp(.4f, 150));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));

        yield return new WaitUntil(() => lerpLightning == false);

        lightningObject.SetActive(false);

        yield return new WaitForSeconds(Random.Range(4, 7));
        
        AudioManager.Instance.Play("ThunderThree");
    }
    #endregion
    
    #region FourthThunder

    private void FourthThunder()
    {
        StartCoroutine(RandomLightningFour());
    }

    private IEnumerator RandomLightningFour()
    {
        lightningObject.SetActive(true);

        StartCoroutine(LerpLightningUp(.5f, 950));

        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));
        
        yield return new WaitUntil(() => lerpLightning == false);

        StartCoroutine(LerpLightningUp(.8f, 1500));

        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));
        
        yield return new WaitUntil(() => lerpLightning == false);

        StartCoroutine(LerpLightningUp(.4f, 350));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningUp(.4f, 350));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));

        yield return new WaitUntil(() => lerpLightning == false);

        StartCoroutine(LerpLightningUp(.8f, 1250));

        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));

        yield return new WaitUntil(() => lerpLightning == false);

        StartCoroutine(LerpLightningUp(.8f, 1500));

        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));

        yield return new WaitUntil(() => lerpLightning == false);

        lightningObject.SetActive(false);

        yield return new WaitForSeconds(Random.Range(4, 7));
        
        AudioManager.Instance.Play("ThunderFour");
    }

    #endregion

    //Lerp the light and white material up
    private IEnumerator LerpLightningUp(float whiteMatAlpha, float randomIntensityNumber)
    {
        lerpLightning = true;
        
        while (whiteMat.color.a < whiteMatAlpha -.01 && lightningLight.intensity < randomIntensityNumber -.01)
        {
            whiteMat.color = new Color(1, 1, 1, Mathf.Lerp(whiteMat.color.a, whiteMatAlpha, Time.deltaTime * 15));
            lightningLight.intensity = Mathf.Lerp(lightningLight.intensity,randomIntensityNumber, Time.deltaTime * 30);
            yield return null;
        }

        lerpLightning = false;
    }

    //Lerp the light and white material down
    private IEnumerator LerpLightningDown(float whiteMatAlpha, float randomIntensityNumber)
    {
        lerpLightning = true;
        
        while (whiteMat.color.a > whiteMatAlpha +.01 && lightningLight.intensity > randomIntensityNumber +.01)
        {
            whiteMat.color = new Color(1, 1, 1, Mathf.Lerp(whiteMat.color.a, whiteMatAlpha, Time.deltaTime * 15));
            lightningLight.intensity = Mathf.Lerp(lightningLight.intensity,randomIntensityNumber, Time.deltaTime * 30);
            yield return null;
        }

        lerpLightning = false;
    }
}
