using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using ShadowGreatWall.Startup;
using ShadowGreatWall.Subscribe;
using ShadowGreatWall.Config;
using ShadowGreatWall.Servers;

namespace ShadowGreatWall
{
    public partial class frmMain : IBaseForm
    {
        private bool forceClose = false;

        public frmMain()
        {
            InitializeComponent();

            InitializeContextMenu();

            InitializeNotifyIcon();
            UpdateNotifyIcon();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                this.Hide();
                this.Opacity = 100;
            }));
        }

        private Icon enableIcon = null;
        private Icon disableIcon = null;

        private void InitializeNotifyIcon()
        {
            enableIcon = Icon.FromHandle(Properties.Resources.proxy_enable.GetHicon());
            disableIcon = Icon.FromHandle(Properties.Resources.proxy_disable.GetHicon());
        }

        private void InitializeContextMenu()
        {
            cmsiSwitchOpen.Checked = StartupMgr.Instance.CurrentConfig.Enable;
            cmsiSwitchClose.Checked = !cmsiSwitchOpen.Checked;
        }

        public void UpdateNotifyIcon()
        {
            if (StartupMgr.Instance.CurrentConfig.Enable)
            {
                this.notifyIcon1.Icon = enableIcon;

                this.notifyIcon1.Text = "代理开启";
            }
            else
            {
                this.notifyIcon1.Icon = disableIcon;

                this.notifyIcon1.Text = "代理关闭";
            }
        }

        private void cmsiSwitchOpen_Click(object sender, EventArgs e)
        {
            StartupMgr.Instance.CurrentConfig.Enable = cmsiSwitchOpen.Checked;
            cmsiSwitchClose.Checked = !cmsiSwitchOpen.Checked;
        }

        private void cmsiSwitchClose_Click(object sender, EventArgs e)
        {
            StartupMgr.Instance.CurrentConfig.Enable = cmsiSwitchClose.Checked;
            cmsiSwitchOpen.Checked = !cmsiSwitchClose.Checked;
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Show();
            }
        } 

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!forceClose)
            {
                this.BeginInvoke(new Action(() =>
                {
                    this.Hide();
                }));

                e.Cancel = true;
            }
        }

        private void cmsiSubscribeConfig_Click(object sender, EventArgs e)
        {
            frmSubscribeConfig frm = new frmSubscribeConfig();
            frm.ShowDialog();
        }

        private void cmsiConfig_Click(object sender, EventArgs e)
        {
            frmConfig frm = new frmConfig();
            frm.ShowDialog();
        }

        private void cmsiSubscribeUpdate_Click(object sender, EventArgs e)
        {
            SubscribeUpdater updater = new SubscribeUpdater();
            updater.Update();

            this.notifyIcon1.ShowBalloonTip(2000, "订阅更新", "订阅更新完成！", ToolTipIcon.Info);
        }

        private void cmsiServerList_Click(object sender, EventArgs e)
        {
            frmServerList frm = new frmServerList();
            frm.ShowDialog();
        }  
    }
}
