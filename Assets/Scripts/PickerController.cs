using UnityEngine;
using System.Collections;
using System.IO;

namespace Zhenyi
{
	public class PickerController : MonoBehaviour
	{
		[SerializeField]
		private DocPicker docPicker;

		//[SerializeField]
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
			docPicker.Show("Select File", "docpickertmp.txt", 1024*1024);
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
			Debug.Log("[hehe]file based on:" + path /*+ " data:" + data*/ + "(which can be read by www) with length: " + data.Length);
			// write file to local
			//string fileName = System.IO.Path.Combine(Application.persistentDataPath, "localtmp.txt");
			//if (File.Exists(fileName))
			//{
			//	File.Delete(fileName);
			//}
			//string filePath = Application.persistentDataPath + "/testFile.txt";
			//StreamWriter sr = File.CreateText(fileName);
			//sr.Write(data);
			//sr.WriteLine ("This is my file.");
			//sr.WriteLine ("I can write ints {0} or floats {1}, and so on.", 1, 4.2);
			//sr.Close ();
			//Debug.Log("[hehe] after creation " + url);
			ih = mg.ih;
			ih.setFileScore(path);
			mg.RefreshModels();
		}
	}
}