using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light mainLight;
    public float minIntensity = 0.5f;   
    public float maxIntensity = 10f;     
    public float flickerSpeed = 3.0f;   

    private float targetIntensity;      
    private float currentChangeSpeed; 

    void Start()
    {
        targetIntensity = Random.Range(minIntensity, maxIntensity);
        currentChangeSpeed = flickerSpeed;
    }

    void Update()
    {
        mainLight.intensity = Mathf.MoveTowards(mainLight.intensity, targetIntensity, currentChangeSpeed * Time.deltaTime);

        if (Mathf.Approximately(mainLight.intensity, targetIntensity))
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
            currentChangeSpeed = Random.Range(flickerSpeed * 0.8f, flickerSpeed * 1.2f); 
        }
    }
}
