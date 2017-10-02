using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterSelection : MonoBehaviour {

	// select the closest cluster
	public int curClosestClusterIdx;
	public Cluster curClosestCluster;

	public Cluster[] clusters;

	// Use this for initialization
	void Start () {
		curClosestCluster = null;
		curClosestClusterIdx = -1;
		clusters = GetComponent<MeshGen> ().ih.clusters;
	}
	
	// Update is called once per frame
	void Update () {
		float mindis = 10;
		curClosestCluster = null;
		curClosestClusterIdx = -1;

		for (int i = 0; i < clusters.Length; i++) {
			float curDis = Vector3.Distance (Camera.main.transform.position, clusters [i].pos);
			if (curDis < mindis) {
				mindis = curDis;
				curClosestCluster = clusters[i];
				curClosestClusterIdx = i;
			}
		}
	}
}
