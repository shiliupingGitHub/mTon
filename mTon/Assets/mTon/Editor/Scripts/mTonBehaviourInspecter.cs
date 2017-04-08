using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Text;
[CustomEditor(typeof(mTonBehaviour))]
public class mTonBehaviourInspecter : Editor {
   public static  List<string> mSelectTypeStr = new List<string>();
    Dictionary<string, mTonFieldInfo> mFields = new Dictionary<string, mTonFieldInfo>();
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
            //System.Object o = mTonMiniJSON.Json.Deserialize(ta.text);
            Dictionary<string, mTonFieldInfo> fields = mTonFieldInfo.Deserilize(ta.text);
            foreach (var f in fields)
            {
               sb.Append("\n");
                sb.Append("\t");
                sb.Append(f.Value.mSzType.ToString());
                sb.Append("\t");
                sb.Append(f.Key);
                sb.Append(";");
                sb.Append("\t//");
                sb.Append(f.Value.mSzDes);
               
            }
            sb.Append("\n");
            sb.Append("\t");
            sb.Append("public  void Init(mTonBehaviour s)\n\t{");
            foreach(var f in fields)
            {
                sb.Append("\n");
                sb.Append("\t\t");
                if(mTonTools.IsValueType(f.Value.mSzType))
                {
                    sb.Append(f.Key + "=s.get_" + f.Value.mSzType.ToLower() + "(\"" + f.Key + "\")");
                }
                else
                    sb.Append(f.Key + "=" + "(" + f.Value.mSzType + ") s.get_object(\"" + f.Key + "\",\"" + f.Value.mSzType + "\")");
                
                sb.Append(";");
                sb.Append("\n");
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
        //base.OnInspectorGUI();
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
            EditorGUILayout.Separator();
            if (cur != selectId)
            {
                selectId = cur;

            }
            ShowInjections("Go");
            ShowInjections("Float");
            ShowInjections("Int");
            ShowInjections("Bool");
            ShowInjections("String");
            ShowSelfDefine();
        }
        if (GUI.changed)
        {
            InitFields();
            serializedObject.FindProperty("mTemplate").stringValue = mSelectTypeStr[selectId];
            EditorUtility.SetDirty(target);
        }
        serializedObject.ApplyModifiedProperties();
    }
    void ShowSelfDefine()
    {
        EditorGUILayout.BeginVertical("box");
        SerializedProperty sp = serializedObject.FindProperty("mSelfDefine");
        EditorGUILayout.LabelField("SelfDefine");
        EditorGUILayout.Space();
        //EditorGUILayout.PropertyField(sp);
        for (int i = 0; i < sp.arraySize; i++)
        {
            SerializedProperty sub = sp.GetArrayElementAtIndex(i);
            ShowSubSelfDefine(sub, i,sp);
        }
        if (GUILayout.Button("add"))
        {
            sp.InsertArrayElementAtIndex(0);
            

        }
        EditorGUILayout.EndVertical();
    }
    void ShowSubSelfDefine(SerializedProperty sub,int i, SerializedProperty array)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(sub);
        if (GUILayout.Button("remove"))
        {
            array.DeleteArrayElementAtIndex(i);
        }
        EditorGUILayout.EndHorizontal();
    }
     void ShowInjections(string szType)
    {
       
        string szFiled = "m" + szType + "Injection";
        SerializedProperty sp = serializedObject.FindProperty(szFiled);
        int totolCount = GetSelectFieldKey(szType).Count;
        if (totolCount == 0)
        {
            sp.ClearArray();
            return;
        }
        EditorGUILayout.LabelField(szType);
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("box");
        for (int i = 0; i < sp.arraySize; i++)
        {
            SerializedProperty sub =  sp.GetArrayElementAtIndex(i);
            ShowInject(sub, szType,sp,i);
        }
        if(totolCount > sp.arraySize)
        {
            if (GUILayout.Button("add"))
            {
                sp.InsertArrayElementAtIndex(0);
            }
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }
    void ShowInject(SerializedProperty sp, string type, SerializedProperty array, int i)
    {
        EditorGUILayout.BeginHorizontal();
        string key = sp.FindPropertyRelative("mKey").stringValue;
        // string title = key + "(" + type + ")";
        int selectId = 0;
        List<string> selectStr = GetSelectFieldKey(type);
        for (int j = 0; j < selectStr.Count; j++)
        {
            if (key == selectStr[j])
            {
                selectId = j;
                break;
            }
        }

        string szType = mFields[selectStr[selectId]].mSzType;
        string szDes = mFields[selectStr[selectId]].mSzDes;
        string szTitle = selectStr[selectId] + "(" + szType + ")(" + szDes+")";
        selectId = EditorGUILayout.Popup(szTitle, selectId, selectStr.ToArray());
        if (key != selectStr[selectId])
        {
            key = selectStr[selectId];
            sp.FindPropertyRelative("mKey").stringValue = key;
            
        }
        ;
        EditorGUILayout.PropertyField(sp.FindPropertyRelative("mV"), new GUIContent(string.Empty));
       if(GUILayout.Button("remove"))
        {
            array.DeleteArrayElementAtIndex(i);
        }
        EditorGUILayout.EndHorizontal();

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

    List<string> GetSelectFieldKey(string szType)
    {
        List<string> ret = new List<string>();
        szType = szType.ToLower();
        foreach(var f in mFields)
        {
            string curType = f.Value.mSzType;
            if(mTonTools.IsValueType(curType)
                )
            {
                if (curType == szType)
                    ret.Add(f.Key);

            }
            else
            {
                if (!mTonTools.IsValueType(szType))
                    ret.Add(f.Key);
            }
        }
        return ret;
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
            mFields = mTonFieldInfo.Deserilize(file);
        }
    }
}
