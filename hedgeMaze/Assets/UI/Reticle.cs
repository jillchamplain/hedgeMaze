using UnityEngine;
using UnityEngine.UI;

public class Reticle : MonoBehaviour
{
    [SerializeField] Image reticleImage;
    [SerializeField] Sprite interactableReticle;
    [SerializeField] Sprite notInteractableReticle;
    Interaction interactionRef;
    void Start()
    {
        interactionRef = Interaction.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (interactionRef.IsInteractableInRange)
        {
            reticleImage.sprite = interactableReticle;
        }
        else
        {
            reticleImage.sprite = notInteractableReticle;
        }
    }
}
