namespace CurveViewer
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}


		private void AddFunctionButton_Click(object sender, EventArgs e)
		{

		}
	}
}

class Graph
{

}

class CalcResult
{
    static double resultMax = 10000000;
    static double resultMin = -10000000;
    public double value;
    public bool defined;
    public CalcResult(double value, bool defined = true)
    {
        this.value = value;
        this.defined = defined;
    }
    public CalcResult(CalcResult calc)
    {
        this.value = calc.value;
        this.defined = calc.defined;
    }
    public static CalcResult operator +(CalcResult a, CalcResult b)
    {
        if (!a.defined || !b.defined) return new CalcResult(0, false);
        if (a.value + b.value < resultMin || resultMax < a.value + b.value) return new CalcResult(0, false);
        return new CalcResult(a.value + b.value);
    }
    public static CalcResult operator +(CalcResult a, double b)
    {
        if (!a.defined) return new CalcResult(0, false);
        if (a.value + b < resultMin || resultMax < a.value + b) return new CalcResult(0, false);
        return new CalcResult(a.value + b);
    }
    public static CalcResult operator -(CalcResult a, CalcResult b)
    {
        if (!a.defined || !b.defined) return new CalcResult(0, false);
        if (a.value - b.value < resultMin || resultMax < a.value - b.value) return new CalcResult(0, false);
        return new CalcResult(a.value - b.value);
    }
    public static CalcResult operator -(CalcResult a, double b)
    {
        if (!a.defined) return new CalcResult(0, false);
        if (a.value - b < resultMin || resultMax < a.value - b) return new CalcResult(0, false);
        return new CalcResult(a.value - b);
    }
    public static CalcResult operator -(CalcResult a)
    {
        if (!a.defined) return a;
        if (-a.value < resultMin || resultMax < -a.value) return new CalcResult(0, false);
        return new CalcResult(-a.value);
    }
    public static CalcResult operator *(CalcResult a, CalcResult b)
    {
        if (!a.defined || !b.defined) return new CalcResult(0, false);
        if (a.value * b.value < resultMin || resultMax < a.value * b.value) return new CalcResult(0, false);
        return new CalcResult(a.value * b.value);
    }
    public static CalcResult operator /(CalcResult a, CalcResult b)
    {
        if (!a.defined || !b.defined) return new CalcResult(0, false);
        if (b.value == 0) return new CalcResult(0, false);
        if (a.value / b.value < resultMin || resultMax < a.value / b.value) return new CalcResult(0, false);
        return new CalcResult(a.value * b.value);
    }
    public static CalcResult operator /(CalcResult a, double b)
    {
        if (!a.defined) return a;
        if (b == 0) return new CalcResult(0, false);
        if (a.value / b < resultMin || resultMax < a.value / b) return new CalcResult(0, false);
        return new CalcResult(a.value / b);
    }
    public static CalcResult sin(CalcResult[] x)
    {
        if (!x[0].defined) return x[0];
        return new CalcResult(Math.Sin(x[0].value));
    }
    public static CalcResult cos(CalcResult[] x)
    {
        if (!x[0].defined) return x[0];
        return new CalcResult(Math.Cos(x[0].value));
    }
    public static CalcResult tan(CalcResult[] x)
    {
        if (!x[0].defined) return x[0];
        double ret = Math.Tan(x[0].value);
        if (ret < resultMin || resultMax < ret) return new CalcResult(0, false);
        return new CalcResult(ret);
    }
    public static CalcResult x(CalcResult[] x)
    {
        if (x[0].value < resultMin || resultMax < x[0].value) return new CalcResult(0, false);
        return new CalcResult(x[0].value);
    }
    public static CalcResult log(CalcResult[] x)
    {
        if (!x[0].defined || !x[1].defined) return new CalcResult(0, false);
        if (x[0].value <= 0 || x[0].value == 1 || x[1].value <= 0) return new CalcResult(0, false);
        if (Math.Log(x[0].value) == 0) return new CalcResult(0, false);
        return new CalcResult(Math.Log(x[1].value) / Math.Log(x[0].value));
    }
    public static CalcResult ln(CalcResult[] x)
    {
        if (!x[0].defined) return x[0];
        if (x[0].value == 1 || x[0].value <= 0) return new CalcResult(0, false);
        return new CalcResult(Math.Log(x[0].value));
    }
    public static CalcResult pow(CalcResult[] x)
    {
        if (!x[0].defined || !x[1].defined) return new CalcResult(0, false);
        if (x[0].value < 0 && (int)x[1].value != x[1].value) return new CalcResult(0, false);
        if (Math.Log(resultMax) < x[1].value * Math.Log(x[0].value)) return new CalcResult(0, false);
        return new CalcResult(Math.Pow(x[0].value, x[1].value));
    }
    public static CalcResult abs(CalcResult[] x)
    {
        if (!x[0].defined) return x[0];
        return new CalcResult(Math.Abs(x[0].value));
    }
    public static CalcResult pi(CalcResult[] x)
    {
        return new CalcResult(Math.PI);
    }
    public static CalcResult e(CalcResult[] x)
    {
        return new CalcResult(Math.E);
    }
}
class Function
{
    static public double delta = 0.00001;
    class DefinedFunction
    {
        public string name;
        public int valNum;
        public Func<CalcResult[], CalcResult> calc;
        public DefinedFunction(string name, int valNum, Func<CalcResult[], CalcResult> calc)
        {
            this.name = name;
            this.valNum = valNum;
            this.calc = calc;
        }
    }
    public class DefinedParameter
    {
        public string name;
        public double value;
        public DefinedParameter()
        {
            this.name = "";
            this.value = 0;
        }
    }
    static List<DefinedFunction> defFunctions = new List<DefinedFunction>(){
        new DefinedFunction("sin",1,CalcResult.sin),
        new DefinedFunction("cos",1,CalcResult.cos),
        new DefinedFunction("tan",1,CalcResult.tan),
        new DefinedFunction("x",0,CalcResult.x),
        new DefinedFunction("log",2,CalcResult.log),
        new DefinedFunction("ln",1,CalcResult.ln),
        new DefinedFunction("pi",0,CalcResult.pi),
        new DefinedFunction("e",0,CalcResult.e)
    };
    static List<DefinedParameter> defParameters = new List<DefinedParameter>();
    static public DefinedParameter AddParameter()
    {
        defParameters.Add(new DefinedParameter());
        return defParameters.Last();
    }
    static public void DeletParameter(DefinedParameter parameter)
    {
        defParameters.Remove(parameter);
    }
    public static class Generator
    {
        static class State
        {
            static string s;
            static int p;
            static public bool error;
            static public void reset(string _s)
            {
                s = _s;
                p = 0;
                error = false;
            }
            static public char begin()
            {
                if (p < s.Length) return s[p];
                else return ' ';
            }
            static public string begin(int n)
            {
                if (p + n <= s.Length) return s.Substring(p, n);
                else return "";
            }
            static public void proceed()
            {
                if (p < s.Length)
                    p++;
            }
            static public void proceed(int n)
            {
                if (p + n <= s.Length)
                    p += n;
            }
            static public void consume(char expected)
            {
                if (begin() == expected) proceed();
                else error = true;
            }
        }

