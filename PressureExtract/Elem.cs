using System;
using System.Collections.Generic;
using System.Text;

namespace PressureExtract
{
    class Elem
    {
        public List<int> nodeId { set; get; }
        public int nbNodes { set; get; }

        Elem(int nNodes, List<int> ln)
        {
            nbNodes = nNodes;
            nodeId = new List<int>(ln);
        }
        public List<int> getodes()
        {
            return nodeId;
        }
    }
}
