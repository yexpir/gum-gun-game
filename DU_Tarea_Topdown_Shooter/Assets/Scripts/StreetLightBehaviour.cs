using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLightBehaviour : MonoBehaviour
{

    [SerializeField] private new Light light;
    [SerializeField] private bool flicker;
    
    public float lightOnTime;
    public float lightOffTime;
    
    private void Start()
    {
        if(flicker) Flicker();
    }

    private void Flicker()
    {
        StartCoroutine(Flickering());
    }

    private IEnumerator Flickering()
    {
        while (gameObject.activeInHierarchy)
        {
            light.gameObject.SetActive(true);
            yield return new WaitForSeconds(Random.Range(0.01f, lightOnTime));
            light.gameObject.SetActive(false);
            yield return new WaitForSeconds(Random.Range(0.01f, lightOffTime));
        }
    }
    
}
