using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class StandardSceneController : MonoBehaviour
{
    public int sceneIndexToLoad;
    public Transform MainLight;
    // Start is called before the first frame
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalVector("_SunDirection", MainLight.forward);

    }

    public void setSceneIndex(int index)
    {
        sceneIndexToLoad = index;
    }

    public void LoadSceneByBuildIndex()
    {
        SceneManager.LoadScene(sceneIndexToLoad, LoadSceneMode.Single);
    }
}
