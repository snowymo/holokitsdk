using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeshGen : MonoBehaviour
{

	//	public GameObject prefab;

	//	public Vector3 pos1;
	//	public Vector3 pos2;
	//
	public GameObject clusterUnit;

	public GameObject tooltipPrefab;

	public float resizeScale;

	//	MyMesh mm;
	public InputHandler ih;

	float duration = 3.0F;
//	float startTime;

	bool isAnimationStart;

	const float MIN_DIS = 0.1f;

	float accumulateTime = 0f;
	float lastFrameTime = 0f;

	public enum Appearance
	{
ShowPoints,
		ShowClusters}

	;

	public Appearance appear_option;
		

	// Use this for initialization
	void Awake ()
	{
		ih = new InputHandler ();
		ih.Initialize ();
		if (appear_option == Appearance.ShowClusters) {
			generateSphereForEachCluster();
		}
		isAnimationStart = false;

	}

	public void RefreshModels() { 
		ih.Initialize ();
		if (appear_option == Appearance.ShowClusters) {
			generateSphereForEachCluster();
		}
	}

	void generateSphereForEachCluster ()
	{
		for (int i = 0; i < ih.clusters.Length; i++) {
			if (ih.clusters[i].go == null)
			{
				ih.clusters[i].go = Instantiate(clusterUnit, ih.clusters[i].pos * resizeScale, Quaternion.identity);
				ih.clusters[i].go.transform.localScale = new Vector3(resizeScale, resizeScale, resizeScale);
				ih.clusters[i].go.name = "cluster" + i.ToString();

				// for debug usage: cluster name
				//GameObject tooltip = Instantiate(tooltipPrefab, ih.clusters[i].go.transform);
				//tooltip.name = "clusterName";
				//tooltip.transform.forward = Camera.main.transform.forward;
				//tooltip.transform.localPosition = new Vector3(0, 0, -0.5f);
				//tooltip.GetComponent<TextMesh>().text = ih.clusters[i].go.name;
			}

			ih.clusters [i].go.GetComponent<Renderer> ().material.color = new Color(ih.clusters[i].color[0], ih.clusters[i].color[1], ih.clusters[i].color[2], 1.0f);
		}
	}

	int slickIndex = 0;
	float t4animation = 0;

	public void detectPressedKeyOrButton()
	{
		foreach(KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
		{
			if (Input.GetKeyDown(kcode))
				Debug.Log("KeyCode down: " + kcode);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		detectPressedKeyOrButton ();
	}
}
