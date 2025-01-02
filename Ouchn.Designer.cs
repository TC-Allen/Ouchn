using Microsoft.VisualBasic.ApplicationServices;
using Ouchn.Util;
using System.ComponentModel;

namespace Ouchn
{
    partial class Ouchn
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
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Ouchn));
            label1 = new Label();
            strCourseid = new TextBox();
            label2 = new Label();
            strCookie = new TextBox();
            btnStart = new Button();
            btnStop = new Button();
            rtb = new RichTextBox();
            cms = new ContextMenuStrip(components);
            exitToolStripMenuItem = new ToolStripMenuItem();
            btnClean = new Button();
            speedInt = new TextBox();
            label3 = new Label();
            cms.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // strCourseid
            // 
            resources.ApplyResources(strCourseid, "strCourseid");
            strCourseid.CausesValidation = false;
            strCourseid.Name = "strCourseid";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // strCookie
            // 
            resources.ApplyResources(strCookie, "strCookie");
            strCookie.CausesValidation = false;
            strCookie.Name = "strCookie";
            // 
            // btnStart
            // 
            resources.ApplyResources(btnStart, "btnStart");
            btnStart.Name = "btnStart";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            resources.ApplyResources(btnStop, "btnStop");
            btnStop.Name = "btnStop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // rtb
            // 
            resources.ApplyResources(rtb, "rtb");
            rtb.Name = "rtb";
            rtb.ReadOnly = true;
            // 
            // cms
            // 
            resources.ApplyResources(cms, "cms");
            cms.ImageScalingSize = new Size(20, 20);
            cms.Items.AddRange(new ToolStripItem[] { exitToolStripMenuItem });
            cms.Name = "contextMenuStrip1";
            // 
            // exitToolStripMenuItem
            // 
            resources.ApplyResources(exitToolStripMenuItem, "exitToolStripMenuItem");
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Click += ExitItemClick;
            // 
            // btnClean
            // 
            resources.ApplyResources(btnClean, "btnClean");
            btnClean.Name = "btnClean";
            btnClean.UseVisualStyleBackColor = true;
            btnClean.Click += btnClean_Click;
            // 
            // speedInt
            // 
            resources.ApplyResources(speedInt, "speedInt");
            speedInt.CausesValidation = false;
            speedInt.Name = "speedInt";
            speedInt.KeyPress += speedInt_KeyPress;
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // Ouchn
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(label3);
            Controls.Add(speedInt);
            Controls.Add(btnClean);
            Controls.Add(rtb);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Controls.Add(strCookie);
            Controls.Add(label2);
            Controls.Add(strCourseid);
            Controls.Add(label1);
            HelpButton = true;
            MaximizeBox = false;
            Name = "Ouchn";
            FormClosing += Ouchn_FormClosing;
            Load += Ouchn_Load;
            cms.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox strCourseid;
        private Label label2;
        private TextBox strCookie;
        private Button btnStart;
        private Button btnStop;
        private RichTextBox rtb;
        private ContextMenuStrip cms;
        private ToolStripMenuItem exitToolStripMenuItem;
        private Button btnClean;
        private TextBox speedInt;
        private Label label3;
    }
}
