/*
 * Created by SharpDevelop.
 * User: sfmoo
 * Date: 19/03/2020
 * Time: 20:30
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace OneNightClient
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.btnReady = new System.Windows.Forms.Button();
            this.lblSetupState = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.listRoles = new System.Windows.Forms.CheckedListBox();
            this.lblLeft = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblImage = new System.Windows.Forms.Label();
            this.btnInputM1 = new System.Windows.Forms.Button();
            this.btnInputM2 = new System.Windows.Forms.Button();
            this.btnInputM3 = new System.Windows.Forms.Button();
            this.btnInput0 = new System.Windows.Forms.Button();
            this.btnInput1 = new System.Windows.Forms.Button();
            this.btnInput2 = new System.Windows.Forms.Button();
            this.btnInput3 = new System.Windows.Forms.Button();
            this.btnInput4 = new System.Windows.Forms.Button();
            this.btnInput5 = new System.Windows.Forms.Button();
            this.btnInput6 = new System.Windows.Forms.Button();
            this.btnInput7 = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblDisplay0 = new System.Windows.Forms.Label();
            this.lblDisplay1 = new System.Windows.Forms.Label();
            this.lblDisplay2 = new System.Windows.Forms.Label();
            this.lblDisplay3 = new System.Windows.Forms.Label();
            this.lblIP = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnect.Location = new System.Drawing.Point(150, 64);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(184, 50);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.BtnConnectClick);
            // 
            // txtIP
            // 
            this.txtIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIP.Location = new System.Drawing.Point(121, 35);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(328, 20);
            this.txtIP.TabIndex = 0;
            // 
            // btnReady
            // 
            this.btnReady.Enabled = false;
            this.btnReady.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReady.Location = new System.Drawing.Point(150, 64);
            this.btnReady.Name = "btnReady";
            this.btnReady.Size = new System.Drawing.Size(184, 50);
            this.btnReady.TabIndex = 2;
            this.btnReady.Text = "Ready";
            this.btnReady.UseVisualStyleBackColor = true;
            this.btnReady.Visible = false;
            this.btnReady.Click += new System.EventHandler(this.BtnReadyClick);
            // 
            // lblSetupState
            // 
            this.lblSetupState.BackColor = System.Drawing.Color.Transparent;
            this.lblSetupState.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSetupState.Location = new System.Drawing.Point(53, 117);
            this.lblSetupState.Name = "lblSetupState";
            this.lblSetupState.Size = new System.Drawing.Size(376, 77);
            this.lblSetupState.TabIndex = 3;
            this.lblSetupState.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Tick += new System.EventHandler(this.TimerTick);
            // 
            // listRoles
            // 
            this.listRoles.BackColor = System.Drawing.Color.Silver;
            this.listRoles.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listRoles.CheckOnClick = true;
            this.listRoles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listRoles.FormattingEnabled = true;
            this.listRoles.Items.AddRange(new object[] {
            "Werewolf",
            "Werewolf",
            "Minion",
            "Doppelgänger",
            "Mason",
            "Mason",
            "Seer",
            "Robber",
            "Troublemaker",
            "Drunk",
            "Insomniac",
            "Hunter",
            "Tanner",
            "Villager",
            "Villager",
            "Villager"});
            this.listRoles.Location = new System.Drawing.Point(53, 197);
            this.listRoles.MultiColumn = true;
            this.listRoles.Name = "listRoles";
            this.listRoles.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.listRoles.Size = new System.Drawing.Size(244, 180);
            this.listRoles.TabIndex = 4;
            this.listRoles.ThreeDCheckBoxes = true;
            this.listRoles.Visible = false;
            this.listRoles.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ListRolesItemCheck);
            // 
            // lblLeft
            // 
            this.lblLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLeft.Location = new System.Drawing.Point(303, 197);
            this.lblLeft.Name = "lblLeft";
            this.lblLeft.Size = new System.Drawing.Size(126, 180);
            this.lblLeft.TabIndex = 5;
            this.lblLeft.Text = "Left to Choose:";
            this.lblLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLeft.Visible = false;
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(150, 64);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(184, 50);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start Game";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Visible = false;
            this.btnStart.Click += new System.EventHandler(this.BtnStartClick);
            // 
            // lblImage
            // 
            this.lblImage.BackColor = System.Drawing.Color.Transparent;
            this.lblImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImage.ForeColor = System.Drawing.Color.Black;
            this.lblImage.Image = ((System.Drawing.Image)(resources.GetObject("lblImage.Image")));
            this.lblImage.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblImage.Location = new System.Drawing.Point(116, 197);
            this.lblImage.Name = "lblImage";
            this.lblImage.Size = new System.Drawing.Size(259, 290);
            this.lblImage.TabIndex = 100;
            this.lblImage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblImage.Visible = false;
            // 
            // btnInputM1
            // 
            this.btnInputM1.Location = new System.Drawing.Point(141, 206);
            this.btnInputM1.Name = "btnInputM1";
            this.btnInputM1.Size = new System.Drawing.Size(64, 86);
            this.btnInputM1.TabIndex = 8;
            this.btnInputM1.Text = "Middle 1";
            this.btnInputM1.UseVisualStyleBackColor = true;
            this.btnInputM1.Visible = false;
            this.btnInputM1.Click += new System.EventHandler(this.InputClick);
            // 
            // btnInputM2
            // 
            this.btnInputM2.Location = new System.Drawing.Point(211, 206);
            this.btnInputM2.Name = "btnInputM2";
            this.btnInputM2.Size = new System.Drawing.Size(64, 86);
            this.btnInputM2.TabIndex = 9;
            this.btnInputM2.Text = "Middle 2";
            this.btnInputM2.UseVisualStyleBackColor = true;
            this.btnInputM2.Visible = false;
            this.btnInputM2.Click += new System.EventHandler(this.InputClick);
            // 
            // btnInputM3
            // 
            this.btnInputM3.Location = new System.Drawing.Point(281, 206);
            this.btnInputM3.Name = "btnInputM3";
            this.btnInputM3.Size = new System.Drawing.Size(64, 86);
            this.btnInputM3.TabIndex = 10;
            this.btnInputM3.Text = "Middle 3";
            this.btnInputM3.UseVisualStyleBackColor = true;
            this.btnInputM3.Visible = false;
            this.btnInputM3.Click += new System.EventHandler(this.InputClick);
            // 
            // btnInput0
            // 
            this.btnInput0.Location = new System.Drawing.Point(141, 298);
            this.btnInput0.Name = "btnInput0";
            this.btnInput0.Size = new System.Drawing.Size(96, 45);
            this.btnInput0.TabIndex = 11;
            this.btnInput0.Text = "Player0";
            this.btnInput0.UseVisualStyleBackColor = true;
            this.btnInput0.Visible = false;
            this.btnInput0.Click += new System.EventHandler(this.InputClick);
            // 
            // btnInput1
            // 
            this.btnInput1.Location = new System.Drawing.Point(141, 349);
            this.btnInput1.Name = "btnInput1";
            this.btnInput1.Size = new System.Drawing.Size(96, 45);
            this.btnInput1.TabIndex = 12;
            this.btnInput1.Text = "Player1";
            this.btnInput1.UseVisualStyleBackColor = true;
            this.btnInput1.Visible = false;
            this.btnInput1.Click += new System.EventHandler(this.InputClick);
            // 
            // btnInput2
            // 
            this.btnInput2.Location = new System.Drawing.Point(141, 400);
            this.btnInput2.Name = "btnInput2";
            this.btnInput2.Size = new System.Drawing.Size(96, 45);
            this.btnInput2.TabIndex = 13;
            this.btnInput2.Text = "Player2";
            this.btnInput2.UseVisualStyleBackColor = true;
            this.btnInput2.Visible = false;
            this.btnInput2.Click += new System.EventHandler(this.InputClick);
            // 
            // btnInput3
            // 
            this.btnInput3.Location = new System.Drawing.Point(141, 451);
            this.btnInput3.Name = "btnInput3";
            this.btnInput3.Size = new System.Drawing.Size(96, 45);
            this.btnInput3.TabIndex = 14;
            this.btnInput3.Text = "Player3";
            this.btnInput3.UseVisualStyleBackColor = true;
            this.btnInput3.Visible = false;
            this.btnInput3.Click += new System.EventHandler(this.InputClick);
            // 
            // btnInput4
            // 
            this.btnInput4.Location = new System.Drawing.Point(248, 298);
            this.btnInput4.Name = "btnInput4";
            this.btnInput4.Size = new System.Drawing.Size(96, 45);
            this.btnInput4.TabIndex = 17;
            this.btnInput4.Text = "Player4";
            this.btnInput4.UseVisualStyleBackColor = true;
            this.btnInput4.Visible = false;
            this.btnInput4.Click += new System.EventHandler(this.InputClick);
            // 
            // btnInput5
            // 
            this.btnInput5.Location = new System.Drawing.Point(248, 349);
            this.btnInput5.Name = "btnInput5";
            this.btnInput5.Size = new System.Drawing.Size(96, 45);
            this.btnInput5.TabIndex = 18;
            this.btnInput5.Text = "Player5";
            this.btnInput5.UseVisualStyleBackColor = true;
            this.btnInput5.Visible = false;
            this.btnInput5.Click += new System.EventHandler(this.InputClick);
            // 
            // btnInput6
            // 
            this.btnInput6.Location = new System.Drawing.Point(248, 400);
            this.btnInput6.Name = "btnInput6";
            this.btnInput6.Size = new System.Drawing.Size(96, 45);
            this.btnInput6.TabIndex = 19;
            this.btnInput6.Text = "Player6";
            this.btnInput6.UseVisualStyleBackColor = true;
            this.btnInput6.Visible = false;
            this.btnInput6.Click += new System.EventHandler(this.InputClick);
            // 
            // btnInput7
            // 
            this.btnInput7.Location = new System.Drawing.Point(248, 451);
            this.btnInput7.Name = "btnInput7";
            this.btnInput7.Size = new System.Drawing.Size(96, 45);
            this.btnInput7.TabIndex = 20;
            this.btnInput7.Text = "Player7";
            this.btnInput7.UseVisualStyleBackColor = true;
            this.btnInput7.Visible = false;
            this.btnInput7.Click += new System.EventHandler(this.InputClick);
            // 
            // txtName
            // 
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtName.Location = new System.Drawing.Point(121, 9);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(328, 20);
            this.txtName.TabIndex = 1;
            // 
            // lblDisplay0
            // 
            this.lblDisplay0.BackColor = System.Drawing.Color.Transparent;
            this.lblDisplay0.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisplay0.ForeColor = System.Drawing.Color.White;
            this.lblDisplay0.Location = new System.Drawing.Point(12, 7);
            this.lblDisplay0.Name = "lblDisplay0";
            this.lblDisplay0.Padding = new System.Windows.Forms.Padding(25);
            this.lblDisplay0.Size = new System.Drawing.Size(239, 322);
            this.lblDisplay0.TabIndex = 22;
            this.lblDisplay0.Text = "Left to Choose:";
            this.lblDisplay0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDisplay0.Visible = false;
            // 
            // lblDisplay1
            // 
            this.lblDisplay1.BackColor = System.Drawing.Color.Transparent;
            this.lblDisplay1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisplay1.ForeColor = System.Drawing.Color.White;
            this.lblDisplay1.Location = new System.Drawing.Point(257, 7);
            this.lblDisplay1.Name = "lblDisplay1";
            this.lblDisplay1.Padding = new System.Windows.Forms.Padding(25);
            this.lblDisplay1.Size = new System.Drawing.Size(239, 322);
            this.lblDisplay1.TabIndex = 23;
            this.lblDisplay1.Text = "Left to Choose:";
            this.lblDisplay1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDisplay1.Visible = false;
            // 
            // lblDisplay2
            // 
            this.lblDisplay2.BackColor = System.Drawing.Color.Transparent;
            this.lblDisplay2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisplay2.ForeColor = System.Drawing.Color.White;
            this.lblDisplay2.Location = new System.Drawing.Point(12, 329);
            this.lblDisplay2.Name = "lblDisplay2";
            this.lblDisplay2.Padding = new System.Windows.Forms.Padding(25);
            this.lblDisplay2.Size = new System.Drawing.Size(239, 322);
            this.lblDisplay2.TabIndex = 24;
            this.lblDisplay2.Text = "Left to Choose:";
            this.lblDisplay2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDisplay2.Visible = false;
            // 
            // lblDisplay3
            // 
            this.lblDisplay3.BackColor = System.Drawing.Color.Transparent;
            this.lblDisplay3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisplay3.ForeColor = System.Drawing.Color.White;
            this.lblDisplay3.Location = new System.Drawing.Point(257, 329);
            this.lblDisplay3.Name = "lblDisplay3";
            this.lblDisplay3.Padding = new System.Windows.Forms.Padding(25);
            this.lblDisplay3.Size = new System.Drawing.Size(239, 322);
            this.lblDisplay3.TabIndex = 25;
            this.lblDisplay3.Text = "Left to Choose:";
            this.lblDisplay3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDisplay3.Visible = false;
            // 
            // lblIP
            // 
            this.lblIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIP.Location = new System.Drawing.Point(3, 33);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(112, 20);
            this.lblIP.TabIndex = 26;
            this.lblIP.Text = "IP:";
            this.lblIP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(3, 7);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(112, 20);
            this.lblName.TabIndex = 27;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(510, 667);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.lblDisplay3);
            this.Controls.Add(this.lblDisplay2);
            this.Controls.Add(this.lblDisplay1);
            this.Controls.Add(this.lblDisplay0);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.btnInput7);
            this.Controls.Add(this.btnInput6);
            this.Controls.Add(this.btnInput5);
            this.Controls.Add(this.btnInput4);
            this.Controls.Add(this.btnInput3);
            this.Controls.Add(this.btnInput2);
            this.Controls.Add(this.btnInput1);
            this.Controls.Add(this.btnInput0);
            this.Controls.Add(this.btnInputM3);
            this.Controls.Add(this.btnInputM2);
            this.Controls.Add(this.btnInputM1);
            this.Controls.Add(this.lblImage);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblLeft);
            this.Controls.Add(this.listRoles);
            this.Controls.Add(this.lblSetupState);
            this.Controls.Add(this.btnReady);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.btnConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "OneNightClient";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.Label lblImage;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Label lblLeft;
		private System.Windows.Forms.CheckedListBox listRoles;
		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.Label lblSetupState;
		private System.Windows.Forms.Button btnReady;
		private System.Windows.Forms.TextBox txtIP;
		private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnInputM1;
        private System.Windows.Forms.Button btnInputM2;
        private System.Windows.Forms.Button btnInputM3;
        private System.Windows.Forms.Button btnInput0;
        private System.Windows.Forms.Button btnInput1;
        private System.Windows.Forms.Button btnInput2;
        private System.Windows.Forms.Button btnInput3;
        private System.Windows.Forms.Button btnInput4;
        private System.Windows.Forms.Button btnInput5;
        private System.Windows.Forms.Button btnInput6;
        private System.Windows.Forms.Button btnInput7;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblDisplay0;
        private System.Windows.Forms.Label lblDisplay1;
        private System.Windows.Forms.Label lblDisplay2;
        private System.Windows.Forms.Label lblDisplay3;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label lblName;
    }
}
