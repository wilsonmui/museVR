using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SceneController : MonoBehaviour
{
	public Material fogMaterial;
	public GameObject fogHole;
	const float INTRO_FADE_TIME_LENGTH = 3f;
	float introFadeTimer;
	bool fadingIntro;
	const float MAX_CLEAR_FOG_MAT_SCALE = 10;
	const float CLEAR_FOG_HOLE_SCALE = 1.05f;
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
		fogHole.transform.localPosition = new Vector3(0.603f, -1.85f, -0.334f);
		fogHole.transform.localScale = new Vector3(100, 100, 100);
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

		fogHole.transform.localScale = new Vector3(fogHole.transform.localScale.x * CLEAR_FOG_HOLE_SCALE,
			fogHole.transform.localScale.y * CLEAR_FOG_HOLE_SCALE,
			fogHole.transform.localScale.z);

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
			fogHole.transform.localPosition = new Vector3(0.603f, -1.85f, -0.334f);
			fogHole.transform.localScale = new Vector3(100, 100, 100);
			clearFogCounter = 0;
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