        static Func<CalcResult, CalcResult> expression()
        {
            Func<CalcResult, CalcResult> ret;
            bool minus = false;
            if (State.begin() == '-')
            {
                minus = true;
                State.proceed();
            }
            var h = term();
            if (h == null) return null;
            if (!minus) ret = x => h(x);
            else ret = x => -h(x);
            while (true)
            {
                if (State.begin() == '+')
                {
                    State.proceed();
                    var f = (Func<CalcResult, CalcResult>)ret.Clone();
                    var g = term();
                    if (g == null) return null;
                    ret = x => f(x) + g(x);
                }
                else if (State.begin() == '-')
                {
                    State.proceed();
                    var f = (Func<CalcResult, CalcResult>)ret.Clone();
                    var g = term();
                    if (g == null) return null;
                    ret = x => f(x) - g(x);
                }
                else break;
            }
            return ret;
        }
        static Func<CalcResult, CalcResult> term()
        {
            Func<CalcResult, CalcResult> ret;
            ret = factor();
            if (ret == null) return null;
            while (true)
            {
                if (State.begin() == '*')
                {
                    State.proceed();
                    var f = (Func<CalcResult, CalcResult>)ret.Clone();
                    var g = factor();
                    if (g == null) return null;
                    ret = x => f(x) * g(x);
                }
                else if (State.begin() == '/')
                {
                    State.proceed();
                    var f = (Func<CalcResult, CalcResult>)ret.Clone();
                    var g = factor();
                    if (g == null) return null;
                    ret = x => f(x) / g(x);
                }
                else break;
            }
            return ret;
        }
        static Func<CalcResult, CalcResult> factor()
        {
            Func<CalcResult, CalcResult> ret;
            ret = factor2();
            if (ret == null) return null;
            if (State.begin() == '^')
            {
                State.proceed();
                var f = (Func<CalcResult, CalcResult>)ret.Clone();
                var g = term();
                if (g == null) return null;
                ret = x => CalcResult.pow(new CalcResult[] { f(x), g(x) });
            }
            return ret;
        }
        static Func<CalcResult, CalcResult> factor2()
        {
            Func<CalcResult, CalcResult> ret = null;
            char par = parenthesis();
            if (par != ' ')
            {
                ret = expression();
                State.consume(par);
            }
            else if (State.begin() == '|')
            {
                State.proceed();
                var f = expression();
                if (f != null) ret = x => CalcResult.abs(new CalcResult[] { f(x) });
                State.consume('|');
            }
            else if (char.IsDigit(State.begin()))
            {
                ret = number();
            }
            else
            {
                ret = func();
                if (ret == null) ret = param();
            }
            if (ret == null) return null;
            while (State.begin() == '\'')
            {
                State.proceed();
                var f = (Func<CalcResult, CalcResult>)ret.Clone();
                ret = x => new CalcResult((f(x + delta) - f(x - delta)) / (2 * delta));
            }
            return ret;
        }
        static Func<CalcResult, CalcResult> func()
        {
            Func<CalcResult, CalcResult> ret = null;
            DefinedFunction function = null;
            foreach (var def in defFunctions)
            {
                if (State.begin(def.name.Length) == def.name)
                {
                    if (function == null || function.name.Length < def.name.Length) function = def;
                }
            }
            if (function == null) return null;
            State.proceed(function.name.Length);
            int prime = 0;
            while (State.begin() == '\'')
            {
                State.proceed();
                prime++;
            }
            if (function.valNum == 0)
            {
                ret = x => function.calc(new CalcResult[] { x });
            }
            else if (function.valNum == 1)
            {
                var f = factor();
                if (f == null) return null;
                ret = x => function.calc(new CalcResult[] { f(x) });
            }
            else
            {
                Func<CalcResult, CalcResult[]> f = new Func<CalcResult, CalcResult[]>(x => new CalcResult[function.valNum]);
                char par = parenthesis();
                if (par == ' ') return null;
                for (int i = 0; i < function.valNum; i++)
                {
                    var g = (Func<CalcResult, CalcResult[]>)f.Clone();
                    var h = expression();
                    if (h == null) return null;
                    int id = i;
                    var set = (CalcResult[] g, int i, CalcResult h) => {
                        g[i] = h;
                        return g;
                    };
                    f = x => (set(g(x), id, h(x)));
                    if (i < function.valNum - 1) State.consume(',');
                    Console.WriteLine(i);
                }
                State.consume(par);
                ret = x => function.calc(f(x));
            }
            while ((prime--) > 0)
            {
                var f = (Func<CalcResult, CalcResult>)ret.Clone();
                ret = x => (f(x + delta) - f(x - delta)) / (2 * delta);
            }
            return ret;
        }
        static char parenthesis()
        {
            char par = ' ';
            switch (State.begin())
            {
                case '(':
                    par = ')';
                    break;
                case '{':
                    par = '}';
                    break;
                case '[':
                    par = ']';
                    break;
                default:
                    return par;
            }
            State.proceed();
            return par;
        }
        static Func<CalcResult, CalcResult> param()
        {
            Func<CalcResult, CalcResult> ret;
            DefinedParameter parameter = null;
            foreach (var def in defParameters)
            {
                if (State.begin(def.name.Length) == def.name)
                {
                    if (parameter == null || parameter.name.Length < def.name.Length) parameter = def;
                }
            }
            if (parameter == null) return null;
            State.proceed(parameter.name.Length);
            bool prime = false;
            while (State.begin() == '\'')
            {
                State.proceed();
                prime = true;
            }
            if (prime) ret = x => new CalcResult(0);
            else ret = x => new CalcResult(parameter.value);
            return ret;
        }
        static Func<CalcResult, CalcResult> number()
        {
            Func<CalcResult, CalcResult> ret;
            double num = 0;
            int decPoint = 0;
            int cnt = 0;
            while (char.IsDigit(State.begin()) || State.begin() == '.')
            {
                if (State.begin() == '.')
                {
                    if (decPoint == 0)
                    {
                        decPoint++;
                    }
                    else
                    {
                        State.error = true;
                    }
                }
                else
                {
                    num *= 10;
                    num += (State.begin() - '0');
                    if (decPoint > 0) decPoint++;
                    cnt++;
                }
                if (cnt > 10)
                {
                    State.error = true;
                    return null;
                }
                State.proceed();
            }
            ret = x => new CalcResult(num / Math.Pow(10, Math.Max(0, decPoint - 1)));
            return ret;
        }
        static public Func<CalcResult, CalcResult> Generate(string formula)
        {
            State.reset(formula);
            var func = expression();
            if (State.begin() != ' ') State.error = true;
            if (State.error) func = null;
            return func;
        }
    }
    public Func<CalcResult, CalcResult> calc;
    public Function() { }
    public void Generate(string formula)
    {
        calc = Generator.Generate(formula);
    }

}