using UnityEngine;

public class FitScaleWithParent : MonoBehaviour
{
    private void Start()
    {
        if(transform.parent is not null)
            Utils.SetChildScaleFitToParent(transform.gameObject, transform.parent.gameObject);
    }
}