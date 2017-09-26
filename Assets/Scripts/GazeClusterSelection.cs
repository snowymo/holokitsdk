using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeClusterSelection : MonoBehaviour {

	Vector3 oriObjScale;
	public Material oriObjMat;
	public Material selectedMat;
	public GameObject tooltipPrefab;
	public GameObject pointUnit;
	float resizeScale;
//	public MeshGen mg;
	InputHandler ih;

	public void GazeEnter() {
		Debug.Log("[hehe]Gaze entered on " + gameObject.name);
//		GetComponent<Renderer>().material.color = Color.red;
//		gameObject.transform.localScale = oriObjScale * 1.1f;
		gameObject.GetComponent<Renderer> ().material = selectedMat;
		// show keywords
		if (gameObject.name.Contains ("cluster")) {
			int clusterIdx = int.Parse (gameObject.name.Substring (7));

			if (ih.clusters [clusterIdx].keywords.Count > 0) {
				Transform tooltiptf = gameObject.transform.Find ("tooltip");
				if (tooltiptf == null) {
					GameObject tooltip = GameObject.Instantiate (tooltipPrefab, gameObject.transform);
					tooltip.name = "tooltip";
					tooltiptf = tooltip.transform;
				}
				tooltiptf.gameObject.SetActive (true);
				tooltiptf.forward = Camera.main.transform.forward;
				tooltiptf.localPosition = new Vector3 (0.5f, 0.5f, -0.5f);
				tooltiptf.GetComponent<TextMesh> ().text = ih.clusters [clusterIdx].keywords [ih.clusters [clusterIdx].keywords.Count - 1].kw;
			}
			// show points
			for (int i = 0; i < ih.clusters [clusterIdx].indicesOfPoints.Count; i++) {
				Transform pointtf = gameObject.transform.Find ("point" + i.ToString ());
//				Transform pointnametf = gameObject.transform.Find ("pointname" + i.ToString ());


				if (pointtf == null) {
//					GameObject pointname = GameObject.Instantiate (tooltipPrefab, gameObject.transform);
//					pointname.name = "pointname" + i.ToString ();
//					pointnametf = pointname.transform;

					GameObject goPts = GameObject.Instantiate (pointUnit, 
						ih.points [ih.clusters [clusterIdx].indicesOfPoints [i]].pos * resizeScale, Quaternion.identity, gameObject.transform);
					pointtf = goPts.transform;
					goPts.name = "point" + i.ToString ();
				}

				pointtf.GetComponent<Renderer> ().material = selectedMat;
//				pointnametf.gameObject.SetActive (true);
//				pointnametf.forward = Camera.main.transform.forward;
//				pointnametf.localPosition = new Vector3 (0.1f, 0.1f, 0);
//				pointnametf.GetComponent<TextMesh> ().text = ih.points [ih.clusters [clusterIdx].indicesOfPoints [i]].label;
//				pointnametf.GetComponent<TextMesh> ().fontSize = 10;

				pointtf.gameObject.SetActive (true);
				pointtf.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
				pointtf.parent = gameObject.transform;

			}
		}
	}

	public void GazeExit() {
		Debug.Log("[hehe]Gaze exit from " + gameObject.name);
//		GetComponent<Renderer>().material.color = Color.white;
//		gameObject.transform.localScale = oriObjScale;
		gameObject.GetComponent<Renderer> ().material = oriObjMat;
		// hide keywords
		Transform tf = gameObject.transform.Find("tooltip");
		if(tf != null)
			tf.gameObject.SetActive(false);
		// hide points
		int clusterIdx = int.Parse (gameObject.name.Substring (7));
		for (int i = 0; i < ih.clusters [clusterIdx].indicesOfPoints.Count; i++) {
			gameObject.transform.Find ("point" + i.ToString ()).gameObject.SetActive (false);
			gameObject.transform.Find ("point" + i.ToString ()).gameObject.GetComponent<Renderer> ().material = oriObjMat;
//			gameObject.transform.Find ("pointname" + i.ToString ()).gameObject.SetActive (false);
		}
	}

	// Use this for initialization
	void Start () {
		ih = GameObject.Find ("MeshGen").GetComponent<MeshGen>().ih;
		resizeScale = 0.02f;
//		oriObjScale = new Vector3 (0.1f, 0.1f, 0.1f);
//		oriObjScale = new Vector3 (0.1f, 0.1f, 0.1f);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
