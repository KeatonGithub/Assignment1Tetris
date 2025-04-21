using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    public Transform[,] grid;
    public int width, height;
    public GameManager managerScript;

    // Start is called before the first frame update
    void Start()
    {
        grid = new Transform[width, height];
        managerScript = gameObject.GetComponent<GameManager>();
    }

    public void UpdateGrid(Transform tetrimino)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null)
                {
                    if (grid[x, y].parent == tetrimino)
                    {
                        grid[x, y] = null;
                    }
                }
            }
        }
        foreach (Transform mino in tetrimino)
        {
            Vector2 pos = Round(mino.position);
            if (pos.y < height)
            {
                //grid[(int)pos.x, (int)pos.y] = mino;
                int xp = (int)math.round(pos.x);
                int yp = (int)math.round(pos.y);
                grid[(int)pos.x, (int)pos.y] = managerScript.grid[xp,yp].transform;

            }
        }
    }
    public static Vector2 Round(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool IsInsideBoarder(Vector2 pos)
    {
        return (int)pos.x >= 0 && (int)pos.x < width && (int)pos.y >= 0 && (int)pos.y < height;
        //if (vMove())
        // {
        // transform.position -= new Vector3(1, 0, 0);  //redundant
        // Debug.Log("Past");
        //}
    }
    public Transform GEtTransformAtGridPosition(Vector2 pos)
    {
        if (pos.y > height - 1)
        {
            return null;
        }
        return grid[(int)pos.x, (int)pos.y];
    }
    public bool IsValidPosition(Transform tetrimino)
    {
        foreach (Transform mino in tetrimino)
        {
            Vector2 pos = Round(mino.position);
            if (!IsInsideBoarder(pos))
            {
                return false;
            }
            if (GEtTransformAtGridPosition(pos) != null && GEtTransformAtGridPosition(pos).parent != tetrimino)
            {
                return false;
            }
        }
        return true;
    }
}


