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
	
	void Start()
    {
		terrain.polyTerrain(polyscale, sizescale, heightscale, perlinscale, 0, 0, deform, deformamount, deformseed, terrainmat);
		for (int x = -2; x < 3; x++)
		{
			for (int z = -2; z < 3; z++)
			{
				if (x == 0 && z == 0)
				{
					continue;
				}

				if ((x <= 1 && x >= -1) && (z <= 1 && z >= -1))
				{
					GameObject temp = terrain.polyTerrain(polyscale / 4, sizescale * 4, heightscale, perlinscale * 4, 0, polyscale * z / (16 * 3 / 5), deform, deformamount, deformseed, terrainmat);
					temp.transform.position = new Vector3(polyscale * sizescale * x, 0, polyscale * sizescale * z);
				}
				else
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
