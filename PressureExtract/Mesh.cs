using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PressureExtract
{
    class Mesh
    {

        List<int> lNode;
        public int nbElem { get; set; }
        public int solidId { get; set; }
        public string solidName { get; set; }

        public Mesh()
        {

        }
        public Mesh(int solid, string solName)
        {
            nbElem = 0;
            solidId = solid;
            solidName = solName;
            lNode = new List<int>();
        }
        public void Add(List<int> ln)
        {
            nbElem++;
            foreach (int node in ln)               
               lNode.Add(node);
            
        }
        public void PrintInfo()
        {
            Console.WriteLine("Mesh " + solidName + ": " + nbElem.ToString() + " elements " + lNode.Count.ToString() + " nodes");
        }
        public List<int> GetListNodes()
        {
            return lNode;
        }
        public void unityNode()
        {
            lNode = lNode.Distinct().ToList();
        }
        
    }
}
