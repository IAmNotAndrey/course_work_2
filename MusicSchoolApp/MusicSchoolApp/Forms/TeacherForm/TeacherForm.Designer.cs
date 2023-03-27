namespace MusicSchoolApp.Forms
{
	partial class TeacherForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TeacherForm));
			this.tabMenu = new System.Windows.Forms.TabControl();
			this.taskEditTabPage = new System.Windows.Forms.TabPage();
			this.tName1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.descriptionField = new System.Windows.Forms.TextBox();
			this.nameField = new System.Windows.Forms.TextBox();
			this.cancelBtn = new System.Windows.Forms.Button();
			this.saveBtn = new System.Windows.Forms.Button();
			this.removeBtn = new System.Windows.Forms.Button();
			this.addBtn = new System.Windows.Forms.Button();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.taskRatingTabPage = new System.Windows.Forms.TabPage();
			this.tName2 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.commentField2 = new System.Windows.Forms.TextBox();
			this.markField2 = new System.Windows.Forms.TextBox();
			this.descriptionField2 = new System.Windows.Forms.TextBox();
			this.nameField2 = new System.Windows.Forms.TextBox();
			this.treeView2 = new System.Windows.Forms.TreeView();
			this.listBox2 = new System.Windows.Forms.ListBox();
			this.taskAssignmentTabPage = new System.Windows.Forms.TabPage();
			this.tName3 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.cancelBtn3 = new System.Windows.Forms.Button();
			this.saveBtn3 = new System.Windows.Forms.Button();
			this.descriptionField3 = new System.Windows.Forms.TextBox();
			this.nameField3 = new System.Windows.Forms.TextBox();
			this.checkedListBoxStudents3 = new System.Windows.Forms.CheckedListBox();
			this.checkedListBoxGroups3 = new System.Windows.Forms.CheckedListBox();
			this.treeView3 = new System.Windows.Forms.TreeView();
			this.logOutBtn = new System.Windows.Forms.Button();
			this.tabMenu.SuspendLayout();
			this.taskEditTabPage.SuspendLayout();
			this.taskRatingTabPage.SuspendLayout();
			this.taskAssignmentTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabMenu
			// 
			this.tabMenu.Controls.Add(this.taskEditTabPage);
			this.tabMenu.Controls.Add(this.taskRatingTabPage);
			this.tabMenu.Controls.Add(this.taskAssignmentTabPage);
			this.tabMenu.Location = new System.Drawing.Point(12, 30);
			this.tabMenu.Name = "tabMenu";
			this.tabMenu.SelectedIndex = 0;
			this.tabMenu.Size = new System.Drawing.Size(931, 544);
			this.tabMenu.TabIndex = 0;
			// 
			// taskEditTabPage
			// 
			this.taskEditTabPage.Controls.Add(this.tName1);
			this.taskEditTabPage.Controls.Add(this.label2);
			this.taskEditTabPage.Controls.Add(this.label1);
			this.taskEditTabPage.Controls.Add(this.descriptionField);
			this.taskEditTabPage.Controls.Add(this.nameField);
			this.taskEditTabPage.Controls.Add(this.cancelBtn);
			this.taskEditTabPage.Controls.Add(this.saveBtn);
			this.taskEditTabPage.Controls.Add(this.removeBtn);
			this.taskEditTabPage.Controls.Add(this.addBtn);
			this.taskEditTabPage.Controls.Add(this.treeView1);
			this.taskEditTabPage.Location = new System.Drawing.Point(4, 29);
			this.taskEditTabPage.Name = "taskEditTabPage";
			this.taskEditTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.taskEditTabPage.Size = new System.Drawing.Size(923, 511);
			this.taskEditTabPage.TabIndex = 0;
			this.taskEditTabPage.Text = "Редактирование занятий";
			this.taskEditTabPage.UseVisualStyleBackColor = true;
			// 
			// tName1
			// 
			this.tName1.AutoSize = true;
			this.tName1.Location = new System.Drawing.Point(3, 3);
			this.tName1.Name = "tName1";
			this.tName1.Size = new System.Drawing.Size(58, 20);
			this.tName1.TabIndex = 10;
			this.tName1.Text = "label11";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(408, 59);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(79, 20);
			this.label2.TabIndex = 9;
			this.label2.Text = "Описание";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(408, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(77, 20);
			this.label1.TabIndex = 8;
			this.label1.Text = "Название";
			// 
			// descriptionField
			// 
			this.descriptionField.Location = new System.Drawing.Point(493, 59);
			this.descriptionField.Multiline = true;
			this.descriptionField.Name = "descriptionField";
			this.descriptionField.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.descriptionField.Size = new System.Drawing.Size(424, 446);
			this.descriptionField.TabIndex = 7;
			this.descriptionField.TextChanged += new System.EventHandler(this.descriptionField_TextChanged);
			// 
			// nameField
			// 
			this.nameField.Location = new System.Drawing.Point(493, 26);
			this.nameField.Name = "nameField";
			this.nameField.Size = new System.Drawing.Size(424, 27);
			this.nameField.TabIndex = 6;
			this.nameField.TextChanged += new System.EventHandler(this.nameField_TextChanged);
			// 
			// cancelBtn
			// 
			this.cancelBtn.Location = new System.Drawing.Point(262, 253);
			this.cancelBtn.Name = "cancelBtn";
			this.cancelBtn.Size = new System.Drawing.Size(94, 29);
			this.cancelBtn.TabIndex = 5;
			this.cancelBtn.Text = "Отмена";
			this.cancelBtn.UseVisualStyleBackColor = true;
			this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
			// 
			// saveBtn
			// 
			this.saveBtn.Location = new System.Drawing.Point(262, 218);
			this.saveBtn.Name = "saveBtn";
			this.saveBtn.Size = new System.Drawing.Size(94, 29);
			this.saveBtn.TabIndex = 4;
			this.saveBtn.Text = "Сохранить";
			this.saveBtn.UseVisualStyleBackColor = true;
			this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
			// 
			// removeBtn
			// 
			this.removeBtn.Location = new System.Drawing.Point(262, 120);
			this.removeBtn.Name = "removeBtn";
			this.removeBtn.Size = new System.Drawing.Size(94, 29);
			this.removeBtn.TabIndex = 3;
			this.removeBtn.Text = "Удалить";
			this.removeBtn.UseVisualStyleBackColor = true;
			this.removeBtn.Click += new System.EventHandler(this.removeBtn_Click);
			// 
			// addBtn
			// 
			this.addBtn.Location = new System.Drawing.Point(262, 85);
			this.addBtn.Name = "addBtn";
			this.addBtn.Size = new System.Drawing.Size(94, 29);
			this.addBtn.TabIndex = 2;
			this.addBtn.Text = "Добавить";
			this.addBtn.UseVisualStyleBackColor = true;
			this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
			// 
			// treeView1
			// 
			this.treeView1.Location = new System.Drawing.Point(3, 26);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(253, 479);
			this.treeView1.TabIndex = 0;
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			this.treeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDown);
			// 
			// taskRatingTabPage
			// 
			this.taskRatingTabPage.Controls.Add(this.tName2);
			this.taskRatingTabPage.Controls.Add(this.label6);
			this.taskRatingTabPage.Controls.Add(this.label5);
			this.taskRatingTabPage.Controls.Add(this.label4);
			this.taskRatingTabPage.Controls.Add(this.label3);
			this.taskRatingTabPage.Controls.Add(this.commentField2);
			this.taskRatingTabPage.Controls.Add(this.markField2);
			this.taskRatingTabPage.Controls.Add(this.descriptionField2);
			this.taskRatingTabPage.Controls.Add(this.nameField2);
			this.taskRatingTabPage.Controls.Add(this.treeView2);
			this.taskRatingTabPage.Controls.Add(this.listBox2);
			this.taskRatingTabPage.Location = new System.Drawing.Point(4, 29);
			this.taskRatingTabPage.Name = "taskRatingTabPage";
			this.taskRatingTabPage.Size = new System.Drawing.Size(923, 511);
			this.taskRatingTabPage.TabIndex = 2;
			this.taskRatingTabPage.Text = "Оценивание";
			this.taskRatingTabPage.UseVisualStyleBackColor = true;
			// 
			// tName2
			// 
			this.tName2.AutoSize = true;
			this.tName2.Location = new System.Drawing.Point(3, 7);
			this.tName2.Name = "tName2";
			this.tName2.Size = new System.Drawing.Size(58, 20);
			this.tName2.TabIndex = 10;
			this.tName2.Text = "label11";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(469, 309);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(107, 20);
			this.label6.TabIndex = 9;
			this.label6.Text = "Комментарий";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(513, 276);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(61, 20);
			this.label5.TabIndex = 8;
			this.label5.Text = "Оценка";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(495, 63);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(79, 20);
			this.label4.TabIndex = 7;
			this.label4.Text = "Описание";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(499, 30);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(77, 20);
			this.label3.TabIndex = 6;
			this.label3.Text = "Название";
			// 
			// commentField2
			// 
			this.commentField2.Location = new System.Drawing.Point(580, 309);
			this.commentField2.Multiline = true;
			this.commentField2.Name = "commentField2";
			this.commentField2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.commentField2.Size = new System.Drawing.Size(340, 185);
			this.commentField2.TabIndex = 5;
			// 
			// markField2
			// 
			this.markField2.Location = new System.Drawing.Point(580, 276);
			this.markField2.Name = "markField2";
			this.markField2.Size = new System.Drawing.Size(125, 27);
			this.markField2.TabIndex = 4;
			// 
			// descriptionField2
			// 
			this.descriptionField2.Enabled = false;
			this.descriptionField2.Location = new System.Drawing.Point(580, 63);
			this.descriptionField2.Multiline = true;
			this.descriptionField2.Name = "descriptionField2";
			this.descriptionField2.ReadOnly = true;
			this.descriptionField2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.descriptionField2.Size = new System.Drawing.Size(340, 207);
			this.descriptionField2.TabIndex = 3;
			// 
			// nameField2
			// 
			this.nameField2.Enabled = false;
			this.nameField2.Location = new System.Drawing.Point(580, 30);
			this.nameField2.Name = "nameField2";
			this.nameField2.ReadOnly = true;
			this.nameField2.Size = new System.Drawing.Size(340, 27);
			this.nameField2.TabIndex = 2;
			// 
			// treeView2
			// 
			this.treeView2.Location = new System.Drawing.Point(242, 30);
			this.treeView2.Name = "treeView2";
			this.treeView2.Size = new System.Drawing.Size(220, 464);
			this.treeView2.TabIndex = 1;
			// 
			// listBox2
			// 
			this.listBox2.FormattingEnabled = true;
			this.listBox2.ItemHeight = 20;
			this.listBox2.Location = new System.Drawing.Point(3, 30);
			this.listBox2.Name = "listBox2";
			this.listBox2.Size = new System.Drawing.Size(233, 464);
			this.listBox2.TabIndex = 0;
			// 
			// taskAssignmentTabPage
			// 
			this.taskAssignmentTabPage.Controls.Add(this.tName3);
			this.taskAssignmentTabPage.Controls.Add(this.label10);
			this.taskAssignmentTabPage.Controls.Add(this.label9);
			this.taskAssignmentTabPage.Controls.Add(this.label8);
			this.taskAssignmentTabPage.Controls.Add(this.label7);
			this.taskAssignmentTabPage.Controls.Add(this.cancelBtn3);
			this.taskAssignmentTabPage.Controls.Add(this.saveBtn3);
			this.taskAssignmentTabPage.Controls.Add(this.descriptionField3);
			this.taskAssignmentTabPage.Controls.Add(this.nameField3);
			this.taskAssignmentTabPage.Controls.Add(this.checkedListBoxStudents3);
			this.taskAssignmentTabPage.Controls.Add(this.checkedListBoxGroups3);
			this.taskAssignmentTabPage.Controls.Add(this.treeView3);
			this.taskAssignmentTabPage.Location = new System.Drawing.Point(4, 29);
			this.taskAssignmentTabPage.Name = "taskAssignmentTabPage";
			this.taskAssignmentTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.taskAssignmentTabPage.Size = new System.Drawing.Size(923, 511);
			this.taskAssignmentTabPage.TabIndex = 1;
			this.taskAssignmentTabPage.Text = "Назначение занятий";
			this.taskAssignmentTabPage.UseVisualStyleBackColor = true;
			// 
			// tName3
			// 
			this.tName3.AutoSize = true;
			this.tName3.Location = new System.Drawing.Point(6, 10);
			this.tName3.Name = "tName3";
			this.tName3.Size = new System.Drawing.Size(58, 20);
			this.tName3.TabIndex = 11;
			this.tName3.Text = "label11";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(366, 317);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(79, 20);
			this.label10.TabIndex = 10;
			this.label10.Text = "Описание";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(367, 284);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(77, 20);
			this.label9.TabIndex = 9;
			this.label9.Text = "Название";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(367, 164);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(78, 20);
			this.label8.TabIndex = 8;
			this.label8.Text = "Учащиеся";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(384, 44);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(61, 20);
			this.label7.TabIndex = 7;
			this.label7.Text = "Группы";
			// 
			// cancelBtn3
			// 
			this.cancelBtn3.Location = new System.Drawing.Point(285, 89);
			this.cancelBtn3.Name = "cancelBtn3";
			this.cancelBtn3.Size = new System.Drawing.Size(94, 29);
			this.cancelBtn3.TabIndex = 6;
			this.cancelBtn3.Text = "Отмена";
			this.cancelBtn3.UseVisualStyleBackColor = true;
			// 
			// saveBtn3
			// 
			this.saveBtn3.Location = new System.Drawing.Point(285, 54);
			this.saveBtn3.Name = "saveBtn3";
			this.saveBtn3.Size = new System.Drawing.Size(94, 29);
			this.saveBtn3.TabIndex = 5;
			this.saveBtn3.Text = "Сохранить";
			this.saveBtn3.UseVisualStyleBackColor = true;
			// 
			// descriptionField3
			// 
			this.descriptionField3.Location = new System.Drawing.Point(451, 317);
			this.descriptionField3.Multiline = true;
			this.descriptionField3.Name = "descriptionField3";
			this.descriptionField3.ReadOnly = true;
			this.descriptionField3.Size = new System.Drawing.Size(466, 188);
			this.descriptionField3.TabIndex = 4;
			// 
			// nameField3
			// 
			this.nameField3.Location = new System.Drawing.Point(451, 284);
			this.nameField3.Name = "nameField3";
			this.nameField3.ReadOnly = true;
			this.nameField3.Size = new System.Drawing.Size(125, 27);
			this.nameField3.TabIndex = 3;
			// 
			// checkedListBoxStudents3
			// 
			this.checkedListBoxStudents3.CheckOnClick = true;
			this.checkedListBoxStudents3.FormattingEnabled = true;
			this.checkedListBoxStudents3.Location = new System.Drawing.Point(451, 164);
			this.checkedListBoxStudents3.Name = "checkedListBoxStudents3";
			this.checkedListBoxStudents3.Size = new System.Drawing.Size(466, 114);
			this.checkedListBoxStudents3.TabIndex = 2;
			// 
			// checkedListBoxGroups3
			// 
			this.checkedListBoxGroups3.CheckOnClick = true;
			this.checkedListBoxGroups3.FormattingEnabled = true;
			this.checkedListBoxGroups3.Location = new System.Drawing.Point(451, 44);
			this.checkedListBoxGroups3.Name = "checkedListBoxGroups3";
			this.checkedListBoxGroups3.Size = new System.Drawing.Size(466, 114);
			this.checkedListBoxGroups3.TabIndex = 1;
			// 
			// treeView3
			// 
			this.treeView3.Location = new System.Drawing.Point(6, 33);
			this.treeView3.Name = "treeView3";
			this.treeView3.Size = new System.Drawing.Size(273, 472);
			this.treeView3.TabIndex = 0;
			// 
			// logOutBtn
			// 
			this.logOutBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("logOutBtn.BackgroundImage")));
			this.logOutBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.logOutBtn.Location = new System.Drawing.Point(909, 12);
			this.logOutBtn.Name = "logOutBtn";
			this.logOutBtn.Size = new System.Drawing.Size(30, 29);
			this.logOutBtn.TabIndex = 1;
			this.logOutBtn.UseVisualStyleBackColor = true;
			this.logOutBtn.Click += new System.EventHandler(this.logOutBtn_Click);
			// 
			// TeacherForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(955, 586);
			this.Controls.Add(this.logOutBtn);
			this.Controls.Add(this.tabMenu);
			this.Name = "TeacherForm";
			this.Text = "TeacherForm";
			this.tabMenu.ResumeLayout(false);
			this.taskEditTabPage.ResumeLayout(false);
			this.taskEditTabPage.PerformLayout();
			this.taskRatingTabPage.ResumeLayout(false);
			this.taskRatingTabPage.PerformLayout();
			this.taskAssignmentTabPage.ResumeLayout(false);
			this.taskAssignmentTabPage.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private TabControl tabMenu;
		private TabPage taskEditTabPage;
		private TabPage taskAssignmentTabPage;
		private TreeView treeView1;
		private Button addBtn;
		private Button removeBtn;
		private Button saveBtn;
		private Button cancelBtn;
		private TextBox nameField;
		private TextBox descriptionField;
		private TabPage taskRatingTabPage;
		private ListBox listBox2;
		private TreeView treeView2;
		private TextBox nameField2;
		private TextBox descriptionField2;
		private TextBox markField2;
		private TextBox commentField2;
		private TreeView treeView3;
		private CheckedListBox checkedListBoxGroups3;
		private CheckedListBox checkedListBoxStudents3;
		private TextBox nameField3;
		private TextBox descriptionField3;
		private Button saveBtn3;
		private Button cancelBtn3;
		private Label label1;
		private Label label2;
		private Label label3;
		private Label label4;
		private Label label5;
		private Label label6;
		private Label label8;
		private Label label7;
		private Label label9;
		private Label label10;
		private Label tName1;
		private Label tName2;
		private Label tName3;
		private Button logOutBtn;
	}
}