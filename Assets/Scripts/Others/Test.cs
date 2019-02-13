using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{


    public class Test : MonoBehaviour
    {
        Coordinate a, w;
        private void Start()
        {
            a = w = new Coordinate();
            w.x = w.x = 10;
            Debug.Log("w.x = "+w.x + " a.x is " +a.x);
        }
    }

    public class Coordinate
    {
        public int x, y, z;
    }
}