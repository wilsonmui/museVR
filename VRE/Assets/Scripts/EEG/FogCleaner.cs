using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FogCleaner : MonoBehaviour
{
	public Material fogMaterial;
	public GameObject fogCavas;
	public float cleanMat = 5;
	public float cleanUi = 5;
    // Start is called before the first frame update
    void Start()
    {
		fogMaterial.SetFloat("_Density", 500);
		fogCavas.GetComponent<Image>().color = new Color32(54, 148, 255, 200);
	}

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.PageDown))
		{
			float currentDensity = fogMaterial.GetFloat("_Density");
			if (currentDensity > cleanMat)
			{
				fogMaterial.SetFloat("_Density", currentDensity - cleanMat);
			}

			Color32 uiColor = fogCavas.GetComponent<Image>().color;
			if (uiColor.a > cleanUi)
			{
				uiColor.a = Convert.ToByte(uiColor.a - cleanUi);
				fogCavas.GetComponent<Image>().color = uiColor;
			}
		} else if(Input.GetKeyDown(KeyCode.PageUp))
		{
			float currentDensity = fogMaterial.GetFloat("_Density");
			fogMaterial.SetFloat("_Density", currentDensity + cleanMat);

			Color32 uiColor = fogCavas.GetComponent<Image>().color;
			uiColor.a = Convert.ToByte(uiColor.a + cleanUi);
			fogCavas.GetComponent<Image>().color = uiColor;
		}
	}
}
