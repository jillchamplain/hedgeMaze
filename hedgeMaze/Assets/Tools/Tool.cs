using UnityEngine;

public abstract class Tool : MonoBehaviour
{
    [SerializeField] public string name;
    public bool isEquipped = false;

    public abstract void Equip();

    public abstract void UnEquip();

    public abstract void Use(GameObject hitObject);
}
