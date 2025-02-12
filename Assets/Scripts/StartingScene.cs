using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingScene : MonoBehaviour
{
    [SerializeField] private GameObject Loading_Go;


    public void Load_EndlessMode()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void Load_LevelMode()
    {
        SceneManager.LoadSceneAsync(2);
    }
}
