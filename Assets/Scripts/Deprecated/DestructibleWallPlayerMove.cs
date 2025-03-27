using UnityEngine;

public class DestructibleWallPlayerMove : MonoBehaviour
{
    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 3f);
    }
}