using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadLevelScene : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D target)
    {
        Debug.Log("oopsie");

        if (target.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
