using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeClusterSelection : MonoBehaviour {

	Vector3 oriObjScale;
	public Material oriObjMat;
	public Material selectedMat;
	public GameObject tooltipPrefab;
//	public MeshGen mg;
	InputHandler ih;

	public void GazeEnter() {
		Debug.Log("[hehe]Gaze entered on " + gameObject.name);
//		GetComponent<Renderer>().material.color = Color.red;
		gameObject.transform.localScale = oriObjScale * 1.1f;
		gameObject.GetComponent<Renderer> ().material = selectedMat;
		Transform tooltiptf = gameObject.transform.Find ("tooltip");
		if (tooltiptf == null) {
			GameObject tooltip = GameObject.Instantiate (tooltipPrefab, gameObject.transform);
			tooltip.name = "tooltip";
			tooltiptf = tooltip.transform;
		}
		tooltiptf.gameObject.SetActive (true);
		tooltiptf.forward = Camera.main.transform.forward;
		tooltiptf.localPosition = new Vector3 (0.5f, 0.5f, 0);
		int clusterIdx = int.Parse(gameObject.name.Substring(7));
		tooltiptf.GetComponent<TextMesh> ().text = ih.clusters [clusterIdx].keywords [0].kw;
	}

	public void GazeExit() {
		Debug.Log("[hehe]Gaze exit from " + gameObject.name);
//		GetComponent<Renderer>().material.color = Color.white;
		gameObject.transform.localScale = oriObjScale;
		gameObject.GetComponent<Renderer> ().material = oriObjMat;
		gameObject.transform.Find("tooltip").gameObject.SetActive(false);
	}

	// Use this for initialization
	void Start () {
		ih = GameObject.Find ("MeshGen").GetComponent<MeshGen>().ih;
		oriObjScale = new Vector3 (0.1f, 0.1f, 0.1f);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
