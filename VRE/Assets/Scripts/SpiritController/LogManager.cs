using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class LogManager : MonoBehaviour
{
	private float LOG_CYCLE = .5f;
	private float PIC_CYCLE = .5f;
	private float m_logTimer;
	private float m_pictureTimer;
	private string m_fileName;
	StreamWriter sr;
	// Start is called before the first frame update


	void Start()
    {
		m_logTimer = 0;
		m_pictureTimer = 0;
		m_fileName = "logdata\\" + System.DateTime.Now.ToString("HHmmss") + ".txt";
		if (File.Exists(m_fileName))
		{
			return;
		}
		sr = File.CreateText(m_fileName);
		
	}

    // Update is called once per frame
    void Update()
    {
		string time = System.DateTime.Now.ToString("HHmmssff");
		m_logTimer += Time.deltaTime;
		Vector3 data = transform.forward;
		if (m_logTimer >= LOG_CYCLE)
		{
			m_logTimer = 0;
			sr.WriteLine(data + time);
			//Debug.Log(data + ", " + time);
		}

		m_pictureTimer += Time.deltaTime;
		if (m_pictureTimer >= PIC_CYCLE)
		{
			m_pictureTimer = 0;
			ScreenCapture.CaptureScreenshot("screenshots\\" + time + ".png");
		}
	}
}
