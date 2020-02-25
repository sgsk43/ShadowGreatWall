using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShadowGreatWall.Core;
using ShadowGreatWall.Startup;

namespace ShadowGreatWall.Subscribe
{
    public partial class frmSubscribeConfig : IBaseForm
    {
        public frmSubscribeConfig()
        {
            InitializeComponent();
        }

        private void frmSubscribeConfig_Load(object sender, EventArgs e)
        {
            foreach (ServerGroup group in StartupMgr.Instance.CurrentConfig.Groups)
            {
                ListViewItem item = new ListViewItem();
                item.Tag = group;
                item.Text = group.Name;
                item.SubItems.Add(group.URL);

                lstData.Items.Add(item);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Hide();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            StartupMgr.Instance.CurrentConfig.Groups.Clear();

            List<string> groups = new List<string>();

            foreach (ListViewItem item in lstData.Items)
            {
                StartupMgr.Instance.CurrentConfig.Groups.Add(item.Tag as ServerGroup);

                groups.Add((item.Tag as ServerGroup).Guid);
            }

            //移除未关联的Group
            for (int i = 0; i < StartupMgr.Instance.CurrentConfig.Servers.Count; )
            {
                IServer server = StartupMgr.Instance.CurrentConfig.Servers[i];

                if (!string.IsNullOrWhiteSpace(server.GroupGuid) && !groups.Contains(server.GroupGuid))
                {
                    StartupMgr.Instance.CurrentConfig.Servers.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            Configuration.Save(StartupMgr.Instance.CurrentConfig);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Hide();
        }

        private void lstData_SelectedIndexChanged(object sender, EventArgs e)
        {
            tsbDelete.Enabled = lstData.SelectedItems.Count > 0;
            tsbModify.Enabled = lstData.SelectedItems.Count == 1;
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            frmSubscribeConfigDialog frm = new frmSubscribeConfigDialog();

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ListViewItem item = new ListViewItem();
                item.Tag = new ServerGroup() {
                    Name = frm.GroupName,
                    URL = frm.GrouURL
                };
                item.Text = frm.GroupName;
                item.SubItems.Add(frm.GrouURL);

                lstData.Items.Add(item);
            }
        }

        private void tsbModify_Click(object sender, EventArgs e)
        {
            if (lstData.SelectedItems.Count != 1)
            {
                return;
            }

            ListViewItem item = lstData.SelectedItems[0];
            ServerGroup group = item.Tag as ServerGroup;

            frmSubscribeConfigDialog frm = new frmSubscribeConfigDialog();
            frm.GroupName = group.Name;
            frm.GrouURL = group.URL;

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                group.Name = frm.GroupName;
                group.URL = frm.GrouURL;

                item.Text = group.Name;
                item.SubItems[1].Text = group.URL;
            }
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (lstData.SelectedItems.Count <= 0)
            {
                return;
            }

            if (MessageBox.Show("删除订阅会使跟该订阅关联的已获取的服务器同时被移除，是否确定？", "删除订阅", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                foreach (ListViewItem item in lstData.SelectedItems)
                {
                    item.Remove();
                }
            }
        }
    }
}
