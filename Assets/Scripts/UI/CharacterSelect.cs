using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject charaSelect;
    public Animator menuAnimations;
    public Animator charaSelectAnimations;
    public StackFrame stackFrame;

    public void DummyDrone()
    {
        StartCoroutine("DDLoad");
    }
    public void EmiliBot()
    {
        StartCoroutine("EbotLoad");
    }
    public void FlyingDrone()
    {
        StartCoroutine("FDLoad");
    }


    public IEnumerator DDLoad()
    {
        charaSelectAnimations.SetBool("Exit", true);

        yield return new WaitForSeconds(0.3f);

        SceneManager.LoadScene(2);
    }

    public IEnumerator EbotLoad()
    {
        charaSelectAnimations.SetBool("Exit", true);

        yield return new WaitForSeconds(0.3f);

        SceneManager.LoadScene(1);
    }

    public IEnumerator FDLoad()
    {
        charaSelectAnimations.SetBool("Exit", true);

        yield return new WaitForSeconds(0.3f);

        SceneManager.LoadScene(3);
    }

    public void BackToMain()
    {
        menuAnimations.SetBool("Next", false);

        charaSelectAnimations.SetBool("Back", true);
    }
}
