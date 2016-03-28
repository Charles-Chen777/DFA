using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFA
{
    public class Format
    {
        public int TransitionStatus;
        public char TransitionString;
        public int TransitionNextStatus;
        public Format(int a, char b)
        {
            this.TransitionStatus = a;
            this.TransitionString = b;
            //this.TransitionNextStatus = c;
        }

        public int GetTransitionNextStatus(Format a)
        {
            int result = -1;
            string Expression =a.TransitionStatus.ToString()+a.TransitionString;
            switch (Expression)
            {
                case "0a":
                    {
                        result=1;
                        break;
                    }
                case "0b":
                    {
                        result = 0;
                        break;
                    }
                case "1a":
                    {
                        result = 1;
                        break;
                    }
                case "1b":
                    {
                        result = 2;
                        break;
                    }
                case "2a":
                    {
                        result = 1;
                        break;
                    }
                case "2b":
                    {
                        result = 3;
                        break;
                    }
                case "3a":
                    {
                        result = 1;
                        break;
                    }
                case "3b":
                    {
                        result = 0;
                        break;
                    }
            }
            return result;
        }

    }
}
