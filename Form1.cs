using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DFA
{
    public partial class Form1 : Form
    {
        public static int startStatus = 0;
        public static int finalStatus = 3;       
        public Form1()
        {
            InitializeComponent();
        }

        public ExpToNFA ExpToNFA = new ExpToNFA();
        NFANode Now_HeadNode = new NFANode(-1, false);
        NFANode Now_TailNode=new NFANode(-1,false);
        ThompsonNode thompsonnode = new ThompsonNode();
        List<ThompsonNode> ThompsonNodeList = new List<ThompsonNode>();
        List<ThompsonNode> ThompsonNodeList_forlink = new List<ThompsonNode>();
        List<BiBao> BiBaoList = new List<BiBao>();
        List<Route> RouteList = new List<Route>();
        Or or_forlink = new Or();
        int startIndex = 0;//节点起始索引

        string[] Status = new string[26] { "A", "B", "C", "D", "E", "F", "G", "H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};
        int StatusIndex = 0;
        List<int> ziji = new List<int>();



        private void button1_Click(object sender, EventArgs e)
        {
            //string text = this.textBox3.Text;
            //char[] Temp = text.ToCharArray();
            //DFA(Temp);
        }

        public void DFA(char[] Temp)
        {
            int start = startStatus;
            for(int i=0;i<Temp.Length;i++)
            {
                Format temp = new Format(start, Temp[i]);
                int result = temp.GetTransitionNextStatus(temp);
                if(result!=-1)
                {
                    start = result;
                }
            }
            if(start==finalStatus)
            {
                MessageBox.Show("符合规范！");
            }
            else
            {
                MessageBox.Show("不符合规范！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string regularExpression = textBox1.Text;
            if (regularExpression[0] == '|' || regularExpression[regularExpression.Length - 1] == '|')
            {
                MessageBox.Show("正规式格式错误!");
                return;
            }
            int count = 0;
            for (int i = 0; i < regularExpression.Length; i++)
            {
                if (regularExpression[i] == ')' && (i + 1) < regularExpression.Length && regularExpression[i + 1] != '*')
                {
                    MessageBox.Show("正规式格式错误!");
                    return;
                }
                if (regularExpression[i] == '(')
                    count++;
                if (regularExpression[i] == ')')
                    count--;
                if (count < 0)
                {
                    MessageBox.Show("正规式格式错误!");
                    return;
                }
            }
            if (count != 0)
            {
                MessageBox.Show("正规式格式错误!");
                return;
            }
            MessageBox.Show("正规式合格");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string Exp = this.textBox1.Text;
            string Back=ExpToNFA.ExpToBack(Exp);
            for (int i = 0; i < Back.Length; i++)
            {
                switch (Back[i])
                {
                    case('+'):
                        {
                            startIndex--;
                            break;
                        }
                    case ('*'):
                        {
                            Star star = new Star(ref startIndex, ref ThompsonNodeList);
                            star.LinkStar(or_forlink, ref ThompsonNodeList);
                            //Now_TailNode.NodeID = star.Star_Tail.NodeID;
                            break;                  
                        }
                    case ('|'):
                        {
                            Or or = new Or(ref startIndex, ref ThompsonNodeList);
                            or.LinkOr(ThompsonNodeList_forlink, ref ThompsonNodeList);
                            or_forlink = or;
                            ThompsonNodeList_forlink.Clear();
                            //Now_TailNode.NodeID = or.Or_Tail.NodeID;
                            break;
                        }
                    default:
                        {
                            //Now_HeadNode.NodeID = startIndex;
                            ThompsonNodeList_forlink.Add(thompsonnode.CreatSingle(Back[i].ToString(), startIndex));
                            thompsonnode.CreatSingle(Back[i].ToString(), ref startIndex, ref ThompsonNodeList);
                            //Now_TailNode.NodeID = startIndex;
                            break;
                        }
                }
            }
            string finalResult=string.Empty;
            finalResult += "From" + "\t" + "varch" + "\t" + "To" + "\r\n"; 

            for(int i=0;i<ThompsonNodeList.Count;i++)
            {
                finalResult += ThompsonNodeList[i].head.NodeID.ToString() + "\t" + ThompsonNodeList[i].edge + "\t" + ThompsonNodeList[i].tail.NodeID.ToString() + "\r\n";
            }
            this.textBox2.Text = finalResult;
            //NFAResult nn = new NFAResult(finalResult);
           // nn.Show();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            
            int HeadIndex=10;
            int TailIndex = 16;

            string bibaoji=string.Empty;

            newnodegetempty(HeadIndex, bibaoji);

            for (int i = 0; i < BiBaoList.Count; i++)
            {
                string[] temp = BiBaoList[i].BiBaoJi.Split(',');
                List<int> tempListfora = new List<int>();
                bool flag = true;
                for (int j = 0; j < temp.Length - 1; j++)
                {
                    int id = Convert.ToInt32(temp[j]);

                    var q_a = (from f in ThompsonNodeList where f.head.NodeID == id && f.edge == "a" select f.tail.NodeID).ToList();

                    if (q_a.Count != 0)
                    {
                        for (int p = 0; p < temp.Length; p++)
                        {
                            if (temp[p] == q_a[0].ToString())
                                flag = false;
                        }
                        if(flag == true)
                        {
                            tempListfora.Add(q_a[0]);
                        }
                        
                    }

                    var q_b = (from f in ThompsonNodeList where f.head.NodeID == id && f.edge == "b" select f.tail.NodeID).ToList();

                    if (q_b.Count != 0)
                    {
                        for (int p = 0; p < temp.Length; p++)
                        {
                            if (temp[p] == q_b[0].ToString())
                                flag = false;
                        }
                        if (flag == true)
                        {
                            tempListfora.Add(q_b[0]);
                        }

                    }
                }
                if(tempListfora.Count>0)
                {
                    string r_to = newnodegetempty(tempListfora, BiBaoList[i].BiBaoJi);
                    Route aaa = new Route(BiBaoList[i].BiBaoName, "a", r_to);
                    RouteList.Add(aaa);
                }


            }

            for(int i=0;i<RouteList.Count;i++)
            {
                MessageBox.Show(RouteList[i].r_from+RouteList[i].r_via+RouteList[i].r_to);
            }
        }

        public string newnodegetempty(int nodeid,string bibaoji)
        {
            //string bibaoji_temp = bibaoji;
            string newroutename = string.Empty;

            ziji.Add(nodeid);
            //string bibaoji_new = string.Empty;
            bibaoji += nodeid.ToString()+",";
            for (int i = 0; ; i++)
            {
                var q = (from f in ThompsonNodeList where f.head.NodeID == ziji[i] && f.edge == "ε" select f.tail.NodeID).ToList();
                if (q.Count != 0)
                {
                    for (int k = 0; k < q.Count;k++ )
                    {
                        bibaoji += q[k].ToString() + ",";
                        ziji.Add(q[k]);
                    }
                    continue;
                }
                var q_check = (from f in BiBaoList where f.BiBaoJi == bibaoji select f).ToList();
                if (q_check.Count == 0)
                {
                    BiBao temp = new BiBao(Status[StatusIndex], bibaoji);
                    newroutename = Status[StatusIndex];
                    BiBaoList.Add(temp);
                    StatusIndex++;
                }
                break;
            }
            ziji.Clear();
            return newroutename;
            //bibaoji = bibaoji_temp;
            //return bibaoji;
        }

        public string newnodegetempty(List<int> nodeid, string bibaoji)
        {
            //string bibaoji_temp = bibaoji;
            string newroutename = string.Empty;

            for (int i = 0; i < nodeid.Count;i++ )
            {
                bibaoji += nodeid[i].ToString() + ",";
            }
                
            for (int i = 0; ; i++)
            {
                var q = (from f in ThompsonNodeList where f.head.NodeID == nodeid[i] && f.edge == "ε" select f.tail.NodeID).ToList();
                if (q.Count != 0)
                {
                    for (int k = 0; k < q.Count; k++)
                    {
                        bibaoji += q[k].ToString() + ",";
                        nodeid.Add(q[k]);
                    }
                    continue;
                }
                var q_check = (from f in BiBaoList where f.BiBaoJi == bibaoji select f).ToList();
                if (q_check.Count == 0)
                {
                    BiBao temp = new BiBao(Status[StatusIndex], bibaoji);
                    newroutename = Status[StatusIndex];
                    BiBaoList.Add(temp);
                    StatusIndex++;
                }
                break;
            }
            //ziji.Clear();
            return newroutename;
            //bibaoji = bibaoji_temp;
            //return bibaoji;
        }
    }
}
