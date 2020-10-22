using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {

        static void Main(string[] args)
        {
            while (true) {
                Console.WriteLine("Enter a statement like 5*5+2/3+(((5)*(7)))-3^2 or exit for exit");
                string input = Console.ReadLine();
                if (input.Equals("exit"))
                    break;
                calculate(input);
            }
            Console.WriteLine("Good Bye");
            Console.ReadLine();
        }
        public static List<double> getNumbers(string s) {
            List<double> nums = new List<double>();
            for (int i = 0; i < s.Length;i++ ){
                string temp = "";
                bool minus = false;
                if (i > 0 && s[i] == '-' && (s[i - 1] == '-' || s[i - 1] == '+' || s[i - 1] == '*' || s[i - 1] == '/'||s[i-1]=='^'))
                {
                    minus = true;
                    i++;
                }
                while (i < s.Length && (s[i] >= '0' && s[i] <= '9' || s[i] == '.'))
                    temp += s[i++];
                if (temp.Length > 0)
                {
                    if(minus)
                        nums.Add(double.Parse(temp)*-1);
                    else
                        nums.Add(double.Parse(temp));
                }
            }
            return nums;
        }
        public static List<char> getOperators(string s) {
            List<char> oper = new List<char>();
            for (int i = 0; i < s.Length;i++ )
                switch (s[i]) { 
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                    case '^':
                        oper.Add(s[i]);
                        break;
                }
            return oper;
        }
        public static string eraseSubString(string s, int start, int end) {
            string res = "";
            for (int i = 0; i < start; i++)
                res += s[i];
            for (int i = end + 1; i < s.Length; i++)
                res += s[i];
            return res;
        }
        public static string eraseTwoElements(string s, int start, int end) {
            string res = "";
            for (int i = 0; i < start; i++)
                res+=s[i];
            for (int i = start + 1; i < end;i++ )
                res+=s[i];
            for (int i = end + 1; i < s.Length; i++)
                res += s[i];
            return res;
        }
        public static string cleanScopes(string s) {
            for (int i = 0; i < s.Length; i++) {
                if (s[i] == ')') {
                    int start=i-1;
                    int count = 0;
                    bool oper = false;
                    while (s[start] != ')' && s[start] != '('){
                        if (s[start] == '+' || s[start] == '-' || s[start] == '*' || s[start] == '/')
                            oper = true;
                        start--;
                    }
                    if (oper)
                        continue;
                    start=i;
                    while (s[start] != '(') {
                        if (s[start] == ')')
                            count++;
                        start--;
                    }
                    while (count > 0) {
                        if (s[start] == '(')
                            count--;
                        start--;
                    }
                    start++;
                    int j = start+1;
                    while (s[j] != ')' && s[j] != '(')
                    {
                        if (s[j] == '+' || s[j] == '-' || s[j] == '*' || s[j] == '/')
                            oper = true;
                        j++;
                    }
                    if (oper)
                        continue;
                    s=eraseTwoElements(s, start, i);
                    i -= 2;
                }
            }
            return s;
        }
        public static string change(string s) {
            string res = "";
            for (int i = 0; i < s.Length;i++ ){
                res += s[i];
                if (i<s.Length-1 && s[i+1] == '(' && s[i] >= '0' && s[i] <= '9' )
                    res += '*';
                if (i < s.Length - 1 && s[i] == ')' && s[i +1] >= '0' && s[i +1] <= '9')
                    res += '*';
                if (i > 0 && i < s.Length - 1&&s[i]=='-'&&s[i+1]=='('&&s[i-1]!=')')
                    res += "1*";
            }
            return res;
        }
        public static string change2(string s) {
            string res = "";
            for (int i = 0; i < s.Length; i++) {
                if (s[i]==' '||i > 0 && s[i] == '-' && (s[i - 1] == '-' || s[i - 1] == '+' || s[i - 1] == '*' || s[i - 1] == '/' || s[i - 1] == '^'))
                    continue;
                res += s[i];
            }
            return res;
        }
        public static int countOperators(string s,int i) { 
            int count=0;
            for(int r=0;r<=i;r++)
                switch (s[r]) {
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                    case '^':
                        count++;
                        break;
                }
            return count;
        }
        public static void calculate(string s) { 
            string exp=cleanScopes(s);
            Console.WriteLine("After clean unnesecery scopes");
            Console.WriteLine(exp);
            exp=change(exp);
            List<double> nums = getNumbers(exp);
            exp = change2(exp);
            exp = "(" + exp + ")";
            List<char> opers = getOperators(exp);
            int index;
            if (nums.Count > 1) {
                while (exp.Length > 0) {
                    int start,end;
                    for (end = 0; end < s.Length; end++)
                        if (exp[end] == ')')
                            break;
                    for (start = end; start >= 0; start--)
                        if (exp[start] == '(')
                            break;
                    for (int i = start+1,c=0; i < end; i++) {
                        if (exp[i] == '^') {
                            index = countOperators(exp, i)-c;
                            nums[index - 1] = Math.Pow(nums[index - 1], nums[index ]);
                            nums.RemoveAt(index);
                            opers.RemoveAt(index - 1);
                            c++;
                        }
                    }
                    for (int i = start+1,c=0,z; i < end; i++)
                    {
                        z = 0;
                        for (int j = start+1; j < i; j++)
                            if (exp[j] == '^')
                                z++;
                            switch (exp[i])
                            {
                                case '*':
                                    index = countOperators(exp, i) - (c+z);
                                    nums[index - 1] *= nums[index];
                                    nums.RemoveAt(index);
                                    opers.RemoveAt(index - 1);
                                    c++;
                                    break;
                                case '/':
                                    index = countOperators(exp, i) - (c+z);
                                    nums[index - 1] /= nums[index];
                                    nums.RemoveAt(index);
                                    opers.RemoveAt(index - 1);
                                    c++;
                                    break;
                            }
                    }
                    for (int i = start+1, c = 0, z; i < end; i++)
                    {
                        z = 0;
                        for (int j = start+1; j < i; j++)
                            if (exp[j] == '^'||exp[j]=='*'||exp[j]=='/')
                                z++;
                        switch (exp[i])
                        {
                            case '+':
                                index = countOperators(exp, i) - (c + z);
                                nums[index - 1] += nums[index];
                                nums.RemoveAt(index);
                                opers.RemoveAt(index - 1);
                                c++;
                                break;
                            case '-':
                                index = countOperators(exp, i) - (c + z);
                                nums[index - 1] -= nums[index];
                                nums.RemoveAt(index);
                                opers.RemoveAt(index - 1);
                                c++;
                                break;
                        }
                    }
                    exp = eraseSubString(exp, start, end);
                }
            }
            Console.WriteLine("Result = "+nums[0]);
        }
    }
}
