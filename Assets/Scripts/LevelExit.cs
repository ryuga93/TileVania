using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float loadWaitTime = 1f;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
        {
            StartCoroutine(LoadNextScene());
        }
    }

    IEnumerator LoadNextScene()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        yield return new WaitForSeconds(loadWaitTime);
        
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }
}
