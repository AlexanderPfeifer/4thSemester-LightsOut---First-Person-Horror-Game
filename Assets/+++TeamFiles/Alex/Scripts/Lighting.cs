using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Lighting : MonoBehaviour
{
    [SerializeField] public GameObject roomLight;
    [SerializeField] public GameObject otherRoomLight;
    [SerializeField] private GameObject lightningObject;
    [SerializeField] private Material whiteMat;
    [SerializeField] public Material blackMat;
    [SerializeField] private Light lightningLight;
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
        randomTimeBetweenLightning -= Time.deltaTime;

        if (randomTimeBetweenLightning <= 0 && PlayerInputs.instance.canInteract)
        {
            var randomLightning = Random.Range(1, 4);

            switch (randomLightning)
            {
                case 1:
                    StartCoroutine(RandomLightningOne());
                    break;
                case 2:
                    StartCoroutine(RandomLightningTwo());
                    break;
                case 3:
                    StartCoroutine(RandomLightningThree());
                    break;
                case 4:
                    StartCoroutine(RandomLightningFour());
                    break;
            }
            
            randomTimeBetweenLightning = Random.Range(minLightningTime, maxLightningTime);
        }
    }

    private IEnumerator RandomLightningOne()
    {
        lightningObject.SetActive(true);

        StartCoroutine(LerpLightningUp(.5f, 8));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));

        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningUp(.5f, 8));

        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningUp(.2f, 3));

        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));
        
        yield return new WaitUntil(() => lerpLightning == false);

        StartCoroutine(LerpLightningUp(.9f, 12));
        
        yield return new WaitUntil(() => lerpLightning == false);

        StartCoroutine(LerpLightningDown(0, 0));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        lightningObject.SetActive(false);

        yield return new WaitForSeconds(Random.Range(4, 7));
        
        AudioManager.Instance.Play("ThunderOne");
    }
    
    private IEnumerator RandomLightningTwo()
    {
        lightningObject.SetActive(true);

        StartCoroutine(LerpLightningUp(.3f, 4));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));

        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningUp(.3f, 4));

        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningUp(.6f, 5));

        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));
        
        yield return new WaitUntil(() => lerpLightning == false);

        StartCoroutine(LerpLightningUp(.3f, 4));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));

        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningUp(.3f, 4));

        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));
        
        yield return new WaitUntil(() => lerpLightning == false);

        StartCoroutine(LerpLightningDown(0, 0));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningUp(1f, 13));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));

        yield return new WaitUntil(() => lerpLightning == false);

        StartCoroutine(LerpLightningUp(.3f, 4));
        
        yield return new WaitUntil(() => lerpLightning == false);

        StartCoroutine(LerpLightningDown(0f, 0));
        
        yield return new WaitUntil(() => lerpLightning == false);

        lightningObject.SetActive(false);

        yield return new WaitForSeconds(Random.Range(4, 7));
        
        AudioManager.Instance.Play("ThunderTwo");
    }
    
    private IEnumerator RandomLightningThree()
    {
        lightningObject.SetActive(true);

        StartCoroutine(LerpLightningUp(1f, 13));

        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));

        yield return new WaitForSeconds(.8f);
        
        StartCoroutine(LerpLightningUp(.4f, 4));
        
        yield return new WaitUntil(() => lerpLightning == false);

        StartCoroutine(LerpLightningDown(0, 0));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningUp(.4f, 4));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));

        yield return new WaitUntil(() => lerpLightning == false);

        lightningObject.SetActive(false);

        yield return new WaitForSeconds(Random.Range(4, 7));
        
        AudioManager.Instance.Play("ThunderThree");
    }
    
    private IEnumerator RandomLightningFour()
    {
        lightningObject.SetActive(true);

        StartCoroutine(LerpLightningUp(.5f, 7));

        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));
        
        yield return new WaitUntil(() => lerpLightning == false);

        StartCoroutine(LerpLightningUp(.8f, 11));

        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));
        
        yield return new WaitUntil(() => lerpLightning == false);

        StartCoroutine(LerpLightningUp(.4f, 4));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningUp(.4f, 4));
        
        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));

        yield return new WaitUntil(() => lerpLightning == false);

        StartCoroutine(LerpLightningUp(.8f, 11));

        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));

        yield return new WaitUntil(() => lerpLightning == false);

        StartCoroutine(LerpLightningUp(.8f, 11));

        yield return new WaitUntil(() => lerpLightning == false);
        
        StartCoroutine(LerpLightningDown(0, 0));

        yield return new WaitUntil(() => lerpLightning == false);

        lightningObject.SetActive(false);

        yield return new WaitForSeconds(Random.Range(4, 7));
        
        AudioManager.Instance.Play("ThunderFour");
    }

    private IEnumerator LerpLightningUp(float whiteMatAlpha, float randomIntensityNumber)
    {
        lerpLightning = true;
        
        while (whiteMat.color.a < whiteMatAlpha -.1 && lightningLight.intensity < randomIntensityNumber -.1)
        {
            whiteMat.color = new Color(1, 1, 1, Mathf.Lerp(whiteMat.color.a, whiteMatAlpha, Time.deltaTime * 15));
            lightningLight.intensity = Mathf.Lerp(lightningLight.intensity,randomIntensityNumber, Time.deltaTime * 15);
            yield return null;
        }

        lerpLightning = false;
    }

    private IEnumerator LerpLightningDown(float whiteMatAlpha, float randomIntensityNumber)
    {
        lerpLightning = true;
        
        while (whiteMat.color.a > whiteMatAlpha +.1 && lightningLight.intensity > randomIntensityNumber +.1)
        {
            whiteMat.color = new Color(1, 1, 1, Mathf.Lerp(whiteMat.color.a, whiteMatAlpha, Time.deltaTime * 15));
            lightningLight.intensity = Mathf.Lerp(lightningLight.intensity,randomIntensityNumber, Time.deltaTime * 15);
            yield return null;
        }

        lerpLightning = false;
    }
}
