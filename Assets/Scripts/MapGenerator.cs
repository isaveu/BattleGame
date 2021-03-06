﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public int SetNewMap(Vector2 size, Vector2 pos, int _seed, int id)
    {
        gameObject.GetComponent<MeshRenderer>().material = MapMaterials[id - 1];
        //Debug.Log("setting map of size " + size + " at position " + pos);
        width = Mathf.RoundToInt(size.x);
        height = Mathf.RoundToInt(size.y);
        gameObject.transform.position = new Vector3(pos.x, 0, pos.y);
        seed = _seed == -1 ? Random.Range(0, 999) : _seed;
        Debug.Log("using seed "+  seed);
        GenerateMap();
        
        return seed;
    }
    private Renderer textureRender;

    public List<Material> MapMaterials;
    public enum DrawMode {NoiseMap, ColourMap};
	public DrawMode drawMode; 

	public int width;
	public int height;
	public float noiseScale;

	public int octaves;

	[Range(0,1)]
	public float persistance;
	public float lacunarity; 

	public int seed; 
	public Vector2 offset;


	public bool autoUpdate;  

	public TerrainType[] regions;  //set the regions in here if they won't change

    public void DrawTexture(Texture2D texture)
    {
        gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = texture;
        //textureRender.transform.localScale = new Vector3 (width, 1, height);
        gameObject.GetComponent<MeshRenderer>().transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public int GetSeed()
    {
        return seed;
    }
    public Color[] colourMap; 

	public void GenerateMap()
    {
		float[,] noiseMap = Noise.GenerateNoiseMap (width, height, seed, noiseScale, octaves, persistance, lacunarity, offset);

		colourMap = new Color[width * height];
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				float currentHeight = noiseMap [x, y];
				//now finf whihc region this current height falls within
				for (int i = 0; i < regions.Length; i++) {
					if (currentHeight <= regions [i].height) {
						colourMap [y * width + x] = regions [i].colour;
						break;

					}
				}
			}
		}
		DrawTexture(TextureGenerator.TextureFromColourMap (colourMap, width, height));
	}

	void OnValidate(){
		if (width < 1) {
            width = 1;
		}
		if (height < 1) {
            height = 1;
		}
		if (lacunarity < 1) {
			lacunarity = 1;
		}
		if (octaves < 0) {
			octaves = 0;
		}
	}
}

[System.Serializable]
public struct TerrainType{
	public string name;
	public float height;
	public Color colour;

}

