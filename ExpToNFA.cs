using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFA
{

    //正规式转化为后缀表达式
    public class ExpToNFA
    {
        
        public string ExpToBack(string Exp)
        {
            InToPost itp = new InToPost(Exp);
            string output = itp.doTrans();  
            //return "ab|*+a+b+b";
            return output;
        }
    }

    //NFA节点类
    public class NFANode
    {
        public int NodeID;
        public bool isEmpty;


        public NFANode(int NodeID, bool isEmpty)
        {
            this.NodeID = NodeID;
            this.isEmpty = isEmpty;
        }
    }

    //汤姆森节点类
    public class ThompsonNode
    {
        public NFANode head;
        public string edge;
        public NFANode tail;

        public ThompsonNode()
        {

        }
        public ThompsonNode(NFANode head, string edge, NFANode tail)
        {
            this.head = head;
            this.edge = edge;
            this.tail = tail;
        }
        //创建带返回值的2点1线
        public ThompsonNode CreatSingle(string edge,int startIndex)
        {
            NFANode a = new NFANode(startIndex, false);
            startIndex++;
            NFANode b = new NFANode(startIndex, false);
            startIndex++;
            ThompsonNode aa = new ThompsonNode(a, edge, b);
            return aa;
        }
        //创建普通2点1线
        public void CreatSingle(string edge, ref int startIndex, ref List<ThompsonNode> ThompsonNodeList)
        {
            NFANode a = new NFANode(startIndex, false);
            startIndex++;
            NFANode b = new NFANode(startIndex, false);
            startIndex++;
            ThompsonNode aa = new ThompsonNode(a, edge, b);
            ThompsonNodeList.Add(aa);
        }

        //创建1对2节点
        public void CreatSingle(string edge, int Index, int endIndex1, int endIndex2, ref List<ThompsonNode> ThompsonNodeList,string type)
        {
            if(type=="1")
            {
                NFANode a = new NFANode(Index, false);
                NFANode b = new NFANode(endIndex1, true);
                NFANode c = new NFANode(endIndex2, true);
                ThompsonNode r1 = new ThompsonNode(a, edge, b);
                ThompsonNode r2 = new ThompsonNode(a, edge, c);
                ThompsonNodeList.Add(r1);
                ThompsonNodeList.Add(r2);
            }
            else
            {
                NFANode a = new NFANode(Index, false);
                NFANode b = new NFANode(endIndex1, true);
                NFANode c = new NFANode(endIndex2, true);
                ThompsonNode r1 = new ThompsonNode(b, edge, a);
                ThompsonNode r2 = new ThompsonNode(c, edge, a);
                ThompsonNodeList.Add(r1);
                ThompsonNodeList.Add(r2);
            }

        }

        //创建空对空节点
        public void CreatSingle(string edge,int Index1,int Index2, ref List<ThompsonNode> ThompsonNodeList)
        {
            NFANode a = new NFANode(Index1, true);//创建空对空节点用于替换
            NFANode b = new NFANode(Index2, true);
            ThompsonNode r1 = new ThompsonNode(a, edge, b);
            ThompsonNodeList.Add(r1);
        }

    }

    //或连接类
    public class Or
    {

        public NFANode Or_Head;
        public NFANode Or_Tail;

        public Or()
        {

        }
        public Or(ref int startIndex, ref List<ThompsonNode> ThompsonNodeList)
        {
            Or_Head = new NFANode(startIndex, false);//Or结构的头节点
            ThompsonNode t1 = new ThompsonNode();
            t1.CreatSingle("ε", startIndex, startIndex + 1, startIndex + 3, ref ThompsonNodeList,"1");
            t1.CreatSingle("empty", startIndex + 1, startIndex + 2, ref ThompsonNodeList);

            startIndex += 5;
            Or_Tail = new NFANode(startIndex, false);//Or结构的尾节点
            t1.CreatSingle("ε", startIndex, startIndex - 3, startIndex - 1, ref ThompsonNodeList,"2");
            t1.CreatSingle("empty", startIndex - 2, startIndex - 1, ref ThompsonNodeList);
            startIndex++;
        }

        public void LinkOr(List<ThompsonNode> temp, ref List<ThompsonNode> ThompsonNodeList)//链接Single类节点
        {
            var q = (from f in ThompsonNodeList where f.head.isEmpty == true && f.edge == "empty" && f.tail.isEmpty == true select f).ToList();
            if(q.Count==0)
            {
                return;
            }
            else
            {
                for (int i = 0; i < q.Count; i++)
                {
                    NFANode temp_head = q[i].head;
                    NFANode temp_tail = q[i].tail;
                    ThompsonNodeList.Remove(q[i]);
                    //ThompsonNodeList.Add(temp[i]);
                    for (int j = 0; j < ThompsonNodeList.Count; j++)
                    {
                        if (ThompsonNodeList[j].head.NodeID == temp_head.NodeID)
                        {
                            ThompsonNodeList[j].head.NodeID = temp[i].head.NodeID;
                        }
                        if (ThompsonNodeList[j].tail.NodeID == temp_head.NodeID)
                        {
                            ThompsonNodeList[j].tail.NodeID = temp[i].head.NodeID;
                        }
                        if (ThompsonNodeList[j].head.NodeID == temp_tail.NodeID)
                        {
                            ThompsonNodeList[j].head.NodeID = temp[i].tail.NodeID;
                        }
                        if (ThompsonNodeList[j].tail.NodeID == temp_tail.NodeID)
                        {
                            ThompsonNodeList[j].tail.NodeID = temp[i].tail.NodeID;
                        }
                    }
                }
            }
        }
    }

    //闭包连接类
    public class Star
    {
        public NFANode Star_Head;
        public NFANode Star_Tail;

        public Star() 
        { 

        }
        public Star(ref int startIndext,ref List<ThompsonNode> ThompsonNodeList)
        {
            Star_Head = new NFANode(startIndext, false);//Star节点的头节点
            NFANode a = new NFANode(startIndext, false);
            startIndext++;
            NFANode b = new NFANode(startIndext, true);
            startIndext++;
            NFANode c = new NFANode(startIndext, true);
            startIndext++;
            NFANode d = new NFANode(startIndext, false);
            Star_Tail = new NFANode(startIndext, false);//Star节点的尾节点
            ThompsonNode t1 = new ThompsonNode(a, "ε", b);
            ThompsonNode t2 = new ThompsonNode(c, "ε", b);
            ThompsonNode t3 = new ThompsonNode(c, "ε", d);
            ThompsonNode t4 = new ThompsonNode(a, "ε", d);
            ThompsonNodeList.Add(t1);
            ThompsonNodeList.Add(t2);
            ThompsonNodeList.Add(t3);
            ThompsonNodeList.Add(t4);
            startIndext++;
        }

        public void LinkStar(Or temp, ref List<ThompsonNode> ThompsonNodeList)//链接Or结构
        {
            var q = (from f in ThompsonNodeList where f.head.isEmpty == true && f.edge == "ε" && f.tail.isEmpty == true select f).ToList();
            if (q.Count == 0)
            {
                return;
            }
            else
            {
                for (int i = 0; i < q.Count; i++)
                {
                    NFANode temp_head = q[i].head;
                    NFANode temp_tail = q[i].tail;
                    //ThompsonNodeList.Remove(q[i]);
                    //ThompsonNodeList.Add(temp[i]);
                    for (int j = 0; j < ThompsonNodeList.Count; j++)
                    {
                        if (ThompsonNodeList[j].head.NodeID == q[i].head.NodeID && ThompsonNodeList[j].tail.NodeID == q[i].tail.NodeID)
                        {
                            ThompsonNodeList[j].head.NodeID = temp.Or_Tail.NodeID;
                            ThompsonNodeList[j].tail.NodeID = temp.Or_Head.NodeID;
                        }
                        if(ThompsonNodeList[j].tail.NodeID==temp_tail.NodeID)
                        {
                            ThompsonNodeList[j].tail.NodeID = temp.Or_Head.NodeID;
                        }

                        if (ThompsonNodeList[j].head.NodeID == temp_head.NodeID)
                        {
                            ThompsonNodeList[j].head.NodeID = temp.Or_Tail.NodeID;
                        }
                    }
                }
            }
        }
    }

    //闭包类
    public class BiBao
    {
        public string BiBaoName;
        public string BiBaoJi;

        public BiBao()
        {
        }

        public BiBao(string BiBaoName, string BiBaoJi)
        {
            this.BiBaoName = BiBaoName;
            this.BiBaoJi = BiBaoJi;
        }

        public List<int> GetBiBaoIntList(string BiBaoJi)
        {
            List<int> temp = new List<int>();
            string[] temp_bibaoji=BiBaoJi.Split(',');
            for(int i=0;i<temp_bibaoji.Length-1;i++)
            {
                temp.Add(Convert.ToInt32(temp_bibaoji[i]));
            }
            return temp;
        }
    }

    //路径类
    public class Route
    {
        public string r_from;
        public string r_via;
        public string r_to;

        public Route(string r_from,string r_via,string r_to)
        {
            this.r_from = r_from;
            this.r_via = r_via;
            this.r_to = r_to;
        }
    }


    //自定义栈

    public class MyStack
    {
        private int maxSize;//栈的最大容量
        private char[] ch;	//栈的数据
        private int top;	//栈头标记

        public MyStack(int s)
        {
            maxSize = s;
            ch = new char[s];
            top = -1;
        }

        //入栈
        public void push(char c)
        {
            ch[++top] = c;
        }

        //出栈
        public char pop()
        {
            return ch[top--];
        }

        public char peek()
        {
            return ch[top];
        }

        public bool isEmpty()
        {
            return top == -1;
        }

        public bool isFull()
        {
            return top == (maxSize - 1);
        }

        public int size()
        {
            return top + 1;
        }

        public char get(int index)
        {
            return ch[index];
        }

        //public void display(String str) {
        //    System.out.print(str);
        //    System.out.print(" Stack (bottom-->top): ");
        //    for (int i = 0; i < size(); i++) {
        //        System.out.print(get(i)+" ");
        //    }
        //    System.out.println();
        //}
    }


    public class InToPost
    {
        private MyStack ms;//自定义栈
        private String input;//输入中缀表达式
        private String output = "";//输出的后缀表达式

        public InToPost(String input)
        {
            this.input = input;
            int size = input.Length;
            ms = new MyStack(size);
        }

        public String doTrans()
        {
            //转换为后缀表达式方法
            for (int i = 0; i < input.Length; i++)
            {
                char ch = input[i];
                //ms.display("for " + ch + " ");
                switch (ch)
                {
                    case '|':
                    //case '-':
                        getOper(ch, 1);
                        break;
                    case '+':
                        getOper(ch, 2);
                        break;
                    case '*':
                        getOper(ch, 3);
                        break;
                    case '(':
                        ms.push(ch);
                        break;
                    case ')':
                        getParent(ch);
                        break;
                    default:
                        output = output + ch;
                        break;
                }//end switch 
            }//end for
            while (!ms.isEmpty())
            {
                //ms.display("While ");
                output = output + ms.pop();
            }
            //ms.display("end while!!");
            return output;
        }

        /**
         * @param ch
         * 获得上一级字符串
         */
        public void getParent(char ch)
        {
            while (!ms.isEmpty())
            {
                char chx = ms.pop();
                if (chx == '(')
                {
                    break;
                }
                else
                {
                    output = output + chx;
                }
            }
        }

        /**
         * @param ch 操作符
         * @param prec1 操作符的优先级
         * 根据操作符的优先级判断是否入栈，及入栈的顺序
         */
        public void getOper(char ch, int prec1)
        {
            while (!ms.isEmpty())
            {//判断栈是否为空
                char operTop = ms.pop();
                if (operTop == '(')
                {
                    ms.push(operTop);
                    break;
                }
                else
                {
                    int prec2;
                    if (operTop == '|')
                    {
                        prec2 = 1;
                    }
                    else if (operTop=='+')
                    {
                        prec2 = 2;
                    }
                    else
                    {
                        prec2 = 3;
                    }
                    if (prec2 < prec1)
                    {
                        ms.push(operTop);
                        break;
                    }
                    else
                    {
                        output = output + operTop;
                    }
                }
            }// end while
            ms.push(ch);
        }
    }






}
