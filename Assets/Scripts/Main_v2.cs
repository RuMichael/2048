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
    void Start()
    {
        for(int i=0;i<4;i++)
            for(int j=0;j<4;j++)
                points[i,j]=0;
        CreateNewPoint(0,0,8);
        CreateNewPoint(0,1,4);
        CreateNewPoint(0,2,2);
        CreateNewPoint(0,3,2);

        CreateNewPoint(1,0,2);
        CreateNewPoint(1,1,2);
        CreateNewPoint(1,2,4);
        CreateNewPoint(1,3,8);
        
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
        bool sum = true; 
        
            if (direction == Direction.Right) // свайп вправо
            {
                for (int y=0; y<4;y++)
                {
                    sum = true ; 
                    for (int x=2;x>=0;x--)
                    {   
                        int xx = x;
                        while ((xx+1)<4 && points[xx,y]!=0)                            
                        {                               
                            if (points[xx+1,y] == 0)    
                            {                                                    
                                points[xx+1,y] = points[xx,y];  
                            }                            
                            else if (points[xx,y] == points[xx+1,y] && sum)   
                            {                         
                                points[xx+1,y] += points[xx,y]; 
                                sum = false;
                            }
                            else{
                                sum=true; 
                                break;
                            }
                               
                            points[xx,y] = 0;
                            xx++;
                            newPoint = true;
                        }
                    }
                }
            }

            if (direction == Direction.Left)       // свайп влево
            {
                for (int y=0; y<4;y++)
                {
                    sum = true ;
                    for (int x=1;x<=3;x++)
                    {
                        int xx = x;
                        while ((xx-1)>=0 && points[xx,y]!=0 )                            
                        {                               
                            if (points[xx-1,y] == 0)                                                        
                                points[xx-1,y] = points[xx,y];                                
                            else if (points[xx,y] == points[xx-1,y] && sum)   
                            {                         
                                points[xx-1,y] += points[xx,y]; 
                                sum=false;
                            }
                            else{
                                sum = true;
                                break;}
                            points[xx,y] = 0;
                            xx--;
                            newPoint = true;
                        }
                    }
                }
            }        
        
            if (direction == Direction.Up)
            {
                for (int x=0; x<4;x++)
                {
                    sum = true ;
                    for (int y=2;y>=0;y--)
                    {
                        int yy = y;
                        while ((yy+1)<4 && points[x,yy]!=0)                            
                        {                               
                            if (points[x,yy+1] == 0)                                                        
                                points[x,yy+1] = points[x,yy];                                
                            else if (points[x,yy] == points[x,yy+1] && sum)   
                            {                         
                                points[x,yy+1] += points[x,yy]; 
                                sum=false;
                            }
                            else{
                                sum=true;
                                break;}
                            points[x,yy] = 0;
                            yy++;
                            newPoint = true;
                        }
                    }
                }
            }

            if (direction == Direction.Down)
            {
                for (int x=0; x<4;x++)
                {
                    sum = true ; 
                    for (int y=1;y<=3;y++)
                    {
                        int yy = y;
                        while ((yy-1)>=0 && points[x,yy]!=0)                            
                        {                               
                            if (points[x,yy-1] == 0)                                                        
                                points[x,yy-1] = points[x,yy];                                
                            else if (points[x,yy] == points[x,yy-1] && sum)   
                            {                         
                                points[x,yy-1] += points[x,yy]; 
                                sum=false;
                            }
                            else{
                                sum = true;
                                break;
                            }
                            points[x,yy] = 0;
                            yy--;
                            newPoint = true;
                        }
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
