namespace CurveViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Init();
        }
        static readonly double scale = 120.0 / Graphics.FromImage(new Bitmap(1200, 800)).DpiX;
        static void SetFontSize(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                if (control.HasChildren)
                {
                    SetFontSize(control);
                }
                if (control.GetType().GetProperty("Font") != null)
                {
                    control.Font = new Font(control.Font.Name, (int)(control.Font.Size * scale));
                }
            }
        }
        void Init()
        {
            //dpiに合わせてフォントサイズを調整
            SetFontSize(this);            
            Function.parametersFlow = parametersFlowLayoutPanel;
            Graph.functionsFlow = functionsFlowLayoutPanel;
            Graph.graph=graph;
            Graph.sizeTextX = sizeTextX;
            Graph.sizeTextY = sizeTextY;
            Graph.centerTextX = centerTextX;
            Graph.centerTextY = centerTextY;
            Graph.axisDisplayX = axisDiplayX;
            Graph.axisDisplayY = axisDisplayY;
            Graph.gridDisplayX = gridDisplayX;
            Graph.gridDisplayY = gridDisplayY;
            Graph.scaleDisplayX = scaleDisplayX;
            Graph.scaleDisplayY = scaleDisplayY;
            Graph.axisColorButtonX = axisColorButtonX;
            Graph.axisColorButtonY = axisColorButtonY;
            Graph.gridColorButtonX = gridColorButtonX;
            Graph.gridColorButtonY = gridColorButtonY;
            Graph.scaleColorButtonX = scaleColorButtonX;
            Graph.scaleColorButtonY = scaleColorButtonY;
            Graph.backColorButton = backColorButton;
            Graph.mousePosLabel = mousePosLabel;
            Graph.Init();
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
            public static PictureBox graph;
            public static FlowLayoutPanel functionsFlow;
            public static Label mousePosLabel;
            public static TextBox sizeTextX, sizeTextY;
            public static TextBox centerTextX, centerTextY;
            public static CheckBox axisDisplayX, axisDisplayY;
            public static CheckBox gridDisplayX, gridDisplayY;
            public static CheckBox scaleDisplayX, scaleDisplayY;
            public static Button axisColorButtonX, axisColorButtonY;
            public static Button gridColorButtonX, gridColorButtonY;
            public static Button scaleColorButtonX, scaleColorButtonY;
            public static Button backColorButton;

            static readonly Vec2 maxCenter = new Vec2(1000, 1000);
            static readonly Vec2 minCenter = new Vec2(-1000, -1000);
            static readonly Vec2 maxSize = new Vec2(1000, 1000);
            static readonly Vec2 minSize = new Vec2(0.01, 0.01);

            static Vec2 center;
            static Vec2 size;
            static List<Function> functions;

            static Graph()
            {
                functions = new List<Function>();
                center = new Vec2();
                size = new Vec2();
                
            }
            public static void Init()
            {
                graph.MouseDown += graph_MouseDown;
                graph.MouseMove += graph_MouseMove;
                graph.MouseUp += graph_MouseUp;
                graph.MouseWheel += graph_MouseWheel;
                sizeTextX.TextChanged += sizeTextX_TextChanged;
                sizeTextY.TextChanged += sizeTextY_TextChanged;
                centerTextX.TextChanged += centerTextX_TextChanged;
                centerTextY.TextChanged += centerTextY_TextChanged;
                axisDisplayX.CheckedChanged += display_CheckedChanged;
                axisDisplayY.CheckedChanged += display_CheckedChanged;
                gridDisplayX.CheckedChanged += display_CheckedChanged;
                gridDisplayY.CheckedChanged += display_CheckedChanged;
                scaleDisplayX.CheckedChanged += display_CheckedChanged;
                scaleDisplayY.CheckedChanged += display_CheckedChanged;
                axisColorButtonX.Click += colorButton_Click;
                axisColorButtonY.Click += colorButton_Click;
                gridColorButtonX.Click += colorButton_Click;
                gridColorButtonY.Click += colorButton_Click;
                scaleColorButtonX.Click += colorButton_Click;
                scaleColorButtonY.Click += colorButton_Click;
                backColorButton.Click += colorButton_Click;

                if (!Load("AutoSave.txt")) Reset();
            }

            public static void Reset()
			{
                SetCenter(new Vec2(0, 0));
                SetSize(new Vec2(5, 5));
                axisDisplayX.Checked = true;
                axisDisplayY.Checked = true;
                gridDisplayX.Checked = true;
                gridDisplayY.Checked = true;
                scaleDisplayX.Checked = true;
                scaleDisplayY.Checked = true;
                axisColorButtonX.BackColor = Color.Black;
                axisColorButtonY.BackColor = Color.Black;
                gridColorButtonX.BackColor = Color.Black;
                gridColorButtonY.BackColor = Color.Black;
                scaleColorButtonX.BackColor = Color.Black;
                scaleColorButtonY.BackColor = Color.Black;
                backColorButton.BackColor = Color.White;
                DeleteAllFunctions();
                Function.DeleteAllParameters();
                AddFunction();
                Function.AddParameter();
                UpDate();
            }
            public static void SaveAsImage(string filename, string extension)
            {
                graph.Refresh();
                switch (extension)
                {
                    case ".jpg":
                        graph.Image.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case ".png":
                        graph.Image.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case ".bmp":
                        graph.Image.Save(filename, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    default:
                        break;
                }
            }
            public static void Save(string filename)
			{
                if (!File.Exists(filename)) File.Create(filename);
                StreamWriter sw = new StreamWriter(filename, false,System.Text.Encoding.UTF8);
                sw.WriteLine(size.x.ToString("0.00"));
                sw.WriteLine(size.y.ToString("0.00"));
                sw.WriteLine(center.x.ToString("0.00"));
                sw.WriteLine(center.y.ToString("0.00"));
                sw.WriteLine(axisDisplayX.Checked ? 1 : 0);
                sw.WriteLine(axisDisplayY.Checked ? 1 : 0);
                sw.WriteLine(gridDisplayX.Checked ? 1 : 0);
                sw.WriteLine(gridDisplayY.Checked ? 1 : 0);
                sw.WriteLine(scaleDisplayX.Checked ? 1 : 0);
                sw.WriteLine(scaleDisplayY.Checked ? 1 : 0);
                Func<Color, string> ColorToString = color =>
                {
                    return color.A + " " + color.R + " " + color.G + " " + color.B;
                };
                sw.WriteLine(ColorToString(axisColorButtonX.BackColor));
                sw.WriteLine(ColorToString(axisColorButtonY.BackColor));
                sw.WriteLine(ColorToString(gridColorButtonX.BackColor));
                sw.WriteLine(ColorToString(gridColorButtonY.BackColor));
                sw.WriteLine(ColorToString(scaleColorButtonX.BackColor));
                sw.WriteLine(ColorToString(scaleColorButtonY.BackColor));
                sw.WriteLine(functions.Count);
                foreach(Function function in functions)
				{
                    sw.WriteLine(function.diplayCheckBox.Checked ? 1 : 0);
                    sw.WriteLine(function.formulaText.Text);
                    sw.WriteLine(ColorToString(function.colorButton.BackColor));
				}
                sw.WriteLine(Function.parameters.Count);
                foreach(Function.Parameter parameter in Function.parameters)
				{
                    sw.WriteLine(parameter.nameText.Text);
                    sw.WriteLine(parameter.value.ToString("0.00"));
                    sw.WriteLine(parameter.min.ToString("0.00"));
                    sw.WriteLine(parameter.max.ToString("0.00"));
				}
                sw.Close();
            }
            static bool TryParseColor(string colorStr,out Color color)
			{
                color= new Color();
                if (colorStr == null) return false;
                string[] arr = colorStr.Split(' ');
                if (arr.Length != 4) return false;
                int[] argb=new int[4];
                
                for (int i = 0; i < 4; i++)
                {
                    int tmp;
                    if (!int.TryParse(arr[i], out tmp)) return false;
                    argb[i] = tmp;
                }
                for (int i = 0; i < 4; i++)
				{
                    int a = argb[i];
                    int b = a;
                    if (argb[i] < 0 || 255 < argb[i])
                    {
                        return false;
                    }
				}
                color = Color.FromArgb(argb[0],argb[1], argb[2], argb[3]);
                return true;
            }
            public static bool Load(string filename)
			{
                if (!File.Exists(filename)) return false;
                StreamReader sr = new StreamReader(filename);
                if (sr == null) return false;
                double sizex, sizey;
                double centerx, centery;
                int dispAxisX, dispAxisY;
                int dispGridX, dispGridY;
                int dispScaleX, dispScaleY;
                Color colorAxisX, colorAxisY;
                Color colorGridX, colorGridY;
                Color colorScaleX, colorScaleY;
                int[] display = new int[6];
                Color[] color= new Color[6];
                if (!double.TryParse(sr.ReadLine(), out sizex)) return false;
                if (!double.TryParse(sr.ReadLine(), out sizey)) return false;
                if (!double.TryParse(sr.ReadLine(), out centerx)) return false;
                if (!double.TryParse(sr.ReadLine(), out centery)) return false;
                for(int i = 0; i < 6; i++)
				{
                    if (!int.TryParse(sr.ReadLine(), out display[i]))
                    {
                        return false;
                    }
				}
                for (int i = 0; i < 6; i++)
                {
                    if (!TryParseColor(sr.ReadLine(), out color[i]))
                    {
                        return false;
                    }
                }
                int fc;
                if (!int.TryParse(sr.ReadLine(), out fc)) return false;
                int[] funcDisplay = new int[fc];
                string[] funcFormula = new string[fc];
                Color[] funcColor = new Color[fc];
                for (int i = 0; i < fc; i++)
				{
                    if (!int.TryParse(sr.ReadLine(), out funcDisplay[i])) return false;
                    funcFormula[i] = sr.ReadLine();
                    if (funcFormula[i] == null) return false;
                    if (!TryParseColor(sr.ReadLine(), out funcColor[i])) return false;
                }
                int pc;
                if (!int.TryParse(sr.ReadLine(), out pc)) return false;
                string[] paramName = new string[pc];
                double[] paramValue = new double[pc];
                double[] paramMin = new double[pc];
                double[] paramMax = new double[pc];
                for(int i = 0; i < pc; i++)
				{
                    double value, min, max;
                    paramName[i] = sr.ReadLine();
                    if (paramName[i] == null) return false;
                    if (!double.TryParse(sr.ReadLine(), out paramValue[i])) return false;
                    if (!double.TryParse(sr.ReadLine(), out paramMin[i])) return false;
                    if (!double.TryParse(sr.ReadLine(), out paramMax[i])) return false;
                }
                sr.Close();
                Reset();
                DeleteAllFunctions();
                Function.DeleteAllParameters();
                SetSize(new Vec2(sizex, sizey));
                SetCenter(new Vec2(centerx, centery));
                axisDisplayX.Checked = display[0] == 0 ? false : true;
                axisDisplayY.Checked = display[1] == 0 ? false : true;
                gridDisplayX.Checked = display[2] == 0 ? false : true;
                gridDisplayY.Checked = display[3] == 0 ? false : true;
                scaleDisplayX.Checked = display[4] == 0 ? false : true;
                scaleDisplayY.Checked = display[5] == 0 ? false : true;
                axisColorButtonX.BackColor = color[0];
                axisColorButtonY.BackColor = color[1];
                gridColorButtonX.BackColor = color[2];
                gridColorButtonY.BackColor = color[3];
                scaleColorButtonX.BackColor = color[4];
                scaleColorButtonY.BackColor = color[5];
                for(int i = 0; i < fc; i++)
				{
                    AddFunction();
                    functions[i].formulaText.Text = funcFormula[i];
                    functions[i].diplayCheckBox.Checked = funcDisplay[i] == 0 ? false : true;
                    functions[i].colorButton.BackColor=funcColor[i];
				}
                for(int i = 0; i < pc; i++)
				{
                    Function.AddParameter();
                    Function.parameters[i].nameText.Text = paramName[i];
                    Function.parameters[i].valueText.Text = paramValue[i].ToString("0.00");
                    Function.parameters[i].minText.Text = paramMin[i].ToString("0.00");
                    Function.parameters[i].maxText.Text = paramMax[i].ToString("0.00");
                }
                UpDate();
                return true;
            }

            //イベントハンドラ
            static bool clicked;
            static Vec2 preMousePoint; 
            static void graph_MouseDown(object sender, MouseEventArgs e)
            {
                clicked = true;
                preMousePoint = new Vec2(e.X, e.Y);
            }
            static void graph_MouseMove(object sender, MouseEventArgs e)
            {
                if (clicked)
                {
                    Vec2 delta = new Vec2(-e.X + preMousePoint.x, e.Y - preMousePoint.y);
                    SetCenter(center + new Vec2(delta.x * size.x / graph.Width, delta.y * size.y / graph.Height));
                    preMousePoint = new Vec2(e.X, e.Y);
                    UpDate();
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
                SetCenter(new Vec2(center.x + (size.x - preSize.x) / 2 + (preSize.x - size.x) * e.X / graph.Width, center.y + (size.y - preSize.y) / 2 + (preSize.y - size.y) * (graph.Height-e.Y) / graph.Height));
                UpDate();
            }
            static void sizeTextX_TextChanged(object sender, EventArgs e)
            {
                if (!sizeTextX.Modified) return;
                double num;
                if (double.TryParse(sizeTextX.Text, out num))
                {
                    SetSize(new Vec2(num, size.y), false);
                    UpDate();
                }
            }
            static void sizeTextY_TextChanged(object sender, EventArgs e)
            {
                if (!sizeTextY.Modified) return;
                double num;
                if (double.TryParse(sizeTextY.Text, out num))
                {
                    SetSize(new Vec2(size.x, num), false);
                    UpDate();
                }
            }
            static void centerTextX_TextChanged(object sender, EventArgs e)
            {
                if (!centerTextX.Modified) return;
                double num;
                if (double.TryParse(centerTextX.Text, out num))
                {
                    SetCenter(new Vec2(num, center.y), false);
                    UpDate();
                }
            }
            static void centerTextY_TextChanged(object sender, EventArgs e)
            {
                if (!centerTextY.Modified) return;
                double num;
                if (double.TryParse(centerTextY.Text, out num))
                {
                    SetCenter(new Vec2(center.x, num), false);
                    UpDate();
                }
            }
            static void display_CheckedChanged(object sender,EventArgs e)
			{
                UpDate();
			}
            
            //イベントハンドラここまで

            static void SetSize(Vec2 s, bool textChange = true)
            {
                size = s;
                if (size.x < minSize.x) size.x = minSize.x;
                if (size.y < minSize.y) size.y = minSize.y;
                if (size.x > maxSize.x) size.x = maxSize.x;
                if (size.y > maxSize.y) size.y = maxSize.y;
                if (textChange)
                {
                    sizeTextX.Text = size.x.ToString("0.00");
                    sizeTextY.Text = size.y.ToString("0.00");
                }
            }
            static void SetCenter(Vec2 p, bool textChange = true)
            {
                center = p;
                if (center.x > maxCenter.x) center.x = maxCenter.x;
                if (center.y > maxCenter.y) center.y = maxCenter.y;
                if (center.x < minCenter.x) center.x = minCenter.x;
                if (center.y < minCenter.y) center.y = minCenter.y;
                if (textChange)
                {
                    centerTextX.Text = center.x.ToString("0.00");
                    centerTextY.Text = center.y.ToString("0.00");
                    centerTextX.Refresh();
                    centerTextY.Refresh();
                }
            }
            static Vec2 ViewPointToGraphPoint(Vec2 p)
            {
                return new Vec2(center.x - size.x / 2 + size.x * p.x / graph.Width, center.y - size.y / 2 + size.y * (graph.Height - p.y) / graph.Height);
            }
            static Vec2 GraphPointToViewPoint(Vec2 p)
            {
                return new Vec2(graph.Width * (0.5 + (p.x - center.x) / size.x), graph.Height - (graph.Height * (0.5 + (p.y - center.y) / size.y)));
            }
            public static void UpDate()
			{
                foreach(Function function in functions)
				{
                    function.Update();
				}
                Save("AutoSave.txt");
                Draw();
			}
            public static void Draw()
            {
                Bitmap canvas = new Bitmap(graph.Width, graph.Height);
                Graphics g = Graphics.FromImage(canvas);

                g.Clear(backColorButton.BackColor);

                Pen pen;
                Brush brush;
                Font font = new Font("MS UI Gothic", (int)(10 * scale));

                //グリッドと目盛の描画
                //Y
                pen = new Pen(gridColorButtonY.BackColor, 1);
                brush = new SolidBrush(scaleColorButtonY.BackColor);
                int logx = (int)Math.Ceiling(Math.Log10(size.x));
                int px = (int)Math.Pow(10, Math.Max(0, -logx + 1));
                logx = Math.Max(1, logx);
                double divx;
                if (size.x * px / Math.Pow(10, logx - 1) <= 3) divx = 0.5;
                else if (size.x * px / Math.Pow(10, logx - 1) <= 6) divx = 1;
                else divx = 2;
                for (int i = 0; i <= 10; i++)
                {
                    double lx = ViewPointToGraphPoint(new Vec2(0, 0)).x;
                    double graphx = (int)(lx * px / divx) / (int)Math.Pow(10, logx - 1) * divx / (double)px * Math.Pow(10, logx - 1) + (i - 1) * Math.Pow(10, logx - 1) / px * divx;
                    int x = (int)GraphPointToViewPoint(new Vec2(graphx, 0)).x;
                    if (gridDisplayY.Checked) g.DrawLine(pen, x, 0, x, graph.Height);
                    string scaleText = graphx.ToString("0.00");
                    int scaley = (int)GraphPointToViewPoint(new Vec2(0, 0)).y;
                    scaley = Math.Max(scaley, 0);
                    scaley = Math.Min(scaley, graph.Height - 15);
                    if (graphx == 0) continue;
                    if (scaleDisplayY.Checked)
                    {
                        if (gridDisplayY.Checked) g.DrawString(scaleText, font, brush, x - scaleText.Length * 10 + 5, scaley);
                        else g.DrawString(scaleText, font, brush, x - scaleText.Length * 5, scaley);
                    }
                }
                //X
                pen = new Pen(gridColorButtonX.BackColor);
                brush = new SolidBrush(scaleColorButtonX.BackColor);
                int logy = (int)Math.Ceiling(Math.Log10(size.y));
                int py = (int)Math.Pow(10, Math.Max(0, -logy + 1));
                logy = Math.Max(1, logy);
                double divy;
                if (size.y * py / Math.Pow(10, logy - 1) <= 3) divy = 0.5;
                else if (size.y * py / Math.Pow(10, logy - 1) <= 6) divy = 1;
                else divy = 2;
                for (int i = 0; i <= 10; i++)
                {
                    double ly = ViewPointToGraphPoint(new Vec2(0, graph.Height)).y;
                    double graphy = (int)(ly * py / divy) / (int)Math.Pow(10, logy - 1) * divy / (double)py * Math.Pow(10, logy - 1) + (i - 1) * Math.Pow(10, logy - 1) / py * divy;
                    int y = (int)GraphPointToViewPoint(new Vec2(0, graphy)).y;
                    if (gridDisplayX.Checked) g.DrawLine(pen, 0, y, graph.Width, y);
                    string scaleText = graphy.ToString("0.00");
                    int scalex = (int)GraphPointToViewPoint(new Vec2(0, 0)).x - scaleText.Length * 10 + 5;
                    scalex = Math.Max(scalex, 0);
                    scalex = Math.Min(scalex, graph.Width - scaleText.Length * 10 + 5);
                    if (graphy == 0) continue;
                    if (scaleDisplayX.Checked)
                    {
                        if (gridDisplayX.Checked) g.DrawString(scaleText, font, brush, scalex, y);
                        else g.DrawString(scaleText, font, brush, scalex, y - 6);
                    }
                }

                //軸の描画
                Vec2 originPoint = GraphPointToViewPoint(new Vec2(0, 0));
                //Y
                pen = new Pen(axisColorButtonY.BackColor, 2);
                if (axisDisplayY.Checked) g.DrawLine(pen, (int)originPoint.x, 0, (int)originPoint.x, graph.Height);
                //X
                pen = new Pen(axisColorButtonX.BackColor, 2);
                if (axisDisplayX.Checked) g.DrawLine(pen, 0, (int)originPoint.y, graph.Width, (int)originPoint.y);

                //関数の描画
                const int d = 1;
                foreach (var function in functions)
                {
                    if (function.calc == null) continue;
                    if (!function.diplayCheckBox.Checked) continue;
                    pen = new Pen(function.colorButton.BackColor, 3);
                    for (int i = 0; i < (graph.Width - 1) / d; i++)
                    {
                        double x1 = ViewPointToGraphPoint(new Vec2(i * d, 0)).x;
                        double x2 = ViewPointToGraphPoint(new Vec2((i + 1) * d, 0)).x;
                        CalcResult y1 = function.calc(new CalcResult(x1));
                        CalcResult y2 = function.calc(new CalcResult(x2));
                        if (!y1.defined || !y2.defined) continue;
                        g.DrawLine(pen, i * d, (int)GraphPointToViewPoint(new Vec2(0, y1.value)).y, (i + 1) * d, (int)GraphPointToViewPoint(new Vec2(0, y2.value)).y);
                    }
                }

                g.Dispose();
                graph.Image = canvas;
                graph.Refresh();
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

        //実装済み関数
        //sin,cos,tan,asin,acos,atan,sinh,cosh,tanh,log,ln,exp,pow,pi,e,abs,max,min,ceil,floor
        class CalcResult
        {
            static readonly double resultMax = 100000;
            static readonly double resultMin = -100000;

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
            public static CalcResult sinh(CalcResult[] x)
			{
                if (!x[0].defined) return x[0];
                if (x[0].value < Math.Log(resultMin + Math.Sqrt(resultMin * resultMin + 1)) || Math.Log(resultMax + Math.Sqrt(resultMax * resultMax + 1)) < x[0].value) return new CalcResult(0, false);
                return new CalcResult(Math.Sinh(x[0].value));
			}
            public static CalcResult cosh(CalcResult[] x)
			{
                if (!x[0].defined) return x[0];
                if (x[0].value < Math.Log(resultMax - Math.Sqrt(resultMax * resultMax - 1)) || Math.Log(resultMax + Math.Sqrt(resultMax * resultMax - 1)) < x[0].value) return new CalcResult(0, false);
                return new CalcResult(Math.Cosh(x[0].value));
			}
            public static CalcResult tanh(CalcResult[] x)
			{
                if (!x[0].defined) return x[0];
                return new CalcResult(Math.Tanh(x[0].value));
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
                if (x[0].value == 0) return new CalcResult(0);
                if (x[0].value == 1) return new CalcResult(1);
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
            public static CalcResult sqrt(CalcResult[] x)
			{
                if (!x[0].defined) return x[0];
                return CalcResult.pow(new CalcResult[] { x[0], new CalcResult(0.5) });
			}
            public static CalcResult cbrt(CalcResult[] x)
			{
                if (!x[0].defined) return x[0];
                return new CalcResult(Math.Cbrt(x[0].value));
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
            //構文解析
            //<expression> := ['-'] <term> [('+' or '-') <term> ('+' or '-') ...]
            //<term>       := <factor> [('*' or '/') <factor> ('*' or '/') ...]
            //<factor>     := <factor2> ['^' <factor2>]
            //<factor2>    := '('<expression>')' or <number> or <func> or <param>
            //<number>     := 数
            //<func>       := <定数> or <一変数関数> or <多変数関数>
            //<param>      := パラメーター名
            //<定数>       := 定数名
            //<一変数関数> := 関数名<factor2>
            //<多変数関数> := 関数名'('<expression>','<expression>[','<expression>...]')'

            static class Generator
            {
                static readonly double delta = 0.01;
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
                    new DefinedFunction("sinh",1,CalcResult.sinh),
                    new DefinedFunction("cosh",1,CalcResult.cosh),
                    new DefinedFunction("tanh",1,CalcResult.tanh),
                    new DefinedFunction("x",0,CalcResult.x),
                    new DefinedFunction("log",2,CalcResult.log),
                    new DefinedFunction("ln",1,CalcResult.ln),
                    new DefinedFunction("exp",1,CalcResult.exp),
                    new DefinedFunction("pi",0,CalcResult.pi),
                    new DefinedFunction("e",0,CalcResult.e),
                    new DefinedFunction("abs",1,CalcResult.abs),
                    new DefinedFunction("pow",2,CalcResult.pow),
                    new DefinedFunction("sqrt",1,CalcResult.sqrt),
                    new DefinedFunction("cbrt",1,CalcResult.cbrt),
                    new DefinedFunction("max",2,CalcResult.max),
                    new DefinedFunction("min",2,CalcResult.min),
                    new DefinedFunction("floor",1,CalcResult.floor),
                    new DefinedFunction("ceil",1,CalcResult.ceil),
                };
                static class State
                {
                    public static string s;
                    public static int p;
                    public static bool error;
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
                        if (p < s.Length) p++;
                    }
                    static public void proceed(int n)
                    {
                        if (p + n <= s.Length) p += n;
                    }
                    static public void consume(char expected)
                    {
                        if (begin() == expected) proceed();
                        else error = true;
                    }
                }
                static Func<CalcResult, CalcResult> expression()
                {
                    Func<CalcResult, CalcResult> res;
                    bool minus = false;
                    if (State.begin() == '-')
                    {
                        minus = true;
                        State.proceed();
                    }
                    var h = term();
                    if (h == null) return null;
                    if (!minus) res = x => h(x);
                    else res = x => -h(x);
                    while (true)
                    {
                        if (State.begin() == '+')
                        {
                            State.proceed();
                            var f = (Func<CalcResult, CalcResult>)res.Clone();
                            var g = term();
                            if (g == null) return null;
                            res = x => f(x) + g(x);
                        }
                        else if (State.begin() == '-')
                        {
                            State.proceed();
                            var f = (Func<CalcResult, CalcResult>)res.Clone();
                            var g = term();
                            if (g == null) return null;
                            res = x => f(x) - g(x);
                        }
                        else break;
                    }
                    return res;
                }
                static Func<CalcResult, CalcResult> term()
                {
                    Func<CalcResult, CalcResult> res;
                    res = factor();
                    if (res == null) return null;
                    while (true)
                    {
                        if (State.begin() == '*')
                        {
                            State.proceed();
                            var f = (Func<CalcResult, CalcResult>)res.Clone();
                            var g = factor();
                            if (g == null) return null;
                            res = x => f(x) * g(x);
                        }
                        else if (State.begin() == '/')
                        {
                            State.proceed();
                            var f = (Func<CalcResult, CalcResult>)res.Clone();
                            var g = factor();
                            if (g == null) return null;
                            res = x => f(x) / g(x);
                        }
                        else break;
                    }
                    return res;
                }
                static Func<CalcResult, CalcResult> factor()
                {
                    Func<CalcResult, CalcResult> res;
                    res = factor2();
                    if (res == null) return null;
                    if (State.begin() == '^')
                    {
                        State.proceed();
                        var f = (Func<CalcResult, CalcResult>)res.Clone();
                        var g = factor();
                        if (g == null) return null;
                        res = x => CalcResult.pow(new CalcResult[] { f(x), g(x) });
                    }
                    return res;
                }
                static Func<CalcResult, CalcResult> factor2()
                {
                    Func<CalcResult, CalcResult> res = null;
                    char par = parenthesis();
                    if (par != ' ')
                    {
                        res = expression();
                        State.consume(par);
                    }
                    else if (State.begin() == '|')
                    {
                        State.proceed();
                        var f = expression();
                        if (f != null) res = x => CalcResult.abs(new CalcResult[] { f(x) });
                        State.consume('|');
                    }
                    else if (char.IsDigit(State.begin()))
                    {
                        res = number();
                    }
                    else
                    {
                        res = func();
                        if (res == null) res = param();
                    }
                    if (res == null || State.error) return null;
                    int prime = 0;
					while (State.begin() == '\'')
					{
                        prime++;
                        State.proceed();
					}
					if (prime > 5)
					{
                        State.error = true;
                        return null;
					}
                    while ((prime--) > 0)
                    {
                        var f = (Func<CalcResult, CalcResult>)res.Clone();
                        res = x => (f(x + delta) - f(x - delta)) / (2 * delta);
                    }
					if (!char.IsDigit(State.begin()))
					{
                        int p = State.p;
                        var f=(Func<CalcResult, CalcResult>)res.Clone();
                        var g = factor();
                        if (g == null)
                        {
                            State.error = false;
                            State.p = p;
                        }
                        else
                        {
                            res = x => f(x) * g(x);
                        }
					}
                    return res;
                }
                static Func<CalcResult, CalcResult> func()
                {
                    Func<CalcResult, CalcResult> res = null;
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
                        res = x => function.calc(new CalcResult[] { x });
                    }
                    else if (function.valNum == 1)
                    {
                        char par = parenthesis();
                        if(par==' ')
						{
                            var f = factor2();
                            if (f == null) return null;
                            res = x => function.calc(new CalcResult[] { f(x) });
                        }
						else
						{
                            var f = expression();
                            if (f == null) return null;
                            res = x => function.calc(new CalcResult[] { f(x) });
                            State.consume(par);
                        }
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
                        res = x => function.calc(f(x));
                    }
					if (prime > 5)
					{
                        State.error = true;
                        return null;
					}
                    while ((prime--) > 0)
                    {
                        var f = (Func<CalcResult, CalcResult>)res.Clone();
                        res = x => (f(x + delta) - f(x - delta)) / (2 * delta);
                    }
                    return res;
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
                    Func<CalcResult, CalcResult> res;
                    Parameter parameter = null;
                    foreach (var def in parameters)
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
                    if (prime) res = x => new CalcResult(0);
                    else res = x => new CalcResult(parameter.value);
                    return res;
                }
                static Func<CalcResult, CalcResult> number()
                {
                    Func<CalcResult, CalcResult> res;
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
                        if (cnt > 5)
                        {
                            State.error = true;
                            return null;
                        }
                        State.proceed();
                    }
                    res = x => new CalcResult(num / Math.Pow(10, Math.Max(0, decPoint - 1)));
                    return res;
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
            //ここまで構文解析//

            //パラメーター関連
            public static FlowLayoutPanel parametersFlow;
            public class Parameter
            {
                
                static readonly double rangeMin = -1000;
                static readonly double rangeMax = 1000;

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
                public Parameter()
                {
                    value = 0;
                    min = -5;
                    max = 5;

                    //動的に生成したコントロールの初期設定
                    //flow
                    flow =new FlowLayoutPanel();
                    flow.FlowDirection = FlowDirection.LeftToRight;
                    flow.Width = 395;
                    flow.Height = 30;
                    flow.WrapContents = false;
                    flow.Margin = new Padding(0);
                    //nameText
                    nameText = new TextBox();
                    nameText.Width = 50;
                    nameText.Font = new Font("Yu Gothic UI", (int)(9 * scale));
                    nameText.TextChanged += nameText_TextChanged;
                    nameText.Margin = new Padding(0);
                    flow.Controls.Add(nameText);
                    //valueText
                    valueText = new TextBox();
                    valueText.Text = value.ToString();
                    valueText.Width = 50;
                    valueText.Font = new Font("Yu Gothic UI", (int)(9 * scale));
                    valueText.TextChanged += valueText_TextChanged;
                    valueText.Margin = new Padding(10, 0, 0, 0);
                    flow.Controls.Add(valueText);
                    //minText
                    minText = new TextBox();
                    minText.Text = min.ToString();
                    minText.Width = 40;
                    minText.Font = new Font("Yu Gothic UI", (int)(9 * scale));
                    minText.TextChanged += minText_TextChanged;
                    minText.Margin = new Padding(17, 0, 0, 0);
                    flow.Controls.Add(minText);
                    //bar
                    bar=new TrackBar();
                    bar.Maximum = (int)(max * 100);
                    bar.Minimum = (int)(min * 100);
                    bar.TickStyle = TickStyle.None;
                    bar.Scroll+=bar_Scroll;
                    bar.Margin = new Padding(0);
                    flow.Controls.Add(bar);
                    //maxText
                    maxText=new TextBox();
                    maxText.Text = max.ToString();
                    maxText.Width = 40;
                    maxText.Font = new Font("Yu Gothic UI", (int)(9 * scale));
                    maxText.TextChanged += maxText_TextChanged;
                    maxText.Margin = new Padding(0);
                    flow.Controls.Add(maxText);
                    //deleteButton
                    deleteButton = new Button();
                    deleteButton.Text = "delete";
                    deleteButton.Height = 27;
                    deleteButton.Font = new Font("Yu Gothic UI", (int)(9 * scale));
                    deleteButton.Click += deleteButton_Click;
                    deleteButton.Margin = new Padding(10, 0, 0, 0);
                    flow.Controls.Add(deleteButton);
                }

                //パラメーターイベントハンドラ
                private void nameText_TextChanged(object sender, EventArgs e)
				{
                    Graph.UpDate();
				}
                private void minText_TextChanged(object sender, EventArgs e)
                {
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
                }
                //パラメーターイベントハンドラここまで
            }

            public static List<Parameter> parameters = new List<Parameter>();
            public static void AddParameter()
            {
                parameters.Add(new Parameter());
                parametersFlow.Controls.Add(parameters.Last().flow);
            }

            static void DeleteParameter(Parameter parameter)
            {
                parametersFlow.Controls.Remove(parameter.flow);
                parameters.Remove(parameter);
                Graph.UpDate();
            }
            public static void DeleteAllParameters()
			{
                for(int i = parameters.Count - 1; i >= 0; i--)
				{
                    DeleteParameter(parameters[i]);
				}
			}
            //ここまでパラメーター関連

            public Func<CalcResult, CalcResult> calc;
            public FlowLayoutPanel flow;
            public CheckBox diplayCheckBox;
            public Label formulaLabel;
            public TextBox formulaText;
            public Button colorButton;
            public Button deleteButton;
            public Function()
            {
                //動的に生成したコントロールの初期設定
                //flow
                flow = new FlowLayoutPanel();
                flow.FlowDirection = FlowDirection.LeftToRight;
                flow.WrapContents = false;
                flow.Width = 395;
                flow.Height = 30;
                flow.Margin = new Padding(0);
                //diplayCheckBox
                diplayCheckBox = new CheckBox();
                diplayCheckBox.Checked = true;
                diplayCheckBox.Width = 20;
                diplayCheckBox.Height = 20;
                diplayCheckBox.CheckedChanged += diplayCheckBox_CheckedChanged;
                diplayCheckBox.Margin = new Padding(20, 4, 0, 0);
                flow.Controls.Add(diplayCheckBox);
                //formulaLabel
                formulaLabel =new Label();
                formulaLabel.Text = "y =";
                formulaLabel.Width = 30;
                formulaLabel.Font = new Font("Yu Gothic UI", (int)(9 * scale));
                formulaLabel.Margin=new Padding(20,1,0,0);
                flow.Controls.Add(formulaLabel);
                //formulaText
                formulaText = new TextBox();
                formulaText.Width = 170;
                formulaText.Font = new Font("Yu Gothic UI", (int)(9 * scale));
                formulaText.TextChanged += formulaText_TextChanged;
                formulaText.Margin = new Padding(0);
                flow.Controls.Add(formulaText);
                //colorButton
                colorButton = new Button();
                colorButton.BackColor = Color.Black;
                colorButton.Width = 27;
                colorButton.Height = 27;
                colorButton.Click += colorButton_Click;
                colorButton.Margin = new Padding(15,0,0,0);
                flow.Controls.Add(colorButton);
                //deleteButton
                deleteButton = new Button();
                deleteButton.Text = "delete";
                deleteButton.Height = 27;
                deleteButton.Font = new Font("Yu Gothic UI", (int)(9 * scale));
                deleteButton.Click += deleteButton_Click;
                deleteButton.Margin = new Padding(15,0,0,0);
                flow.Controls.Add(deleteButton);
            }

            //関数のイベントハンドラ
            void diplayCheckBox_CheckedChanged(object sender, EventArgs e)
            {
                Graph.UpDate();
            }
            void formulaText_TextChanged(object sender, EventArgs e)
            {
                Graph.UpDate();
            }
            void deleteButton_Click(object sender, EventArgs e)
            {
                Graph.DeleteFunction(this);
            }
            public void Update()
			{
                calc = Generator.Generate(formulaText.Text);
			}
        }
        static void colorButton_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            Button button = (Button)sender;
            colorDialog.Color = button.BackColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                button.BackColor = colorDialog.Color;
            }
            Graph.Draw();
        }
        private void addFunctionButton_Click(object sender, EventArgs e)
        {
            Graph.AddFunction();
        }
        private void clearFunctionButton_Click(object sender, EventArgs e)
		{
            Graph.DeleteAllFunctions();
		}
		private void addParameterButton_Click(object sender, EventArgs e)
		{
            Function.AddParameter();
		}
		private void clearParameterButton_Click(object sender, EventArgs e)
		{
            Function.DeleteAllParameters();
		}
        private void saveButton_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
        }
        private void resetButton_Click(object sender, EventArgs e)
		{
            Graph.Reset();
		}
        private void loadButton_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
        }
        private void saveImageButton_Click(object sender, EventArgs e)
		{
            saveImageFileDialog.ShowDialog();
		}

		private void saveImageFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
            Graph.SaveAsImage(saveImageFileDialog.FileName, System.IO.Path.GetExtension(saveImageFileDialog.FileName));
        }

		private void saveFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
            Graph.Save(saveFileDialog.FileName);
		}

		private void openFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!Graph.Load(openFileDialog.FileName))
			{
                MessageBox.Show("failed to load the data", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}

