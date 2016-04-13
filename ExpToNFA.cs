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

        public Sigle(string edge, ref int startIndex, ref List<ThompsonNode> ThompsonNodeList,int Type)
        {
            if(Type==1)
            {
                NFANode a = new NFANode(startIndex, false);
                startIndex++;
                NFANode b = new NFANode(startIndex, false);
                startIndex++;
                ThompsonNode aa = new ThompsonNode(a, edge, b);
                ThompsonNodeList.Add(aa);
            }
            else
            {

            }

        }

    }

    public class Or
    {
        public Or(int startIndex)
        {
            t1 = new ThompsonNode_Digit(startIndex, "ε", startIndex + 1);
            t2 = new ThompsonNode_Digit(startIndex, "ε", startIndex + 3);
            t3 = new ThompsonNode_Digit(startIndex + 2, "ε", startIndex + 5);
            t4 = new ThompsonNode_Digit(startIndex + 4, "ε", startIndex + 5);
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
