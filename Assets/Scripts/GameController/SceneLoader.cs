using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void SceneLoaderFunction(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }

}
