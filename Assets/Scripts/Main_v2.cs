using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Main_v2 : MonoBehaviour
{

    enum Direction : byte
    {
        NULL=0,Up=1,Down=2,Left=3,Right=4
    }
    Direction direction = Direction.NULL;
    bool newPoint = true, checkedGrid =true;
    int[,] points = new int[4,4];
    public Text[] HudPoint = new Text[16];

    const int size = 4;

    void Start()
    {
        Refresh();
    }
    void Refresh()
    {
        for(int i=0;i<4;i++)
            for(int j=0;j<4;j++)
                points[i,j] = 0;
        direction = Direction.NULL;
        CreateNewPoint();
        newPoint = true;
        CreateNewPoint();
        ShowGrid();
    }

    void Update()
    {
        PressKey();
    }
    void CreateNewPoint()
    {        
        int rndPosition;
        int posX, posY;
        do
        {
            rndPosition = Random.Range(0,16);
            posX = rndPosition%4;
            posY = rndPosition/4;
        }
        while (points[posX,posY] != 0); 
        int val = Random.Range(0,11);
        points[posX,posY]  = (val<10) ? 2:4;
        newPoint=false;
    }

    void CreateNewPoint(int posX, int posY, int value) //для тестирования
    {                
        points[posX, posY]  = value;
    }
    void CheckGrid()
    {
        foreach (var item in points)        
            if (item == 0) return;   
        CheckGameOver();     
    }

    private struct Point {
        public int X;
        public int Y;

        public Point(int x, int y) {
            X = x;
            Y = y;
        }    
    }
    private Point ConvertUp(int i, int j) {
        return new Point(i,size - j - 1);
    }
    private Point ConvertDown(int i, int j) {
        return new Point(i,j);
    }
    private Point ConvertLeft(int i, int j) {
        return new Point(j, i);
    }
    private Point ConvertRight(int i, int j) {
        return new Point(size - j - 1, i);
    }

    void SwapPoints()
    {
        System.Func<int, int, Point> convert;
        int xInc, yInc , nextX, nextY;
        switch (direction) {
            case Direction.Up:
                convert = ConvertUp;
                xInc = 0;
                yInc = 1;
                break;
            case Direction.Down:
                convert = ConvertDown;
                xInc = 0;
                yInc = -1;
                break;
            case Direction.Left:
                convert = ConvertLeft;
                xInc = -1;
                yInc = 0;
                break;
            case Direction.Right:
                convert = ConvertRight;
                xInc = 1;
                yInc = 0;
                break;
            default:
                return;
        }
        bool sum = true; 
        
        for (int i = 0; i < size; i++)
        {
            sum = true;
            for (int j = 1; j < size; j++)
            {
                Point point = convert(i, j);
                nextX = point.X + xInc;
                nextY = point.Y + yInc;
                while ( nextX < size && nextX>=0 && nextY<size && nextY>=0 && points[point.X, point.Y] != 0)
                {
                    if (points[nextX,nextY] == 0)
                        points[nextX,nextY] = points[point.X, point.Y];
                    else if (points[point.X, point.Y] == points[nextX,nextY] && sum)
                    {
                        points[nextX,nextY] += points[point.X, point.Y];
                        sum = false;
                    }
                    else
                    {
                        sum = true;
                        break;
                    }
                    points[point.X, point.Y] = 0;
                    point.X = nextX; 
                    point.Y = nextY;
                    nextX += xInc;
                    nextY += yInc;
                    newPoint = true;
                }
            }
        }   
            
        if (checkedGrid)
        {
            if (newPoint)
                CreateNewPoint();
            CheckGrid();
        }
        
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
            SwapPoints();   
        ShowGrid();  
    }    

    void ShowGrid()
    {
        for (int x=0;x<4;x++)
            for (int y=0;y<4;y++)            
                HudPoint[y*4 + x].text = (points[x,y] != 0)? points[x,y].ToString():"";
    }

    void CheckGameOver()
    {
        int[,] tmpPoints = NewArray(points);
        for (byte i=1;i<5;i++)
        {
            newPoint = false;
            direction = (Direction)i;
            checkedGrid =false;
            SwapPoints();
            checkedGrid = true;
            newPoint = true;
            if (!CheckEquality(tmpPoints))
            {
                points = tmpPoints;
                break;
            }
            else {
                Refresh();
                break;
            }
        }
    }

    bool CheckEquality(int [,] tmp)
    {
        for (int i = 0 ; i<size; i++)
            for (int j = 0; j<size; j++)
                if (tmp[i,j] != points[i,j])
                    return false;
        return true;
    }

    int[,] NewArray(int[,] array)
    {
        int[,] nArray = new int[size,size];
        for (int i=0;i<size;i++)
            for(int j=0;j<size;j++)
                nArray[i,j]=array[i,j];
        return nArray;
    }
}
