using UnityEngine;

public class TeleportCircle : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        // 회전 값을 업데이트
        if (Input.GetKey(KeyCode.LeftShift)) // 왼쪽 쉬프트 키를 눌렀을 때
        {
            // 쉬프트를 누르면 TimeScale이 0이기 때문에 DeltaTime으로 한다면 돌아가지않음 -> Time.unscaledDeltaTime으로 해결
            transform.Rotate(Vector3.forward * rotationSpeed * Time.unscaledDeltaTime);
        }
    }
}
