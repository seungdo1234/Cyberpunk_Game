using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Scroller : MonoBehaviour
{
    private MeshRenderer render;
    private float offset;
    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
       // offset += Time.deltaTime * speed;
        // 배경 material offset 변경
       // render.material.mainTextureOffset = new Vector2(offset, 0);
    }
    public void BG_Scroll(float moveDirection)
    {
        offset += Time.deltaTime * moveDirection;
        render.material.mainTextureOffset = new Vector2(offset, 0);
    }

}
