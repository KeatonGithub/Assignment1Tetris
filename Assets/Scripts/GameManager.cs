using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Tilemaps;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GameManager : MonoBehaviour
{
    public GameObject[] Tetriminos;
    public float MoveFre = 0.8f;
    private float passedTime = 0;
    int height = 20;
    int width = 10;
    public int rando;


    public GameObject CurrentTetrimino;
    public GameObject blockPrefab;
    public GameObject[,] grid;

    public GridScript gridScrip;

    // Start is called before the first frame update
    void Start()
    {
        gridScrip = gameObject.GetComponent<GridScript>();
        grid = new GameObject[width,height];
        SpawnTetrimino();
    }

    // Update is called once per frame
    void Update()
    {
        passedTime += Time.deltaTime;
        if (passedTime >= MoveFre)
        {
            passedTime -= MoveFre;
            MoveTEtrimino(Vector3.down);
        }
        UserInput();
    }
    void UserInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveTEtrimino(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveTEtrimino(Vector3.right);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CurrentTetrimino.transform.Rotate(0, 0, 90);
            if (!IsValidPosition())
            {
                CurrentTetrimino.transform.Rotate(0, 0, -90);
                // transform.RotateAround(transform.TransformPoint(rotationP), new Vector3(0, 0, 1), 90);
                //if (!vMove())
                // transform.RotateAround(rotationP, new Vector3(0, 0, 1), -90);
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            MoveFre = 0.2f;

        }
        else
        {
            MoveFre = 0.8f;
        }
    }
    void SpawnTetrimino()
    {
        int index = UnityEngine.Random.Range(0, Tetriminos.Length);
        CurrentTetrimino = Instantiate(Tetriminos[index], new Vector3(5, 19, 0), Quaternion.identity);
    }

    void MoveTEtrimino(Vector3 dircetion)
    {
        CurrentTetrimino.transform.position += dircetion;
        if (!IsValidPosition())
        {
            CurrentTetrimino.transform.position -= dircetion;
            if (dircetion == Vector3.down)
            {
                BlockSet();
                GetComponent<GridScript>().UpdateGrid(CurrentTetrimino.transform);
                
                
                CheckForLines();
                CheckForLines();
                CheckForLines();
                SpawnTetrimino();
                

            }
        }
    }
    bool IsValidPosition()
    {
        return GetComponent<GridScript>().IsValidPosition(CurrentTetrimino.transform);
    }

    void BlockSet()
    {
        int blocksize = 4;
        if (CurrentTetrimino.tag ==  "I-tetrimino")
        {
            blocksize = 5;
        }
        GameObject[] block = new GameObject[blocksize];

        for (int i = 0; i < blocksize; i++)
        {
            
            
            block[i] = CurrentTetrimino.transform.GetChild(i).gameObject;
            

            //Vector3 pos;
            Vector3 pos = new Vector3(block[i].transform.position.x, block[i].transform.position.y, 0);
            GameObject newBlock = Instantiate(blockPrefab, pos, Quaternion.identity); //force all blocks to spawn at z zero. fixes the block tx not appearing in game view;
            newBlock.GetComponent<SpriteRenderer>().color = block[i].GetComponent<SpriteRenderer>().color;

            int x = (int)Math.Round(block[i].transform.position.x);
            int y = (int)Math.Round(block[i].transform.position.y);

            grid[x,y] = newBlock;
        }
        Debug.Log($"CurrentTetrimino '{CurrentTetrimino.name}' has {CurrentTetrimino.transform.childCount} children.");

        Destroy(CurrentTetrimino);


    }
    void CheckForLines()
    {
        for (int y = 0; y < height; y++)
        {
            if (IsLineFull(y))
            {
                ClearLine(y);
                ShiftRowsDown(y);
            }
        }
    }
    bool IsLineFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
            {
                Debug.Log("eau");
                return false; //full row
                
            }
        }
        Debug.Log("nonfl");
        return true; //not full
    }
    void ClearLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, y].gameObject);
            gridScrip.grid[x, y] = null;
        }
    }
    void ShiftRowsDown(int clearRow)
    {
        for (int y = clearRow; y < height - 1; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[x, y] = grid[x, y + 1];
                if (grid[x, y] != null)
                {
                    grid[x, y].transform.position += Vector3.down;
                }
                grid[x, y + 1] = null;

                

                gridScrip.grid[x, y] = gridScrip.grid[x, y + 1];
                if (gridScrip.grid[x, y] != null)
                {
                    //gridScrip.grid[x, y].transform.position += Vector3.down;
                }
                gridScrip.grid[x, y + 1] = null;
            }
        }

    }
}

