using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeywordDisplay : MonoBehaviour {

	Cluster curCluster, prevCluster;

//	Cluster[] clusters;

	GameObject kwprefab;

	// Use this for initialization
	void Start () {
//		curCluster = GetComponent<ClusterSelection> ().curClosestCluster;
//		clusters = GetComponent<MeshGen> ().ih.clusters;
		prevCluster = null;
	}
	
	// Update is called once per frame
	void Update () {
		curCluster = GetComponent<ClusterSelection> ().curClosestCluster;
		if(prevCluster != null)
			prevCluster.go.GetComponentInChildren<TextMesh>().text = "";

		if (curCluster != null) {
			// display the keywords
//			GameObject go = GameObject.Instantiate(kwprefab);
//			go.transform.parent = curCluster.go.transform;
//			go.GetComponent<TextMesh> ().text = curCluster.keywords.ToString ();
			curCluster.go.GetComponentInChildren<TextMesh>().text = curCluster.keywords[0].kw;
			curCluster.go.GetComponentInChildren<TextMesh> ().transform.rotation = Camera.main.transform.rotation;
		}
		prevCluster = curCluster;
	}
}
