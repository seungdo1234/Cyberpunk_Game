using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDropTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject boxPrefab; // 박스
    [SerializeField]
    private GameObject dangerLine; // Box 공격 표시
    [SerializeField]
    private Transform[] boxLayerPoints; // 박스 생성 레이어 포인트
    [SerializeField]
    private Transform[] dangerLineLayerPoints; // 박스 생성 레이어 포인트

    private int layerNum = 0;
    private bool isTrigger = false;
    private bool boxDropReady = false; // 박스 드랍 준비 
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
