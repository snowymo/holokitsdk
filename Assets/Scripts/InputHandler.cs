//#define ZHENYIDEBUG

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

// using UnityEngine;
// using System.Collections;

public class KeyWord
{
	public string kw;
	public float score;

	public KeyWord()
	{
		kw = "";
		score = 0;
	}

	public KeyWord(string st, float f)
	{
		kw = st;
		score = f;
	}
}

public class Cluster
{
	public Vector3 pos;
	//	public int[] indicesOfPoints;
	public List<int> indicesOfPoints;
	public List<KeyWord> keywords;
	public float color_score;
	public Vector3 color;

	public MyMesh mm;
	public GameObject go;
	//	public TextMesh keywordDisplay;

	//	const int POINT_SIZE_EACH_CLUSTER = 63;

	public Cluster()
	{
		pos = new Vector3();
		//		indicesOfPoints = new int[POINT_SIZE_EACH_CLUSTER];
		indicesOfPoints = new List<int>();
		keywords = new List<KeyWord>();
		color_score = 0;

		mm = new MyMesh();


	}
}

public class Point
{
	public Vector3 pos;
	public string label;
	public int cluster_group;
	public Point()
	{
		pos = new Vector3();
		label = "";
		cluster_group = -1;
	}
}



// load necessary files and parse it here
public class InputHandler
{
	const int POINT_SIZE = 50000;
	public const int CLUSTER_SIZE = 800;
	string file_3dembedding = "Data/3d_embedding.txt";
	string file_clusterlabel = "Data/clusters_C800.txt";
	string file_clusterpos = "Data/cluster_centers_C800.txt";
	string file_keywords = "Data/cluster_word_scores.txt";
	string file_score = "Data/benchmark_index_rgb.txt";//"Data/rgb.txt";

	public Point[] points;
	public Cluster[] clusters;

	int pointIdx4Start;
	int clusterIdx4Start;

	public float maxColorScore;
	public float minColorScore;

	delegate void call4load(string[] entry);

	call4load loadHandler;

	public void setFileScore(string fn)
	{
		file_score = fn;
#if ZHENYIDEBUG
		Debug.Log("[hehe] file score:" + file_score);
#endif
	}

	private void addPointItem(string[] stPoint)
	{
		if (stPoint.Length == 3)
		{
			float x = float.Parse(stPoint[0], System.Globalization.NumberStyles.Float);
			float y = float.Parse(stPoint[1], System.Globalization.NumberStyles.Float);
			float z = float.Parse(stPoint[2], System.Globalization.NumberStyles.Float);
			if (points[pointIdx4Start] == null)
				points[pointIdx4Start] = new Point();
			points[pointIdx4Start++].pos = new Vector3(x, y, z);
		}
	}

	private void addClusterPos(string[] stCluster)
	{
		if (stCluster.Length == 4)
		{
			float x = float.Parse(stCluster[0], System.Globalization.NumberStyles.Float);
			float y = float.Parse(stCluster[1], System.Globalization.NumberStyles.Float);
			float z = float.Parse(stCluster[2], System.Globalization.NumberStyles.Float);
			if (clusters[clusterIdx4Start] == null)
				clusters[clusterIdx4Start] = new Cluster();
			clusters[clusterIdx4Start++].pos = new Vector3(x, y, z);
		}
	}

	private void addClusterLabel(string[] stLabel)
	{
		if (stLabel.Length == 2)
		{
			string l = stLabel[0];
			float idx = float.Parse(stLabel[1], System.Globalization.NumberStyles.Float);
			if (points[pointIdx4Start] == null)
				points[pointIdx4Start] = new Point();
			//			clusters [clusterIdx4Start++].label = l;
			points[pointIdx4Start].label = l;
			points[pointIdx4Start].cluster_group = (int)idx % CLUSTER_SIZE;
			clusters[(int)(idx) % CLUSTER_SIZE].indicesOfPoints.Add(pointIdx4Start++);
		}
	}

