using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SceneController : MonoBehaviour
{
	public Material fogMaterial;
	public GameObject fogCavas;
	const float INTRO_FADE_TIME_LENGTH = 3f;
	float introFadeTimer;
	bool fadingIntro;
	const float MAX_CLEAR_FOG_MAT_SCALE = 10;
	const float CLEAR_FOG_UI_SCALE = 5;
	const int CLEAR_FOG_MAX_COUNTER = 99;
	int clearFogCounter = 0;
	public GameObject introMusic;
	public AudioSource introAudio;

	public GameObject birdMusic;
	public List<AudioSource> birdAudioList;
	const float BIRD_FADE_OUT = .5f;
    // Start is called before the first frame update
    void Start()
    {
		introAudio = introMusic.GetComponent<AudioSource>();
		SetUpBirdAudio();
		introFadeTimer = 0;
		fadingIntro = false;
		fogMaterial.SetFloat("_Density", 500);
		fogCavas.GetComponent<Image>().color = new Color32(54, 148, 255, 200);
	}

    // Update is called once per frame
    void Update()
    {
		keyControl();

		if (fadingIntro == true)
		{
			FadeOutIntro();
		}
	}

	public bool ClearFog()
	{
		if (clearFogCounter == CLEAR_FOG_MAX_COUNTER) return true; ;
		clearFogCounter++;
		Debug.Log(clearFogCounter);
		float currentDensity = fogMaterial.GetFloat("_Density");
		float clearMat = (MAX_CLEAR_FOG_MAT_SCALE - clearFogCounter * .1f);
		if (currentDensity > clearMat)
		{
			fogMaterial.SetFloat("_Density", currentDensity - clearMat);
		}

		Color32 uiColor = fogCavas.GetComponent<Image>().color;
		if (uiColor.a > CLEAR_FOG_UI_SCALE)
		{
			uiColor.a = Convert.ToByte(uiColor.a - CLEAR_FOG_UI_SCALE);
			fogCavas.GetComponent<Image>().color = uiColor;
		}

		return false;
	}

	void keyControl()
	{
		if (Input.GetKeyDown(KeyCode.PageDown))
		{
			ClearFog();
		}
		else if (Input.GetKeyDown(KeyCode.R))
		{
			fogMaterial.SetFloat("_Density", 500);
			fogCavas.GetComponent<Image>().color = new Color32(54, 148, 255, 200);
		}
		
	}
	public void StopIntroMusic()
	{
		fadingIntro = true;
	}

	void FadeOutIntro()
	{
		if(introAudio.volume > Time.deltaTime / INTRO_FADE_TIME_LENGTH)
		{
			introAudio.volume -= Time.deltaTime / INTRO_FADE_TIME_LENGTH;
		}
		else
		{
			introAudio.volume = 0;
			fadingIntro = false;
		}
	}
	public bool MuteBirdTweet()
	{
		bool isEnd = false;
		for(int i = 0; i < birdAudioList.Count; i++)
		{
			if(birdAudioList[i].volume > BIRD_FADE_OUT)
			{
				birdAudioList[i].volume -= BIRD_FADE_OUT;
			}
			else
			{
				birdAudioList[i].volume = 0;
				isEnd = true;
			}
		}
		return isEnd;
	}

	void SetUpBirdAudio()
	{
		birdAudioList = new List<AudioSource>();
		for (int i = 0; i < birdMusic.transform.childCount; i++)
		{
			birdAudioList.Add(birdMusic.transform.GetChild(i).gameObject.GetComponent<AudioSource>());
		}
	}
}
