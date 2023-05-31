using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Turret Danger라인 스크립트
// Danger라인이 lerpTime 동안 나왔다가 서서히 크기가 줄어들면서 사라짐
public class TurretDangerLine : MonoBehaviour
{
    [SerializeField]
    private float lerpTime; // 지속 시간
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
            // 선형 보간 
            float alpha = Mathf.Lerp(1f, 0f, currentTime / lerpTime);
            float scaleY = Mathf.Lerp(0.32f, 0, currentTime / lerpTime);
            dangerLinespriteRenderer.color = new Color(dangerLinespriteRenderer.color.r, dangerLinespriteRenderer.color.g, dangerLinespriteRenderer.color.b, alpha);
            transform.localScale = new Vector3(transform.localScale.x, scaleY, 1);
            yield return null;
        }
        Destroy(gameObject);
    }
}
