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
        public Form1()
        {
            InitializeComponent();
        }

        public ExpToNFA ExpToNFA = new ExpToNFA();
        //NFANode Now_HeadNode = new NFANode(-1, false);
        //NFANode Now_TailNode=new NFANode(-1,false);

        int HeadIndex = -1;//生成的NFA序列的头节点序号
        int TailIndex = -1;//生成的NFA序列的尾巴节点序号
        ThompsonNode thompsonnode = new ThompsonNode();
        List<ThompsonNode> ThompsonNodeList = new List<ThompsonNode>();
        List<ThompsonNode> ThompsonNodeList_forlink = new List<ThompsonNode>();
        List<BiBao> BiBaoList = new List<BiBao>();
        BiBao BiBaoMethod = new BiBao();
        List<Route> RouteList = new List<Route>();
        string HeadRouteStatus = string.Empty;//DFA头节点标识
        string TailRouteStatus = string.Empty;//DFA尾巴节点标识

        Or or_forlink = new Or();
        int startIndex = 0;//NFANode节点起始索引

        string[] Status = new string[26] { "A", "B", "C", "D", "E", "F", "G", "H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};
        int StatusIndex = 0;
        List<int> ziji = new List<int>();


        //化简DFA的比较函数
        public bool CompareRoute(string from_A, string from_B)
        {
            var q_A = (from f in RouteList where f.r_from == from_A select f.r_to).ToList();
            var q_B = (from f in RouteList where f.r_from == from_B select f.r_to).ToList();

            string R_A = string.Empty;
            string R_B = string.Empty;

            for (int i = 0; i < q_A.Count; i++)
            {
                R_A += q_A[i];
            }
            for (int i = 0; i < q_B.Count; i++)
            {
                R_B += q_B[i];
            }
            if (R_A == R_B && (from_A != TailRouteStatus || from_B != TailRouteStatus))
            {
                return true;
            }
            return false;
        }

        //化简DFA
        private void button1_Click(object sender, EventArgs e)
        {
            //string text = this.textBox3.Text;
            //char[] Temp = text.ToCharArray();
            //DFA(Temp);
            var q = (from f in RouteList group RouteList by f.r_from into g select new { g.Key }).ToList();
            for (int i = 0; i < q.Count-1; i++)
            {
                for(int j=i+1;j<q.Count-1;j++)
                {
                    if(CompareRoute(q[i].Key,q[j].Key))
                    {
                        var q_remove = (from f in RouteList where f.r_from == q[j].Key select f).ToList();
                        for (int k = 0; k < q_remove.Count;k++ )
                        {
                            Route temp = q_remove[k];
                            RouteList.Remove(temp);
                        }

                        for(int m=0;m<RouteList.Count;m++)
                        {
                            if(RouteList[m].r_to==q[j].Key)
                            {
                                RouteList[m].r_to = q[i].Key;
                            }
                        }

                    }
                }
            }

            string finalResult = string.Empty;

            finalResult += "From" + "\t" + "via" + "\t" + "To" + "\r\n";

            for (int i = 0; i < RouteList.Count; i++)
            {
                finalResult += RouteList[i].r_from.ToString() + "\t" + RouteList[i].r_via + "\t" + RouteList[i].r_to.ToString() + "\r\n";
            }
            this.textBox5.Text = finalResult;
            
        }

        //正规式检测
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

        //转换为NFA
        private void button4_Click(object sender, EventArgs e)
        {
            this.textBox1.Enabled = false;
            this.textBox3.Enabled = false;
            string Exp = this.textBox1.Text;
            //转换为后缀表达式
            string Back=ExpToNFA.ExpToBack(Exp);

            for (int i = 0; i < Back.Length; i++)
            {
                switch (Back[i])
                {
                    case('+'):
                        {
                            //startIndex--;
                            break;
                        }
                    case ('*'):
                        {
                            Star star = new Star(ref startIndex, ref ThompsonNodeList);
                            star.LinkStar(or_forlink, ref ThompsonNodeList);
                            HeadIndex = star.Star_Head.NodeID;//标记头节点
                            TailIndex = star.Star_Tail.NodeID;//标记尾巴节点
                            break;                  
                        }
                    case ('|'):
                        {
                            Or or = new Or(ref startIndex, ref ThompsonNodeList);
                            or.LinkOr(ThompsonNodeList_forlink, ref ThompsonNodeList);
                            or_forlink = or;
                            ThompsonNodeList_forlink.Clear();
                            HeadIndex = or.Or_Head.NodeID;//标记头节点
                            TailIndex = or.Or_Tail.NodeID;//标记尾巴节点
                            break;
                        }
                    default:
                        {
                            if (Back[i+1]=='+')
                            {
                                startIndex--;
                            }
                            ThompsonNodeList_forlink.Add(thompsonnode.CreatSingle(Back[i].ToString(), startIndex));
                            thompsonnode.CreatSingle(Back[i].ToString(), ref startIndex, ref ThompsonNodeList);
                            TailIndex = startIndex-1;//标记尾巴节点
                            break;
                        }
                }
            }
            string finalResult=string.Empty;
            finalResult += "From" + "\t" + "edge" + "\t" + "To" + "\r\n"; 

            for(int i=0;i<ThompsonNodeList.Count;i++)
            {
                finalResult += ThompsonNodeList[i].head.NodeID.ToString() + "\t" + ThompsonNodeList[i].edge + "\t" + ThompsonNodeList[i].tail.NodeID.ToString() + "\r\n";
            }
            this.textBox2.Text = finalResult;
        }

        //转换为DFA
        private void button2_Click(object sender, EventArgs e)
        {
            string bibaoji=string.Empty;

            newnodegetempty(HeadIndex, bibaoji);

            for (int i = 0; i < BiBaoList.Count; i++)
            {
                List<int> tempListfora = BiBaoMethod.GetBiBaoIntList(BiBaoList[i].BiBaoJi);
                string bibaojifora = string.Empty;
                string bibaojiforb = string.Empty;
                //字符a
                var q_aa = (from f in ThompsonNodeList where f.edge == "a" select new { headNodeID = f.head.NodeID, tailNodeID = f.tail.NodeID }).ToList();
                var q_a = (from f in q_aa where tempListfora.Contains(f.headNodeID) select f.tailNodeID).ToList();
                if (q_a.Count != 0)
                {
                    for (int j = 0; j < q_a.Count; j++)
                    {
                        ziji.Add(q_a[j]);
                        bibaojifora += q_a[j].ToString() + ",";
                    }
                }

                if (ziji.Count > 0)
                {

                    string r_to = newnodegetempty(ziji, bibaojifora);
                    Route aaa = new Route(BiBaoList[i].BiBaoName, "a", r_to);
                    RouteList.Add(aaa);
                }
                else
                {
                    string r_to = newnodegetempty(BiBaoMethod.GetBiBaoIntList(BiBaoList[i].BiBaoJi), BiBaoList[i].BiBaoJi);
                    Route aaa = new Route(BiBaoList[i].BiBaoName, "a", r_to);
                    RouteList.Add(aaa);
                }  

                //字符b
                var q_bb = (from f in ThompsonNodeList where f.edge == "b" select new{headNodeID=f.head.NodeID,tailNodeID=f.tail.NodeID}).ToList();
                var q_b = (from f in q_bb where tempListfora.Contains(f.headNodeID) select f.tailNodeID).ToList();
                if (q_b.Count != 0)
                {
                    for (int j = 0; j < q_b.Count; j++)
                    {
                        ziji.Add(q_b[j]);
                        bibaojiforb += q_b[j].ToString() + ",";
                    }
                }
                if (ziji.Count > 0)
                {
                    string r_to = newnodegetempty(ziji, bibaojiforb);
                    Route aaa = new Route(BiBaoList[i].BiBaoName, "b", r_to);
                    RouteList.Add(aaa);
                }
                else
                {
                    string r_to = newnodegetempty(BiBaoMethod.GetBiBaoIntList(BiBaoList[i].BiBaoJi), BiBaoList[i].BiBaoJi);
                    Route aaa = new Route(BiBaoList[i].BiBaoName, "b", r_to);
                    RouteList.Add(aaa);
                }
            }


            //结果制表
            string finalResult = string.Empty;

            finalResult += "From" + "\t" + "via" + "\t" + "To" + "\r\n"; 

            for(int i=0;i<RouteList.Count;i++)
            {
                finalResult += RouteList[i].r_from.ToString() + "\t" + RouteList[i].r_via + "\t" + RouteList[i].r_to.ToString() + "\r\n";
            }
            this.textBox4.Text = finalResult;
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
                    HeadRouteStatus = newroutename;
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

            bool flag = true;
            //string bibaoji_temp = bibaoji;
            string newroutename = string.Empty;

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
                }
                if (i < nodeid.Count-1)
                {
                    continue;
                }  
                var q_check = (from f in BiBaoList where f.BiBaoJi == bibaoji select f).ToList();
                List<int> A = BiBaoMethod.GetBiBaoIntList(bibaoji);
                for (int k = 0; k < BiBaoList.Count; k++)
                {
                    List<int> B = BiBaoMethod.GetBiBaoIntList(BiBaoList[k].BiBaoJi);
                    //使用差集比较两个闭包集合是否相等
                    if (A.Except(B).Count() == 0)
                    {
                        newroutename=BiBaoList[k].BiBaoName;
                        flag = false;
                        break;
                    }
                }
                if (flag == true)
                {
                    BiBao temp = new BiBao(Status[StatusIndex], bibaoji);
                    newroutename = Status[StatusIndex];
                    TailRouteStatus = newroutename;
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


        //识别文本
        private void button5_Click(object sender, EventArgs e)
        {
            string exp=this.textBox3.Text;
            string nowr_from = HeadRouteStatus;//从头结点开始识别
            for(int i=0;i<exp.Length;i++)
            {
                string temp=exp[i].ToString();
                var q = (from f in RouteList where f.r_from == nowr_from && f.r_via == temp select f.r_to).ToList();
                nowr_from = q[0];
            }

            if (nowr_from == TailRouteStatus)//到达尾节点
            {
                MessageBox.Show("符合规范");
            }
            else
            {
                MessageBox.Show("不符合规范");
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Application.Restart();
            //System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
