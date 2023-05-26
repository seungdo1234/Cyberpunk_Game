using UnityEngine;

public class TeleportCircle : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        // ȸ�� ���� ������Ʈ
        if (Input.GetKey(KeyCode.LeftShift)) // ���� ����Ʈ Ű�� ������ ��
        {
            // ����Ʈ�� ������ TimeScale�� 0�̱� ������ DeltaTime���� �Ѵٸ� ���ư������� -> Time.unscaledDeltaTime���� �ذ�
            transform.Rotate(Vector3.forward * rotationSpeed * Time.unscaledDeltaTime);
        }
    }
}
