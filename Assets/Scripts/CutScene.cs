using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutScene : MonoBehaviour
{
    private PlayableDirector cutsceneDirector;
    
    // Start is called before the first frame update
    void Start()
    {
        cutsceneDirector = GetComponent<PlayableDirector>();
        cutsceneDirector.gameObject.SetActive(false);
    }

    public void PlayCutScene(float delay)
    {
        cutsceneDirector.gameObject.SetActive(true);
       // StartCoroutine(CutSceneEnd(delay));
        
    }
    private IEnumerator CutSceneEnd(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
    }
}
