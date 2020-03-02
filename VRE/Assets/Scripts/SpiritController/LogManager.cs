using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class LogManager : MonoBehaviour
{
	private float LOG_CYCLE = .5f;
	private float PIC_CYCLE = .5f;
	private float logTimer;
	private float pictureTimer;
	OscSimpl.Examples.EegReceiver eegReceiver;
	SceneController sceneController;
	StreamWriter eegLogFile;
	StreamWriter eventLogFile;
	string logDataPath = "logdata\\";
	const string EEG_LOG_FILE = "eeg_log_file";
	const string EVENT_LOG_FILE = "event_log_file";
	// Start is called before the first frame update


	void Start()
	{
		logTimer = 0;
		pictureTimer = 0;
		eegReceiver = GetComponent<OscSimpl.Examples.EegReceiver>();
		sceneController = GetComponent<SceneController>();
		

	}

	// Update is called once per frame
	void Update()
	{
		string time = System.DateTime.Now.ToString("MM_dd_HH_mm_ss_ff");
		Screenshot(time);
		LogEegData();

	}

	void Screenshot(string time)
	{
		pictureTimer += Time.deltaTime;
		if (pictureTimer >= PIC_CYCLE)
		{
			pictureTimer = 0;
			ScreenCapture.CaptureScreenshot("screenshots\\" + time + ".png");
		}
	}

	void CreateLogFiles()
	{
		if (!File.Exists(logDataPath + EVENT_LOG_FILE + System.DateTime.Now.ToString("MMdd_HH_mm_ss") + ".txt"))
		{
			eventLogFile = File.CreateText(logDataPath + EVENT_LOG_FILE + System.DateTime.Now.ToString("MM_dd_HH_mm_ss") + ".txt");
		}
		if (!File.Exists(logDataPath + EEG_LOG_FILE + System.DateTime.Now.ToString("MMdd_HH_mm_ss") + ".txt"))
		{
			eegLogFile = File.CreateText(logDataPath + EEG_LOG_FILE + System.DateTime.Now.ToString("MM_dd_HH_mm_ss") + ".txt");
		}
	}

	void LogEegData()
	{

	}

	public void LogEventData(string msg)
	{
		eventLogFile.WriteLine(msg + "," + System.DateTime.Now.ToString());
	}
}
