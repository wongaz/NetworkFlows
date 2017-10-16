﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Priority_Queue;

namespace NetworkFlows
{ 
    class PreflowPush
    {
        public static void DistancePrint(Node[] NodeArray)
        {
            foreach (Node n in NodeArray)
            {
                Console.WriteLine("NodeID: "+n.NodeID);
                Console.WriteLine("\tNode Distance: "+n.DistanceLabel);
                Console.WriteLine("\tExcess: " +n.Excess);
            }
        }

        public static void PreProcess(int s, int t, Node[] arrayNodes)
        {
            Node Source = arrayNodes[s - 1];
            Node Sink = arrayNodes[t - 1];
            Sink.SetDistanceLabel(0);
            Source.SetDistanceLabel(arrayNodes.Length);
            Stack<Node> List = new Stack<Node>();
            List.Push(Source);
            //Getting all the Labels
            while (List.Count != 0)
            {
                Node curNode = List.Peek();
                List<Arc> OutgoingArcs = curNode.GetAllArcs();
                foreach (Arc edge in OutgoingArcs)
                {
                    if (edge.GetCapacity()!=0)
                    {
                        Node endNode = edge.EndNode;
                        if (endNode.DistanceLabel < curNode.DistanceLabel)
                        {
                            curNode.SetDistanceLabel(endNode.DistanceLabel + 1);

                            if (List.Count == 0)
                            {
                                return;
                            }

                            List.Pop();
                        }
                        else
                        {
                            List.Push(edge.EndNode);
                        }
                    }
                }
            }
        }

        public static void PreflowPushAlgorithmQueue(int s, int t, Node[] arrayNodes)
        {
            PreProcess(s,t,arrayNodes);
            DistancePrint(arrayNodes);
            Node Source = arrayNodes[s - 1];
            Node Sink = arrayNodes[t - 1];
            //Start the Preflow Push
            Queue<Node> List = new Queue<Node>();
            decimal deficit = 0;
            //Initial Push
            foreach (Arc a in Source.GetAllArcs())
            {
                Node startNode = a.StartNode;
                Node endNode = a.EndNode;
                endNode.SetExcess(a.Capacity);
                endNode.GetArc(startNode).SetCapactity(a.Capacity);
                deficit += a.Capacity;
                a.SetCapactity(0);
                List.Enqueue(endNode);
            }
            Source.SetExcess(-deficit);
            
            while (List.Count != 0)
            {
                Node currentNode = List.Dequeue();
                Boolean AnyAdmissable = false;
                foreach (Arc a in currentNode.AllArcs)
                {
                    Node EndNode = a.EndNode;
                    if (currentNode.DistanceLabel == EndNode.DistanceLabel + 1 && a.GetCapacity()!=0)
                    { 
                        AnyAdmissable = true; 
                        //Capacity is the Pushing Limiter
                        if (a.Capacity < currentNode.Excess)
                        {
                            EndNode.SetExcess(EndNode.Excess+a.Capacity);
                            Arc reverseArc = EndNode.GetArc(currentNode);
                            reverseArc.AddCapacity(a.Capacity);
                            currentNode.SetExcess(currentNode.Excess-a.Capacity);
                            a.SetCapactity(0);
                            List.Enqueue(currentNode);
                        }
                        //Excess is the Pushing Limiter
                        else if (a.Capacity > currentNode.Excess)
                        {
                            EndNode.SetExcess(EndNode.Excess + currentNode.Excess);
                            Arc reverseArc = EndNode.GetArc(currentNode);
                            reverseArc.AddCapacity(currentNode.Excess);
                            a.SetCapactity(a.Capacity-currentNode.Excess);
                            currentNode.SetExcess(0);
                        }

                        if (EndNode != Sink)
                        {
                            List.Enqueue(EndNode);
                        }
                    }
                }
                //Relabel
                if (!AnyAdmissable)
                {
                    int minDist = Int32.MaxValue/2;
                    foreach (Arc kArc in currentNode.AllArcs)
                    {
                        Node EndNode = kArc.EndNode;
                        int val = EndNode.DistanceLabel;
                        if (val < minDist && kArc.GetCapacity() != 0)
                        {
                            minDist = val;
                        }
                    }
                    currentNode.SetDistanceLabel(minDist+1);
                }
            }
        }
        public static void PreflowPushAlgorithmStack(int s, int t, Node[] arrayNodes)
        {
            Console.WriteLine("Labeling Distances");
            PreProcess(s, t, arrayNodes);
            
            DistancePrint(arrayNodes);
            Node Source = arrayNodes[s - 1];
            Node Sink = arrayNodes[t - 1];
            //Start the Preflow Push
            Stack<Node> List = new Stack<Node>();
            decimal deficit = 0;

            //Initial Push
            Console.WriteLine("Starting Initial Push");
            foreach (Arc a in Source.GetAllArcs())
            {
                Node startNode = a.StartNode;
                Node endNode = a.EndNode;
                endNode.SetExcess(a.Capacity);
                endNode.GetArc(startNode).SetCapactity(a.Capacity);
                deficit += a.Capacity;
                a.SetCapactity(0);
                List.Push(endNode);
            }
            Source.SetExcess(-deficit);
            Console.WriteLine("Starting Algorithm");
            while (List.Count != 0)
            {
                Node currentNode = List.Pop();
                Boolean AnyAdmissable = false;
                foreach (Arc a in currentNode.AllArcs)
                {
                    Node EndNode = a.EndNode;
                    if (currentNode.DistanceLabel == EndNode.DistanceLabel + 1 && a.GetCapacity() != 0)
                    {
                        AnyAdmissable = true;
                        //Capacity is the Pushing Limiter
                        if (a.Capacity < currentNode.Excess)
                        {
                            EndNode.SetExcess(EndNode.Excess + a.Capacity);
                            Arc reverseArc = EndNode.GetArc(currentNode);
                            reverseArc.AddCapacity(a.Capacity);
                            currentNode.SetExcess(currentNode.Excess - a.Capacity);
                            a.SetCapactity(0);
                            List.Push(currentNode);
                        }
                        //Excess is the Pushing Limiter
                        else if (a.Capacity > currentNode.Excess)
                        {
                            EndNode.SetExcess(EndNode.Excess + currentNode.Excess);
                            Arc reverseArc = EndNode.GetArc(currentNode);
                            reverseArc.AddCapacity(currentNode.Excess);
                            a.SetCapactity(a.Capacity - currentNode.Excess);
                            currentNode.SetExcess(0);
                        }

                        if (EndNode != Sink)
                        {
                            List.Push(EndNode);
                        }
                    }
                }
                //Relabel
                if (!AnyAdmissable)
                {
                    int minDist = Int32.MaxValue / 2;
                    foreach (Arc kArc in currentNode.AllArcs)
                    {
                        Node EndNode = kArc.EndNode;
                        int val = EndNode.DistanceLabel;
                        if (val < minDist && kArc.GetCapacity() != 0)
                        {
                            minDist = val;
                        }
                    }
                    currentNode.SetDistanceLabel(minDist + 1);
                }
            }
        }

