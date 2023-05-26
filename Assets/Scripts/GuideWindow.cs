using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideWindow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    public void OffWindow()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
    public void GuideOn()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
