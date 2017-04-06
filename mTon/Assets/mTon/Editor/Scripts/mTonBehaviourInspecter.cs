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
            sb.Append("public	override void Init(mTonBehaviour s)\n\t{");
            foreach(var f in fields)
            {
                sb.Append("\n");
                sb.Append("\t\t");
               
                if(f.Value.ToString() == "int")
                {
                    sb.Append(f.Key + "= s.GetInject(\"" + f.Key + "\")");
                    sb.Append(".");
                    sb.Append("mInt");
                }
                else if(f.Value.ToString() =="float")
                {
                    sb.Append(f.Key + "= s.GetInject(\"" + f.Key + "\")");
                    sb.Append(".");
                    sb.Append("mFloat");
                }
                else if(f.Value.ToString() == "string")
                {
                    sb.Append(f.Key + "= s.GetInject(\"" + f.Key + "\")");
                    sb.Append(".");
                    sb.Append("mText");
                }
                else if (f.Value.ToString() == "bool")
                {
                    sb.Append(f.Key + "= s.GetInject(\"" + f.Key + "\")");
                    sb.Append(".");
                    sb.Append("mBool");
                }
                else if(f.Value.ToString() == "GameObject")
                {
                    sb.Append(f.Key + "= s.GetInject(\"" + f.Key + "\")");
                    sb.Append(".");
                    sb.Append("mGo");
                }
                else
                {
                    sb.Append("if (null != s.GetInject(\"" + f.Key + "\"))\n\t\t\t");
                    sb.Append(f.Key + "= s.GetInject(\"" + f.Key + "\")");
                    sb.Append(".");
                    sb.Append("mGo.GetComponent<" + f.Value.ToString()+">()");
                }
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
        //if (mDraw)
        //    DrawGUI();
        //else
            base.OnInspectorGUI();
    }
    void DrawGUI()
    {
        serializedObject.Update();
        serializedObject.UpdateIfDirtyOrScript();
        mTonBehaviour mton = (mTonBehaviour)target;
        int selectId = 0;

        for (int i = 0; i < mSelectTypeStr.Count; i++)
        {
            if (mSelectTypeStr[i] == mton.mTemplate)
            {
                selectId = i;
                break;
            }
        }
        if (mSelectTypeStr.Count > 0)
        {
            EditorGUILayout.BeginHorizontal();
            int cur = EditorGUILayout.Popup(selectId, mSelectTypeStr.ToArray());
            if (GUILayout.Button("ReloadClass"))
                InitClass();
            if (GUILayout.Button("ReloadField"))
                InitFields();
           
            EditorGUILayout.EndHorizontal();
            string szMod = mDraw ? "freedom" : "comstom";
            if (GUILayout.Button(szMod))
                mDraw = !mDraw;
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Separator();
            if (cur != selectId)
            {
                selectId = cur;

            }
            if (mton.mTemplate != mSelectTypeStr[selectId])
            {
                mton.mTemplate = mSelectTypeStr[selectId];
                mton.Injecttion.Clear();
                InitFields();
            }
            EditorGUILayout.BeginVertical();


            if (null != mton.Injecttion)
            {
                List<mTonInjection> rm = new List<mTonInjection>();
                foreach (var inject in mton.Injecttion)
                {
                    if (!mFields.ContainsKey(inject.mKey)
                        ||string.IsNullOrEmpty(inject.key)
                        )
                        rm.Add(inject);
                }
                foreach (var r in rm)
                {
                    mton.Injecttion.Remove(r);
                }
            }

            List<string> mLost = new List<string>();
            foreach (var f in mFields)
            {
                mTonInjection curInject = null;
                if (null != mton.Injecttion)
                {
                    foreach (var inject in mton.Injecttion)
                    {
                        if (inject.mKey == f.Key)
                        {
                            curInject = inject;
                            break;
                        }
                    }
                }

                if (null != curInject)
                {
                    string type = f.Value.ToString();
                    ShowInject(curInject, type);
                    // System.Type.GetType(type);
                }
                else
                    mLost.Add(f.Key);
            }
            foreach (var k in mLost)
            {
               
                mTonInjection curInject = new mTonInjection();
                curInject.mKey = k;
                mton.Injecttion.Add(curInject);
                ShowInject(curInject, mFields[k].ToString());
            }

            EditorGUILayout.EndVertical();

        }
        serializedObject.ApplyModifiedProperties();
    }
    void ShowInject(mTonInjection curInject, string type)
    {
        if (type == "int")
        {
            int i = EditorGUILayout.IntField(curInject.mKey + "(" + type + ")", curInject.mInt);
            if(i != curInject.mInt)
            {
                curInject.mInt = i;
                curInject.mFloat = 0;
                curInject.mGo = null;
                curInject.mText = null;
                curInject.mBool = false;
                UnityEditor.EditorUtility.SetDirty(serializedObject.targetObject);
                serializedObject.SetIsDifferentCacheDirty();
                serializedObject.Update();
                serializedObject.ApplyModifiedProperties();
                serializedObject.UpdateIfDirtyOrScript();
            }
           
        }
        else if (type == "float")
        {
            float t = EditorGUILayout.FloatField(curInject.mKey + "(" + type + ")", curInject.mFloat);
            if(t != curInject.mFloat)
            {
                curInject.mInt = 0;
                curInject.mFloat = t;
                curInject.mGo = null;
                curInject.mText = null;
                curInject.mBool = false;
                serializedObject.SetIsDifferentCacheDirty();
                UnityEditor.EditorUtility.SetDirty(serializedObject.targetObject);
                serializedObject.Update();
                serializedObject.ApplyModifiedProperties();
                serializedObject.UpdateIfDirtyOrScript();
            }
           
        }
        else if (type == "bool")
        {
            bool temp = EditorGUILayout.Toggle(curInject.mKey + "(" + type + ")", curInject.mBool);
            if(temp != curInject.mBool)
            {
                curInject.mInt = 0;
                curInject.mFloat = 0;
                curInject.mGo = null;
                curInject.mText = null;
                curInject.mBool = temp ;
                UnityEditor.EditorUtility.SetDirty(serializedObject.targetObject);
                serializedObject.SetIsDifferentCacheDirty();
                serializedObject.Update();
                serializedObject.ApplyModifiedProperties();
                serializedObject.UpdateIfDirtyOrScript();
            }
           
        }
        else if(type == "string")
        {
            string temp = EditorGUILayout.TextField(curInject.mKey + "(" + type + ")", curInject.mText);
            if(temp != curInject.mText)
            {
                curInject.mInt = 0;
                curInject.mFloat = 0;
                curInject.mGo = null;
                curInject.mText =  temp;
                curInject.mBool = false;
                UnityEditor.EditorUtility.SetDirty(serializedObject.targetObject);
                serializedObject.SetIsDifferentCacheDirty();
                serializedObject.Update();
                serializedObject.ApplyModifiedProperties();
                serializedObject.UpdateIfDirtyOrScript();
            }
        
        }
        else
        {
            GameObject temp = (GameObject)EditorGUILayout.ObjectField(curInject.mKey + "(" + type + ")", curInject.mGo, typeof(GameObject), true);
            if(temp != curInject.mGo)
            {
                curInject.mInt = 0;
                curInject.mFloat = 0;
                curInject.mGo = temp;
                curInject.mText = null;
                curInject.mBool = false;
                UnityEditor.EditorUtility.SetDirty(serializedObject.targetObject);
                serializedObject.SetIsDifferentCacheDirty();
                serializedObject.UpdateIfDirtyOrScript();
                serializedObject.Update();
                serializedObject.ApplyModifiedProperties();
            }
         
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
