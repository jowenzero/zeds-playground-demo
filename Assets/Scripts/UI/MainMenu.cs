using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject charaSelect;
    public Animator menuAnimations;
    public Animator charaSelectAnimations;
    public void CharacterSelect()
    {
        menuAnimations.SetBool("Next", true);
        charaSelectAnimations.SetBool("Back", false);

        if (charaSelect.activeSelf == false)
        {
            charaSelect.SetActive(true);
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
}
