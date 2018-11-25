using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateGameObject
{
    //# Shift, % Ctrl & Alt
    [MenuItem("ProjectUtility/Test/Create Game Object %#C")]
    public static void Create()
    {
        GameObject g = new GameObject("New GameObject");
        Debug.Log(Selection.activeObject.name);

        MonoBehaviour b = (MonoBehaviour)Selection.activeObject;
        if(b != null)
        {
            
        }

        //string[] types = AssetDatabase.FindAssets("t:Script");

        //foreach (string str in types)
        //{
        //    Debug.Log(str);
        //}
        //Debug.Log(System.Type.GetType(typeof(CreateGameObject).ToString()).ToString());
    }
}
