namespace CurveViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            Function.parametersFlow = parametersFlowLayoutPanel;
            Graph.functionsFlow = functionsFlowLayoutPanel;
            Graph.graph=graph;
            Graph.sizexText = sizexText;
            Graph.sizeyText = sizeyText;
            Graph.centerxText = centerxText;
            Graph.centeryText = centeryText;
            Graph.displayAxisX=displayAxisX;
            Graph.displayAxisY=displayAxisY;
            Graph.xAxisColor = xAxisColor;
            Graph.yAxisColor = yAxisColor;
            Graph.displayGridX = displayGridX;
            Graph.displayGridY = displayGridY;
            Graph.xGridColor = xGridColor;
            Graph.yGridColor = yGridColor;
            Graph.displayScaleX = displayScaleX;
            Graph.displayScaleY = displayScaleY;
            Graph.xScaleColor = xScaleColor;
            Graph.yScaleColor = yScaleColor;
            Graph.backColorButton = backColorButton;
            Graph.Init();
            Graph.mousePosLabel = mousePos;
        }
        
        class Vec2
        {
            public double x, y;
            public Vec2()
            {
                this.x = 0;
                this.y = 0;
            }
            public Vec2(double x, double y)
            {
                this.x = x;
                this.y = y;
            }
            public Vec2(Vec2 v)
			{
                this.x = v.x;
                this.y = v.y;
			}
            public static Vec2 operator +(Vec2 a,Vec2 b)
			{
                return new Vec2(a.x + b.x, a.y + b.y);
			}
        }
        class Graph
        {
            static Vec2 maxOffset;
            static Vec2 minOffset;
            static Vec2 offset;
            static Vec2 maxSize;
            static Vec2 minSize;
            static Vec2 size;
            static List<Function> functions;
            public static PictureBox graph;
            public static FlowLayoutPanel functionsFlow;
            public static Label mousePosLabel;
            public static TextBox sizexText;
            public static TextBox sizeyText;
            public static TextBox centerxText;
            public static TextBox centeryText;
            public static CheckBox displayAxisX;
            public static CheckBox displayAxisY;
            public static Button xAxisColor;
            public static Button yAxisColor;
            public static CheckBox displayGridX;
            public static CheckBox displayGridY;
            public static Button xGridColor;
            public static Button yGridColor;
            public static CheckBox displayScaleX;
            public static CheckBox displayScaleY;
            public static Button xScaleColor;
            public static Button yScaleColor;
            public static ColorDialog xAxisColorDialog;
            public static ColorDialog yAxisColorDialog;
            public static ColorDialog xScaleColorDialog;
            public static ColorDialog yScaleColorDialog;
            public static ColorDialog xGridColorDialog;
            public static ColorDialog yGridColorDialog;
            public static Button backColorButton;
            public static ColorDialog backColor;

            static bool clicked;
            static Vec2 preViewPoint;
            static Graph()
            {
                functions = new List<Function>();
                offset = new Vec2();
                maxOffset = new Vec2(1000, 1000);
                minOffset = new Vec2(-1000, -1000);
                maxSize = new Vec2(1000, 1000);
                minSize = new Vec2(0.01, 0.01);
                size = new Vec2();
                xAxisColorDialog = new ColorDialog();
                yAxisColorDialog = new ColorDialog();
                xScaleColorDialog = new ColorDialog();
                yScaleColorDialog = new ColorDialog();
                xGridColorDialog = new ColorDialog();
                yGridColorDialog = new ColorDialog();
                backColor = new ColorDialog();
            }
            public static void Init()
            {
                graph.MouseDown += graph_MouseDown;
                graph.MouseMove += graph_MouseMove;
                graph.MouseUp += graph_MouseUp;
                graph.MouseWheel += graph_MouseWheel;
                sizexText.TextChanged += sizexText_TextChanged;
                sizeyText.TextChanged += sizeyText_TextChanged;
                centerxText.TextChanged+=centerxText_TextChanged;
                centeryText.TextChanged+=centeryText_TextChanged;
                displayAxisX.CheckedChanged += display_CheckedChanged;
                displayAxisY.CheckedChanged += display_CheckedChanged;
                displayGridX.CheckedChanged+=display_CheckedChanged;
                displayGridY.CheckedChanged += display_CheckedChanged;
                displayScaleX.CheckedChanged += display_CheckedChanged;
                displayScaleY.CheckedChanged += display_CheckedChanged;
                xAxisColor.Click+=xAxisColor_Click;
                yAxisColor.Click+=yAxisColor_Click;
                xGridColor.Click += xGridColor_Click;
                yGridColor.Click += yGridColor_Click;
                xScaleColor.Click += xScaleColor_Click;
                yScaleColor.Click += yScaleColor_Click;
                backColorButton.Click += backColorButton_Click;
                SetSize(new Vec2(5, 5));
                sizexText.Text = "5.00";
                sizeyText.Text = "5.00";
                centerxText.Text = "0.00";
                centeryText.Text = "0.00";
                AddFunction();
                Function.AddParameter();
                Draw();
            }
            static void graph_MouseDown(object sender, MouseEventArgs e)
            {
                if (!clicked)
                {
                    clicked = true;
                    preViewPoint = new Vec2(e.X, e.Y);
                }
            }
            static void graph_MouseMove(object sender, MouseEventArgs e)
            {
                if (clicked)
                {
                    Vec2 delta = new Vec2(-e.X + preViewPoint.x, e.Y - preViewPoint.y);
                    SetOffset(offset + new Vec2(delta.x * size.x / graph.Width, delta.y * size.y / graph.Height));
                    preViewPoint = new Vec2(e.X, e.Y);
                    Draw();
                }
                Vec2 mousePos = ViewPointToGraphPoint(new Vec2(e.X, e.Y));
                mousePosLabel.Text = "(mouseX, mouseY)=(" + mousePos.x.ToString("0.00") + ", " + mousePos.y.ToString("0.00") + ")";

            }
            static void graph_MouseUp(object sender, MouseEventArgs e)
            {
                clicked = false;
            }
            static void graph_MouseWheel(object sender, MouseEventArgs e)
            {
                Vec2 preSize = new Vec2(size);
                SetSize(new Vec2(size.x * Math.Pow(0.9, e.Delta / 100), size.y * Math.Pow(0.9, e.Delta / 100)));
                SetOffset(new Vec2(offset.x + (size.x - preSize.x) / 2 + (preSize.x - size.x) * e.X / graph.Width, offset.y + (size.y - preSize.y) / 2 + (preSize.y - size.y) * (graph.Height-e.Y) / graph.Height));
                Draw();
            }
            static void sizexText_TextChanged(object sender, EventArgs e)
            {
                if (!sizexText.Modified) return;
                double num;
                if (double.TryParse(sizexText.Text, out num))
                {
                    SetSize(new Vec2(num, size.y), false);
                    UpDate();
                }
            }
            static void sizeyText_TextChanged(object sender, EventArgs e)
            {
                if (!sizeyText.Modified) return;
                double num;
                if (double.TryParse(sizeyText.Text, out num))
                {
                    SetSize(new Vec2(size.x, num), false);
                    UpDate();
                }
            }
            static void centerxText_TextChanged(object sender, EventArgs e)
            {
                if (!centerxText.Modified) return;
                double num;
                if (double.TryParse(centerxText.Text, out num))
                {
                    SetOffset(new Vec2(num, offset.y), false);
                    UpDate();
                }
            }
            static void centeryText_TextChanged(object sender, EventArgs e)
            {
                if (!centeryText.Modified) return;
                double num;
                if (double.TryParse(centeryText.Text, out num))
                {
                    SetOffset(new Vec2(offset.x, num), false);
                    UpDate();
                }
            }
            static void display_CheckedChanged(object sender,EventArgs e)
			{
                UpDate();
			}
            static void xAxisColor_Click(object sender, EventArgs e)
            {
                if (xAxisColorDialog.ShowDialog() == DialogResult.OK)
                {
                    xAxisColor.BackColor = xAxisColorDialog.Color;
                }
                Draw();
            }
            static void yAxisColor_Click(object sender, EventArgs e)
            {
                if (yAxisColorDialog.ShowDialog() == DialogResult.OK)
                {
                    yAxisColor.BackColor = yAxisColorDialog.Color;
                }
                Draw();
            }
            static void xGridColor_Click(object sender, EventArgs e)
            {
                if (xGridColorDialog.ShowDialog() == DialogResult.OK)
                {
                    xGridColor.BackColor = xGridColorDialog.Color;
                }
                Draw();
            }
            static void yGridColor_Click(object sender, EventArgs e)
            {
                if (yGridColorDialog.ShowDialog() == DialogResult.OK)
                {
                    yGridColor.BackColor = yGridColorDialog.Color;
                }
                Draw();
            }
            static void xScaleColor_Click(object sender, EventArgs e)
            {
                if (xScaleColorDialog.ShowDialog() == DialogResult.OK)
                {
                    xScaleColor.BackColor = xScaleColorDialog.Color;
                }
                Draw();
            }
            static void yScaleColor_Click(object sender, EventArgs e)
            {
                if (yScaleColorDialog.ShowDialog() == DialogResult.OK)
                {
                    yScaleColor.BackColor = yScaleColorDialog.Color;
                }
                Draw();
            }
            static void backColorButton_Click(object sender, EventArgs e)
            {
                if (backColor.ShowDialog() == DialogResult.OK)
                {
                    backColorButton.BackColor = backColor.Color;
                }
                Draw();
            }
            static void SetSize(Vec2 s, bool textChange = true)
            {
                size = s;
                if (size.x < minSize.x) size.x = minSize.x;
                if (size.y < minSize.y) size.y = minSize.y;
                if (size.x > maxSize.x) size.x = maxSize.x;
                if (size.y > maxSize.y) size.y = maxSize.y;
                if (textChange)
                {
                    sizexText.Text = size.x.ToString("0.00");
                    sizeyText.Text = size.y.ToString("0.00");
                }
            }
            static void SetOffset(Vec2 p,bool textChange=true)
			{
                offset = p;
                if (offset.x > maxOffset.x)offset.x = maxOffset.x;
                if (offset.y > maxOffset.y)offset.y = maxOffset.y;
                if (offset.x < minOffset.x)offset.x = minOffset.x;
                if (offset.y < minOffset.y)offset.y = minOffset.y;
                if (textChange || offset.x != p.x || offset.y != p.y)
                {
                    centerxText.Text = offset.x.ToString("0.00");
                    centeryText.Text = offset.y.ToString("0.00");
                }
			}
            static Vec2 ViewPointToGraphPoint(Vec2 p)
            {
                return new Vec2(offset.x - size.x / 2 + size.x * p.x / graph.Width, offset.y - size.y / 2 + size.y * (graph.Height - p.y) / graph.Height);
            }
            static Vec2 GraphPointToViewPoint(Vec2 p)
            {
                return new Vec2(graph.Width * (0.5 + (p.x - offset.x) / size.x), graph.Height - (graph.Height * (0.5 + (p.y - offset.y) / size.y)));
            }
            public static void UpDate()
			{
                foreach(Function function in functions)
				{
                    function.Update();
				}
                Draw();
			}
            public static void Draw()
            {
                Bitmap canvas=new Bitmap(graph.Width, graph.Height);
                Graphics g=Graphics.FromImage(canvas);
                Pen pen = new Pen(Color.Black, 1);
                Vec2 viewOriginPoint = GraphPointToViewPoint(new Vec2(0,0));
                graph.BackColor = backColorButton.BackColor;

                int logx = (int)Math.Ceiling(Math.Log10(size.x));
                int mx = (int)Math.Pow(10, Math.Max(0, -logx+1));
                logx = Math.Max(1, logx);
                double divx;
                if (size.x * mx / Math.Pow(10, logx - 1) <= 3) divx = 0.5;
                else if (size.x * mx / Math.Pow(10, logx - 1) <= 6) divx = 1;
                else divx = 2;
                for (int i = 0; i <= 10; i++)
                {
                    pen.Color = yGridColorDialog.Color;
                    double lx = ViewPointToGraphPoint(new Vec2(0, 0)).x;
                    double graphx = (int)(lx * mx / divx) / (int)Math.Pow(10, logx - 1) * divx / (double)mx * Math.Pow(10, logx - 1) + (i - 1) * Math.Pow(10, logx - 1) / mx * divx;
                    int x = (int)GraphPointToViewPoint(new Vec2(graphx, 0)).x;
                    if (displayGridY.Checked) g.DrawLine(pen, x, 0, x, graph.Height);
                    string scaleText = graphx.ToString("0.00");
                    int scaley = (int)GraphPointToViewPoint(new Vec2(0, 0)).y;
                    scaley = Math.Max(scaley, 0);
                    scaley = Math.Min(scaley, graph.Height - 15);
                    Brush brush = new SolidBrush(yScaleColorDialog.Color);
                    if (graphx == 0) continue;
                    if (displayScaleY.Checked)
                    {
                        if (displayGridY.Checked) g.DrawString(scaleText, new Font("MS UI Gothic", 10), brush, x - scaleText.Length * 10 + 5, scaley);
                        else g.DrawString(scaleText, new Font("MS UI Gothic", 10), brush, x - scaleText.Length * 5, scaley);
                    }
                }

                
                int logy = (int)Math.Ceiling(Math.Log10(size.y));
                int my = (int)Math.Pow(10, Math.Max(0, -logy + 1));
                logy = Math.Max(1, logy);
                double divy;
                if (size.y * my / Math.Pow(10, logy - 1) <= 3) divy = 0.5;
                else if (size.y * my / Math.Pow(10, logy - 1) <= 6) divy = 1;
                else divy = 2;
                for (int i = 0; i <= 10; i++)
                {
                    pen.Color = xGridColorDialog.Color;
                    double ly = ViewPointToGraphPoint(new Vec2(0, graph.Height)).y;
                    double graphy = (int)(ly * my / divy) / (int)Math.Pow(10, logy - 1) * divy / (double)my * Math.Pow(10, logy - 1) + (i - 1) * Math.Pow(10, logy - 1) / my * divy;
                    int y = (int)GraphPointToViewPoint(new Vec2(0, graphy)).y;
                    if (displayGridX.Checked) g.DrawLine(pen, 0, y, graph.Width, y);
                    string scaleText = graphy.ToString("0.00");
                    int scalex = (int)GraphPointToViewPoint(new Vec2(0, 0)).x - scaleText.Length * 10 + 5;
                    scalex = Math.Max(scalex, 0);
                    scalex = Math.Min(scalex, graph.Width - scaleText.Length * 10 + 5);
                    Brush brush = new SolidBrush(xScaleColorDialog.Color);
                    if (graphy == 0) continue;
                    if (displayScaleX.Checked)
                    {
                        if (displayGridX.Checked) g.DrawString(scaleText, new Font("MS UI Gothic", 10), brush, scalex, y);
                        else g.DrawString(scaleText, new Font("MS UI Gothic", 10), brush, scalex, y - 6);
                    }
                }

                pen =new Pen(yAxisColorDialog.Color,3);
                if(displayAxisY.Checked)g.DrawLine(pen, (int)viewOriginPoint.x, 0, (int)viewOriginPoint.x, graph.Height);//yé≤
                pen = new Pen(xAxisColorDialog.Color, 3);
                if (displayAxisX.Checked) g.DrawLine(pen, 0, (int)viewOriginPoint.y, graph.Width, (int)viewOriginPoint.y);//xé≤
                int d = 5;
                foreach (var function in functions) {
                    if (function.calc == null) continue;
                    if (!function.diplayCheckBox.Checked) continue;
                    pen.Color=function.colorDialog.Color;
                    for (int i = 0; i < (graph.Width - 1) / d; i++)
                    {
                        double x1 = ViewPointToGraphPoint(new Vec2(i * d, 0)).x;
                        double x2 = ViewPointToGraphPoint(new Vec2((i + 1) * d, 0)).x;
                        CalcResult y1 = function.calc(new CalcResult(x1));
                        CalcResult y2 = function.calc(new CalcResult(x2));
                        if (!y1.defined || !y2.defined) continue;
                        g.DrawLine(pen,i*d,(int)GraphPointToViewPoint(new Vec2(0,y1.value)).y,(i+1)*d, (int)GraphPointToViewPoint(new Vec2(0, y2.value)).y);
                    }
                }
                g.Dispose();
                graph.Image = canvas;
            }
            public static void AddFunction()
            {
                functions.Add(new Function());
                functionsFlow.Controls.Add(functions.Last().flow);
            }
            public static void DeleteFunction(Function function)
			{
                functionsFlow.Controls.Remove(function.flow);
                functions.Remove(function);
                UpDate();
			}
            public static void DeleteAllFunctions()
			{
                for(int i = functions.Count - 1; i >= 0; i--)
				{
                    DeleteFunction(functions[i]);
				}
			}
        }

        //é¿ëïçœÇ›ä÷êî
        //sin,cos,tan
        //asin,acos,atan
        //log,ln,exp
        //pow
        //pi,e
        //abs
        //max,min
        //ceil,floor
        class CalcResult
        {
            static double resultMax = 10000;
            static double resultMin = -10000;
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
                return new CalcResult(a.value / b.value);
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
            public static CalcResult asin(CalcResult[] x)
            {
                if (!x[0].defined) return x[0];
                if (x[0].value < -1 || 1 < x[0].value) return new CalcResult(0, false);
                return new CalcResult(Math.Asin(x[0].value));
            }
            public static CalcResult acos(CalcResult[] x)
            {
                if (!x[0].defined) return x[0];
                if (x[0].value < -1 || 1 < x[0].value) return new CalcResult(0, false);
                return new CalcResult(Math.Acos(x[0].value));
            }
            public static CalcResult atan(CalcResult[] x)
            {
                if (!x[0].defined) return x[0];
                return new CalcResult(Math.Atan(x[0].value));
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
                if (x[0].value == 0||x[0].value==1) return new CalcResult(1);
                if (x[0].value < 0)
                {
                    int x1 = (int)x[1].value;
                    if (x1 != x[1].value) return new CalcResult(0, false);
                    if (Math.Log(resultMax) < x1 * Math.Log(-x[0].value)) return new CalcResult(0, false);
                    return new CalcResult(Math.Pow(x[0].value, x1));
                }
                if(x[0].value<0&&(int)x[1].value!=x[1].value)return new CalcResult(0, false);
                if (Math.Log(resultMax) < x[1].value * Math.Log(x[0].value)) return new CalcResult(0, false);
                return new CalcResult(Math.Pow(x[0].value, x[1].value));
            }
            public static CalcResult exp(CalcResult[] x)
			{
                if (!x[0].defined) return x[0];
                return CalcResult.pow(new CalcResult[] { new CalcResult(Math.E), x[0] });
			}
            public static CalcResult max(CalcResult[] x)
			{
                if(!x[0].defined||!x[1].defined)return new CalcResult(0, false);
                return new CalcResult(Math.Max(x[0].value, x[1].value));
			}
            public static CalcResult min(CalcResult[] x)
            {
                if (!x[0].defined || !x[1].defined) return new CalcResult(0, false);
                return new CalcResult(Math.Min(x[0].value, x[1].value));
            }
            public static CalcResult floor(CalcResult[] x)
			{
                if (!x[0].defined) return x[0];
                return new CalcResult(Math.Floor(x[0].value));
			}
            public static CalcResult ceil(CalcResult[] x)
            {
                if (!x[0].defined) return x[0];
                return new CalcResult(Math.Ceiling(x[0].value));
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
            static double delta = 0.01;
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
            
            static List<DefinedFunction> defFunctions = new List<DefinedFunction>(){
                new DefinedFunction("sin",1,CalcResult.sin),
                new DefinedFunction("cos",1,CalcResult.cos),
                new DefinedFunction("tan",1,CalcResult.tan),
                new DefinedFunction("asin",1,CalcResult.asin),
                new DefinedFunction("acos",1,CalcResult.acos),
                new DefinedFunction("atan",1,CalcResult.atan),
                new DefinedFunction("x",0,CalcResult.x),
                new DefinedFunction("log",2,CalcResult.log),
                new DefinedFunction("ln",1,CalcResult.ln),
                new DefinedFunction("exp",1,CalcResult.exp),
                new DefinedFunction("pi",0,CalcResult.pi),
                new DefinedFunction("e",0,CalcResult.e),
                new DefinedFunction("abs",1,CalcResult.abs),
                new DefinedFunction("pow",2,CalcResult.pow),
                new DefinedFunction("max",2,CalcResult.max),
                new DefinedFunction("min",2,CalcResult.min),
                new DefinedFunction("floor",1,CalcResult.floor),
                new DefinedFunction("ceil",1,CalcResult.ceil),
            };
            
            static class Generator
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
                        var g = factor();
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
                            var set = (CalcResult[] g, int i, CalcResult h) =>
                            {
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
                        if (def.nameText.Text == "") continue;
                        if (State.begin(def.nameText.Text.Length) == def.nameText.Text)
                        {
                            if (parameter == null || parameter.nameText.Text.Length < def.nameText.Text.Length) parameter = def;
                        }
                    }
                    if (parameter == null) return null;
                    State.proceed(parameter.nameText.Text.Length);
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

            /////////////////////////////////////////////////////////////////////////////////////////////////

            static Color initColor;

            static Function()
			{
                initColor = Color.Black;
			}
            public static FlowLayoutPanel parametersFlow;
            public class DefinedParameter
            {
                
                static double rangeMin = -10000;
                static double rangeMax = 10000;
                public double value;
                public double min;
                public double max;
                public FlowLayoutPanel flow;
                public TextBox nameText;
                public TextBox minText;
                public TextBox maxText;
                public TextBox valueText;
                public TrackBar bar;
                public Button deleteButton;
                public DefinedParameter()
                {
                    value = 0;
                    min = -5;
                    max = 5;
                    //flow
                    flow =new FlowLayoutPanel();
                    flow.FlowDirection = FlowDirection.LeftToRight;
                    flow.Width = 400;
                    flow.Height = 30;
                    flow.WrapContents = false;
                    flow.Margin = new Padding(0);

                    //name
                    nameText = new TextBox();
                    nameText.Width = 50;
                    nameText.TextChanged += nameText_TextChanged;
                    nameText.Margin = new Padding(0, 0, 0, 0);
                    flow.Controls.Add(nameText);

                    //valueText
                    valueText = new TextBox();
                    valueText.Text = value.ToString();
                    valueText.Width = 50;
                    valueText.TextChanged += valueText_TextChanged;
                    valueText.Margin = new Padding(10, 0, 0, 0);
                    flow.Controls.Add(valueText);

                    //minText
                    minText = new TextBox();
                    minText.Text = min.ToString();
                    minText.Width = 40;
                    minText.TextChanged += minText_TextChanged;
                    minText.Margin = new Padding(20, 0, 0, 0);
                    flow.Controls.Add(minText);

                    //bar
                    bar=new TrackBar();
                    bar.Maximum = (int)(max * 100);
                    bar.Minimum = (int)(min * 100);
                    bar.TickStyle = TickStyle.None;
                    bar.Scroll+=bar_Scroll;
                    bar.Margin = new Padding(0, 0, 0, 0);
                    flow.Controls.Add(bar);

                    //maxText
                    maxText=new TextBox();
                    maxText.Text = max.ToString();
                    maxText.Width = 40;
                    maxText.TextChanged += maxText_TextChanged;
                    maxText.Margin = new Padding(0, 0, 0, 0);
                    flow.Controls.Add(maxText);


                    //deleteButton
                    deleteButton = new Button();
                    deleteButton.Height = 27;
                    deleteButton.Text = "delete";
                    deleteButton.Click += deleteButton_Click;
                    deleteButton.Margin = new Padding(10, 0, 0, 0);
                    flow.Controls.Add(deleteButton);

                }
                private void nameText_TextChanged(object sender, EventArgs e)
				{
                    Graph.UpDate();
				}
                private void minText_TextChanged(object sender, EventArgs e)
                {
                    if (!minText.Modified) return;
                    double num;
                    if (double.TryParse(minText.Text, out num))
                    {
                        min = Math.Max(num, rangeMin);
                        min = Math.Min(min, rangeMax);
                        max = Math.Max(max, min);
                        maxText.Text = max.ToString();
                        bar.Maximum = (int)(max * 100);
                        bar.Minimum = (int)(min * 100);
                        Graph.UpDate();
                    }
                }
                private void maxText_TextChanged(object sender, EventArgs e)
                {
                    if (!maxText.Modified) return;
                    double num;
                    if (double.TryParse(maxText.Text, out num))
                    {
                        max = Math.Min(num, rangeMax);
                        max = Math.Max(max, rangeMin);
                        min = Math.Min(max, min);
                        minText.Text = min.ToString();
                        bar.Maximum = (int)(max * 100);
                        bar.Minimum = (int)(min * 100);
                        Graph.UpDate();
                    }
                }
                private void valueText_TextChanged(object sender, EventArgs e)
                {
                    if (!valueText.Modified) return;
                    double num;
                    if (double.TryParse(valueText.Text, out num))
                    {
                        value = num;
                        value = Math.Min(value, rangeMax);
                        value = Math.Max(value, rangeMin);
                        max = Math.Max(value, max);
                        min = Math.Min(value, min);
                        maxText.Text = max.ToString();
                        minText.Text = min.ToString();
                        valueText.Text = value.ToString();
                        bar.Maximum = (int)(max * 100);
                        bar.Minimum = (int)(min * 100);
                        bar.Value = (int)(value * 100);
                        Graph.UpDate();
                    }
                }
                private void bar_Scroll(object sender, EventArgs e)
				{
                    value=(double)bar.Value/100;
                    valueText.Text = value.ToString();
                    Graph.UpDate();
				}
                private void deleteButton_Click(object sender, EventArgs e)
                {
                    DeleteParameter(this);
                    Graph.Draw();
                }
            }
            static List<DefinedParameter> defParameters = new List<DefinedParameter>();
            public static void AddParameter()
            {
                defParameters.Add(new DefinedParameter());
                parametersFlow.Controls.Add(defParameters.Last().flow);
            }

            static void DeleteParameter(DefinedParameter parameter)
            {
                parametersFlow.Controls.Remove(parameter.flow);
                defParameters.Remove(parameter);
                Graph.UpDate();
            }
            public static void DeleteAllParameters()
			{
                for(int i = defParameters.Count - 1; i >= 0; i--)
				{
                    DeleteParameter(defParameters[i]);
				}
			}

            public Func<CalcResult, CalcResult> calc;
            public FlowLayoutPanel flow;
            public CheckBox diplayCheckBox;
            public Label formulaLabel;
            public TextBox formulaText;
            public ColorDialog colorDialog;
            public Button colorButton;
            public Button deleteButton;
            public Function()
            {
                //flow
                flow = new FlowLayoutPanel();
                flow.FlowDirection = FlowDirection.LeftToRight;
                flow.Width = 400;
                flow.Height = 30;
                flow.WrapContents = false;
                flow.Margin = new Padding(0);

                //diplayCheckBox
                diplayCheckBox = new CheckBox();
                diplayCheckBox.Width = 20;
                diplayCheckBox.Height = 20;
                diplayCheckBox.CheckedChanged += diplayCheckBox_CheckedChanged;
                diplayCheckBox.Margin = new Padding(20, 4, 0, 0);
                diplayCheckBox.Checked = true;
                flow.Controls.Add(diplayCheckBox);

                //formulaLabel
                formulaLabel =new Label();
                formulaLabel.Width = 30;
                formulaLabel.Text = "y =";
                formulaLabel.Margin=new Padding(10,1,0,0);
                flow.Controls.Add(formulaLabel);

                //formulaText
                formulaText = new TextBox();
                formulaText.Width = 150;
                formulaText.TextChanged += formulaText_TextChanged;
                formulaText.Margin = new Padding(0, 0, 0, 0);
                flow.Controls.Add(formulaText);

                //colorButton
                colorButton = new Button();
                colorDialog = new ColorDialog();
                colorDialog.Color = initColor;
                colorButton.BackColor = initColor;
                colorButton.Width = 27;
                colorButton.Height = 27;
                colorButton.Click += colorButton_Click;
                colorButton.Margin = new Padding(15,0,0,0);
                flow.Controls.Add(colorButton);

                //deleteButton
                deleteButton = new Button();
                deleteButton.Height = 27;
                deleteButton.Text = "delete";
                deleteButton.Click += deleteButton_Click;
                deleteButton.Margin = new Padding(15,0,0,0);
                flow.Controls.Add(deleteButton);
            }
            private void diplayCheckBox_CheckedChanged(object sender, EventArgs e)
            {
                Graph.Draw();
            }
            private void formulaText_TextChanged(object sender, EventArgs e)
            {
                Update();
                Graph.Draw();
            }
            private void colorButton_Click(object sender, EventArgs e)
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    colorButton.BackColor = colorDialog.Color;
                }
                Graph.Draw();
            }
            private void deleteButton_Click(object sender, EventArgs e)
            {
                Graph.DeleteFunction(this);
                Graph.Draw();
            }
            public void Update()
			{
                Generate(formulaText.Text);
			}
            public void Generate(string formula)
            {
                calc = Generator.Generate(formula);
            }
        }
        private void AddFunctionButton_Click(object sender, EventArgs e)
        {
            Graph.AddFunction();
        }
        private void ClearFunctionButton_Click(object sender, EventArgs e)
		{
            Graph.DeleteAllFunctions();
		}

		private void AddParameterButton_Click(object sender, EventArgs e)
		{
            Function.AddParameter();
		}

		private void ClearParameterButton_Click(object sender, EventArgs e)
		{
            Function.DeleteAllParameters();
		}
	}
}

