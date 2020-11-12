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
        //Shader.SetGlobalVector("_SunDirection", MainLight.forward);

    }
    // Update is called once per frame
    public void setSceneIndex(int index)
    {
        sceneIndexToLoad = index;
    }

    public void LoadSceneByBuildIndex()
    {
        SceneManager.LoadScene(sceneIndexToLoad, LoadSceneMode.Single);
    }
}
