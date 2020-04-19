using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController
{
    public int size;

    public List<List<Tile>> grid;

    public Sprite[] terrainSprites;

    public Sprite[] marshSprites;

    public TileController(int size)
    {
        this.size = size;
        this.terrainSprites = Resources.LoadAll<Sprite>("Sprites/Terrain");
        this.marshSprites = Resources.LoadAll<Sprite>("Sprites/Marsh");
        this.grid = new List<List<Tile>>();
    }

    public void InitGrid()
    {
        // Make List of List of Tiles
        for (int i = 0; i < this.size; i++)
        {
            List<Tile> gridX = new List<Tile>();
            for (int j = 0; j < this.size; j++)
            {
                // Use neighbouring tiles to define current tile's moisture +/- 20%
                float moistureNorth = 0.5f;
                float moistureWest = 0.5f;
                float moistureNorthWest = 0.5f;
                if (i > 0)
                {
                    moistureNorth = grid[i-1][j].moisture;
                }
                if (j > 0)
                {
                    moistureWest = gridX[j-1].moisture;
                }
                if (i > 0 && j > 0)
                {
                    moistureNorthWest = grid[i-1][j-1].moisture;
                }
                float moisture = (moistureNorth + moistureWest + moistureNorthWest)/3.0f * Random.Range(0.8f, 1.2f);

                // Construct tile
                Sprite ts = this.terrainSprites[Random.Range(0, terrainSprites.Length)];
                Vector3 location = new Vector3(-size/2f+j, -size/2f+i, 1f);
                Tile t = new Tile(moisture, ts, location, j, i, true);

                // Add marshy ponds if over moisture threshold
                if (t.isMarshy)
                {
                    List<MarshSubTile> marshSubTiles = new List<MarshSubTile>();
                    for (int k = 0; k < Random.Range(1,8); k++)
                    {
                        Sprite ms = this.marshSprites[Random.Range(0, marshSprites.Length)];
                        float rot = Random.Range(0f, 1f);
                        Point mp = new Point(Random.Range(0, 80), Random.Range(0, 80));
                        bool flipX = (Random.Range(0, 1) == 1);
                        bool flipY = (Random.Range(0, 1) == 1);
                        marshSubTiles.Add(new MarshSubTile(ms, mp, rot, flipX, flipY, t));
                    }
                    t.marsh = marshSubTiles.ToArray();
                }
                gridX.Add(t);
            }
            grid.Add(gridX);
        }
    }
}
