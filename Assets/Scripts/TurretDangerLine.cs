using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Turret Danger���� ��ũ��Ʈ
// Danger������ lerpTime ���� ���Դٰ� ������ ũ�Ⱑ �پ��鼭 �����
public class TurretDangerLine : MonoBehaviour
{
    [SerializeField]
    private float lerpTime; // ���� �ð�
    [SerializeField]
    private SpriteRenderer dangerLinespriteRenderer;
     
     
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartDangerLine());
    }

    private IEnumerator StartDangerLine()
    {
        float currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;
            // ���� ���� 
            float alpha = Mathf.Lerp(1f, 0f, currentTime / lerpTime);
            float scaleY = Mathf.Lerp(0.32f, 0, currentTime / lerpTime);
            dangerLinespriteRenderer.color = new Color(dangerLinespriteRenderer.color.r, dangerLinespriteRenderer.color.g, dangerLinespriteRenderer.color.b, alpha);
            transform.localScale = new Vector3(transform.localScale.x, scaleY, 1);
            yield return null;
        }
        Destroy(gameObject);
    }
}
