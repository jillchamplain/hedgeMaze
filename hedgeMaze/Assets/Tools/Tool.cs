using UnityEngine;

public abstract class Tool : MonoBehaviour
{
    [SerializeField] public string toolName;
    [SerializeField] public GameObject model;
    [SerializeField] public Vector3 toolHoldTransformOffset;
    public bool isEquipped = false;

    public abstract void AddedToInventory();

    public abstract void RemovedFromInventory();

    public abstract void Equip();

    public abstract void UnEquip();

    public abstract void Use(GameObject hitObject);

    public abstract void StopUse();
}
