using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Crystal : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private float coolTime; // 지속 시간
    [SerializeField]
    private GameObject crystalCoolTimeImage;
    [SerializeField]
    private Transform canvasTransform; // UI를 표현하는 Canvas 오브젝트의 Transform

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private IEnumerator CrystalCoolTime()
    {
        yield return new WaitForSeconds(coolTime);
        gameObject.layer = 13;
        spriteRenderer.color = new Color(255, 255, 255);
    }
    private void SpawnCoolTimeImage()
    {
        GameObject imageClone = Instantiate(crystalCoolTimeImage);
        // Slider UI 오브젝트를 parent("Canvas" 오브젝트)의 자식으로 설정
        // Tip. UI는 캔버스의 자식오브젝트로 설정되어 있어야 화면에 보인다.
        imageClone.transform.SetParent(canvasTransform);
        // 계층 설정으로 바뀐 크기를 다시 (1, 1, 1)로 설정
        imageClone.transform.localScale = Vector3.one;

        // Slider UI가 쫒아다닐 대상을 본인으로 설정
        imageClone.GetComponent<CrystalCoolTime>().SetUp(transform, coolTime);

    }
    public void CrystalUse()
    {
        gameObject.layer = 14;
        spriteRenderer.color = new Color(.7f, .7f, .7f);
        SpawnCoolTimeImage();
        StartCoroutine(CrystalCoolTime());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