        public static void SAPA(int s, int t, Node[] Nodes)
        {
            Node Source = Nodes[s - 1];
            Node Sink = Nodes[t - 1];

        }

        public static void PopulateArray(Node[] arrayNode)
        {
            for (int k = 0; k < arrayNode.Length; k++)
            {
                
                arrayNode[k] = new Node((k+1));
                
            }
        }

        static void Main(string[] args)
        {
            string FileLocation = @"C:\Users\wongaz\Documents\MA446ProjectData\Project2\";
            string FileName = "elist96.rmf";
            int StartNodeID=0;
            int EndNodeID=0;
            Node[] ArrayOfNodes = null;
            Arc[] ArrayOfArcs = null;
            int i = 1;
            foreach (var line in File.ReadLines(FileLocation + FileName))
            {
                switch (i)
                {
                    case 1:
                        //Console.WriteLine("Case 1");
                        ArrayOfNodes =new Node[Int32.Parse(line)];
                        PopulateArray(ArrayOfNodes);
                        i++;
                        break;
                    case 2:
                        //Console.WriteLine("Case 2");
                        ArrayOfArcs = new Arc[Int32.Parse(line)];
                        i++;
                        break;
                    case 3:
                        //Console.WriteLine("Case 3");
                        StartNodeID = Int32.Parse(line);
                        i++;
                        break;
                    case 4:
                        //Console.WriteLine("Case 4");
                        i++;
                        EndNodeID = Int32.Parse(line);
                        break;
                    default:
                        String[] LineContents = line.Split(' ');
                        //Console.WriteLine(line);
                        if (line != "")
                        {
                            int start = Int32.Parse(LineContents[0]); 
                            int end = Int32.Parse(LineContents[2]);
                            decimal d = Decimal.Parse(LineContents[4], System.Globalization.NumberStyles.Float);
                            //Console.WriteLine(d);
                            Node StartNode = ArrayOfNodes[start - 1];
                            Node EndNode = ArrayOfNodes[end - 1];
                            Arc additionalArc =new Arc(StartNode,EndNode,d, true);
                            Arc residualArc = new Arc(EndNode, StartNode, 0, false);
                            //StartNode.AddOutgoingArc(additionalArc);
                            //EndNode.AddIncomingArc(additionalArc);
                            StartNode.AddToAllArcs(additionalArc);
                            EndNode.AddToAllArcs(residualArc);
                        }
                        break;
                }
            }
            Stopwatch clock = new Stopwatch();
            clock.Start();
            PreflowPushAlgorithmStack(StartNodeID, EndNodeID, ArrayOfNodes);

            clock.Stop();
            Decimal Excess = ArrayOfNodes[EndNodeID - 1].Excess;

            Console.WriteLine("Total Flow: "+ Excess);
            Console.WriteLine("Elapsed Time: "+clock.ElapsedMilliseconds/1000);

            Console.Read();
        }
    }
}