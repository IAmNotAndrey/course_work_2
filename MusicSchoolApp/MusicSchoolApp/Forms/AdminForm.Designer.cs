﻿namespace MusicSchoolApp.Forms
{
	partial class AdminForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminForm));
			this.tabMenu = new System.Windows.Forms.TabControl();
			this.usersTabPage = new System.Windows.Forms.TabPage();
			this.groupsTabPage = new System.Windows.Forms.TabPage();
			this.studentsGroupsTabPage = new System.Windows.Forms.TabPage();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			this.tabMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// tabMenu
			// 
			this.tabMenu.Controls.Add(this.usersTabPage);
			this.tabMenu.Controls.Add(this.groupsTabPage);
			this.tabMenu.Controls.Add(this.studentsGroupsTabPage);
			this.tabMenu.Location = new System.Drawing.Point(12, 32);
			this.tabMenu.Name = "tabMenu";
			this.tabMenu.SelectedIndex = 0;
			this.tabMenu.Size = new System.Drawing.Size(776, 30);
			this.tabMenu.TabIndex = 0;
			// 
			// usersTabPage
			// 
			this.usersTabPage.Location = new System.Drawing.Point(4, 29);
			this.usersTabPage.Name = "usersTabPage";
			this.usersTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.usersTabPage.Size = new System.Drawing.Size(768, 0);
			this.usersTabPage.TabIndex = 0;
			this.usersTabPage.Text = "Пользователи";
			this.usersTabPage.UseVisualStyleBackColor = true;
			// 
			// groupsTabPage
			// 
			this.groupsTabPage.Location = new System.Drawing.Point(4, 29);
			this.groupsTabPage.Name = "groupsTabPage";
			this.groupsTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.groupsTabPage.Size = new System.Drawing.Size(768, 0);
			this.groupsTabPage.TabIndex = 1;
			this.groupsTabPage.Text = "Группы";
			this.groupsTabPage.UseVisualStyleBackColor = true;
			// 
			// studentsGroupsTabPage
			// 
			this.studentsGroupsTabPage.Location = new System.Drawing.Point(4, 29);
			this.studentsGroupsTabPage.Name = "studentsGroupsTabPage";
			this.studentsGroupsTabPage.Size = new System.Drawing.Size(768, 0);
			this.studentsGroupsTabPage.TabIndex = 2;
			this.studentsGroupsTabPage.Text = "Группы-ученики";
			this.studentsGroupsTabPage.UseVisualStyleBackColor = true;
			// 
			// dataGridView1
			// 
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Location = new System.Drawing.Point(12, 68);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowHeadersWidth = 51;
			this.dataGridView1.RowTemplate.Height = 29;
			this.dataGridView1.Size = new System.Drawing.Size(673, 390);
			this.dataGridView1.TabIndex = 1;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(694, 236);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(94, 29);
			this.button1.TabIndex = 2;
			this.button1.Text = "Удалить";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(694, 201);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(94, 29);
			this.button2.TabIndex = 3;
			this.button2.Text = "Добавить";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(694, 68);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(94, 29);
			this.button3.TabIndex = 4;
			this.button3.Text = "Сохранить";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(694, 103);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(94, 29);
			this.button4.TabIndex = 5;
			this.button4.Text = "Отмена";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// button5
			// 
			this.button5.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button5.BackgroundImage")));
			this.button5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.button5.Location = new System.Drawing.Point(754, 12);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(27, 29);
			this.button5.TabIndex = 6;
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// AdminForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 477);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.tabMenu);
			this.Name = "AdminForm";
			this.Text = "AdminForm";
			this.tabMenu.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private TabControl tabMenu;
		private TabPage usersTabPage;
		private TabPage groupsTabPage;
		private TabPage studentsGroupsTabPage;
		private DataGridView dataGridView1;
		private Button button1;
		private Button button2;
		private Button button3;
		private Button button4;
		private Button button5;
	}
}