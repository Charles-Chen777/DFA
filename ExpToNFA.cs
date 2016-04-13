using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFA
{
    public class ExpToNFA
    {
        //正规式转化为后缀表达式
        public string ExpToBack(string Exp)
        {
            return "ab|*abb";
        }
    }

    public class NFANode
    {
        int NodeID;
        bool isEmpty;


        public NFANode(int NodeID, bool isEmpty)
        {
            this.NodeID = NodeID;
            this.isEmpty = isEmpty;
        }
    }

    public class ReplaceRecord
    {
        int BeReplaced;
        int result;
    }

    public class ThompsonNode
    {
        public NFANode from;
        public string edge;
        public NFANode to;
        public ThompsonNode(NFANode from, string edge, NFANode to)
        {
            this.from = from;
            this.edge = edge;
            this.to = to;
        }
    }


    public class Sigle
    {

        public Sigle(string edge, ref int startIndex, ref List<ThompsonNode> ThompsonNodeList)
        {
            NFANode a = new NFANode(startIndex, false);
            startIndex++;
            NFANode b = new NFANode(startIndex, false);
            startIndex++;
            ThompsonNode aa = new ThompsonNode(a, edge, b);
            ThompsonNodeList.Add(aa);
        }

        public Sigle(string edge, int Index, int endIndex1, int endIndex2, ref List<ThompsonNode> ThompsonNodeList)
        {
            NFANode a = new NFANode(Index, false);
            NFANode b = new NFANode(endIndex1, false);
            NFANode c = new NFANode(endIndex2, false);
            ThompsonNode r1 = new ThompsonNode(a, edge, b);
            ThompsonNode r2 = new ThompsonNode(a, edge, c);
            ThompsonNodeList.Add(r1);
            ThompsonNodeList.Add(r2);
        }

    }

    public class Or
    {
        public Or(ref int startIndex, ref List<ThompsonNode> ThompsonNodeList)
        {
            Sigle s1 = new Sigle("ε", startIndex, startIndex + 1, startIndex + 3, ref ThompsonNodeList);
            startIndex += 5;
            Sigle s2 = new Sigle("ε", startIndex, startIndex - 3, startIndex - 3, ref ThompsonNodeList);
        }
    }

    public class Star
    {
        public ThompsonNode_Digit t1;
        public ThompsonNode_Digit t2;
        public ThompsonNode_Digit t3;
        public ThompsonNode_Digit t4;

        public Star(int startIndex)
        {
            t1 = new ThompsonNode_Digit(startIndex, "ε", startIndex + 1);
            t2 = new ThompsonNode_Digit(startIndex + 2, "ε", startIndex + 1);
            t3 = new ThompsonNode_Digit(startIndex + 2, "ε", startIndex + 3);
            t4 = new ThompsonNode_Digit(startIndex, "ε", startIndex + 3);
        }

        public List<ThompsonNode_Digit> LinkStar(List<ThompsonNode_Digit> ThompsonNode_DigitList)
        {

            List<ThompsonNode_Digit> t = new List<ThompsonNode_Digit>();
            t.Add(t1);
            t.Add(t2);
            t.Add(t3);
            t.Add(t4);

            for (int i = 0; i < ThompsonNode_DigitList.Count; i++)
            {
                t.Add(ThompsonNode_DigitList[i]);
            }
            return t;
        }
    }


}
