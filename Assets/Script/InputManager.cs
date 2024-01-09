using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private new Camera camera;

    /// <summary>
    /// Which layer takes input of the mouse position on the plane
    /// </summary>
    [SerializeField]
    private LayerMask placementLayerMask;
    [SerializeField]
    private LayerMask robotLayerMask;
    [SerializeField]
    private LayerMask moveableLayerMask;

    private float MAX_MOUSE_DISTANCE = 100f;

    private Vector3 lastPosition;

    public Vector3 GetSelectedMapPosition() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = camera.nearClipPlane;
        Ray ray = camera.ScreenPointToRay(mousePos);

        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, MAX_MOUSE_DISTANCE, placementLayerMask))
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
        
        if (Physics.Raycast(ray, out hit, MAX_MOUSE_DISTANCE, robotLayerMask))
        {
            return hit.collider.transform.gameObject;
        }

        return null;
    }

    public bool IsMoveable()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = camera.nearClipPlane;
        Ray ray = camera.ScreenPointToRay(mousePos);

        return Physics.Raycast(ray, MAX_MOUSE_DISTANCE, moveableLayerMask);
    }

    public bool IsRobot()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, MAX_MOUSE_DISTANCE, robotLayerMask);
    }
}
