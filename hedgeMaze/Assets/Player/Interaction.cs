using UnityEngine;

public class Interaction : MonoBehaviour
{
    public static Interaction instance;
    [SerializeField] LayerMask interactionMask;
    [SerializeField] float interactionRange;

    public bool IsInteractableInRange {  get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy (gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        LookRaycast();
        if (Input.GetMouseButtonDown(0))
        {
            CheckRaycast();
        }
    }

    void CheckRaycast()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactionRange, interactionMask))
        {
            var selection = hit.transform;
            Debug.Log(selection.parent.parent.parent.name);
        }

    }

    void LookRaycast()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        IsInteractableInRange = Physics.Raycast(ray, out hit, interactionRange, interactionMask);
    }
}
