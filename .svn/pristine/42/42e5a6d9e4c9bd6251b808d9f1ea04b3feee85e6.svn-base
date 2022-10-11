using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace WL.Editor
{
	internal class MvcEditor : EditorWindow
	{
		private static string Output_Path = "/Scripts/HotFix/UI/";
		private static MvcEditor m_Win;

		public string m_ClassName;
		
		[MenuItem("W1_Tools/MVC Creater")]
		public static void BuildAssetBundle()
		{
			m_Win = GetWindow<MvcEditor>("MVC Creater");
			m_Win.minSize = new Vector2(300, 250);
			m_Win.Show();
		}

		protected void OnGUI()
		{
			//绘制标题
			GUILayout.Space(10);
			GUIStyle textStyle = new GUIStyle();
			textStyle.fontSize = 24;
			textStyle.normal.textColor = Color.white;
			textStyle.alignment = TextAnchor.MiddleCenter;
			GUILayout.Label("MVC Creater", textStyle);
			textStyle.fontSize = 18;
			GUILayout.Label("create model view controller", textStyle);
			GUILayout.Space(10);
			
			//类名
			GUILayout.Space(10);
			EditorGUILayout.LabelField("Class name");
			GUILayout.Space(25);
			m_ClassName = EditorGUILayout.TextField(m_ClassName);

			//按钮
			GUILayout.Space(30);
			if (GUILayout.Button("Generate 生成"))
			{
				Generate(m_ClassName);
			}
		}

		private static void Generate(string clsName)
		{
			if(string.IsNullOrEmpty(clsName))
            {
				EditorUtility.DisplayDialog("Error", $"Input class name", "Ok");
				return;
			}

			var path = Application.dataPath + Output_Path + clsName;
			if (Directory.Exists(path))
			{
				EditorUtility.DisplayDialog("Error", $"{clsName} exist", "Ok");
				return;
			}

			Directory.CreateDirectory(path);
			if (!Directory.Exists(path))
            {
				EditorUtility.DisplayDialog("Error", $"{path} create error~~", "Ok");
				return;
			}

			//var model_path = path + "/Model/";
			//Directory.CreateDirectory(model_path);

			var view_path = path;
			Directory.CreateDirectory(view_path);

			WriteFile(path + "/" + clsName + "Mgr.cs", clsName, "MgrTemplate.txt");
			WriteFile(path + "/" + clsName + "Comp.cs", clsName, "CompTemplate.txt");
			WriteFile(path + "/" + clsName + "View.cs", clsName, "ViewTemplate.txt");

			m_Win.Close();

			AssetDatabase.Refresh();
		}

		private static void WriteFile(string path, string clsName, string templateName)
        {
			var fr = new FileStream(Application.dataPath + Output_Path + templateName, FileMode.Open, FileAccess.Read);
            var sr = new StreamReader(fr);
            var txt = sr.ReadToEnd();
            sr.Dispose();
            txt = txt.Replace("XXXX", clsName);

            var fw = new FileStream(path, FileMode.Append, FileAccess.Write);
			var sw = new StreamWriter(fw);
			sw.Write(txt);
			sw.Dispose();

			UnityEngine.Debug.Log(path);
		}
	}

}