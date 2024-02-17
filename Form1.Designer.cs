
namespace cosmosDatalog
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this._serialPort = new System.IO.Ports.SerialPort(this.components);
            this.bn_Exit = new System.Windows.Forms.Button();
            this.bn_Stop = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pBox_Run = new System.Windows.Forms.PictureBox();
            this.lbRunStop = new System.Windows.Forms.Label();
            this.lbLogFileName = new System.Windows.Forms.Label();
            this.tbToggle = new System.Windows.Forms.TextBox();
            this.rtbStatus = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbPort = new System.Windows.Forms.ComboBox();
            this.bn_Start = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBox_Run)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 2000;
            // 
            // bn_Exit
            // 
            this.bn_Exit.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.bn_Exit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bn_Exit.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bn_Exit.Image = global::N530Datalog.Properties.Resources.exit;
            this.bn_Exit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.bn_Exit.Location = new System.Drawing.Point(206, 234);
            this.bn_Exit.Name = "bn_Exit";
            this.bn_Exit.Size = new System.Drawing.Size(75, 78);
            this.bn_Exit.TabIndex = 23;
            this.bn_Exit.Text = "Exit";
            this.bn_Exit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.bn_Exit.UseVisualStyleBackColor = false;
            this.bn_Exit.Click += new System.EventHandler(this.bn_Exit_Click);
            // 
            // bn_Stop
            // 
            this.bn_Stop.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.bn_Stop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bn_Stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bn_Stop.Image = global::N530Datalog.Properties.Resources.stop;
            this.bn_Stop.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.bn_Stop.Location = new System.Drawing.Point(125, 234);
            this.bn_Stop.Name = "bn_Stop";
            this.bn_Stop.Size = new System.Drawing.Size(75, 78);
            this.bn_Stop.TabIndex = 22;
            this.bn_Stop.Text = "Stop";
            this.bn_Stop.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.bn_Stop.UseVisualStyleBackColor = false;
            this.bn_Stop.Click += new System.EventHandler(this.bn_Stop_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.groupBox1.Controls.Add(this.pBox_Run);
            this.groupBox1.Controls.Add(this.lbRunStop);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.groupBox1.Location = new System.Drawing.Point(72, 81);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(163, 127);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SYSTEM";
            // 
            // pBox_Run
            // 
            this.pBox_Run.BackColor = System.Drawing.Color.Red;
            this.pBox_Run.Location = new System.Drawing.Point(30, 60);
            this.pBox_Run.Name = "pBox_Run";
            this.pBox_Run.Size = new System.Drawing.Size(100, 50);
            this.pBox_Run.TabIndex = 2;
            this.pBox_Run.TabStop = false;
            // 
            // lbRunStop
            // 
            this.lbRunStop.AutoSize = true;
            this.lbRunStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbRunStop.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbRunStop.Location = new System.Drawing.Point(39, 28);
            this.lbRunStop.Name = "lbRunStop";
            this.lbRunStop.Size = new System.Drawing.Size(75, 20);
            this.lbRunStop.TabIndex = 1;
            this.lbRunStop.Text = "  STOP  ";
            // 
            // lbLogFileName
            // 
            this.lbLogFileName.AutoSize = true;
            this.lbLogFileName.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lbLogFileName.Location = new System.Drawing.Point(305, 28);
            this.lbLogFileName.Name = "lbLogFileName";
            this.lbLogFileName.Size = new System.Drawing.Size(43, 13);
            this.lbLogFileName.TabIndex = 19;
            this.lbLogFileName.Text = "Status :";
            // 
            // tbToggle
            // 
            this.tbToggle.Location = new System.Drawing.Point(361, 25);
            this.tbToggle.Name = "tbToggle";
            this.tbToggle.Size = new System.Drawing.Size(19, 20);
            this.tbToggle.TabIndex = 18;
            // 
            // rtbStatus
            // 
            this.rtbStatus.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.rtbStatus.Location = new System.Drawing.Point(305, 51);
            this.rtbStatus.Name = "rtbStatus";
            this.rtbStatus.Size = new System.Drawing.Size(262, 261);
            this.rtbStatus.TabIndex = 17;
            this.rtbStatus.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(42, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Port :";
            // 
            // cbPort
            // 
            this.cbPort.FormattingEnabled = true;
            this.cbPort.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9",
            "COM10"});
            this.cbPort.Location = new System.Drawing.Point(77, 29);
            this.cbPort.Name = "cbPort";
            this.cbPort.Size = new System.Drawing.Size(121, 21);
            this.cbPort.TabIndex = 15;
            this.cbPort.SelectedIndexChanged += new System.EventHandler(this.cbPort_SelectedIndexChanged);
            // 
            // bn_Start
            // 
            this.bn_Start.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.bn_Start.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bn_Start.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bn_Start.Image = global::N530Datalog.Properties.Resources.start;
            this.bn_Start.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.bn_Start.Location = new System.Drawing.Point(44, 234);
            this.bn_Start.Name = "bn_Start";
            this.bn_Start.Size = new System.Drawing.Size(75, 78);
            this.bn_Start.TabIndex = 21;
            this.bn_Start.Text = "Start";
            this.bn_Start.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.bn_Start.UseVisualStyleBackColor = false;
            this.bn_Start.Click += new System.EventHandler(this.bn_Start_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button1.Location = new System.Drawing.Point(498, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(60, 41);
            this.button1.TabIndex = 24;
            this.button1.Text = "CLEAR";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.ClientSize = new System.Drawing.Size(627, 370);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.bn_Exit);
            this.Controls.Add(this.bn_Stop);
            this.Controls.Add(this.bn_Start);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbLogFileName);
            this.Controls.Add(this.tbToggle);
            this.Controls.Add(this.rtbStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbPort);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "N530 DATA LOGGER V1.0";
            this.Shown += new System.EventHandler(this.bn_Start_Click);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBox_Run)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.IO.Ports.SerialPort _serialPort;
        private System.Windows.Forms.Button bn_Exit;
        private System.Windows.Forms.Button bn_Stop;
        private System.Windows.Forms.Button bn_Start;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pBox_Run;
        private System.Windows.Forms.Label lbRunStop;
        private System.Windows.Forms.Label lbLogFileName;
        private System.Windows.Forms.TextBox tbToggle;
        private System.Windows.Forms.RichTextBox rtbStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbPort;
        private System.Windows.Forms.Button button1;
    }
}

