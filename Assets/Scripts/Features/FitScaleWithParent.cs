using UnityEngine;

public class FitScaleWithParent : MonoBehaviour
{
    [SerializeField] private Transform parent;
    
    private void Start()
    {
        FitScale();
    }

    public void FitScale()
    {
        Vector3 localScale = new Vector3(
            1f / parent.localScale.x, 
            1f / parent.localScale.y, 
            1f / parent.localScale.z);
        
        transform.localScale = localScale;
    }
}