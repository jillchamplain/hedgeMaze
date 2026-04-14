using UnityEngine;

public class ShearsCuttingEffects : MonoBehaviour
{
    [SerializeField] Shears shearsRef;
    public void OnChop()
    {
        shearsRef.CutEffects();
    }
}
