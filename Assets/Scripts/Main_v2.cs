using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Main_v2 : MonoBehaviour
{

    enum Direction : byte
    {
        Up=1,Down=2,Left=3,Right=4,NULL=0
    }
    Direction direction = Direction.NULL;
    bool newPoint = true;
    int[,] points = new int[4,4];
    public Text[] HudPoint = new Text[16];

    const int size = 4;

    void Start()
    {
        for(int i=0;i<4;i++)
            for(int j=0;j<4;j++)
                points[i,j]=0;
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
        while (points[posX,posY] != 0); 
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
        CheckGameOver();     
        return false;
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
        switch (direction) {
            case Direction.Up:
                convert = ConvertUp;
                break;
            case Direction.Down:
                convert = ConvertDown;
                break;
            case Direction.Left:
                convert = ConvertLeft;
                break;
            case Direction.Right:
                convert = ConvertRight;
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
                int jj = j;
                while (convert(i,jj-1).Y >=0 && points[point.X, point.Y] != 0)
                {
                    
                    if (points[point.X, convert(i,jj-1).Y] == 0)
                        points[point.X, convert(i,jj-1).Y] = points[point.X, point.Y];
                    else if (points[point.X, point.Y] == points[point.X, convert(i,jj-1).Y] && sum)
                    {
                        points[point.X, convert(i,jj-1).Y] += points[point.X, point.Y];
                        sum = false;
                    }
                    else
                    {
                        sum = true;
                        break;
                    }
                    points[point.X, point.Y] = 0;
                    jj--;
                    //if (direction == Direction.Up || direction == Direction.Down)
                        //y = (direction == Direction.Up) ? y + 1 : y - 1;
                    //else
                        //x = (direction == Direction.Left) ? x - 1 : x + 1;
                    newPoint = true;
                }

                /*while ((y-1)>=0 && points[x,y]!=0)      //down                      
                    {                               
                        if (points[x,y-1] == 0)                                                        
                            points[x,y-1] = points[x,yy];                                
                        else if (points[x,y] == points[x,y-1] && sum)   
                        {                         
                            points[x,y-1] += points[x,y]; 
                            sum=false;
                        }
                        else{
                            sum = true;
                            break;
                        }
                        points[x,y] = 0;
                        y--;
                        newPoint = true;
                    }*/




/*
                if (direction == Direction.Up || direction == Direction.Down)
                {
                    x = i;
                    y = (direction == Direction.Up) ? size - j -1 : j;
                }
                else                
                {
                    x = (direction == Direction.Right) ? size - j -1 : j;
                    y = i;
                }*/
                

                /* while (((direction == Direction.Right) ? point.X + 1 < size : (direction == Direction.Left) ? point.X - 1 >= 0 : (direction == Direction.Up) ? point.Y + 1 < size : point.Y - 1 >= 0) && points[point.X, point.Y] != 0)
                {
                    
                    if (points[FindCorrectPosition(point.X,"x"), FindCorrectPosition(point.Y, "y")] == 0)
                        points[FindCorrectPosition(point.X,"x"), FindCorrectPosition(point.Y, "y")] = points[point.X, point.Y];
                    else if (points[point.X, point.Y] == points[FindCorrectPosition(point.X,"x"), FindCorrectPosition(point.Y, "y")] && sum)
                    {
                        points[FindCorrectPosition(point.X,"x"), FindCorrectPosition(point.Y, "y")] += points[point.X, point.Y];
                        sum = false;
                    }
                    else
                    {
                        sum = true;
                        break;
                    }
                    points[point.X, point.Y] = 0;
                    if (direction == Direction.Up || direction == Direction.Down)
                        point.Y = (direction == Direction.Up) ? point.Y + 1 : point.Y - 1;
                    else
                        point.X = (direction == Direction.Left) ? point.X - 1 : point.X + 1;
                    newPoint = true;
                }*/
            }
        }
        if (CheckGrid())        
            if (newPoint)
                CreateNewPoint();
        
    }

    int FindCorrectPosition(int value, string s)
    {
        return (s == "x") ? (direction == Direction.Up || direction == Direction.Down) ? value : (direction == Direction.Left) ? value - 1 : value + 1 :
        (direction == Direction.Left || direction == Direction.Right) ? value : (direction == Direction.Down) ? value - 1 : value + 1;
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
        int[,] tmpPoints = points;
        for (byte i=1;i<5;i++)
        {
            direction = (Direction) i;
            SwapPoints();
            if (!CheckEquality(tmpPoints))
                points = tmpPoints;
            else 
                Start();
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

}
