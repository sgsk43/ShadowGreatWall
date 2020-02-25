using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace ShadowGreatWall
{
    public partial class IBaseForm : Form
    {
        public IBaseForm()
        {
            InitializeComponent();

            this.Icon = Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
        }

        private void IBaseForm_Load(object sender, EventArgs e)
        {
            HookOKCancleButton();
        }

        private void HookOKCancleButton()
        {
            Type thisType = this.GetType();

            FieldInfo field = thisType.GetField("btnOK", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            if (field != null)
            {
                this.AcceptButton = (Button)field.GetValue(this);
            }

            field = thisType.GetField("btnCancel", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            if (field != null)
            {
                this.CancelButton = (Button)field.GetValue(this);
            }
        }
    }
}

