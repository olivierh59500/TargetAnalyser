﻿namespace TargetAnalyser
{
    partial class FormAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.linkEmail = new System.Windows.Forms.LinkLabel();
            this.linkWeb = new System.Windows.Forms.LinkLabel();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblVer = new System.Windows.Forms.Label();
            this.lblApp = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.linkIcons8 = new System.Windows.Forms.LinkLabel();
            this.labelIconsBy = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.White;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(543, 137);
            this.pictureBox2.TabIndex = 26;
            this.pictureBox2.TabStop = false;
            // 
            // linkEmail
            // 
            this.linkEmail.AutoSize = true;
            this.linkEmail.Location = new System.Drawing.Point(187, 174);
            this.linkEmail.Name = "linkEmail";
            this.linkEmail.Size = new System.Drawing.Size(169, 20);
            this.linkEmail.TabIndex = 25;
            this.linkEmail.TabStop = true;
            this.linkEmail.Text = "markwoan@gmail.com";
            this.linkEmail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkEmail_LinkClicked);
            // 
            // linkWeb
            // 
            this.linkWeb.AutoSize = true;
            this.linkWeb.Location = new System.Drawing.Point(187, 154);
            this.linkWeb.Name = "linkWeb";
            this.linkWeb.Size = new System.Drawing.Size(163, 20);
            this.linkWeb.TabIndex = 24;
            this.linkWeb.TabStop = true;
            this.linkWeb.Text = "github.com/woanware";
            this.linkWeb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkWeb_LinkClicked);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(422, 257);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(108, 32);
            this.btnClose.TabIndex = 23;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblVer
            // 
            this.lblVer.AutoSize = true;
            this.lblVer.BackColor = System.Drawing.Color.White;
            this.lblVer.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVer.Location = new System.Drawing.Point(446, 58);
            this.lblVer.Name = "lblVer";
            this.lblVer.Size = new System.Drawing.Size(81, 25);
            this.lblVer.TabIndex = 29;
            this.lblVer.Text = "v1.0.0";
            // 
            // lblApp
            // 
            this.lblApp.AutoSize = true;
            this.lblApp.BackColor = System.Drawing.Color.White;
            this.lblApp.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApp.Location = new System.Drawing.Point(117, 44);
            this.lblApp.Name = "lblApp";
            this.lblApp.Size = new System.Drawing.Size(328, 44);
            this.lblApp.TabIndex = 28;
            this.lblApp.Text = "TargetAnalyser";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(13, 23);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(111, 91);
            this.pictureBox1.TabIndex = 27;
            this.pictureBox1.TabStop = false;
            // 
            // linkIcons8
            // 
            this.linkIcons8.AutoSize = true;
            this.linkIcons8.Location = new System.Drawing.Point(258, 227);
            this.linkIcons8.Name = "linkIcons8";
            this.linkIcons8.Size = new System.Drawing.Size(89, 20);
            this.linkIcons8.TabIndex = 30;
            this.linkIcons8.TabStop = true;
            this.linkIcons8.Text = "icons8.com";
            this.linkIcons8.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkIcons8_LinkClicked);
            // 
            // labelIconsBy
            // 
            this.labelIconsBy.AutoSize = true;
            this.labelIconsBy.Location = new System.Drawing.Point(187, 227);
            this.labelIconsBy.Name = "labelIconsBy";
            this.labelIconsBy.Size = new System.Drawing.Size(68, 20);
            this.labelIconsBy.TabIndex = 31;
            this.labelIconsBy.Text = "Icons by";
            // 
            // FormAbout
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(543, 299);
            this.Controls.Add(this.labelIconsBy);
            this.Controls.Add(this.linkIcons8);
            this.Controls.Add(this.lblVer);
            this.Controls.Add(this.lblApp);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.linkEmail);
            this.Controls.Add(this.linkWeb);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAbout";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.LinkLabel linkEmail;
        private System.Windows.Forms.LinkLabel linkWeb;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblVer;
        private System.Windows.Forms.Label lblApp;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel linkIcons8;
        private System.Windows.Forms.Label labelIconsBy;
    }
}