using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mino : MonoBehaviour
{
    int value;   
    public int GetValue
    {
        get
        {
            return value;
        } 
    }

    public Vector3 GetPos
    {
        get
        {
            return transform.localPosition;
        }
    }

    public Text valueView;

    void Start()
    {
        value = 2;   
    }
    void Update()
    {
        
    }
    public void DeletePoint()
    {
        Destroy(gameObject);
    }

    /*public void MoveMino(Vector3 value)
    {
        transform.localPosition += value;
    }*/

    public static Mino operator +(Mino c1, Mino c2)
    {
        if (c1.value == c2.value)
            c1.value += c2.value;                
        return c1;
    }

    public override bool Equals(object obj)
        {
            var element = obj as Mino;
            return element != null && value == element.value;
        }

        public override int GetHashCode()
        {
            return -1937169414 + value.GetHashCode();
        }

        public static bool operator ==(Mino left, Mino right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Mino left, Mino right)
        {
            return !Equals(left, right);

        }
}
