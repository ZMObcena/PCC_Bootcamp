using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    [SerializeField] private float minWaitTime;
    [SerializeField] private float maxWaitTime;

    private Light _light;
 
    private void Start()
    {
        _light = GetComponentInChildren<Light>();
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            _light.enabled = ! _light.enabled;
        }
    }
}
