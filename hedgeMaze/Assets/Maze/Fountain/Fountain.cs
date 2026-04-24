using UnityEngine;

public class Fountain : MonoBehaviour
{
    [SerializeField] public float refillAmount;
    [SerializeField] public GameObject wateringCanHolder;
    bool isPumping = false;
}
