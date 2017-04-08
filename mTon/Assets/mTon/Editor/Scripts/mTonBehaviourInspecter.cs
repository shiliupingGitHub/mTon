using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Text;
[CustomEditor(typeof(mTonBehaviour))]
public class mTonBehaviourInspecter : Editor {
   public static  List<string> mSelectTypeStr = new List<string>();
    Dictionary<string, System.Object> mFields = new Dictionary<string, System.Object>();
    bool mDraw = true;
    void OnEnable()
    {
        InitClass();
        InitFields();
    }
    [MenuItem("mTon/CreateCS")]
    public static void CreateCS()
    {
        StringBuilder sb = new StringBuilder();
        if(Selection.activeObject is TextAsset)
        {
            TextAsset ta = (TextAsset)Selection.activeObject;
            sb.Append("using UnityEngine;\n");
            sb.Append("public  partial class " + ta.name + " : mTonBase \n{");
            System.Object o = mTonMiniJSON.Json.Deserialize(ta.text);
            Dictionary<string, System.Object> fields = o as Dictionary<string, System.Object>;
            foreach (var f in fields)
            {
                sb.Append("\n");
                sb.Append("\t");
                sb.Append(f.Value.ToString());
                sb.Append("\t");
                sb.Append(f.Key);
                sb.Append(";");
            }
            sb.Append("\n");
            sb.Append("\t");
            sb.Append("public  void Init(mTonBehaviour s)\n\t{");
            sb.Append("\n\t\t mTonInjection temp = null;");
            foreach(var f in fields)
            {
                sb.Append("\n");
                sb.Append("\t\t");
                sb.Append("temp = " + "s.GetInject(\"" + f.Key + "\");\n");
                sb.Append("\t\t");
                sb.Append("if (null != temp)\n\t\t{\n\t\t\t");
                if (f.Value.ToString() == "int")
                {
                    sb.Append(f.Key + "= temp.");
      
                    sb.Append("mInt");
                }
                else if(f.Value.ToString() =="float")
                {
                    sb.Append(f.Key + "= temp.");
                    sb.Append("mFloat");
                }
                else if(f.Value.ToString() == "string")
                {
                    sb.Append(f.Key + "= temp.");
                    sb.Append("mText");
                }
                else if (f.Value.ToString() == "bool")
                {
                    sb.Append(f.Key + "= temp.");
                    sb.Append("mBool");
                }
                else if(f.Value.ToString() == "GameObject")
                {
                    sb.Append(f.Key + "= temp.");
                    sb.Append("mGo");
                }
                else
                {
                    sb.Append("if(temp.mGo != null)\n\t\t\t\t");
                    sb.Append(f.Key + "= temp.");
                    sb.Append("mGo.GetComponent<" + f.Value.ToString()+">()");
                }
                
                sb.Append(";");
                sb.Append("\n");
                sb.Append("\t\t}\n");
            }
            sb.Append("\n\t}");
            sb.Append("\n}");
            string relativePath = File.ReadAllText(Application.dataPath + "/mTon/Editor/mTon.txt");
            string dp = Application.dataPath + "/" + relativePath;
            if (Directory.Exists(dp))
                Directory.CreateDirectory(dp);
            string filePath = dp +"/" + ta.name + ".cs";
            File.WriteAllText(filePath,sb.ToString());
            AssetDatabase.Refresh();
        }
        
    }
    public override void OnInspectorGUI()
    {
        DrawGUI();
    }
    void DrawGUI()
    {
        
        string template = serializedObject.FindProperty("mTemplate").stringValue;
        int selectId = 0;
        for (int i = 0; i < mSelectTypeStr.Count; i++)
        {
            if (mSelectTypeStr[i] == template)
            {
                selectId = i;
                break;
            }
        }
        if (mSelectTypeStr.Count > 0)
        {
            InitFields();
            EditorGUILayout.BeginHorizontal();
            int cur = EditorGUILayout.Popup(selectId, mSelectTypeStr.ToArray());
            EditorGUILayout.EndHorizontal();
     
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Separator();
            if (cur != selectId)
            {
                selectId = cur;

            }
            EditorGUILayout.BeginVertical("box");

            SerializedProperty sp = serializedObject.FindProperty("Injecttion");

            if (null != sp)
            {
                for(int i = 0; i < sp.arraySize; i++)
                {
                    SerializedProperty sub = sp.GetArrayElementAtIndex(i);
                    
                    string key = sub.FindPropertyRelative("mKey").stringValue;
                    if (!mFields.ContainsKey(key))
                        sp.DeleteArrayElementAtIndex(i);
                    else
                    {
                        ShowInject(sub, mFields[key].ToString());
                    }
                }
                foreach(var f in mFields)
                {
                    bool contain = false;
                    for (int i = 0; i < sp.arraySize; i++)
                    {
                        SerializedProperty sub = sp.GetArrayElementAtIndex(i);
                        string key = sub.FindPropertyRelative("mKey").stringValue;
                        if (key == f.Key)
                            contain = true;
                    }
                    if(!contain)
                    {
                        sp.InsertArrayElementAtIndex(0);
                        SerializedProperty sub = sp.GetArrayElementAtIndex(0);
                        SerializedProperty keySp = sub.FindPropertyRelative("mKey");
                        keySp.stringValue = f.Key;
                        ShowInject(sub, f.Value.ToString());
                    }
                }
            }

            EditorGUILayout.EndVertical();
    

        }
        if (GUI.changed)
        {
            InitFields();
            serializedObject.FindProperty("mTemplate").stringValue = mSelectTypeStr[selectId];
            EditorUtility.SetDirty(target);
        }
        serializedObject.ApplyModifiedProperties();
    }
    void ShowInject(SerializedProperty sp, string type)
    {
        string key = sp.FindPropertyRelative("mKey").stringValue;
        if (type == "int")
        {
            string title = key + "(" + type + ")";
            EditorGUILayout.PropertyField(sp.FindPropertyRelative("mInt"), new GUIContent(title));
           
        }
        else if (type == "float")
        {
            string title = key + "(" + type + ")";
            EditorGUILayout.PropertyField(sp.FindPropertyRelative("mFloat"), new GUIContent(title));

        }
        else if (type == "bool")
        {

            string title = key + "(" + type + ")";
            EditorGUILayout.PropertyField(sp.FindPropertyRelative("mBool"), new GUIContent(title));
        }
        else if(type == "string")
        {

            string title = key + "(" + type + ")";
            EditorGUILayout.PropertyField(sp.FindPropertyRelative("mText"), new GUIContent(title));
        }
        else
        {
            string title = key + "(" + type + ")";
            EditorGUILayout.PropertyField(sp.FindPropertyRelative("mGo"), new GUIContent(title));

        }
    }
    public static void InitClass()
    {
        string[] fs = Directory.GetFiles(Application.dataPath + "/mTon/Editor/Config");
        mTonBehaviourInspecter.mSelectTypeStr.Clear();
        foreach (var f in fs)
        {
            if (f.EndsWith("meta"))
                continue;
            string s = Path.GetFileNameWithoutExtension(f);
            mTonBehaviourInspecter.mSelectTypeStr.Add(s);
        }
    }
    void InitFields()
    {
        mTonBehaviour mton = (mTonBehaviour)target;
        if(!string.IsNullOrEmpty(mton.mTemplate))
        {
            string szPath = Application.dataPath + "/mTon/Editor/Config/" + mton.mTemplate + ".txt";
            mFields.Clear();
            if (!File.Exists(szPath))
                return;
           string file =  File.ReadAllText(szPath);
            System.Object o = mTonMiniJSON.Json.Deserialize(file);
            mFields = o as Dictionary<string, System.Object>;
        }
    }
}
