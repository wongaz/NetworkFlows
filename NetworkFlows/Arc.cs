using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkFlows
{
    public class Arc
    {
        public Node StartNode;
        public Node EndNode;
        public Decimal Capacity;
        public Boolean Saturated = false;
        public Boolean Original;
        public Arc(Node start, Node end, Decimal initCapacity, Boolean o)
        {
            this.StartNode = start;
            this.EndNode = end;
            this.Capacity = initCapacity;
            this.Original = o;
        }

        public Node GetStartNode()
        {
            return this.StartNode;
        }

        public Node GetEndNode()
        {
            return this.EndNode;
        }

        public Decimal GetCapacity()
        {
            return this.Capacity;
        }

        public void SetSaturated()
        {
            this.Saturated = true;
        }

        public void SetCapactity(Decimal dec)
        {
            this.Capacity = dec;
        }

        public void AddCapacity(Decimal val)
        {
            this.Capacity += val;
        }
    }
}