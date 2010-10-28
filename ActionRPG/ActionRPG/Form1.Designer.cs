namespace ActionRPG
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.AnimationWindow = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.AnimationWindow)).BeginInit();
            this.SuspendLayout();
            // 
            // AnimationWindow
            // 
            this.AnimationWindow.BackColor = System.Drawing.SystemColors.ControlLight;
            this.AnimationWindow.Location = new System.Drawing.Point(47, 33);
            this.AnimationWindow.Name = "AnimationWindow";
            this.AnimationWindow.Size = new System.Drawing.Size(800, 800);
            this.AnimationWindow.TabIndex = 0;
            this.AnimationWindow.TabStop = false;
            this.AnimationWindow.Paint += new System.Windows.Forms.PaintEventHandler(this.AnimationWindow_Paint);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 40;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.ClientSize = new System.Drawing.Size(892, 866);
            this.Controls.Add(this.AnimationWindow);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.AnimationWindow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox AnimationWindow;
        private System.Windows.Forms.Timer timer1;
    }
}

