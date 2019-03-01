using System;
using System.Collections.Generic;
using System.Text;

namespace PressureExtract
{
    class Node
    {
        public int ID { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        private List<float> temperature;
        private List<float> pressure;
        public Node(int Id, float x, float y, float z)
        {
            ID = Id;
            X = x;
            Y = y;
            Z = z;
            temperature = new List<float>
            {
                Capacity = 0
            };
            pressure = new List<float>
            {
                Capacity = 0
            };
        }
        public void AddPressure(float p)
        {
            pressure.Add(p);
        }
        public float GetPressure(int idx)
        {
            return pressure[idx];
        }
        public void AddTemperature(float t)
        {
            temperature.Add(t);
        }
        public float GetTemperature(int idx)
        {
            return temperature[idx];
        }
        public void PrintInfo()
        {
            Console.WriteLine("Node " + ID.ToString() + ": " + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString());
        }
        public float Dist(Node n)
        {
            return (float) Math.Sqrt(Math.Pow(X - n.X, 2) + Math.Pow(Y - n.Y, 2) + Math.Pow(Z - n.Z, 2));
        }

    }
}
