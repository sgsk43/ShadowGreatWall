namespace ShadowGreatWall
{
    partial class frmMain
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
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.代理开关SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsiSwitchOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsiSwitchClose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.服务器SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.订阅ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsiSubscribeConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsiSubscribeUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsiConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsiServerList = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.代理开关SToolStripMenuItem,
            this.toolStripMenuItem1,
            this.服务器SToolStripMenuItem,
            this.订阅ToolStripMenuItem,
            this.toolStripMenuItem3,
            this.cmsiConfig});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 126);
            // 
            // 代理开关SToolStripMenuItem
            // 
            this.代理开关SToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsiSwitchOpen,
            this.cmsiSwitchClose});
            this.代理开关SToolStripMenuItem.Name = "代理开关SToolStripMenuItem";
            this.代理开关SToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.代理开关SToolStripMenuItem.Text = "代理开关(&S)";
            // 
            // cmsiSwitchOpen
            // 
            this.cmsiSwitchOpen.CheckOnClick = true;
            this.cmsiSwitchOpen.Name = "cmsiSwitchOpen";
            this.cmsiSwitchOpen.Size = new System.Drawing.Size(106, 22);
            this.cmsiSwitchOpen.Text = "开(&O)";
            this.cmsiSwitchOpen.Click += new System.EventHandler(this.cmsiSwitchOpen_Click);
            // 
            // cmsiSwitchClose
            // 
            this.cmsiSwitchClose.Checked = true;
            this.cmsiSwitchClose.CheckOnClick = true;
            this.cmsiSwitchClose.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cmsiSwitchClose.Name = "cmsiSwitchClose";
            this.cmsiSwitchClose.Size = new System.Drawing.Size(106, 22);
            this.cmsiSwitchClose.Text = "关(&C)";
            this.cmsiSwitchClose.Click += new System.EventHandler(this.cmsiSwitchClose_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // 服务器SToolStripMenuItem
            // 
            this.服务器SToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsiServerList});
            this.服务器SToolStripMenuItem.Name = "服务器SToolStripMenuItem";
            this.服务器SToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.服务器SToolStripMenuItem.Text = "服务器(&S)";
            // 
            // 订阅ToolStripMenuItem
            // 
            this.订阅ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsiSubscribeConfig,
            this.toolStripMenuItem2,
            this.cmsiSubscribeUpdate});
            this.订阅ToolStripMenuItem.Name = "订阅ToolStripMenuItem";
            this.订阅ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.订阅ToolStripMenuItem.Text = "订阅(&U)";
            // 
            // cmsiSubscribeConfig
            // 
            this.cmsiSubscribeConfig.Name = "cmsiSubscribeConfig";
            this.cmsiSubscribeConfig.Size = new System.Drawing.Size(117, 22);
            this.cmsiSubscribeConfig.Text = "配置(&C)";
            this.cmsiSubscribeConfig.Click += new System.EventHandler(this.cmsiSubscribeConfig_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(114, 6);
            // 
            // cmsiSubscribeUpdate
            // 
            this.cmsiSubscribeUpdate.Name = "cmsiSubscribeUpdate";
            this.cmsiSubscribeUpdate.Size = new System.Drawing.Size(117, 22);
            this.cmsiSubscribeUpdate.Text = "更新(&U)";
            this.cmsiSubscribeUpdate.Click += new System.EventHandler(this.cmsiSubscribeUpdate_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(149, 6);
            // 
            // cmsiConfig
            // 
            this.cmsiConfig.Name = "cmsiConfig";
            this.cmsiConfig.Size = new System.Drawing.Size(152, 22);
            this.cmsiConfig.Text = "设置(&C)";
            this.cmsiConfig.Click += new System.EventHandler(this.cmsiConfig_Click);
            // 
            // cmsiServerList
            // 
            this.cmsiServerList.Name = "cmsiServerList";
            this.cmsiServerList.Size = new System.Drawing.Size(152, 22);
            this.cmsiServerList.Text = "服务器列表(&L)";
            this.cmsiServerList.Click += new System.EventHandler(this.cmsiServerList_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 364);
            this.Name = "frmMain";
            this.Opacity = 0D;
            this.Text = "系统状态";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 代理开关SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cmsiSwitchOpen;
        private System.Windows.Forms.ToolStripMenuItem cmsiSwitchClose;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 订阅ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cmsiSubscribeConfig;
        private System.Windows.Forms.ToolStripMenuItem cmsiSubscribeUpdate;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem cmsiConfig;
        private System.Windows.Forms.ToolStripMenuItem 服务器SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cmsiServerList;
    }
}