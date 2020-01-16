using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
	public float timer;
	public string playerName;
	public bool trialStart;
	public Text timerText;
	public Text playerText;

    // Start is called before the first frame update
    void Start()
    {
		timer = 0f;
		timerText.text = "";
		playerText.text = playerName;

		trialStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (trialStart)
		{
			timer += Time.deltaTime;
			timerText.text = timer.ToString("0.0" + "s");
		}
    }
}
