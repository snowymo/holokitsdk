using UnityEngine;
using System.Collections;
using HoloKit;
using System;

public class TangoPhoneSpaceController : PhoneSpaceControllerBase
{

	public Camera MultiCamera;
	public Camera LeftCamera;
	public Camera RightCamera;

	private BarrelDistortion leftBarrel;
	private BarrelDistortion rightBarrel;

	private bool previousCameraSeeThrough;

	public float FOV
	{
		get
		{
			return LeftCamera.fieldOfView;
		}
		set
		{
			LeftCamera.fieldOfView = value;
			RightCamera.fieldOfView = value;
			MultiCamera.fieldOfView = value;
		}
	}

	public float BarrelRadius
	{
		get
		{
			return leftBarrel.FovRadians;
		}
		set
		{
			leftBarrel.FovRadians = value;
			rightBarrel.FovRadians = value;
		}
	}

	public override float PupilDistance
	{
		get
		{
			return RightCamera.transform.localPosition.x - LeftCamera.transform.localPosition.x;
		}
		set
		{
			float halfDist = value / 2;
			LeftCamera.transform.localPosition = new Vector3(-halfDist, 0, 0);
			RightCamera.transform.localPosition = new Vector3(halfDist, 0, 0);
		}
	}

	public override Vector3 CameraOffset
	{
		get
		{
			return MultiCamera.transform.localPosition;
		}

		set
		{
			MultiCamera.transform.localPosition = value;
		}
	}

	public override float PhoneScreenHeight
	{
		get
		{
			throw new NotImplementedException();
		}

		set
		{
			throw new NotImplementedException();
		}
	}

	public override float PhoneScreenWidth
	{
		get
		{
			throw new NotImplementedException();
		}

		set
		{
			throw new NotImplementedException();
		}
	}

	public override float FresnelLensFocalLength
	{
		get
		{
			throw new NotImplementedException();
		}

		set
		{
			throw new NotImplementedException();
		}
	}

	public override float ScreenToFresnelDistance
	{
		get
		{
			throw new NotImplementedException();
		}

		set
		{
			throw new NotImplementedException();
		}
	}

	public override float FresnelToEyeDistance
	{
		get
		{
			throw new NotImplementedException();
		}

		set
		{
			throw new NotImplementedException();
		}
	}

	public override float ViewportHeightRatio
	{
		get
		{
			throw new NotImplementedException();
		}

		set
		{
			throw new NotImplementedException();
		}
	}

	public override float RedDistortionFactor
	{
		get
		{
			throw new NotImplementedException();
		}

		set
		{
			throw new NotImplementedException();
		}
	}

	public override float GreenDistortionFactor
	{
		get
		{
			throw new NotImplementedException();
		}

		set
		{
			throw new NotImplementedException();
		}
	}

	public override float BlueDistortionFactor
	{
		get
		{
			throw new NotImplementedException();
		}

		set
		{
			throw new NotImplementedException();
		}
	}

	public override float BarrelDistortionFactor
	{
		get
		{
			throw new NotImplementedException();
		}

		set
		{
			throw new NotImplementedException();
		}
	}

	void Start()
	{
		leftBarrel = LeftCamera.GetComponent<BarrelDistortion>();
		rightBarrel = RightCamera.GetComponent<BarrelDistortion>();

		StartCoroutine(fullScreenClear());
	}

	private IEnumerator fullScreenClear()
	{
		CameraClearFlags oldClearFlags = MultiCamera.clearFlags;
		Color oldBackgroundColor = MultiCamera.backgroundColor;
		bool oldEnabled = MultiCamera.enabled;

		MultiCamera.enabled = true;
		MultiCamera.clearFlags = CameraClearFlags.SolidColor;
		MultiCamera.backgroundColor = Color.black;

		yield return null;
		yield return null;

		MultiCamera.enabled = oldEnabled;
		MultiCamera.clearFlags = oldClearFlags;
		MultiCamera.backgroundColor = oldBackgroundColor;
	}
}
