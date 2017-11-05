using UnityEngine;
using System.Collections;

namespace Zhenyi
{
	public class PickerController : MonoBehaviour
	{
		[SerializeField]
		private DocPicker docPicker;

		[SerializeField]
		private InputHandler ih;

		[SerializeField]
		private MeshGen mg;

		//[SerializeField]
		//private MeshRenderer imageRenderer;

		void Awake()
		{
			docPicker.Completed += (string path) =>
			{
				StartCoroutine(LoadFile(path));
			};
		}

		public void OnPressShowPicker()
		{
			print("[hehe] OnPressShowPicker");
			docPicker.Show("Select File", "docpicker", 1024*256);
		}

		//private IEnumerator LoadImage(string path, MeshRenderer output)
		//{
		//	var url = "file://" + path;
		//	var www = new WWW(url);
		//	yield return www;

		//	var texture = www.texture;
		//	if (texture == null)
		//	{
		//		Debug.LogError("Failed to load data url:" + url);
		//	}

		//	output.material.mainTexture = texture;
		//}

		private IEnumerator LoadFile(string path)
		{
			var url = "file://" + path;
			var www = new WWW(url);
			yield return www;

			var data = www.text;
			if (data == null)
			{
				Debug.LogError("Failed to load data url:" + url);
			}
			Debug.Log("data:" + data);
			ih.setFileScore(url.ToString());
			mg.RefreshModels();
		}
	}
}