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



        private void button1_Click(object sender, EventArgs e)
        {
            string text = this.textBox1.Text;
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


        public const int NFA_Length = 128;
        public NFA[] nfa = new NFA[NFA_Length];
        private int index = 0;          //数组nfa当前的下标指针
        private int from = 0;          //数组nfa的from值
        private int[] flag = new int[10];
        private int p_flag = 0;
        private int[] flagIndex = new int[10];
        private int p_flagIndex = 0;
        private string regularExpression;
        private void button2_Click(object sender, EventArgs e)
        {
            regularExpression = textBox1.Text;
            index = -1;
            from = -1;
            nfa[++index] = new NFA(from, 'S');
            //textBox2.Text = "";
            for (int i = 0; i < regularExpression.Length; i++)
            {
                if (regularExpression[i] != '(' && regularExpression[i] != ')' && regularExpression[i] != '*')
                {
                    if ((i + 1) < regularExpression.Length && regularExpression[i + 1] == '|')
                    {
                        from++;
                        nfa[index].to = from;
                        nfa[++index] = new NFA(from, 'ε');
                        nfa[++index] = new NFA(from, 'ε');
                        nfa[++index] = new NFA(++from, regularExpression[i]);
                        nfa[index - 2].to = from;
                        i = i + 2;                                                                                  //跳过 '/' 
                        nfa[++index] = new NFA(++from, regularExpression[i]);
                        nfa[index - 2].to = from;
                        nfa[++index] = new NFA(++from, 'ε');
                        nfa[index - 1].to = from;
                        nfa[index - 2].to = from;
                    }
                    else if ((i + 1) < regularExpression.Length && regularExpression[i + 1] == '*')
                    {
                        nfa[++index] = new NFA(++from, 'ε');
                        int flagsIndex = index;
                        nfa[index - 1].to = from;
                        nfa[++index] = new NFA(from, 'ε');

                        nfa[++index] = new NFA(++from, regularExpression[i]);
                        nfa[index - 1].to = from;
                        nfa[++index] = new NFA(++from, 'ε');
                        nfa[index].to = from - 1;
                        nfa[index - 1].to = from;
                        nfa[++index] = new NFA(from, 'ε');
                        nfa[flagsIndex].to = from;
                    }

                    else
                    {
                        nfa[++index] = new NFA(++from, regularExpression[i]);
                        nfa[index - 1].to = from;
                    }
                }
                else if (regularExpression[i] == '(')
                {
                    nfa[++index] = new NFA(++from, 'ε');
                    nfa[index - 1].to = from;
                    flagIndex[p_flagIndex] = index;
                    p_flagIndex++;
                    nfa[++index] = new NFA(from, 'ε');
                    flag[p_flag] = from + 1;
                    p_flag++;
                }
                else if (regularExpression[i] == ')')
                {

                    nfa[++index] = new NFA(++from, 'ε');
                    nfa[index - 1].to = from;
                    p_flag--;
                    nfa[index].to = flag[p_flag];
                    nfa[++index] = new NFA(from, 'ε');
                    p_flagIndex--;
                    nfa[flagIndex[p_flagIndex]].to = from;
                    i = i + 1;                                                                               //跳过 '*' 
                }
            }
            nfa[index].to = nfa[index].from + 1;
            string finalResult = string.Empty;
            finalResult += "From" + "\t" + "varch" + "\t" + "To" + "\r\n"; 
            //textBox2.Text = "From" + "\t" + "varch" + "\t" + "To" + "\r\n";
            for (int i = 0; i <= index; i++)
            {
                //textBox2.Text += nfa[i].from.ToString() + "\t" + nfa[i].varch.ToString() + "\t" + nfa[i].to.ToString() + "\r\n";
                finalResult += nfa[i].from.ToString() + "\t" + nfa[i].varch.ToString() + "\t" + nfa[i].to.ToString() + "\r\n";
            }
            NFAResult nn = new NFAResult(finalResult);
            nn.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            regularExpression = textBox1.Text;
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
        }

        public struct NFA
        {
            public int from;
            public char varch;
            public int to;
            public NFA(int from, char varch)
            {
                this.from = from;
                this.varch = varch;
                this.to = -1;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string Exp = this.textBox1.Text;
            string Back=ExpToNFA.ExpToBack(Exp);
        }
    }
}
