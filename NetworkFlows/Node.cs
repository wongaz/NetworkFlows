using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkFlows
{
    public class Node 
    {
        public Node PredecessorNode;
        public int NodeID;
        public int DistanceLabel = Int32.MaxValue/2;
        public decimal Excess = 0;
        public List<Arc> OutgoingArcs = new List<Arc>();
        public List<Arc> IncomingArcs = new List<Arc>();
        public List<Arc> AllArcs = new List<Arc>();
        public Boolean InStructure = false;
        public Boolean addedOnce = false;

        public Node(int nodeID)
        {
            NodeID = nodeID;
        }

        public void swapInStructure()
        {
            this.InStructure = !this.InStructure;
        }

        public void swapAddedOnce()
        {
            if (addedOnce)
            {
                addedOnce = false;
            }
            else
            {
                addedOnce = true;
            }
        }

        public void AddIncomingArc(Arc newArc)
        {
            IncomingArcs.Add(newArc);
        }

        public List<Arc> GetIncomingArcs()
        {
            return this.IncomingArcs;
        }

        public void AddOutgoingArc(Arc newArc)
        {
            OutgoingArcs.Add(newArc);
        }

        public List<Arc> GetOutgoingArcs()
        {
            return this.OutgoingArcs;
        }

        public void AddToAllArcs(Arc a)
        {
            this.AllArcs.Add(a);
        }

        public List<Arc> GetAllArcs()
        {
            return this.AllArcs;
        }

        public int GetDistanceLabel()
        {
            return this.DistanceLabel;
        }

        public void SetDistanceLabel(int num)
        {
            this.DistanceLabel = num;
        }

        public int GetNodeID()
        {
            return NodeID;
        }

        public string ToString()
        { 
            return "NodeID: " + NodeID;
            
        }

        public void SetExcess(decimal num)
        {
            this.Excess = num;
        }

        public decimal GetExcess()
        {
            return this.Excess;
        }

        public Arc GetArc(Node endNode)
        {
            foreach (Arc a in AllArcs)
            {
                if (a.EndNode == endNode)
                {
                    return a;
                }
            }
            return null;
        }
    }
}