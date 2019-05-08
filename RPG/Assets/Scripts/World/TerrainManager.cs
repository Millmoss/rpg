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
	public float perlinoffset = 100;
	public bool deform = false;
	public float deformamount = .1f;
	public int deformseed = 15;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
