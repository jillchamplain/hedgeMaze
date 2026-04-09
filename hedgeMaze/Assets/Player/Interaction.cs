using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] LayerMask interactionMask;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LookRaycast();
        }
    }

    void LookRaycast()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast( ray, out hit, 100f, interactionMask))
        {
            var selection = hit.transform;
            Debug.Log(interactionMask);
            Debug.Log(selection.gameObject);
        }
    }
}
