using UnityEngine;

public class Particle : MonoBehaviour
{
    [SerializeField] float lifeTime;
    void Start()
    {
        if(GetComponent<ParticleSystem>())
        {
            lifeTime = GetComponent<ParticleSystem>().main.duration;
        }

        Destroy(gameObject, lifeTime);
    }

    public void Play()
    {
        if(GetComponent <ParticleSystem>())
        {
            GetComponent<ParticleSystem>().Play();
        }
    }

}
