using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDropTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject boxPrefab; // �ڽ�
    [SerializeField]
    private GameObject dangerLine; // Box ���� ǥ��
    [SerializeField]
    private Transform[] boxLayerPoints; // �ڽ� ���� ���̾� ����Ʈ
    [SerializeField]
    private Transform[] dangerLineLayerPoints; // �ڽ� ���� ���̾� ����Ʈ

    private int layerNum = 0;
    private bool isTrigger = false;
    private bool boxDropReady = false; // �ڽ� ��� �غ� 
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BoxDrop());
    }

    private IEnumerator BoxDropDelay()
    {
        yield return new WaitForSeconds(3f);
        boxDropReady = false;
    }
    private IEnumerator BoxDrop()
    {

        while (true)
        {
            GameObject dangerLineClone = Instantiate(dangerLine, dangerLineLayerPoints[layerNum].position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
            Destroy(dangerLineClone);
            GameObject boxClone = Instantiate(boxPrefab, boxLayerPoints[layerNum].position, Quaternion.identity);
            layerNum = 1 - layerNum;
            yield return new WaitForSeconds(3f);
        }
    }

}
