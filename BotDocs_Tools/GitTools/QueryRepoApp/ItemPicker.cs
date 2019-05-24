using ReportUtils;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
