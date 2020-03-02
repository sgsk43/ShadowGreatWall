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

namespace ShadowGreatWall.Servers
{
    public partial class frmServerList : IBaseForm
    {
        private Dictionary<string, ListViewGroup> serverGroups = new Dictionary<string, ListViewGroup>();

        public frmServerList()
        {
            InitializeComponent();

            NotificationMgr.Instance.OnNotification += new NotificationMgr.OnNotificationDelegate(OnNotification);
        }

        private void frmServerList_Load(object sender, EventArgs e)
        {
            ShowServerList();
        }

        private void ShowServerList()
        {
            serverGroups.Clear();
            lstData.Items.Clear();
            lstData.Groups.Clear();

            Configuration config = StartupMgr.Instance.CurrentConfig;

            foreach (ServerGroup group in config.Groups)
            {
                ListViewGroup lstGroup = new ListViewGroup();
                lstGroup.Tag = group;
                lstGroup.Header = group.Name;
                serverGroups.Add(group.Guid, lstGroup);
            }

            List<ListViewItem> items = new List<ListViewItem>();

            foreach (IServer server in config.Servers)
            {
                ListViewItem item = new ListViewItem();
                item.Tag = server;
                item.Text = server.Describe;
                item.SubItems.Add(server.ServerType.ToString());

                if (!string.IsNullOrWhiteSpace(server.GroupGuid) && serverGroups.ContainsKey(server.GroupGuid))
                {
                    item.Group = serverGroups[server.GroupGuid];
                }

                items.Add(item);
            }

            foreach (ListViewGroup group in serverGroups.Values)
            {
                if (group.Items.Count > 0)
                {
                    lstData.Groups.Add(group);
                }
            }

            lstData.Items.AddRange(items.ToArray());
        }

        private void OnNotification(NotifycationType type)
        {
            if (type == NotifycationType.SubscribeUpdated)
            {
                ShowServerList();
            }
        }

        private void lstData_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
