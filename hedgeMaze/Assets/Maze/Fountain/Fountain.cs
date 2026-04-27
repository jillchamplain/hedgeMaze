using System.Collections;
using Ubisoft.Systems.Audio;
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
    [SerializeField] SoundStreamSO waterDrop;
    bool isPumping = false;



    Coroutine pumpStopCoroutine;

    public void Pump()
    {
        if (!wateringCan)
            return;


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
                if (isPumping == false)
                {
                    Debug.Log("WHYYYY");
                    isPumping = true;
                    AudioManager.instance.PlayAudio(new AudioRequest(waterDrop).SetPoint(transform.position));
                    particleSystem.Play();
                }
                if (wateringCan)
                    wateringCan.Refill(this);
            }


            visuals.amount += change;
        }
        if (MousePos <= 0)
        {
            float change = Mathf.Clamp(MousePos / sensitivity, 0, maxDownSpeed);
            visuals.amount += MousePos / sensitivity;
            particleSystem.Stop();
            isPumping = false;
        }
    }

    private void Update()
    {
        if (!isPumping)
        {
            particleSystem.Stop();
        }
    }

    IEnumerator WaitForPumpStop()
    {
        yield return null; 
        isPumping = false;
        CameraController.instance.canLook = true;
    }

}
