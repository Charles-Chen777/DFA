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

    public class ThompsonMinNode
    {
        public int Index;
        public string Name;
        public ThompsonMinNode(int index,string name)
        {
            this.Index = index;
            this.Name = name;
        }
    }
    public class ThompsonNode
    {
        public string from;
        public string edge;
        public string to;
        public ThompsonNode(string from, string edge, string to)
        {
            this.from = from;
            this.edge = edge;
            this.to = to;
        }
    }

    public class ThompsonNode_Digit
    {
        public int from;
        public string edge;
        public int to;
        public ThompsonNode_Digit(int from, string edge, int to)
        {
            this.from = from;
            this.edge = edge;
            this.to = to;
        }
    }

    public class Sigle
    {

        public ThompsonNode_Digit t;
        public Sigle(int startIndex)
        {
            t = new ThompsonNode_Digit(startIndex, "ε", startIndex+1);
        }
    }

    public class Or
    {
        public ThompsonNode_Digit t1;
        public ThompsonNode_Digit t2;
        public ThompsonNode_Digit t3;
        public ThompsonNode_Digit t4;
        public Or(int startIndex)
        {
            t1 = new ThompsonNode_Digit(startIndex, "ε", startIndex + 1);
            t2 = new ThompsonNode_Digit(startIndex, "ε", startIndex + 3);
            t3 = new ThompsonNode_Digit(startIndex + 2, "ε", startIndex + 5);
            t4 = new ThompsonNode_Digit(startIndex + 4, "ε", startIndex + 5);
        }
        public List<ThompsonNode_Digit> LinkOr(List<ThompsonNode_Digit> ThompsonNode_DigitList)
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
