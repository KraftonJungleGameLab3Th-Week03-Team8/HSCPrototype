using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // ���� �÷��̾�
    public float smoothSpeed = 5f; // ���󰡴� �ӵ�
    public Vector3 offset = new Vector3(0f, 0f, -10f); // ī�޶� ��ġ ����

    private void LateUpdate()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}
