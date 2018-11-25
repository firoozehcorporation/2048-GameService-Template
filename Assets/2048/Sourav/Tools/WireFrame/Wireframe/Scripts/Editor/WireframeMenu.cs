using UnityEngine;
using System.Collections;
using Sourav.Utilities.WireFrame.MeshTools;
using System.Linq;
using UnityEditor;

namespace Sourav.Utilities.WireFrame.Integration
{
    public static class WireframeMenu
    {
        private const string m_root = "Sourav/Tools/WireFrame/Wireframe/";

        public static GameObject InstantiatePrefab(string name)
        {
            Object prefab = AssetDatabase.LoadAssetAtPath("Assets/" + m_root + "Prefabs/" + name, typeof(GameObject));
            return (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        }

        [MenuItem("ProjectUtility/Tools/Wireframe/Create Projector")]
        public static void CreateProjector()
        {
            GameObject wp = InstantiatePrefab("WireframeProjector.prefab");
            Undo.RegisterCreatedObjectUndo(wp, "Wireframe.CreateProjector");
            PrefabUtility.DisconnectPrefabInstance(wp);
            Selection.activeObject = wp;
        }

        [MenuItem("ProjectUtility/Tools/Wireframe/Prepare", validate = true)]
        public static bool CanPrepareMesh()
        {
            return Selection.gameObjects.Length > 0 &&
                (Selection.gameObjects.Any(g => g.GetComponent<MeshFilter>()) ||
                 Selection.gameObjects.Any(g => g.GetComponent<SkinnedMeshRenderer>()));
        }

        [MenuItem("ProjectUtility/Tools/Wireframe/Prepare")]
        public static void PrepareMesh()
        {
            GameObject[] gos = Selection.gameObjects;
            for(int i = 0; i < gos.Length; ++i)
            {
                MeshFilter mf = gos[i].GetComponent<MeshFilter>();
                if(mf != null && mf.sharedMesh != null)
                {
                    mf.sharedMesh = PrepareMesh(gos[i], mf, mf.sharedMesh);
                }
                SkinnedMeshRenderer smr = gos[i].GetComponent<SkinnedMeshRenderer>();
                if(smr != null && smr.sharedMesh != null)
                {
                    smr.sharedMesh = PrepareMesh(gos[i], smr, smr.sharedMesh);
                }
            }
        }

        private static Mesh PrepareMesh(GameObject go, Component c, Mesh mesh)
        {
            Undo.RecordObject(c, "Wireframe.PrepareMesh");
            Mesh duplicate = Object.Instantiate(mesh);
            duplicate.name = mesh.name;
            Barycentric.CalculateBarycentric(duplicate);
            EditorUtility.SetDirty(go);
            return duplicate;
        }

        [MenuItem("ProjectUtility/Tools/Wireframe/Apply", validate = true)]
        public static bool CanApplyWireframeMaterial()
        {
            return Selection.gameObjects.Length > 0 &&
                (Selection.gameObjects.Any(g => g.GetComponent<MeshFilter>()) ||
                 Selection.gameObjects.Any(g => g.GetComponent<SkinnedMeshRenderer>()));
        }

        [MenuItem("ProjectUtility/Tools/Wireframe/Apply")]
        public static void ApplyWireframeMaterial()
        {
            Material wfm = Resources.Load<Material>("WireframeDefault");

            GameObject[] gos = Selection.gameObjects;
            for (int i = 0; i < gos.Length; ++i)
            {
                MeshRenderer mr = gos[i].GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    mr.sharedMaterials = ApplyWireframeMaterial(wfm, gos[i], mr, mr.sharedMaterials);
                }
                SkinnedMeshRenderer smr = gos[i].GetComponent<SkinnedMeshRenderer>();
                if(smr != null)
                {
                    smr.sharedMaterials = ApplyWireframeMaterial(wfm, gos[i], smr, smr.sharedMaterials);
                }
            }
        }

        private static Material[] ApplyWireframeMaterial(Material wfm, GameObject go, Component c, Material[] materials)
        {
            Undo.RecordObject(c, "Wireframe.ApplyWireframeMaterial");
            if (materials.Length == 0)
            {
                System.Array.Resize(ref materials, 1);
            }

            for (int m = 0; m < materials.Length; m++)
            {
                materials[m] = wfm;
            }
            EditorUtility.SetDirty(go);
            return materials;
        }

        [MenuItem("ProjectUtility/Tools/Wireframe/Save Mesh", validate = true)]
        public static bool CanSaveMesh()
        {
            GameObject[] selectedObjects = Selection.gameObjects;
            if (selectedObjects.Length == 0)
            {
                return false;
            }

            return selectedObjects.Any(so => so.GetComponent<MeshFilter>() != null);
        }

        [MenuItem("ProjectUtility/Tools/Wireframe/Save Mesh")]
        public static void SaveMesh()
        {
            GameObject[] selectedObjects = Selection.gameObjects;
            MeshUtils.SaveMesh(selectedObjects, "WireFrame/");
        }

        public static void SaveMesh(Mesh mesh, string root, string name)
        {
            if (!System.IO.Directory.Exists(Application.dataPath + "/" + root + "/SavedMeshes"))
            {
                AssetDatabase.CreateFolder("Assets/" + root.Remove(root.Length - 1), "SavedMeshes");
            }

            string path = "Assets/" + root + "SavedMeshes/" + name + ".prefab";
            if (string.IsNullOrEmpty(AssetDatabase.GetAssetPath(mesh)))
            {
                AssetDatabase.CreateAsset(mesh, AssetDatabase.GenerateUniqueAssetPath(path));
                Debug.Log("Mesh saved: " + path);
            }

            AssetDatabase.SaveAssets();
        }
    }

}
