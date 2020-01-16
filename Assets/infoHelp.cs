using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infoHelp : MonoBehaviour
{
    public GameObject infoEbot;
    public GameObject infoDD;
    public GameObject infoFD;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            infoEbot.SetActive(false);
            infoDD.SetActive(false);
            infoFD.SetActive(false);
        }
    }
    public void InfoEmilibot()
    {
        infoEbot.SetActive(true);
    }
    public void InfoDummyDrone()
    {
        infoDD.SetActive(true);
    }
    public void InfoFlyingDrone()
    {
        infoFD.SetActive(true);
    }
}
