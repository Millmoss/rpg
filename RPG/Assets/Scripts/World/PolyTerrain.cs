using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PolyTerrain : MonoBehaviour
{
	private Material terrainmat;
	private int polyscale;
	private float heightscale;
	private float sizescale;
	private float perlinscale;
	private bool deform;
	private float deformamount;
	private int deformseed;
	private int lod;
	private float perlinoffsetx;
	private float perlinoffsetz;
	private int octaves;
	private float persistance;
	private float lacunarity;
	private TerrainType[] regions;

	private Vector3[] terrainVertices;
	private GameObject polyTerrainMesh;

	public GameObject polyTerrain(TerrainManager tm, float perox, float peroz, int l)
	{
		polyscale = tm.polyscale;
		sizescale = tm.sizescale;
		heightscale = tm.heightscale;
		perlinscale = tm.perlinscale;
		deform = tm.deform;
		deformamount = tm.deformamount;
		deformseed = tm.deformseed;
		terrainmat = tm.terrainmat;
		octaves = tm.octaves;
		persistance = tm.persistance;
		lacunarity = tm.lacunarity;
		regions = tm.regions;

		perlinoffsetx = perox;
		perlinoffsetz = peroz;
		lod = l;

		polyscale += 1;
		polyTerrainMesh = new GameObject("Terrain");
		polyTerrainMesh.layer = 9;
		polyTerrainMesh.AddComponent<MeshFilter>();
		polyTerrainMesh.GetComponent<MeshFilter>().mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
		polyTerrainMesh.AddComponent<MeshRenderer>();
		polyTerrainMesh.GetComponent<MeshRenderer>().material = terrainmat;
		Random.InitState(deformseed);
		terrainVertices = new Vector3[polyscale * polyscale + (polyscale - 1) * (polyscale - 1)];
		int[] terrainTriangles = new int[(polyscale - 1) * (polyscale - 1) * 12];

		for (int z = 0; z < polyscale; z++)
		{
			for (int x = 0; x < polyscale; x++)
			{
				terrainVertices[z * polyscale + x] = new Vector3(x * sizescale, gety(x, z), z * sizescale);
				if (x < polyscale - 1 && z < polyscale - 1)
					terrainVertices[polyscale * polyscale + ((polyscale - 1) * z) + x] = new Vector3((x + 0.5f) * sizescale, gety(x + 0.5f, z + 0.5f), (z + 0.5f) * sizescale);
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

		Vector3[] terrainNormals = polyTerrainMesh.GetComponent<MeshFilter>().mesh.normals;
		
		for (int z = 0; z < polyscale; z++)
		{
			for (int x = 0; x < polyscale; x++)
			{
				Vector3 n = Vector3.zero;
				if (x == 0 || x == polyscale - 1 || z == 0 || z == polyscale - 1)
				{
					Vector3 posa = new Vector3((x - 1) * sizescale, gety(x - 1, z), (z) * sizescale);
					Vector3 posc = new Vector3((x + 1) * sizescale, gety(x + 1, z), (z) * sizescale);
					Vector3 posb = new Vector3((x) * sizescale, gety(x, (z - 1)), (z - 1) * sizescale);
					Vector3 posd = new Vector3((x) * sizescale, gety(x, (z + 1)), (z + 1) * sizescale);

					Vector3 va = posa - terrainVertices[z * polyscale + x];
					Vector3 vb = posb - terrainVertices[z * polyscale + x];
					n += Vector3.Cross(va, vb);
					va = posb - terrainVertices[z * polyscale + x];
					vb = posc - terrainVertices[z * polyscale + x];
					n += Vector3.Cross(va, vb);
					va = posc - terrainVertices[z * polyscale + x];
					vb = posd - terrainVertices[z * polyscale + x];
					n += Vector3.Cross(va, vb);
					va = posd - terrainVertices[z * polyscale + x];
					vb = posa - terrainVertices[z * polyscale + x];
					n += Vector3.Cross(va, vb);
					n = n / 4;
					n.Normalize();
					terrainNormals[z * polyscale + x] = -n;
				}
				else
				{
					//skips a bunch of iterations
				}
			}
		}
		for (int z = 0; z < polyscale - 1; z++)
		{
			for (int x = 0; x < polyscale - 1; x++)
			{

			}
		}

		polyTerrainMesh.GetComponent<MeshFilter>().mesh.normals = terrainNormals;

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

		//polyTerrainMesh.AddComponent<MeshCollider>();

		//ObjExporter.MeshToFile(polyTerrainMesh.GetComponent<MeshFilter>(), "Assets/TerrainMesh" + id + ".obj");

		polyTerrainMesh.layer = 0;
		polyTerrainMesh.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		
		return polyTerrainMesh;
	}

	public float gety(float xpos, float zpos)
	{
		//original crappy terrain code
		/*float y = Perlin.Noise(xpos * perlinscale + perlinoffsetx, zpos * perlinscale + perlinoffsetz) * heightscale;
		float m = Mathf.PerlinNoise(xpos * mperlinscale + (perlinoffsetx + mountainoffset) * mperlinscale, zpos * mperlinscale + (perlinoffsetz + mountainoffset) * mperlinscale);
		float p = Mathf.PerlinNoise(xpos * pperlinscale + (perlinoffsetx + plainsoffset) * pperlinscale, zpos * pperlinscale + (perlinoffsetz + plainsoffset) * pperlinscale);
		if (m > mountaincutoff)
		{
			m = (m - mountaincutoff) / (1f - mountaincutoff);
			y = y * (1 + m * m * mheightscale);
		}
		else
			y = y * p * p;
		return y;*/

		//better terrain code

		float amplitude = 1;
		float frequency = 1;
		float y = 0;

		for (int o = 0; o < octaves; o++)
		{
			float sampx = (xpos + perlinoffsetx) * perlinscale * frequency;
			float sampz = (zpos + perlinoffsetz) * perlinscale * frequency;
			float pval = Perlin.Noise(sampx, sampz);
			//float pval = Mathf.PerlinNoise(sampx, sampz) * 2 - 1;

			y += pval * amplitude;
			amplitude *= persistance;
			frequency *= lacunarity;
		}

		return y * heightscale;
	}

	public Color getcolor(float xpos, float ypos, float zpos)
	{
		int closest = 0;
		float dif = 2;
		float yact = ypos / heightscale;
		for (int i = 0; i < regions.Length; i++)
		{
			float d = Mathf.Abs(Mathf.Abs(yact) - Mathf.Abs(regions[i].height));
			if (d < dif)
			{
				dif = d;
				closest = i;
			}
		}
		return regions[closest].color;
	}

	public int getregion(float xpos, float ypos, float zpos)
	{
		return 0;
	}
}
