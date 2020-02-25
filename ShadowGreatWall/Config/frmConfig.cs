using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShadowGreatWall.Startup;
using ShadowGreatWall.Core;

namespace ShadowGreatWall.Config
{
    public partial class frmConfig : IBaseForm
    {
        #region [初始化]
        public frmConfig()
        {
            InitializeComponent();

            Initialize();
        }

        private void frmConfig_Load(object sender, EventArgs e)
        {

        }

        private void Initialize()
        {
            Configuration config = StartupMgr.Instance.CurrentConfig;

            //前置代理
            chkProxyEnable.Checked = config.Proxy.Enable;
            chkUpdateByProy.Checked = config.Proxy.UpdateVia;
            txtProxyHost.Text = config.Proxy.Host;
            txtProxyPort.Text = config.Proxy.Port.ToString();
            txtProxyAccount.Text = config.Proxy.Accout;
            txtProxyPassword.Text = config.Proxy.Password;
            txtProxyUserAgent.Text = config.Proxy.UserAgent;

            switch (config.Proxy.Mode)
            { 
                case ProxyMode.Http:
                    cmbProxyType.SelectedIndex = 1;
                    break;
                default:
                    cmbProxyType.SelectedIndex = 0;
                    break;
            }
            
        }
        #endregion

        private void chkProxyEnable_CheckedChanged(object sender, EventArgs e)
        {
            chkUpdateByProy.Enabled = chkProxyEnable.Checked;
            cmbProxyType.Enabled = chkProxyEnable.Checked;
            txtProxyAccount.Enabled = chkProxyEnable.Checked;
            txtProxyHost.Enabled = chkProxyEnable.Checked;
            txtProxyPassword.Enabled = chkProxyEnable.Checked;
            txtProxyPort.Enabled = chkProxyEnable.Checked;
            txtProxyUserAgent.Enabled = chkProxyEnable.Checked;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Hide();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Configuration config = StartupMgr.Instance.CurrentConfig;

            //前置代理
            try
            {
                config.Proxy.Port = Convert.ToInt32(txtProxyPort.Text);
            }
            catch
            {
                MessageBox.Show("请输入合法的端口号");
            }

            config.Proxy.Enable = chkProxyEnable.Checked;
            config.Proxy.UpdateVia = chkUpdateByProy.Checked;
            config.Proxy.Host = txtProxyHost.Text;
            config.Proxy.Accout = txtProxyAccount.Text;
            config.Proxy.Password = txtProxyPassword.Text;
            config.Proxy.UserAgent = txtProxyUserAgent.Text;

            switch (cmbProxyType.SelectedIndex)
            { 
                case 1:
                    config.Proxy.Mode = ProxyMode.Http;
                    break;
                default:
                    config.Proxy.Mode = ProxyMode.Socks5;
                    break;
            }

            Configuration.Save(config);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Hide();
        }
    }
}
