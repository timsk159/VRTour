using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildGlobe
{
    public class GlobeMeshData
    {
        public VertexAttribs attribs;
        public int[] triangleIndices;

        public GlobeMeshData(VertexAttribs attribs, int[] triangleIndices)
        {
            this.attribs = attribs;
            this.triangleIndices = triangleIndices;
        }
    }
    public class VertexAttribs
    {
        public Vector3[] position;
        //public List<Vector3> normal;
        //public List<Vector3> tangent;
        //public List<Vector3> binormal;
        public Vector4[] color;
        public Vector2[] uv0;
        public Vector2[] uv1;
    }

    public static Mesh CreateMesh(GlobeMeshData meshData)
    {
        var mesh = new Mesh();
        mesh.vertices = meshData.attribs.position;
        mesh.uv = meshData.attribs.uv0;
        mesh.triangles = meshData.triangleIndices;
        return mesh;
    }

    public static GlobeMeshData GetGlobeData()
    {
        float uScale = 1.0f;
        float vScale = 1.0f;
        // Make four rows at the polar caps in the place of one
        // to diminish the degenerate triangle issue.
        int poleVertical = 3;
        int uniformVertical = 64;
        int horizontal = 128;
        int vertical = uniformVertical + poleVertical * 2;
        float radius = 100.0f;

        int vertexCount = (horizontal + 1) * (vertical + 1);

        VertexAttribs attribs = new VertexAttribs();
        attribs.position = new Vector3[vertexCount];
        attribs.uv0 = new Vector2[vertexCount];
        attribs.color = new Vector4[vertexCount];

        for (int y = 0; y <= vertical; y++)
        {
            float yf;
            if (y <= poleVertical)
            {
                yf = (float)y / (poleVertical + 1) / uniformVertical;
            }
            else if (y >= vertical - poleVertical)
            {
                yf = (float)(uniformVertical - 1 + ((float)(y - (vertical - poleVertical - 1)) / (poleVertical + 1))) / uniformVertical;
            }
            else
            {
                yf = (float)(y - poleVertical) / uniformVertical;
            }
            float lat = (yf - 0.5f) * Mathf.PI;
            float cosLat = Mathf.Cos(lat);
            for (int x = 0; x <= horizontal; x++)
            {
                float xf = (float)x / (float)horizontal;
                float lon = (0.5f + xf) * Mathf.PI * 2;
                int curIndex = y * (horizontal + 1) + x;

                if (x == horizontal)
                {
                    // Make sure that the wrap seam is EXACTLY the same
                    // xyz so there is no chance of pixel cracks.
                    attribs.position[curIndex] = attribs.position[y * (horizontal + 1) + 0];
                }
                else
                {
                    var posX = radius * Mathf.Cos(lon) * cosLat;
                    var posY = radius * Mathf.Sin(lon) * cosLat;
                    var posZ = radius * Mathf.Sin(lat);
                    attribs.position[curIndex] = new Vector3(posX, posY, posZ);
                }

                // With a normal mapping, half the triangles degenerate at the poles,
                // which causes seams between every triangle.  It is better to make them
                // a fan, and only get one seam.
                attribs.uv0[curIndex] = new Vector2(y == 0 || y == vertical ? 0.5f : xf * uScale, (1.0f - yf) * vScale);

                attribs.color[curIndex] = new Vector4(1, 1, 1, 1);
            }
        }

        int[] triangleIndices = new int[horizontal * vertical * 6];

        int index = 0;
        for (int x = 0; x < horizontal; x++)
        {
            for (int y = 0; y < vertical; y++)
            {
                triangleIndices[index + 0] = y * (horizontal + 1) + x;
                triangleIndices[index + 1] = y * (horizontal + 1) + x + 1;
                triangleIndices[index + 2] = (y + 1) * (horizontal + 1) + x;
                triangleIndices[index + 3] = (y + 1) * (horizontal + 1) + x;
                triangleIndices[index + 4] = y * (horizontal + 1) + x + 1;
                triangleIndices[index + 5] = (y + 1) * (horizontal + 1) + x + 1;
                index += 6;
            }
        }

        Vector2[] vec2UVs = attribs.uv0;

        for (int i = 0; i < vec2UVs.Length; i++)
        {
            vec2UVs[i] = new Vector2(1.0f - vec2UVs[i].x, vec2UVs[i].y);
        }

        attribs.uv0 = vec2UVs;

        return new GlobeMeshData(attribs, triangleIndices);
    }
}