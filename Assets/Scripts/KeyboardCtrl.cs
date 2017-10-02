using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloKit;

public class KeyboardCtrl : MonoBehaviour {

	public bool isShow;

	// Use this for initialization
	void Start () {
		isShow = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			isShow = !isShow;
		}
		if(Input.GetKeyUp(KeyCode.Mouse0)) {
			isShow = !isShow;
		}

//		Vector2 touchPosition;
//		if (HoloKitInputManager.Instance.GetTouchBegan (out touchPosition)) {
//			isShow = !isShow;
//		}
//		if (HoloKitInputManager.Instance.GetTouchBegan (out touchPosition)) {
//			isShow = !isShow;
//		}
	}
}
