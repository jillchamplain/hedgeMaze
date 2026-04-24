using System.Collections;
using UnityEngine;

public class Fountain : MonoBehaviour
{
    [SerializeField] public float refillAmount;
    [SerializeField] float refillPumpTime;
    
    [Header("References")]
    [SerializeField] PumpVisuals visuals;
    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] public WateringCan wateringCan;
    [SerializeField] public GameObject wateringCanHolder;
    bool isPumping = false;

    public void Pump()
    {
        if(!isPumping)
            StartCoroutine(PumpTimer());
    }

    IEnumerator PumpTimer()
    {
        isPumping = true;
        particleSystem.Play();
        yield return new WaitForSeconds(refillPumpTime);
        wateringCan.Refill(this);
        particleSystem.Stop();
        isPumping = false;
    }
}
