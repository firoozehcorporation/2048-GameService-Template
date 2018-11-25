using UnityEngine;
using System.Collections.Generic;

namespace Sourav.Utilities.WireFrame
{
    public class Barycentric : MonoBehaviour
    {
        const int val = 255;
        private static Color32[] palette = { new Color32(val, 0, 0, 0), new Color32(0, val, 0, 0), new Color32(0, 0, val, 0) };

        public static void CalculateBarycentric(Mesh mesh)
        {
            int[] tris = mesh.triangles;
            List<int> err;
       
            Color32[] barycentric = GetBarycentricCoordinates(mesh.vertexCount, tris, out err);
            List<Color32> colors = new List<Color32>(barycentric);

            if (err.Count > 0)
            {
                List<Vector3> vertices = new List<Vector3>(mesh.vertices);
                List<Vector3> normals = new List<Vector3>(mesh.normals);
                
                List<Vector4> tangents = new List<Vector4>(mesh.tangents);
                List<Vector2> uv = new List<Vector2>(mesh.uv);
                List<Vector2> uv2 = new List<Vector2>(mesh.uv2);
                List<Vector2> uv3 = new List<Vector2>(mesh.uv3);
                List<Vector2> uv4 = new List<Vector2>(mesh.uv4);
                List<BoneWeight> boneWeights = new List<BoneWeight>(mesh.boneWeights);

                int[] triangles = tris;
                for (int i = 0; i < err.Count; ++i)
                {
                    int t = err[i];

                    int i0 = triangles[t];
                    int i1 = triangles[t + 1];
                    int i2 = triangles[t + 2];

                    Vector3 v0 = vertices[i0];
                    Vector3 v1 = vertices[i1];
                    Vector3 v2 = vertices[i2];

                    triangles[t] = vertices.Count;
                    vertices.Add(v0);
                    triangles[t + 1] = vertices.Count;
                    vertices.Add(v1);
                    triangles[t + 2] = vertices.Count;
                    vertices.Add(v2);
                    colors.Add(palette[0]);
                    colors.Add(palette[1]);
                    colors.Add(palette[2]);

                    if (normals.Count > 0)
                    {
                        Vector3 n0 = normals[i0];
                        Vector3 n1 = normals[i1];
                        Vector3 n2 = normals[i2];
                        normals.Add(n0);
                        normals.Add(n1);
                        normals.Add(n2);
                    }

                    if (tangents.Count > 0)
                    {
                        Vector4 t0 = tangents[i0];
                        Vector4 t1 = tangents[i1];
                        Vector4 t2 = tangents[i2];
                        tangents.Add(t0);
                        tangents.Add(t1);
                        tangents.Add(t2);
                    }

                    if (uv.Count > 0)
                    {
                        Vector2 u0 = uv[i0];
                        Vector2 u1 = uv[i1];
                        Vector2 u2 = uv[i2];
                        uv.Add(u0);
                        uv.Add(u1);
                        uv.Add(u2);
                    }

                    if (uv2.Count > 0)
                    {
                        Vector2 u0 = uv2[i0];
                        Vector2 u1 = uv2[i1];
                        Vector2 u2 = uv2[i2];
                        uv2.Add(u0);
                        uv2.Add(u1);
                        uv2.Add(u2);
                    }

                    if (uv3.Count > 0)
                    {
                        Vector2 u0 = uv3[i0];
                        Vector2 u1 = uv3[i1];
                        Vector2 u2 = uv3[i2];
                        uv3.Add(u0);
                        uv3.Add(u1);
                        uv3.Add(u2);
                    }

                    if (uv4.Count > 0)
                    {
                        Vector2 u0 = uv4[i0];
                        Vector2 u1 = uv4[i1];
                        Vector2 u2 = uv4[i2];
                        uv4.Add(u0);
                        uv4.Add(u1);
                        uv4.Add(u2);
                    }

                    if (boneWeights.Count > 0)
                    {
                        BoneWeight b0 = boneWeights[i0];
                        BoneWeight b1 = boneWeights[i1];
                        BoneWeight b2 = boneWeights[i2];

                        boneWeights.Add(b0);
                        boneWeights.Add(b1);
                        boneWeights.Add(b2);
                    }
                }

                mesh.SetVertices(vertices);

                int offset = 0;
                for(int s = 0; s < mesh.subMeshCount; ++s)
                {
                    int[] stris = mesh.GetTriangles(s);
                    System.Array.Copy(tris, offset, stris, 0, stris.Length);
                    offset += stris.Length;
                    mesh.SetTriangles(stris, s);
                }

                mesh.SetColors(colors);
                mesh.SetNormals(normals);
                mesh.SetTangents(tangents);
                mesh.SetUVs(0, uv);
                mesh.SetUVs(1, uv2);
                mesh.SetUVs(2, uv3);
                mesh.SetUVs(3, uv4);
                mesh.boneWeights = boneWeights.ToArray();
            }
            else
            {
                mesh.SetColors(colors);
            }
        }

        private static Color32[] GetBarycentricCoordinates(
            int verticesLength, 
            int[] tris,
            out List<int> err)
        {
            err = new List<int>();

            int[] colorIndices = new int[verticesLength];            
            const int undefined = 0;
            for (int i = 0; i < tris.Length; i += 3)
            {
                int v0 = tris[i];
                int v1 = tris[i + 1];
                int v2 = tris[i + 2];

                int c0 = colorIndices[v0];
                int c1 = colorIndices[v1];
                int c2 = colorIndices[v2];

                if (c0 == undefined)
                {
                    for (int c = 1; c <= 3; c++)
                    {
                        if (c != c1 && c != c2)
                        {
                            colorIndices[v0] = c;
                            c0 = c;
                            break;
                        }
                    }
                }
                if (c1 == undefined)
                {
                    for (int c = 1; c <= 3; c++)
                    {
                        if (c != c0 && c != c2)
                        {
                            colorIndices[v1] = c;
                            c1 = c;
                            break;
                        }
                    }
                }
                if (c2 == undefined)
                {
                    for (int c = 1; c <= 3; c++)
                    {
                        if (c != c0 && c != c1)
                        {
                            colorIndices[v2] = c;
                            c2 = c;
                            break;
                        }
                    }
                }

                if (c0 == c1 || c0 == c2 || c1 == c2)
                {
                    err.Add(i);
                }
            }

            Color32[] colors = new Color32[colorIndices.Length];
            for (int i = 0; i < colorIndices.Length; i++)
            {
                if (colorIndices[i] != undefined)
                {
                    //vertex is not referenced by triangles
                    //colorIndices[i] = 1;
                    colors[i] = palette[colorIndices[i] - 1];
                }
                
            }
            return colors;
        }

        private void Start()
        {
            MeshFilter filter = GetComponent<MeshFilter>();
            if(filter != null)
            {
                Mesh mesh = filter.sharedMesh;
                if (mesh != null)
                {
                    CalculateBarycentric(mesh);
                }
            }

            SkinnedMeshRenderer skinned = GetComponent<SkinnedMeshRenderer>();
            if(skinned != null)
            {
                Mesh mesh = skinned.sharedMesh;
                if (mesh != null)
                {
                    CalculateBarycentric(mesh);
                }
            }    
        }

        
        static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

    }

}
