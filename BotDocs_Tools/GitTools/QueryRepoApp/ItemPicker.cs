using ReportUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QueryRepoApp
{
    public partial class ItemPicker : Form
    {
        public ItemPicker()
        {
            InitializeComponent();
        }

        public void SetItems(IList<RepositoryInfo> repos)
        {
            listBox.Items.AddRange(repos.Select(i => $"{i.PathToRoot}, {i.Branch} branch").ToArray());
            listBox.SelectedIndex = 0;
        }

        public int SelectedIndex => listBox.SelectedIndex;
    }
}
