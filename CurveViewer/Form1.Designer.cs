namespace CurveViewer
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.graph = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.AddFunctionButton = new System.Windows.Forms.Button();
			this.ClearFunctionButton = new System.Windows.Forms.Button();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.label5 = new System.Windows.Forms.Label();
			this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
			this.AddParameterButton = new System.Windows.Forms.Button();
			this.ClearParameterButton = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.graph)).BeginInit();
			this.tableLayoutPanel2.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.flowLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.flowLayoutPanel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.75772F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.24228F));
			this.tableLayoutPanel1.Controls.Add(this.graph, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(1182, 753);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// graph
			// 
			this.graph.Dock = System.Windows.Forms.DockStyle.Fill;
			this.graph.Location = new System.Drawing.Point(4, 4);
			this.graph.Name = "graph";
			this.tableLayoutPanel1.SetRowSpan(this.graph, 3);
			this.graph.Size = new System.Drawing.Size(722, 745);
			this.graph.TabIndex = 0;
			this.graph.TabStop = false;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel1, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel2, 0, 2);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(733, 259);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(445, 241);
			this.tableLayoutPanel2.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(92, 28);
			this.label1.TabIndex = 0;
			this.label1.Text = "functions";
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.AddFunctionButton);
			this.flowLayoutPanel1.Controls.Add(this.ClearFunctionButton);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 31);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(439, 34);
			this.flowLayoutPanel1.TabIndex = 1;
			// 
			// AddFunctionButton
			// 
			this.AddFunctionButton.Location = new System.Drawing.Point(3, 3);
			this.AddFunctionButton.Name = "AddFunctionButton";
			this.AddFunctionButton.Size = new System.Drawing.Size(94, 29);
			this.AddFunctionButton.TabIndex = 0;
			this.AddFunctionButton.Text = "add";
			this.AddFunctionButton.UseVisualStyleBackColor = true;
			this.AddFunctionButton.Click += new System.EventHandler(this.AddFunctionButton_Click);
			// 
			// ClearFunctionButton
			// 
			this.ClearFunctionButton.Location = new System.Drawing.Point(150, 3);
			this.ClearFunctionButton.Margin = new System.Windows.Forms.Padding(50, 3, 3, 3);
			this.ClearFunctionButton.Name = "ClearFunctionButton";
			this.ClearFunctionButton.Size = new System.Drawing.Size(94, 29);
			this.ClearFunctionButton.TabIndex = 1;
			this.ClearFunctionButton.Text = "clear";
			this.ClearFunctionButton.UseVisualStyleBackColor = true;
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Controls.Add(this.flowLayoutPanel3);
			this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 71);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(439, 167);
			this.flowLayoutPanel2.TabIndex = 2;
			// 
			// flowLayoutPanel3
			// 
			this.flowLayoutPanel3.Controls.Add(this.label2);
			this.flowLayoutPanel3.Controls.Add(this.label3);
			this.flowLayoutPanel3.Controls.Add(this.label4);
			this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Left;
			this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 3);
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			this.flowLayoutPanel3.Size = new System.Drawing.Size(438, 30);
			this.flowLayoutPanel3.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(61, 20);
			this.label2.TabIndex = 0;
			this.label2.Text = "formula";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(70, 0);
			this.label3.Margin = new System.Windows.Forms.Padding(3, 0, 20, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(43, 20);
			this.label3.TabIndex = 1;
			this.label3.Text = "color";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(136, 0);
			this.label4.Margin = new System.Windows.Forms.Padding(3, 0, 10, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 20);
			this.label4.TabIndex = 2;
			this.label4.Text = "display";
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 1;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.label5, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel4, 0, 1);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(733, 507);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 3;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(445, 242);
			this.tableLayoutPanel3.TabIndex = 2;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label5.Location = new System.Drawing.Point(3, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(110, 28);
			this.label5.TabIndex = 0;
			this.label5.Text = "parameters";
			// 
			// flowLayoutPanel4
			// 
			this.flowLayoutPanel4.Controls.Add(this.AddParameterButton);
			this.flowLayoutPanel4.Controls.Add(this.ClearParameterButton);
			this.flowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel4.Location = new System.Drawing.Point(3, 31);
			this.flowLayoutPanel4.Name = "flowLayoutPanel4";
			this.flowLayoutPanel4.Size = new System.Drawing.Size(439, 35);
			this.flowLayoutPanel4.TabIndex = 1;
			// 
			// AddParameterButton
			// 
			this.AddParameterButton.Location = new System.Drawing.Point(3, 3);
			this.AddParameterButton.Name = "AddParameterButton";
			this.AddParameterButton.Size = new System.Drawing.Size(94, 29);
			this.AddParameterButton.TabIndex = 0;
			this.AddParameterButton.Text = "add";
			this.AddParameterButton.UseVisualStyleBackColor = true;
			// 
			// ClearParameterButton
			// 
			this.ClearParameterButton.Location = new System.Drawing.Point(103, 3);
			this.ClearParameterButton.Name = "ClearParameterButton";
			this.ClearParameterButton.Size = new System.Drawing.Size(94, 29);
			this.ClearParameterButton.TabIndex = 1;
			this.ClearParameterButton.Text = "clear";
			this.ClearParameterButton.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1182, 753);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "Form1";
			this.Text = "Curve Viewer";
			this.tableLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.graph)).EndInit();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel3.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.flowLayoutPanel4.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private TableLayoutPanel tableLayoutPanel1;
		private PictureBox graph;
		private TableLayoutPanel tableLayoutPanel2;
		private Label label1;
		private FlowLayoutPanel flowLayoutPanel1;
		private Button AddFunctionButton;
		private Button ClearFunctionButton;
		private FlowLayoutPanel flowLayoutPanel2;
		private FlowLayoutPanel flowLayoutPanel3;
		private Label label2;
		private Label label3;
		private Label label4;
		private TableLayoutPanel tableLayoutPanel3;
		private Label label5;
		private FlowLayoutPanel flowLayoutPanel4;
		private Button AddParameterButton;
		private Button ClearParameterButton;
	}
}