using UnityEngine;

public class TutorialPrompt : MonoBehaviour
{
    [SerializeField] string tutorialTitle;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            TutorialManager.instance.TutorialDisplay(tutorialTitle);
        }
    }
}
