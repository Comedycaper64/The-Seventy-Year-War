using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld instance;

    //Allows raycast interactions for gameobjects on the specified layermask
    [SerializeField]
    private LayerMask mousePlaneLayerMask;

    private void Awake()
    {
        instance = this;
    }

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
        if (
            Physics.Raycast(
                ray,
                out RaycastHit raycastHit,
                float.MaxValue,
                instance.mousePlaneLayerMask
            )
        )
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.negativeInfinity;
        }
    }
}
