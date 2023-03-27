namespace MusicSchoolApp.Forms
{
	partial class StudentForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StudentForm));
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.studentNameField = new System.Windows.Forms.Label();
			this.nameField = new System.Windows.Forms.TextBox();
			this.teacherField = new System.Windows.Forms.TextBox();
			this.descriptionField = new System.Windows.Forms.TextBox();
			this.markField = new System.Windows.Forms.TextBox();
			this.commentField = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.logOutBtn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// treeView1
			// 
			this.treeView1.Location = new System.Drawing.Point(12, 32);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(257, 489);
			this.treeView1.TabIndex = 0;
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			this.treeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDown);
			// 
			// studentNameField
			// 
			this.studentNameField.AutoSize = true;
			this.studentNameField.Location = new System.Drawing.Point(12, 9);
			this.studentNameField.Name = "studentNameField";
			this.studentNameField.Size = new System.Drawing.Size(50, 20);
			this.studentNameField.TabIndex = 1;
			this.studentNameField.Text = "label1";
			// 
			// nameField
			// 
			this.nameField.Location = new System.Drawing.Point(427, 32);
			this.nameField.Name = "nameField";
			this.nameField.ReadOnly = true;
			this.nameField.Size = new System.Drawing.Size(361, 27);
			this.nameField.TabIndex = 2;
			// 
			// teacherField
			// 
			this.teacherField.Location = new System.Drawing.Point(427, 65);
			this.teacherField.Name = "teacherField";
			this.teacherField.ReadOnly = true;
			this.teacherField.Size = new System.Drawing.Size(361, 27);
			this.teacherField.TabIndex = 3;
			// 
			// descriptionField
			// 
			this.descriptionField.Location = new System.Drawing.Point(427, 98);
			this.descriptionField.Multiline = true;
			this.descriptionField.Name = "descriptionField";
			this.descriptionField.ReadOnly = true;
			this.descriptionField.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.descriptionField.Size = new System.Drawing.Size(361, 172);
			this.descriptionField.TabIndex = 4;
			// 
			// markField
			// 
			this.markField.Location = new System.Drawing.Point(427, 276);
			this.markField.Name = "markField";
			this.markField.ReadOnly = true;
			this.markField.Size = new System.Drawing.Size(125, 27);
			this.markField.TabIndex = 5;
			// 
			// commentField
			// 
			this.commentField.Location = new System.Drawing.Point(427, 309);
			this.commentField.Multiline = true;
			this.commentField.Name = "commentField";
			this.commentField.ReadOnly = true;
			this.commentField.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.commentField.Size = new System.Drawing.Size(361, 212);
			this.commentField.TabIndex = 6;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(344, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(77, 20);
			this.label1.TabIndex = 8;
			this.label1.Text = "Название";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(304, 65);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(117, 20);
			this.label2.TabIndex = 9;
			this.label2.Text = "Преподаватель";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(342, 98);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(79, 20);
			this.label3.TabIndex = 10;
			this.label3.Text = "Описание";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(360, 276);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(61, 20);
			this.label4.TabIndex = 11;
			this.label4.Text = "Оценка";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(314, 309);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(107, 20);
			this.label5.TabIndex = 12;
			this.label5.Text = "Комментарий";
			// 
			// logOutBtn
			// 
			this.logOutBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("logOutBtn.BackgroundImage")));
			this.logOutBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.logOutBtn.Location = new System.Drawing.Point(759, 0);
			this.logOutBtn.Name = "logOutBtn";
			this.logOutBtn.Size = new System.Drawing.Size(29, 29);
			this.logOutBtn.TabIndex = 13;
			this.logOutBtn.UseVisualStyleBackColor = true;
			this.logOutBtn.Click += new System.EventHandler(this.logOutBtn_Click);
			// 
			// StudentForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 533);
			this.Controls.Add(this.logOutBtn);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.commentField);
			this.Controls.Add(this.markField);
			this.Controls.Add(this.descriptionField);
			this.Controls.Add(this.teacherField);
			this.Controls.Add(this.nameField);
			this.Controls.Add(this.studentNameField);
			this.Controls.Add(this.treeView1);
			this.Name = "StudentForm";
			this.Text = "StudedntForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private TreeView treeView1;
		private Label studentNameField;
		private TextBox nameField;
		private TextBox teacherField;
		private TextBox descriptionField;
		private TextBox markField;
		private TextBox commentField;
		private Label label1;
		private Label label2;
		private Label label3;
		private Label label4;
		private Label label5;
		private Button logOutBtn;
	}
}