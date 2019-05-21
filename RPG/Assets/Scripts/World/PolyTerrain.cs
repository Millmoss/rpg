using System;
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
	private Color[] terrainColors;

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
		//terrainVertices = new Vector3[polyscale * polyscale + (polyscale - 1) * (polyscale - 1)];
		terrainVertices = new Vector3[polyscale * polyscale];
		//int[] terrainTriangles = new int[(polyscale - 1) * (polyscale - 1) * 12];
		int[] terrainTriangles = new int[(polyscale - 1) * (polyscale - 1) * 6];
		terrainColors = new Color[terrainVertices.Length];

		for (int z = 0; z < polyscale; z++)
		{
			for (int x = 0; x < polyscale; x++)
			{
				if (z % 2 == 0)
				{
					terrainVertices[z * polyscale + x] = new Vector3(x * sizescale, gety(x, z), z * sizescale);
					terrainColors[z * polyscale + x] = getcolor(x * sizescale, terrainVertices[z * polyscale + x].y, z * sizescale);
				}
				else
				{
					terrainVertices[z * polyscale + x] = new Vector3((x + 0.5f) * sizescale, gety(x + 0.5f, z), z * sizescale);
					terrainColors[z * polyscale + x] = getcolor((x + 0.5f) * sizescale, terrainVertices[z * polyscale + x].y, z * sizescale);
				}
				//if (x < polyscale - 1 && z < polyscale - 1)
					//terrainVertices[polyscale * polyscale + ((polyscale - 1) * z) + x] = new Vector3((x + 0.5f) * sizescale, gety(x + 0.5f, z + 0.5f), (z + 0.5f) * sizescale);
			}
		}

		int cur = 0;
		for (int z = 0; z < polyscale - 1; z += 2)
		{
			for (int x = 0; x < polyscale - 1; x++)
			{
				terrainTriangles[cur + 2] = z * polyscale + x;
				terrainTriangles[cur + 1] = z * polyscale + x + 1;
				terrainTriangles[cur + 0] = (z + 1) * polyscale + x;
				terrainTriangles[cur + 3] = z * polyscale + x + 1;
				terrainTriangles[cur + 4] = (z + 1) * polyscale + x;
				terrainTriangles[cur + 5] = (z + 1) * polyscale + x + 1;
				cur += 6;
				if (z + 1 < polyscale - 1)
				{
					terrainTriangles[cur + 2] = (z + 1) * polyscale + x;
					terrainTriangles[cur + 1] = (z + 2) * polyscale + x + 1;
					terrainTriangles[cur + 0] = (z + 2) * polyscale + x;
					terrainTriangles[cur + 3] = (z + 1) * polyscale + x;
					terrainTriangles[cur + 4] = (z + 2) * polyscale + x + 1;
					terrainTriangles[cur + 5] = (z + 1) * polyscale + x + 1;
					cur += 6;
				}
				/*
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
				cur += 12;*/
			}
		}

		polyTerrainMesh.GetComponent<MeshFilter>().mesh.vertices = terrainVertices;
		polyTerrainMesh.GetComponent<MeshFilter>().mesh.triangles = terrainTriangles;
		polyTerrainMesh.GetComponent<MeshFilter>().mesh.colors = terrainColors;
		polyTerrainMesh.GetComponent<MeshFilter>().mesh.RecalculateNormals();
		polyTerrainMesh.GetComponent<MeshRenderer>().material = terrainmat;

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

		/*Texture2D terrainTexture = new Texture2D(polyscale, polyscale, TextureFormat.ARGB32, false);

		Color gr = Color.green;
		gr = new Color(gr.r, gr.g, gr.b);

		for (int z = 0; z < polyscale; z++)
		{
			for (int x = 0; x < polyscale; x++)
			{
				//terrainTexture.SetPixel(x, z, new Color(gr.r * (.4f * areaMap[x, z].y / heightscale) + gr.r / 4, gr.g * (.4f * areaMap[x, z].y / heightscale) + gr.g / 4, gr.b * (.4f * areaMap[x, z].y / heightscale) + gr.b / 4, 0));
			}
		}
		terrainTexture.Apply();*/

		//terrainmat.mainTexture = terrainTexture;

		//polyTerrainMesh.AddComponent<MeshCollider>();

		//ObjExporter.MeshToFile(polyTerrainMesh.GetComponent<MeshFilter>(), "Assets/TerrainMesh" + id + ".obj");

		polyTerrainMesh.layer = 0;
		polyTerrainMesh.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		
		return polyTerrainMesh;
	}

	/*public float gety(float xpos, float zpos)		//backup in case shit goes bad
	{
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
	}*/

	public float gety(float xpos, float zpos)
	{
		float amplitude = 1;
		float frequency = 1;
		float y = 0;

		for (int o = 0; o < octaves; o++)
		{
			float sampx = (xpos - 100) * perlinscale * frequency;
			float sampz = (zpos - 100) * perlinscale * frequency;
			float pval = Perlin.Noise(sampx, sampz);
			//float pval = Mathf.PerlinNoise(sampx, sampz) * 2 - 1;

			y += pval * amplitude;
			amplitude *= persistance;
			frequency *= lacunarity;
		}

		float region = getregion(y);
		if (Mathf.Abs(Mathf.Round(region) - region) < 0.01f)
		{

		}

		return y;
	}

	public float getymountain(float xpos, float zpos)
	{
		float amplitude = 1;
		float frequency = 1;
		float y = 0;
		float lac = lacunarity;
		float pers = persistance;

		for (int o = 0; o < 4; o++)
		{
			float sampx = (xpos + perlinoffsetx) * perlinscale * frequency;
			float sampz = (zpos + perlinoffsetz) * perlinscale * frequency;
			float pval = Perlin.Noise(sampx, sampz);
			//float pval = Mathf.PerlinNoise(sampx, sampz) * 2 - 1;

			y += pval * amplitude;
			amplitude *= pers;
			frequency *= lac;
		}

		return y * heightscale;
	}

	public Color getcolor(float xpos, float ypos, float zpos)
	{
		int closest = 0;
		int second = 0;
		float dif = 2;
		float seconddif = 2;
		float yact = ypos / heightscale;
		for (int i = 0; i < regions.Length; i++)
		{
			float d = Mathf.Abs(yact - regions[i].height);
			if (d < dif)
			{
				seconddif = dif;
				second = closest;
				dif = d;
				closest = i;
			}
			else if (d < seconddif)
			{
				seconddif = d;
				second = i;
			}
		}

		float p = 2 * (Mathf.Abs(regions[closest].height - yact) / Mathf.Abs(regions[closest].height - regions[second].height));
		float l = 1;
		if (p > regions[closest].cutoff && regions[closest].height > regions[second].height)
			l = 1 - (p - regions[closest].cutoff) / (1f - regions[closest].cutoff);
		Color r = Color.Lerp(regions[second].color, regions[closest].color, l);

		return r;
	}

	public List<Tuple<int, float>> getregion(float ypos)	//list of regions at this location with the weight of each regions. All weights should equate to 1
	{
		return 0;
	}
}
