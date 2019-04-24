using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Main_v2 : MonoBehaviour
{

    enum Direction
    {
        Up,Down,Left,Right,NULL
    }
    Direction direction = Direction.NULL;
    bool newPoint = true;
    int[,] points = new int[4,4];
    public Text[] HudPoint = new Text[16];

    int size = 4;

    void Start()
    {
        for(int i=0;i<4;i++)
            for(int j=0;j<4;j++)
                points[i,j]=0;
        //CreateNewPoint(0,0,8);
        //CreateNewPoint(0,1,4);
        //CreateNewPoint(0,2,2);
        //CreateNewPoint(0,3,2);

        //CreateNewPoint(1,0,2);
        //CreateNewPoint(1,1,2);
        //CreateNewPoint(1,2,4);
        //CreateNewPoint(1,3,8);

        //CreateNewPoint(0, 0, 8);
        //CreateNewPoint(1, 0, 4);
        //CreateNewPoint(2, 0, 2);
        //CreateNewPoint(3, 0, 2);

        //CreateNewPoint(0, 1, 0);
        //CreateNewPoint(1, 1, 2);
        //CreateNewPoint(2, 1, 2);
        //CreateNewPoint(3, 1, 4);

        //CreateNewPoint(0, 2, 4);
        //CreateNewPoint(0, 3, 4);

        CreateNewPoint();
        newPoint = true;
        CreateNewPoint();

        ShowGrid();
    }
    void Update()
    {
        PressKey();
        ShowGrid();
    }
    void CreateNewPoint()
    {        
        if (!CheckGrid() || !newPoint)
            return;
        int rndPosition;
        int posX, posY;

        do
        {
            rndPosition = Random.Range(0,16);
            posX = rndPosition%4;
            posY = rndPosition/4;
        }
        while (points[posX,posY] != 0 && CheckGrid()); 
        int val = Random.Range(0,11);
        points[posX,posY]  = (val<10)? 2:4;
        newPoint=false;
    }

    void CreateNewPoint(int posX, int posY, int value) //для тестирования
    {                
        points[posX, posY]  = value;
    }
    bool CheckGrid()
    {
        foreach (var item in points)        
            if (item == 0) return true;        
        return false;
    }
    void SwapPoints(Direction direction)
    {

        if (direction == Direction.NULL)
            return;
        bool sum = true; 
        
        for (int i = 0; i < size; i++)
        {
            sum = true;
            for (int j = 1; j < size; j++)
            {
                int x, y;
                if (direction == Direction.Up || direction == Direction.Down)
                {
                    x = i;
                    y = (direction == Direction.Up) ? size - j -1 : j;
                }
                else                
                {
                    x = (direction == Direction.Right) ? size - j -1 : j;
                    y = i;
                }
                

                while (((direction == Direction.Right) ? x + 1 < size : (direction == Direction.Left) ? x - 1 >= 0 : (direction == Direction.Up) ? y + 1 < size : y - 1 >= 0) && points[x, y] != 0)
                {
                    if (points[((direction == Direction.Up || direction == Direction.Down) ? x : (direction == Direction.Left) ? x - 1 : x + 1), ((direction == Direction.Left || direction == Direction.Right) ? y : (direction == Direction.Up) ? y + 1 : y - 1)] == 0)
                        points[(direction == Direction.Up || direction == Direction.Down) ? x : (direction == Direction.Left) ? x - 1 : x + 1, (direction == Direction.Left || direction == Direction.Right) ? y : (direction == Direction.Up) ? y + 1 : y - 1] = points[x, y];
                    else if (points[x, y] == points[(direction == Direction.Up || direction == Direction.Down) ? x : (direction == Direction.Left) ? x - 1 : x + 1, (direction == Direction.Left || direction == Direction.Right) ? y : (direction == Direction.Up) ? y + 1 : y - 1] && sum)
                    {
                        points[(direction == Direction.Up || direction == Direction.Down) ? x : (direction == Direction.Left) ? x - 1 : x + 1, (direction == Direction.Left || direction == Direction.Right) ? y : (direction == Direction.Up) ? y + 1 : y - 1] += points[x, y];
                        sum = false;
                    }
                    else
                    {
                        sum = true;
                        break;
                    }
                    points[x, y] = 0;
                    if (direction == Direction.Up || direction == Direction.Down)
                        y = (direction == Direction.Up) ? y + 1 : y - 1;
                    else
                        x = (direction == Direction.Left) ? x - 1 : x + 1;
                    newPoint = true;
                }
                
            }
        }
        

        CreateNewPoint();
        newPoint = true;
    }
    void PressKey()
    {
        direction=Direction.NULL;
        if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown( KeyCode.S))
            direction = Direction.Down;
            else if (Input.GetKeyDown(KeyCode.UpArrow) ||Input.GetKeyDown(KeyCode.W))
                direction = Direction.Up;
                else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                    direction = Direction.Left;
                    else if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                        direction = Direction.Right;
        if (direction!= Direction.NULL)
        {
            //newPoint = true;
            SwapPoints(direction);
        }
    }
    bool CheckPosition(int posX, int posY)
    {
        return points[posX,posY] == 0;            
    }
    bool CheckPosition(out int value)
    {
        value=3;
        return true;
    }

    void ShowGrid()
    {
        for (int x=0;x<4;x++)
            for (int y=0;y<4;y++)            
                HudPoint[y*4 + x].text = (points[x,y] != 0)? points[x,y].ToString():"";
    }

}
