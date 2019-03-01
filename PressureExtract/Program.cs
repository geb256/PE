using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using System.Threading.Tasks;


namespace PressureExtract
{
 
     class Program
    {
        static List<string> parse(string line, string del)
        {
            List<string> result=new List<string>( line.Split(del));
            return result;
        }
        static bool IsInterface(int nodenb, List<Node> mn, List<int>mnId, int[] meshNodesId )
        {
            return false;
        }
        static void Main(string[] args)
        {
            const Int32 BufferSize = 128;
            string baseName;
            const string del = ",";
            List<Node> meshNodes = new List<Node>();
            List<Mesh> problem = new List<Mesh>();
            Mesh cMesh;
            int nbMeshes = 0;
            const int nbCores = 12;
            
            Console.WriteLine("Enter problem base name:");
            baseName=Console.ReadLine();
            string meshName = baseName + "g.inp";

            using (var filestream = File.OpenRead(meshName))
                using (var streamReader = new StreamReader(filestream, System.Text.Encoding.UTF8, true, BufferSize))
                {
                String line;
                List<List<string>> sline = new List<List<string>>();
                while ((line = streamReader.ReadLine()) != null)
                    // read the file and store in list of string
                    sline.Add(parse(line,del));
                Console.WriteLine("Mesh file read. Analising...");
                int lineNb = 0;
                while (sline[lineNb][0].Substring(0, 5) != "*NODE")
                    lineNb++;
                lineNb++;
                while(sline[lineNb][0].Substring(0,5)!="*ELEM")
                {
                    Int32.TryParse(sline[lineNb][0], out int id);
                    float.TryParse(sline[lineNb][1], out float x);
                    float.TryParse(sline[lineNb][2], out float y);
                    float.TryParse(sline[lineNb][3], out float z);

                    meshNodes.Add(new Node(id, x, y, z));
                    
                    lineNb++;
                }
                Console.WriteLine(meshNodes.Count.ToString() + " nodes created");
                Console.WriteLine("Last node created :" + meshNodes[meshNodes.Count - 1].ID);
                int maxIdNode = meshNodes.Max(x => x.ID);
                int[] meshNodesId = new int[maxIdNode+1];
                for (int i = 0; i < meshNodes.Count; i++)
                    meshNodesId[meshNodes[i].ID] = i;
                
                // read tet elements
                while(sline[lineNb][1].Substring(6,2) == "C3")
                {
                    bool exists = false;
                    string setName = sline[lineNb][2].Substring(7);
                    Mesh tMesh=new Mesh();
                    foreach (Mesh m in problem)
                        if (m.solidName == setName)
                         {
                            tMesh = m;
                            exists = true;
                            break;
                        }
                    if (exists)
                        cMesh = tMesh;
                    else
                        cMesh = new Mesh(nbMeshes++, setName);
                    lineNb++;
                    while (sline[lineNb][0].Substring(0,1)!="*")
                    {
                        List<int> lNode = new List<int>();
                        Int32.TryParse(sline[lineNb][1], out int n1);
                        Int32.TryParse(sline[lineNb][2], out int n2);
                        Int32.TryParse(sline[lineNb][3], out int n3);
                        Int32.TryParse(sline[lineNb][4], out int n4);
                        lNode.Add(n1);lNode.Add(n2);lNode.Add(n3);lNode.Add(n4);
                        cMesh.Add(lNode);
                        lineNb++;
                    }
                    if (!exists)
                    {
                        problem.Add(cMesh);
                        Console.WriteLine(cMesh.solidName);
                    }
                }
                foreach (Mesh m in problem)
                    m.unityNode();
                
                // List all volumes read and ask which ones to compute interface for
                foreach (Mesh m in problem)
                    Console.WriteLine(m.solidId.ToString() + ": " + m.solidName + "\t" + m.GetListNodes().Count.ToString());

                List<int> shellId = new List<int>();
                List<int> alloyId = new List<int>();
                List<int> coreId = new List<int>();
                Console.WriteLine("Volume(s) for alloy comma separated: ");
                string answer= Console.ReadLine();
                string[] ans = answer.Split(",");
                foreach (string a in ans)
                    alloyId.Add(Convert.ToInt32(a));
                Console.WriteLine("Volume(s) for core comma separated: ");
                answer = Console.ReadLine();
                ans = answer.Split(",");
                foreach (string a in ans)
                    coreId.Add(Convert.ToInt32(a));
                Console.WriteLine("Volume(s) for shell comma separated: ");
                answer = Console.ReadLine();
                ans = answer.Split(",");
                foreach (string a in ans)
                    shellId.Add(Convert.ToInt32(a));

                // merge list of nodes
                List<int> alloyNodesId = new List<int>();
                foreach (int a in alloyId)
                    alloyNodesId = alloyNodesId.Concat(problem[a].GetListNodes()).ToList();
                alloyNodesId = alloyNodesId.Distinct().ToList();
                Console.WriteLine(alloyNodesId.Count.ToString() + " Nodes in alloy volume");

                List<int> coreNodesId = new List<int>();
                foreach (int a in coreId)
                    coreNodesId = coreNodesId.Concat(problem[a].GetListNodes()).ToList();
                coreNodesId = coreNodesId.Distinct().ToList();
                Console.WriteLine(coreNodesId.Count.ToString() + " Nodes in core volume");

                List<int> shellNodesId = new List<int>();
                foreach (int a in shellId)
                    shellNodesId = shellNodesId.Concat(problem[a].GetListNodes()).ToList();
                shellNodesId = shellNodesId.Distinct().ToList();
                Console.WriteLine(shellNodesId.Count.ToString() + " Nodes in shell volume");

                //identify interfacial nodes
                List<int> interfaceNodeAlloyShell = new List<int>();
                for(int i=0; i<alloyNodesId.Count-nbCores; i=i+nbCores)
                {
                    Task[] t= new Task[nbCores];
                    bool[] ret = new bool[nbCores];
                    int node = i + j;
                    for (int j = 0; j < nbCores; j++)
                    {
                        t[j] = new Task(delegate ()
                        {
                            ret[j] = IsInterface(meshNodesId[node], meshNodes, shellNodesId, meshNodesId);
                        };
                        t[j].Start();

                    foreach(int o in shellNodesId)
                    {
                        if (meshNodes[meshNodesId[n]].Dist(meshNodes[meshNodesId[o]]) < 0.001)
                            interfaceNodeAlloyShell.Add(n);
                    }
                }
                Console.WriteLine(interfaceNodeAlloyShell.Count.ToString() + " Nodes in alloy-shell interface");

                List<int> interfaceNodeAlloyCore = new List<int>();
                foreach (int n in alloyNodesId)
                {
                    foreach (int o in coreNodesId)
                    {
                        if (meshNodes[meshNodesId[n]].Dist(meshNodes[meshNodesId[o]]) < 0.001)
                            interfaceNodeAlloyCore.Add(n);
                    }
                }
                Console.WriteLine(interfaceNodeAlloyCore.Count.ToString() + " Nodes in alloy-core interface");



                Console.ReadKey();
                }
            
        }
    }
}
