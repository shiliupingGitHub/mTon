using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
public class mTonPostprocessor : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        mTonBehaviourInspecter.InitClass();
    }


}
