namespace MusicSchoolApp.Lab3Task
{
	partial class TaskInputForm
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
			this.okBtn = new System.Windows.Forms.Button();
			this.cancelBtn = new System.Windows.Forms.Button();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.markField = new System.Windows.Forms.TextBox();
			this.commentField = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// okBtn
			// 
			this.okBtn.Location = new System.Drawing.Point(112, 256);
			this.okBtn.Name = "okBtn";
			this.okBtn.Size = new System.Drawing.Size(94, 29);
			this.okBtn.TabIndex = 0;
			this.okBtn.Text = "ОК";
			this.okBtn.UseVisualStyleBackColor = true;
			this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
			// 
			// cancelBtn
			// 
			this.cancelBtn.Location = new System.Drawing.Point(212, 256);
			this.cancelBtn.Name = "cancelBtn";
			this.cancelBtn.Size = new System.Drawing.Size(94, 29);
			this.cancelBtn.TabIndex = 1;
			this.cancelBtn.Text = "Отмена";
			this.cancelBtn.UseVisualStyleBackColor = true;
			this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(12, 12);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(294, 28);
			this.comboBox1.TabIndex = 2;
			// 
			// markField
			// 
			this.markField.Location = new System.Drawing.Point(12, 46);
			this.markField.Name = "markField";
			this.markField.PlaceholderText = "Оценка";
			this.markField.Size = new System.Drawing.Size(125, 27);
			this.markField.TabIndex = 3;
			// 
			// commentField
			// 
			this.commentField.Location = new System.Drawing.Point(12, 79);
			this.commentField.Multiline = true;
			this.commentField.Name = "commentField";
			this.commentField.PlaceholderText = "Комментарий";
			this.commentField.Size = new System.Drawing.Size(294, 171);
			this.commentField.TabIndex = 4;
			// 
			// TaskInputForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(318, 297);
			this.Controls.Add(this.commentField);
			this.Controls.Add(this.markField);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.cancelBtn);
			this.Controls.Add(this.okBtn);
			this.Name = "TaskInputForm";
			this.Text = "TaskImportForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Button okBtn;
		private Button cancelBtn;
		private ComboBox comboBox1;
		private TextBox markField;
		private TextBox commentField;
	}
}