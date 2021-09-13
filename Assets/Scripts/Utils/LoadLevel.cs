using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadLevel : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadPart1()
    {
        SceneManager.LoadScene("Scenes/Main", LoadSceneMode.Single);
    }

    public void LoadPart2()
    {
        SceneManager.LoadScene("Scenes/main_part2", LoadSceneMode.Single);
    }
}
