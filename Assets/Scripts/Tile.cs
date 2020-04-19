using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public float moisture;

    public Color color;

    const float MARSHY_THRESHOLD = 0.9f;

    public bool isPlantable;

    public bool isMarshy;

    public Sprite sprite;

    public MarshSubTile[] marsh;

    public Vector3 location;

    public int x;

    public int y;

    public Tile(float moisture, Sprite sprite, Vector3 location = new Vector3(), int x = 0, int y = 0, bool isPlantable = true)
    {
        this.moisture = moisture;
        SelectTileColor(moisture);
        this.sprite = sprite;
        this.isPlantable = isPlantable;
        this.location = location;
        this.x = x;
        this.y = y;
        if (this.moisture > MARSHY_THRESHOLD)
        {
            this.isMarshy = true;
        }
        else
        {
            this.isMarshy = false;
        }
    }

    public void SelectTileColor(float moisture)
    {
        float r;
        float g;
        float b;
        if (moisture > 0.8f)
        {
            r = 0f;
            g = 0.4f;
            b = 0.1f;
        }
        else if (moisture > 0.5f)
        {
            r = 0f;
            g = 0.8f - (0.5f * moisture);
            b = 0.2f - (0.125f * moisture);
        }
        else if (moisture > 0.2f)
        {
            r = 0.8f - (2f * moisture);
            g = 0.8f - (0.5f * moisture);
            b = 0.2f;
        }
        else
        {
            r = 0.8f - (1.5f * moisture);
            g = 0.8f - (0.5f * moisture);
            b = 0.2f;
        }
        this.color = new Color(r,g,b);
    }
}

public class MarshSubTile
{
    public Sprite sprite;

    public Point point;

    public float rotation;

    public bool flipX;

    public bool flipY;

    public Vector3 location;

    public Tile tile;

    public MarshSubTile(Sprite sprite, Point point, float rotation, bool flipX, bool flipY, Tile tile)
    {
        this.sprite = sprite;
        this.point = point;
        this.rotation = rotation;
        this.flipX = flipX;
        this.flipY = flipY;
        this.tile = tile;
        float locX = tile.location.x - 0.5f + (point.x/100f);
        float locY = tile.location.y - 0.5f + (point.y/100f);
        this.location = new Vector3(locX, locY, 0.99f);
    }
}

public class Point
{
    public int x;
    
    public int y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
