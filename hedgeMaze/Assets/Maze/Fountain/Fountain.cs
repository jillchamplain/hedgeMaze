using System.Collections;
using UnityEngine;

public class Fountain : MonoBehaviour
{
    [SerializeField] public float refillAmount;
    [SerializeField] float refillPumpTime;
    [SerializeField] float sensitivity;
    [SerializeField] float maxUpSpeed;
    [SerializeField] float maxDownSpeed;
    [SerializeField] float speedToPlayWater;

    [Header("References")]
    [SerializeField] PumpVisuals visuals;
    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] public WateringCan wateringCan;
    [SerializeField] public GameObject wateringCanHolder;
    bool isPumping = false;



    bool pumpStopped = false;
    Coroutine pumpStopCoroutine;

    public void Pump()
    {
        if (!wateringCan)
            return;

        isPumping = true;
        pumpStopped = false;

        if (pumpStopCoroutine != null)
            StopCoroutine(pumpStopCoroutine);
        pumpStopCoroutine = StartCoroutine(WaitForPumpStop());

        CameraController.instance.canLook = false;
        float MousePos = CameraController.instance.mouseY;

        if (MousePos > 0)
        {
            float change = Mathf.Clamp(MousePos / sensitivity, 0, maxUpSpeed);
            if (change > speedToPlayWater)
            {
                particleSystem.Play();
                if (wateringCan)
                    wateringCan.Refill(this);
            }
            else
            {
                particleSystem.Stop();
            }

            visuals.amount += change;
        }
        if (MousePos < 0)
        {
            float change = Mathf.Clamp(MousePos / sensitivity, 0, maxDownSpeed);
            visuals.amount += MousePos / sensitivity;
            particleSystem.Stop();
        }

    }

    IEnumerator WaitForPumpStop()
    {
        yield return null; 
        pumpStopped = true;
        isPumping = false;
        CameraController.instance.canLook = true;
    }

    private void Update()
    {

    }

    IEnumerator PumpTimer()
    {
        isPumping = true;
        particleSystem.Play();
        yield return new WaitForSeconds(refillPumpTime);
        if(wateringCan)
            wateringCan.Refill(this);
        particleSystem.Stop();
        isPumping = false;
    }
}
