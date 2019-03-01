using System;

namespace myNode
{
    class Node
    {
        private int Id;
        private float x;
        private float y;
        private float z;
        private List<float> pressure;
        private List<float> temperature;
        private List<float> time;

        public Node(int ID, float X, float Y, float Z)
        {
            Id = ID;
            x = X;
            y = Y;
            z = Z;
            pressure = new List<float>()
            {
                Capacity = 0
            };
            temperature = new List<float>()
            {
                Capacity = 0
            };
            time = new List<float>()
            {
                Capacity = 0
            };

        }
        int getId()
        {
            return Id;
        }
        float getX()
        {
            return x;
        }
        float getY()
        {
            return y;
        }
        float getZ()
        {
            return z;
        }

    }
}