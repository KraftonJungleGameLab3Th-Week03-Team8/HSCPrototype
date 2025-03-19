using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // 따라갈 플레이어
    public float smoothSpeed = 5f; // 따라가는 속도
    public Vector3 offset = new Vector3(0f, 0f, -10f); // 카메라 위치 조정

    private void LateUpdate()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}
