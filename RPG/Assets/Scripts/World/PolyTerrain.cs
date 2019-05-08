using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PolyTerrain : MonoBehaviour
{
	private Vector3[] terrainVertices;
	private GameObject polyTerrainMesh;

	void Start()
	{
		//AssetDatabase.CreateAsset(polyTerrainMesh.GetComponent<MeshFilter>().mesh, "Assets/HillMesh");
		//AssetDatabase.SaveAssets();
	}

	void Update()
	{
		//for (int z = 0; z < polyscale * 10; z++)
		{
			//for (int x = 0; x < polyscale * 10; x++)
			{
				//float f = Perlin.Noise(x * perlinscale + perlinoffset, z * perlinscale + perlinoffset) * heightscale;
			}
		}
	}

	public GameObject polyTerrain(int polyscale, float sizescale, float heightscale, float perlinscale, float perlinoffsetx, float perlinoffsetz, bool deform, float deformamount, int deformseed, Material terrainmat)
	{
		polyscale += 1;
		polyTerrainMesh = new GameObject("Plane");
		polyTerrainMesh.layer = 9;
		polyTerrainMesh.AddComponent<MeshFilter>();
		polyTerrainMesh.GetComponent<MeshFilter>().mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
		polyTerrainMesh.AddComponent<MeshRenderer>();
		polyTerrainMesh.GetComponent<MeshRenderer>().material = terrainmat;
		Random.InitState(deformseed);
		terrainVertices = new Vector3[polyscale * polyscale + (polyscale - 1) * (polyscale - 1)];    //areamap in the first polyscale square, the inbetweens in the second
		int[] terrainTriangles = new int[(polyscale - 1) * (polyscale - 1) * 12];

		for (int z = 0; z < polyscale; z++)
		{
			for (int x = 0; x < polyscale; x++)
			{
				terrainVertices[z * polyscale + x] = new Vector3(x * sizescale, Mathf.PerlinNoise(x * perlinscale + perlinoffsetx, z * perlinscale + perlinoffsetz) * heightscale, z * sizescale);
			}
		}

		for (int z = 0; z < polyscale - 1; z++)
		{
			for (int x = 0; x < polyscale - 1; x++)
			{
				terrainVertices[polyscale * polyscale + ((polyscale - 1) * z) + x] = new Vector3((x + 0.5f) * sizescale, Mathf.PerlinNoise((x + 0.5f) * perlinscale + perlinoffsetx, (z + 0.5f) * perlinscale + perlinoffsetz) * heightscale, (z + 0.5f) * sizescale);
			}
		}

		int cur = 0;
		for (int z = 0; z < polyscale - 1; z++)
		{
			for (int x = 0; x < polyscale - 1; x++)
			{
				//setting triangles
				terrainTriangles[cur + 0] = z * polyscale + x;
				terrainTriangles[cur + 1] = polyscale * polyscale + ((polyscale - 1) * z) + x;
				terrainTriangles[cur + 2] = z * polyscale + x + 1;
				terrainTriangles[cur + 5] = polyscale * polyscale + ((polyscale - 1) * z) + x;
				terrainTriangles[cur + 4] = z * polyscale + x + 1;
				terrainTriangles[cur + 3] = (z + 1) * polyscale + x + 1;
				terrainTriangles[cur + 6] = z * polyscale + x;
				terrainTriangles[cur + 7] = (z + 1) * polyscale + x;
				terrainTriangles[cur + 8] = polyscale * polyscale + ((polyscale - 1) * z) + x;
				terrainTriangles[cur + 11] = (z + 1) * polyscale + x;
				terrainTriangles[cur + 10] = polyscale * polyscale + ((polyscale - 1) * z) + x;
				terrainTriangles[cur + 9] = (z + 1) * polyscale + x + 1;
				//increment
				cur += 12;
			}
		}

		polyTerrainMesh.GetComponent<MeshFilter>().mesh.vertices = terrainVertices;
		polyTerrainMesh.GetComponent<MeshFilter>().mesh.triangles = terrainTriangles;
		polyTerrainMesh.GetComponent<MeshFilter>().mesh.RecalculateNormals();

		Vector2[] terrainUV = new Vector2[terrainVertices.Length];

		for (int z = 0; z < polyscale; z++)
		{
			for (int x = 0; x < polyscale; x++)
			{
				//terrainUV[z * polyscale + x] = new Vector2(areaMap[x, z].z / polyscale / 5, areaMap[x, z].x / polyscale / 5);
			}
		}

		for (int z = 0; z < polyscale - 1; z++)
		{
			for (int x = 0; x < polyscale - 1; x++)
			{
				//terrainUV[polyscale * polyscale + ((polyscale - 1) * z) + x] = new Vector2((areaMap[x, z].z + .5f * sizescale) / polyscale / 5, (areaMap[x, z].x + .5f * sizescale) / polyscale / 5);
			}
		}

		//polyTerrainMesh.GetComponent<MeshFilter>().mesh.uv = terrainUV;

		Texture2D terrainTexture = new Texture2D(polyscale, polyscale, TextureFormat.ARGB32, false);

		Color gr = Color.green;
		gr = new Color(gr.r, gr.g, gr.b);

		for (int z = 0; z < polyscale; z++)
		{
			for (int x = 0; x < polyscale; x++)
			{
				//terrainTexture.SetPixel(x, z, new Color(gr.r * (.4f * areaMap[x, z].y / heightscale) + gr.r / 4, gr.g * (.4f * areaMap[x, z].y / heightscale) + gr.g / 4, gr.b * (.4f * areaMap[x, z].y / heightscale) + gr.b / 4, 0));
			}
		}
		terrainTexture.Apply();

		terrainmat.mainTexture = terrainTexture;

		polyTerrainMesh.AddComponent<MeshCollider>();

		return polyTerrainMesh;
	}
}