	private void addClusterKeyWords(string[] stKeyWords)
	{
		float idx = float.Parse(stKeyWords[0], System.Globalization.NumberStyles.Float);

		string pattern = @"\(\'([^\(\)]+)\',\s([^\(\)]+)\)";//(,\(([^\(\)]+)\)+)\]
		foreach (Match m in Regex.Matches(stKeyWords[1], pattern))
		{
			string kw = m.Groups[1].Value;
			float kw_score = float.Parse(m.Groups[2].Value, System.Globalization.NumberStyles.Float);
			clusters[(int)idx % CLUSTER_SIZE].keywords.Add(new KeyWord(kw, kw_score));
		}
	}

	private void addClusterColorScore(string[] stColor)
	{
		if (stColor.Length == 2)
		{
			float idx = float.Parse(stColor[0], System.Globalization.NumberStyles.Float);
			float color_score = 0;
			if (stColor[1] != "NULL")
				color_score = float.Parse(stColor[1], System.Globalization.NumberStyles.Float);
			clusters[(int)(idx) % CLUSTER_SIZE].color_score = color_score;
			if (color_score > maxColorScore)
				maxColorScore = color_score;
			if (color_score < minColorScore)
				minColorScore = color_score;
		}
	}

	private void addClusterColor(string[] stColor)
	{
		if (stColor.Length == 4)
		{
			float idx = float.Parse(stColor[0], System.Globalization.NumberStyles.Float);
			float r = 0f;
			float g = 0f;
			float b = 0f;
			r = float.Parse(stColor[1], System.Globalization.NumberStyles.Float);
			g = float.Parse(stColor[2], System.Globalization.NumberStyles.Float);
			b = float.Parse(stColor[3], System.Globalization.NumberStyles.Float);
			clusters[(int)(idx) % CLUSTER_SIZE].color = new Vector3(r / 256.0f, g / 256.0f, b / 256.0f);
		}
	}

	float MinVisibleWaveLength = 350.0f;
	float MaxVisibleWaveLength = 650.0f;
	Vector3 generateColor(float colorScore)
	{
		float colorNormScore = (colorScore - minColorScore) / (maxColorScore - minColorScore) * (MaxVisibleWaveLength - MinVisibleWaveLength) + MinVisibleWaveLength;
		Vector3 rgb = new Vector3();
		if (colorNormScore < 439f)
		{
			rgb[0] = -(colorNormScore - 440f) / (440f - 380f);
			rgb[1] = 0.0f;
			rgb[2] = 1.0f;
		}
		else if (colorNormScore < 489f)
		{
			rgb[0] = 0.0f;
			rgb[1] = (colorNormScore - 440f) / (490f - 440f);
			rgb[2] = 1.0f;
		}
		else if (colorNormScore < 509f)
		{
			rgb[0] = 0.0f;
			rgb[1] = 1.0f;
			rgb[2] = -(colorNormScore - 510f) / (510f - 490f);
		}
		else if (colorNormScore < 579)
		{
			rgb[0] = (colorNormScore - 510f) / (580f - 510f);
			rgb[1] = 1.0f;
			rgb[2] = 0.0f;
		}
		else if (colorNormScore < 644f)
		{
			rgb[0] = 1.0f;
			rgb[1] = -(colorNormScore - 645f) / (645f - 580f);
			rgb[2] = 0.0f;
		}
		else
		{
			rgb[0] = 1.0f;
			rgb[1] = 0.0f;
			rgb[2] = 0.0f;
		}
		return rgb;
	}

