using UnityEngine;
using System.Collections;

public class CreateTilemap : MonoBehaviour 
{
	public Texture2D map;

	public GameObject floor;
	public Color floorColor;

	public GameObject wall;
	public Color wallColor;

	public GameObject platform;
	public Color platformColor;

	public GameObject column;
	public Color columnColor;

	public GameObject ladder;
	public Color ladderColor;

	public GameObject playerSpawn;
	public Color playerSpawnColor;



	void Start() 
	{
		for (int y = 0; y < map.height; y++)
		{
			for (int x = 0; x < map.width; x++)
			{
				Color clr = map.GetPixel(x, y);
				if (clr == floorColor)
					CreateTile(new Vector3(x, y, 0), floor);
				else if (clr == wallColor)
					CreateTile(new Vector3(x, y, 0), wall);
				else if (clr == platformColor)
				{
					CreateTile(new Vector3(x, y, 0), platform);
				}
				else if (clr == columnColor)
				{
					CreateTile(new Vector3(x, y, 0), column);
				}
				else if (clr == ladderColor)
				{
					CreateTile(new Vector3(x, y, 0), ladder);
				}
				else if (clr == playerSpawnColor)
				{
					CreateTile(new Vector3(x, y, 0), playerSpawn);
				}
			}
		}
	}

	void CreateTile(Vector3 pos, GameObject tile)
	{
		var t = Instantiate(tile, pos, Quaternion.identity) as GameObject;
		t.transform.parent = transform;
	}
}
