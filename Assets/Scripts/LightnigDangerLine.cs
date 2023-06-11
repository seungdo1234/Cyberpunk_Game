using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightnigDangerLine : MonoBehaviour
{
    [SerializeField]
    private float lerpTime; // 지속 시간
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Alpha());
    }
    private IEnumerator Alpha()
    {
        float currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;
            // 선형 보간 
            float alpha = Mathf.Lerp(1f, 0f, currentTime / lerpTime);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            yield return null;
        }
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