	private bool loadFromFile(string fileName, char[] splitter)
	{
		// Handle any problems that might arise when reading the text
		try
		{
			string line;
			// Create a new StreamReader, tell it which file to read and what encoding the file was saved as
			if (Application.isMobilePlatform)
			{
#if UNITY_ANDROID
				fileName = Application.streamingAssetsPath + "/" + fileName;
				WWW reader = new WWW (fileName);
				while (!reader.isDone) {
				}

				Debug.Log("[hehe] TestSA " + fileName + "\t" + reader.text.Substring(0,30));
				string[] lines = reader.text.Split(new char[]{'\n'},StringSplitOptions.RemoveEmptyEntries);
				Debug.Log("[hehe] lines " + lines.Length);
				for(int i = 0; i < lines.Length; i++){
					string[] entries = lines[i].Split (splitter, StringSplitOptions.RemoveEmptyEntries);
					if (entries.Length > 0)
						//							addPointItem(entries);
						loadHandler (entries);
				}
#endif

#if UNITY_IPHONE
				if (!fileName.Contains(Application.persistentDataPath))
				{
					fileName = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
				}
				//else
				//{
				//	fileName = "file://" + fileName;
				//	//var www = new WWW(fileName);
				//}
				Debug.Log("[hehe]: file name example:" + fileName);
				if (File.Exists(fileName))
				{
					FileInfo theSourceFile = new FileInfo(fileName);
					StreamReader reader = theSourceFile.OpenText();
					do
					{
						line = reader.ReadLine();
						if (line != null)
						{
							// Do whatever you need to do with the text line, it's a string now
							// In this example, I split it into arguments based on comma
							// deliniators, then send that array to DoStuff()

							string[] entries = line.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
							if (entries.Length > 0)
								//							addPointItem(entries);
								loadHandler(entries);
						}
						// now loadeddata have full content that you whis to get from ios device 
					} while (line != null);
				}
				else
				{
					Debug.LogError("[hehe]file:" + fileName + " cannot be found");
				}

#endif
				return true;
			}
			else
			{
				StreamReader theReader = new StreamReader(fileName, Encoding.Default);
				using (theReader)
				{
					// While there's lines left in the text file, do this:
					do
					{
						line = theReader.ReadLine();
						if (line != null)
						{
							string[] entries = line.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
							if (entries.Length > 0)
								loadHandler(entries);
						}
					} while (line != null);
					theReader.Close();
					return true;
				}
			}
		}
		// If anything broke in the try block, we throw an exception with information on what didn't work
		catch (System.Exception e)
		{
			Debug.Log("[hehe]" + e.Message);
			return false;
		}
	}


	// Use this for initialization
	public void Initialize()
	{
		maxColorScore = -1000000;
		minColorScore = 1000000;

		pointIdx4Start = 0;
		loadHandler = addPointItem;
#if ZHENYIDEBUG
		Debug.Log ("[hehe]start points loading");
#endif
		loadFromFile(file_3dembedding, new char[] { ' ' });
#if ZHENYIDEBUG
		Debug.Log ("[hehe]finish points loading");
#endif

		clusterIdx4Start = 0;
		loadHandler = addClusterPos;
#if ZHENYIDEBUG
		Debug.Log ("[hehe]start cluster loading");
#endif
		loadFromFile(file_clusterpos, new char[] { ',' });
#if ZHENYIDEBUG
		Debug.Log ("[hehe]finish cluster loading");
#endif

		pointIdx4Start = 0;
		loadHandler = addClusterLabel;

#if ZHENYIDEBUG
		Debug.Log ("[hehe]start cluster label loading");
#endif
		loadFromFile(file_clusterlabel, new char[] { ',' });
#if ZHENYIDEBUG
		Debug.Log ("[hehe]finish cluster label loading");
#endif

		pointIdx4Start = 0;
		loadHandler = addClusterKeyWords;
#if ZHENYIDEBUG
		Debug.Log ("[hehe]start cluster key words loading");
#endif
		loadFromFile(file_keywords, new char[] { '\t' });
#if ZHENYIDEBUG
		Debug.Log ("[hehe]finish cluster key words loading");
#endif

		pointIdx4Start = 0;
		loadHandler = addClusterColor;
#if ZHENYIDEBUG
		Debug.Log ("[hehe]start cluster color score loading");
#endif
		loadFromFile (file_score, new char[]{','});
#if ZHENYIDEBUG
		Debug.Log ("[hehe]finish cluster color score loading");
#endif
	}

	public InputHandler(){
		points = new Point[POINT_SIZE];
		clusters = new Cluster[CLUSTER_SIZE];
	}
}
