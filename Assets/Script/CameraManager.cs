using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Transform pov1;
    [SerializeField]
    private Transform pov2;
    [SerializeField]
    private Transform pov3;
    [SerializeField]
    private Transform pov4;

    private Transform lookAt;
    private float MOVE_SPEED = 20f;
    private float YAW_SPEED = 5f;

    private void Start()
    {
        lookAt = pov1;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            lookAt = pov1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            lookAt = pov2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            lookAt = pov3;

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            lookAt = pov4;
        }

        transform.position = Vector3.MoveTowards(transform.position, lookAt.position, MOVE_SPEED * Time.deltaTime);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, lookAt.eulerAngles, YAW_SPEED * Time.deltaTime);
    }
}
