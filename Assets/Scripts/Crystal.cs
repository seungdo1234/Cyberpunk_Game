using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Crystal : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private float coolTime; // ���� �ð�
    [SerializeField]
    private GameObject crystalCoolTimeImage;
    [SerializeField]
    private Transform canvasTransform; // UI�� ǥ���ϴ� Canvas ������Ʈ�� Transform

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
        // Slider UI ������Ʈ�� parent("Canvas" ������Ʈ)�� �ڽ����� ����
        // Tip. UI�� ĵ������ �ڽĿ�����Ʈ�� �����Ǿ� �־�� ȭ�鿡 ���δ�.
        imageClone.transform.SetParent(canvasTransform);
        // ���� �������� �ٲ� ũ�⸦ �ٽ� (1, 1, 1)�� ����
        imageClone.transform.localScale = Vector3.one;

        // Slider UI�� �i�ƴٴ� ����� �������� ����
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
