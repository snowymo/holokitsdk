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
		// which does not work because too many objects
//		for (int i = 0; i < 10000; i++) {
//			GameObject.Instantiate (prefab, new Vector3 (Random.Range (0, 10), Random.Range (0, 10), Random.Range (0, 10)),Quaternion.identity);
//		}
//		mm = new MyMesh();
//		// 65000 vertices at most
//		for (int i = 0; i < 4000; i++) {
//			mm.addOneObj (pos1);
//			mm.addOneObj (pos2);
//		}

		ih = new InputHandler ();
		ih.Initialize ();
		if (appear_option == Appearance.ShowPoints) {
			generateMeshForEachClusterWithPoints ();
		} else if (appear_option == Appearance.ShowClusters) {
//			generateMeshForEachCluster ();
			generateSphereForEachCluster();
		}
		isAnimationStart = false;

	}



	void generateMeshForEachClusterWithPoints ()
	{
		for (int i = 0; i < ih.clusters.Length; i++) {
			ih.clusters [i].go = GameObject.Instantiate (clusterUnit);
			ih.clusters [i].go.name = "cluster" + i.ToString ();
			for (int j = 0; j < ih.clusters [i].indicesOfPoints.Count; j++) {
//				print ("clusters[i].indicesOfPoints:" + j + "point:" + ih.points [ih.clusters [i].indicesOfPoints [j]]);
				ih.clusters [i].mm.addOneObj (ih.points [ih.clusters [i].indicesOfPoints [j]].pos);
			}
			Mesh mesh = ih.clusters [i].go.GetComponent<MeshFilter> ().mesh;
			mesh.vertices = ih.clusters [i].mm.vertices;
			mesh.uv = ih.clusters [i].mm.uvs;
			mesh.triangles = ih.clusters [i].mm.triangles;
			mesh.normals = ih.clusters [i].mm.norms;
			mesh.RecalculateNormals ();
		}
	}

	void generateMeshForEachCluster ()
	{
		for (int i = 0; i < ih.clusters.Length; i++) {
			ih.clusters [i].go = GameObject.Instantiate (clusterUnit);
			ih.clusters [i].go.name = "cluster" + i.ToString ();
//			for (int j = 0; j < ih.clusters [i].indicesOfPoints.Count; j++) {
//				print ("clusters[i].indicesOfPoints:" + j + "point:" + ih.points [ih.clusters [i].indicesOfPoints [j]]);
				ih.clusters [i].mm.addOneObj (ih.clusters[i].pos);
//			}
			Mesh mesh = ih.clusters [i].go.GetComponent<MeshFilter> ().mesh;
			mesh.vertices = ih.clusters [i].mm.vertices;
			mesh.uv = ih.clusters [i].mm.uvs;
			mesh.triangles = ih.clusters [i].mm.triangles;
			mesh.normals = ih.clusters [i].mm.norms;
			mesh.RecalculateNormals ();
		}
	}


	void generateSphereForEachCluster ()
	{
		for (int i = 0; i < ih.clusters.Length; i++) {
			ih.clusters [i].go = GameObject.Instantiate (clusterUnit, ih.clusters[i].pos * resizeScale, Quaternion.identity);
			ih.clusters [i].go.transform.localScale = new Vector3 (resizeScale, resizeScale, resizeScale);
			ih.clusters [i].go.name = "cluster" + i.ToString ();
//			Vector3 rgb = generateColor (ih.clusters [i].color_score);
//			print ("cluster" + i.ToString () + " " + ih.clusters [i].color_score + ":" + rgb.ToString ());
//			Material mat = new Material (ih.clusters [i].go.GetComponent<Renderer> ().material);
//			mat.color = new Color (rgb [0], rgb [1], rgb [2], 0.5f);
			ih.clusters [i].go.GetComponent<Renderer> ().material.color = new Color (ih.clusters [i].color [0], ih.clusters [i].color [1], ih.clusters [i].color [2], 0.5f);
			GameObject tooltip = GameObject.Instantiate (tooltipPrefab, ih.clusters [i].go.transform);
			tooltip.name = "clusterName";
			tooltip.transform.forward = Camera.main.transform.forward;
			tooltip.transform.localPosition = new Vector3 (0, 0, -0.5f);
			tooltip.GetComponent<TextMesh> ().text = ih.clusters [i].go.name;
			//			for (int j = 0; j < ih.clusters [i].indicesOfPoints.Count; j++) {
			//				print ("clusters[i].indicesOfPoints:" + j + "point:" + ih.points [ih.clusters [i].indicesOfPoints [j]]);
//			ih.clusters [i].mm.addOneObj (ih.clusters[i].pos);
			//			}
//			Mesh mesh = ih.clusters [i].go.GetComponent<MeshFilter> ().mesh;
//			mesh.vertices = ih.clusters [i].mm.vertices;
//			mesh.uv = ih.clusters [i].mm.uvs;
//			mesh.triangles = ih.clusters [i].mm.triangles;
//			mesh.normals = ih.clusters [i].mm.norms;
//			mesh.RecalculateNormals ();
		}
	}

	int slickIndex = 0;
	float t4animation = 0;
	void animatePointCloud ()
	{
		if (isAnimationStart) {
			// let us do one cluster every frame
			if (slickIndex == 0) {
				accumulateTime += Time.time - lastFrameTime;
				t4animation = accumulateTime / duration;
			}
//			t4animation += 0.05f;
			print ("animated: slickIndex\t" + slickIndex + " t:" + t4animation);
			animateEachSlice (slickIndex * 100, (slickIndex + 1) * 100, t4animation);
			++slickIndex;
			if (slickIndex >= (InputHandler.CLUSTER_SIZE / 100 + 1))
				slickIndex -= (InputHandler.CLUSTER_SIZE / 100 + 1);
		}
	}

	void animateEachSlice (int startIdx, int endIdx, float t)
	{
		for (int i = startIdx; i < Mathf.Min( endIdx,ih.clusters.Length); i++) {
			for (int j = 0; j < ih.clusters [i].indicesOfPoints.Count; j++) {
				Vector3 updatedPos = Vector3.Lerp (ih.points [ih.clusters [i].indicesOfPoints [j]].pos,
					                     ih.clusters [i].pos,
					                     Mathf.SmoothStep (0.0f, 1.0f, t));
				ih.clusters [i].mm.updateOneObj (updatedPos, j);
				if (t > 1.5f)
					isAnimationStart = false;
			}
			Mesh mesh = ih.clusters [i].go.GetComponent<MeshFilter> ().mesh;
			mesh.vertices = ih.clusters [i].mm.vertices;
			//			mesh.uv = ih.clusters [i].mm.uvs;
			//			mesh.triangles = ih.clusters [i].mm.triangles;
			//			mesh.normals = ih.clusters [i].mm.norms;
		}
	}

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
		if (Input.GetKey (KeyCode.M)) {
//			startTime = Time.time;
			isAnimationStart = true;
		}
		animatePointCloud ();
		lastFrameTime = Time.time;

//		detectPressedKeyOrButton ();
//		 = GetComponent<MeshFilter>().mesh;
//		mesh.vertices = mm.vertices;
//		mesh.uv = mm.uvs;
//		mesh.triangles = mm.triangles;
//		mesh.normals = mm.norms;
	}
}
