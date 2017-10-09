using System.Collections;
using System.Collections.Generic;
using HoloKit;
using UnityEngine;

public class SelectDisplay : MonoBehaviour {

	InputHandler ih;

	public Material selectedMat;

	public GameObject tooltipPrefab;
	public GameObject pointUnit;
	float resizeScale;

	public bool isSelect;

	// Use this for initialization
	void Start () {
		ih = GameObject.Find ("MeshGen").GetComponent<MeshGen>().ih;
		resizeScale = 0.02f;
		isSelect = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyUp (KeyCode.Mouse0)) {
			if (!GetComponent<GazeClusterSelection> ().isGazeEnter) {
				deselectCluster ();
			}
		}

		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			if (GetComponent<GazeClusterSelection> ().isGazeEnter)
				selectCluster ();
		}

		if (HoloKitInputManager.Instance.GetKeyDown(HoloKitKeyCode.U))
		{
			if (GetComponent<GazeClusterSelection>().isGazeEnter)
				selectCluster();
			else
				deselectCluster();
		}
	}

	void deselectCluster(){
		int clusterIdx = int.Parse (gameObject.name.Substring (7));
		gameObject.GetComponent<Renderer> ().material.color = 
			new Color (ih.clusters [clusterIdx].color [0], ih.clusters [clusterIdx].color [1], ih.clusters [clusterIdx].color [2], 0.5f);
		Transform tf = gameObject.transform.Find ("tooltip");
		if (tf != null) {
			tf.gameObject.SetActive (false);
			// hide points
			for (int i = 0; i < ih.clusters [clusterIdx].indicesOfPoints.Count; i++) {
				gameObject.transform.Find ("point" + i.ToString ()).gameObject.SetActive (false);
				//			gameObject.transform.Find ("point" + i.ToString ()).gameObject.GetComponent<Renderer> ().material = oriObjMat;
				gameObject.transform.Find ("point" + i.ToString ()).gameObject.GetComponent<Renderer> ().material.color = 
					new Color (ih.clusters [clusterIdx].color [0], ih.clusters [clusterIdx].color [1], ih.clusters [clusterIdx].color [2], 0.5f);
				//			gameObject.transform.Find ("pointname" + i.ToString ()).gameObject.SetActive (false);
			}
		}
		isSelect = false;
	}

	void selectCluster(){
		if (gameObject.name.Contains ("cluster")) {
			isSelect = true;

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
				if(ih.clusters[clusterIdx].keywords.Count > 1)
					tooltiptf.GetComponent<TextMesh>().text += "\n" + ih.clusters[clusterIdx].keywords[ih.clusters[clusterIdx].keywords.Count - 2].kw;
				if (ih.clusters[clusterIdx].keywords.Count > 2)
					tooltiptf.GetComponent<TextMesh>().text += "\n" + ih.clusters[clusterIdx].keywords[ih.clusters[clusterIdx].keywords.Count - 3].kw;
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
}
