using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeClusterSelection : MonoBehaviour {

//	Vector3 oriObjScale;
	Material oriObjMat;
	public Material selectedMat;
//	public GameObject tooltipPrefab;
//	public GameObject pointUnit;
//	float resizeScale;
//	public MeshGen mg;
	//InputHandler ih;
	public bool isGazeEnter;

	public void GazeEnter() {
		//Debug.Log("[hehe]Gaze entered on " + gameObject.name);
//		GetComponent<Renderer>().material.color = Color.red;
//		gameObject.transform.localScale = oriObjScale * 1.1f;
		gameObject.GetComponent<Renderer> ().material = selectedMat;
		isGazeEnter = true;
//		if (GetComponent<KeyboardCtrl> ().isShow) {
			// show keywords
			
//		}
	}

	public void GazeExit() {
		//Debug.Log("[hehe]Gaze exit from " + gameObject.name);
//		int clusterIdx = int.Parse (gameObject.name.Substring (7));
		isGazeEnter = false;
//		GetComponent<Renderer>().material.color = Color.white;
//		gameObject.transform.localScale = oriObjScale;
		if(GetComponent<SelectDisplay>().isSelect == false)
			gameObject.GetComponent<Renderer> ().material = oriObjMat;

		
	}

	// Use this for initialization
	void Start () {
		//ih = GameObject.Find ("MeshGen").GetComponent<MeshGen>().ih;
		isGazeEnter = false;
		oriObjMat = gameObject.GetComponent<Renderer> ().material;
//		resizeScale = 0.02f;
//		oriObjScale = new Vector3 (0.1f, 0.1f, 0.1f);
//		oriObjScale = new Vector3 (0.1f, 0.1f, 0.1f);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
