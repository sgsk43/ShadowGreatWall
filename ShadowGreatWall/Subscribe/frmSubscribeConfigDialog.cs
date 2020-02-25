using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShadowGreatWall.Subscribe
{
    public partial class frmSubscribeConfigDialog : IBaseForm
    {
        public frmSubscribeConfigDialog()
        {
            InitializeComponent();
        }

        public string GroupName
        {
            get
            {
                return txtName.Text;
            }
            set
            {
                txtName.Text = value;
            }
        }

        public string GrouURL
        {
            get
            {
                return txtURL.Text;
            }
            set
            {
                txtURL.Text = value;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Hide();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("请输入组名称");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtURL.Text))
            {
                MessageBox.Show("请输入URL");
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Hide();
        }
    }
}
