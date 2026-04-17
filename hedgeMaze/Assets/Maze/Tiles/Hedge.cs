using UnityEngine;

public class Hedge : MonoBehaviour
{
    [SerializeField] MeshRenderer outerMesh;
    [SerializeField] Node myNode;
    MaterialPropertyBlock block;
    float startDissolve;
    void Start()
    {
        block = new MaterialPropertyBlock();

        outerMesh.GetPropertyBlock(block);

        block.SetVector("_ObjectPosition", new Vector2(transform.position.x, transform.position.z));
        startDissolve = outerMesh.material.GetFloat("_DissolveAmount");

        outerMesh.SetPropertyBlock(block);

    }

    private void OnEnable()
    {
        myNode.onCut += Dissolve;
    }

    private void OnDisable()
    {
        myNode.onCut -= Dissolve;
    }

    void Dissolve()
    {
        outerMesh.GetPropertyBlock(block);
        float dissolve = Mathf.Lerp(0.18f, startDissolve, (float)myNode.CutsRemaining / (float)myNode.cutsNeeded);
        block.SetFloat("_DissolveAmount", dissolve);

        outerMesh.SetPropertyBlock(block);

    }
}
