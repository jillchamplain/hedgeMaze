using UnityEngine;

public class Stalker : MonoBehaviour
{
    [SerializeField] Vector2Int directionToPlayer;
    public Vector2Int GetDirectionToPlayer() {  return directionToPlayer; }
    public void SetDirectionToPlayer(Vector2Int newDirection)
    {
        directionToPlayer = newDirection;

        gameObject.transform.rotation = Quaternion.identity;

        if(directionToPlayer.x == -1)
        {
            gameObject.transform.Rotate(new Vector3(0, -90, 0));
        }
        else if(directionToPlayer.x == 1)
        {
            gameObject.transform.Rotate(new Vector3(0, 90, 0));
        }

        if(directionToPlayer.y == -1)
        {
            gameObject.transform.Rotate(new Vector3(0, -180, 0));
        }
        else if( directionToPlayer.y == 1)
        {
            gameObject.transform.Rotate(new Vector3(0, 180, 0));
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
