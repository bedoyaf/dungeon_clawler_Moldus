using UnityEngine;

public class FlashLightController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Camera mainCamera; 
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float angleOffset = 90f;
    private void Start()
    {
    }

    void Update()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - player.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - angleOffset;
        float smoothedAngle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, Time.deltaTime * rotationSpeed);
        transform.rotation = Quaternion.Euler(0, 0, smoothedAngle);
    }
}
