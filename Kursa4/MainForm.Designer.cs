
namespace _5Sem_Kursa4
{

    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.skip1 = new System.Windows.Forms.Button();
            this.skip2 = new System.Windows.Forms.Button();
            this.RoundLabel = new System.Windows.Forms.Label();
            this.TimeLabel = new System.Windows.Forms.Label();
            this.NumberOutCards1Label = new System.Windows.Forms.Label();
            this.NumberOutCards2Label = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1000, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // gameToolStripMenuItem
            // 
            this.gameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            this.gameToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.gameToolStripMenuItem.Text = "Game";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::_5Sem_Kursa4.Properties.Resources.BackgroundImage1;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(0, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1000, 664);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // skip1
            // 
            this.skip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(255)))), ((int)(((byte)(112)))));
            this.skip1.Location = new System.Drawing.Point(92, 511);
            this.skip1.Name = "skip1";
            this.skip1.Size = new System.Drawing.Size(75, 30);
            this.skip1.TabIndex = 4;
            this.skip1.Text = "Cancel";
            this.skip1.UseVisualStyleBackColor = false;
            this.skip1.Click += new System.EventHandler(this.skip1_Click);
            // 
            // skip2
            // 
            this.skip2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(255)))), ((int)(((byte)(112)))));
            this.skip2.Location = new System.Drawing.Point(92, 177);
            this.skip2.Name = "skip2";
            this.skip2.Size = new System.Drawing.Size(75, 30);
            this.skip2.TabIndex = 5;
            this.skip2.Text = "Cancel";
            this.skip2.UseVisualStyleBackColor = false;
            this.skip2.Visible = false;
            this.skip2.Click += new System.EventHandler(this.skip2_Click);
            // 
            // RoundLabel
            // 
            this.RoundLabel.AutoSize = true;
            this.RoundLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(255)))), ((int)(((byte)(112)))));
            this.RoundLabel.Font = new System.Drawing.Font("Times New Roman", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RoundLabel.Location = new System.Drawing.Point(8, 37);
            this.RoundLabel.Name = "RoundLabel";
            this.RoundLabel.Size = new System.Drawing.Size(0, 40);
            this.RoundLabel.TabIndex = 6;
            // 
            // TimeLabel
            // 
            this.TimeLabel.AutoSize = true;
            this.TimeLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(255)))), ((int)(((byte)(112)))));
            this.TimeLabel.Font = new System.Drawing.Font("Times New Roman", 26.25F, System.Drawing.FontStyle.Bold);
            this.TimeLabel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.TimeLabel.Location = new System.Drawing.Point(882, 37);
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.Size = new System.Drawing.Size(53, 40);
            this.TimeLabel.TabIndex = 7;
            this.TimeLabel.Text = "lol";
            // 
            // NumberOutCards1Label
            // 
            this.NumberOutCards1Label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(255)))), ((int)(((byte)(112)))));
            this.NumberOutCards1Label.Font = new System.Drawing.Font("Times New Roman", 26.25F, System.Drawing.FontStyle.Bold);
            this.NumberOutCards1Label.Location = new System.Drawing.Point(882, 367);
            this.NumberOutCards1Label.Name = "NumberOutCards1Label";
            this.NumberOutCards1Label.Size = new System.Drawing.Size(106, 40);
            this.NumberOutCards1Label.TabIndex = 8;
            this.NumberOutCards1Label.Text = "label1";
            this.NumberOutCards1Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // NumberOutCards2Label
            // 
            this.NumberOutCards2Label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(255)))), ((int)(((byte)(112)))));
            this.NumberOutCards2Label.Font = new System.Drawing.Font("Times New Roman", 26.25F, System.Drawing.FontStyle.Bold);
            this.NumberOutCards2Label.Location = new System.Drawing.Point(882, 308);
            this.NumberOutCards2Label.Name = "NumberOutCards2Label";
            this.NumberOutCards2Label.Size = new System.Drawing.Size(106, 40);
            this.NumberOutCards2Label.TabIndex = 9;
            this.NumberOutCards2Label.Text = "label1";
            this.NumberOutCards2Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 688);
            this.Controls.Add(this.NumberOutCards2Label);
            this.Controls.Add(this.NumberOutCards1Label);
            this.Controls.Add(this.TimeLabel);
            this.Controls.Add(this.RoundLabel);
            this.Controls.Add(this.skip2);
            this.Controls.Add(this.skip1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(1016, 726);
            this.MinimumSize = new System.Drawing.Size(1016, 726);
            this.Name = "MainForm";
            this.Text = "Card game \"Football\"";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem gameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button skip1;
        private System.Windows.Forms.Button skip2;
        private System.Windows.Forms.Label RoundLabel;
        private System.Windows.Forms.Label TimeLabel;
        private System.Windows.Forms.Label NumberOutCards1Label;
        private System.Windows.Forms.Label NumberOutCards2Label;

    }
}


