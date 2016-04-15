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
        Or or_forlink = new Or();
        int startIndex = 0;//节点起始索引



        private void button1_Click(object sender, EventArgs e)
        {
            string text = this.textBox3.Text;
            char[] Temp = text.ToCharArray();
            DFA(Temp);
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
            string[] Status = new string[8] { "A", "B", "C", "D", "E", "F", "G", "H" };
            int StatusIndex=0;
            for(int i=0;i<ThompsonNodeList.Count;i++)
            {
                int temp = ThompsonNodeList[i].head.NodeID;
                var q = (from f in ThompsonNodeList where f.head.NodeID == temp && f.edge == "ε" select f.tail.NodeID).ToList();
                string bibaoji=string.Empty;
                for(int j=0;j<q.Count;j++)
                {
                    bibaoji+=q[j].ToString();
                }
                
                var check = (from f in BiBaoList where f.BiBaoJi == bibaoji select f).ToList();
                if(check.Count==0)
                {
                    BiBao bibao1 = new BiBao(Status[StatusIndex], bibaoji);
                    BiBaoList.Add(bibao1);
                }         
            }
            for (int i = 0; i < BiBaoList.Count;i++ )
            {
                MessageBox.Show(BiBaoList[i].BiBaoName + " " + BiBaoList[i].BiBaoJi);

            }
                
        }
    }
}
