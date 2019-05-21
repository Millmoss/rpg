using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
	public Material terrainmat;
	public int polyscale = 100;
	public float heightscale = 10;
	public float sizescale = .5f;
	public float perlinscale = .0375f;
	public bool deform = false;
	public float deformamount = .1f;
	public int deformseed = 15;
	public PolyTerrain terrain;
	public int gennum = 5;
	public int octaves = 1;
	public float persistance = 1;
	public float lacunarity = 2;

	public TerrainType[] regions;

	void Start()
	{
		int cent = Mathf.CeilToInt(((float)gennum) / 2f);

		// * ~ * ~ * this is garbage but it fixes a bug * ~ * ~ *
		GameObject fixer = terrain.polyTerrain(this, polyscale * cent, polyscale * cent, 0);
		Destroy(fixer);
		fixer = terrain.polyTerrain(this, polyscale * cent, polyscale * cent, 0);
		Destroy(fixer);
		// * ~ * ~ * this is garbage but it fixes a bug * ~ * ~ *

		for (int x = 0; x < gennum; x++)
		{
			for (int z = 0; z < gennum; z++)
			{
				//if (cent == x && cent == z)
				{
					//continue;
				}

				//if ((x <= cent + 1 && x >= cent - 1) && (z <= cent + 1 && z >= cent - 1))
				{
					GameObject temp = terrain.polyTerrain(this, polyscale * x, polyscale * z, 0);
					temp.transform.position = new Vector3(polyscale * sizescale * (x - cent), 0, polyscale * sizescale * (z - cent));
				}
				//else
				{
					//GameObject temp = terrain.polyTerrain(polyscale / 2, sizescale * 2, heightscale, perlinscale * 2, polyscale * sizescale * x, polyscale * sizescale * z, deform, deformamount, deformseed, terrainmat);
					//temp.transform.position = new Vector3(polyscale * sizescale * x, 0, polyscale * sizescale * z);
				}
			}
		}
    }
	
    void Update()
    {
        
    }
}

[System.Serializable]
public struct TerrainType
{
	public string name;
	public float height;
	public float cutoff;
	public Color color;
}

public enum Region
{
	Plains,
	Hills,
	Mountain,
	
}