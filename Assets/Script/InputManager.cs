using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera camera;

    /// <summary>
    /// Which layer takes input of the mouse position on the plane
    /// </summary>
    [SerializeField]
    private LayerMask placementLayerMask;
    [SerializeField]
    private LayerMask robotLayerMask;

    private Vector3 lastPosition;
    
    public Vector3 GetSelectedMapPosition() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = camera.nearClipPlane;
        Ray ray = camera.ScreenPointToRay(mousePos);

        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }

    public GameObject GetSelectedRobotIndicator()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = camera.nearClipPlane;
        Ray ray = camera.ScreenPointToRay(mousePos);

        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 100, robotLayerMask))
        {
            return hit.collider.transform.gameObject;
        }

        return null;
    }
}
