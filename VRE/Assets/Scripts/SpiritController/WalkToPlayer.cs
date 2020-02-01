using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkToPlayer : MonoBehaviour
{
	private bool m_IsComplete;
	private Animator m_Animator;
	private float m_Timer;
	private float m_StartWalkTime;
	private float m_AdjustTime;
	private float m_StopWalkTime;
	private bool m_IsInAir;
	private Vector3 m_InitRot;
	public GameObject m_Player;
	private float m_RotateStr = 1f;
	// Start is called before the first frame update
	void Start()
	{
		m_IsComplete = false;
		m_Timer = 0;
		m_IsInAir = true;
		m_StartWalkTime = 4;
		m_AdjustTime = 0.25f;
		m_StopWalkTime = m_StartWalkTime + m_AdjustTime + 3.5f;
		m_Animator = gameObject.GetComponent<Animator>();
	}
	// Update is called once per frame
	void Update()
    {
		if(m_IsInAir == true)
			gameObject.transform.position -= new Vector3(0, 0.003f, 0);
		if (m_IsComplete == true) return;

		
		m_Timer += Time.deltaTime;
		Debug.Log("Timer:" + m_Timer.ToString());
		if (m_Timer >= m_StartWalkTime && m_Timer <= m_StopWalkTime - m_AdjustTime)
		{
			float str = Mathf.Min(m_RotateStr * Time.deltaTime, 1);
			Quaternion targetRotation = Quaternion.LookRotation(m_Player.transform.position - transform.position);
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);
			m_Animator.SetBool("inWalk", true);
		}
		else if (m_Timer >= m_StopWalkTime - m_AdjustTime && m_Timer <= m_StopWalkTime)
		{
			Quaternion targetRotation = Quaternion.LookRotation(m_Player.transform.position - transform.position);
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.5f);
		}
		else if (m_Timer >= m_StopWalkTime)
		{
			m_Animator.SetBool("inWalk", false);
			m_IsComplete = true;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		Debug.Log("Enter");
		m_IsInAir = false;
	}

	private void OnCollisionExit(Collision collision)
	{
		Debug.Log("Exit");
		m_IsInAir = true;
	}
}
