using CE.DbConnect.Models;
using CE.DbUpgrade.Data;
using CE.DbUpgrade.Models;
using CE.DbUpgrade.Ports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CE.DbConnectionHelper.Dialogs
{
    public partial class UpgradeDatabaseView : Form
    {
        #region enums
        protected enum FormState
        {
            Loading,
            Ready,
            Busy
        }
        #endregion

        #region properties
        protected IList<IReleaseVersion> Releases { get; set; } = new List<IReleaseVersion>();
        public DatabaseInstance Database { get; set; } = new DatabaseInstance();
        private FormState _state;
        protected FormState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                UpdateStatus(_state);
            }
        }
        #endregion

        #region ctor / initialization
        public UpgradeDatabaseView()
        {
            InitializeComponent();
        }
        private void UpgradeDatabaseView_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeForm();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        protected virtual void InitializeForm()
        {
            try
            {
                State = FormState.Loading;

                DisplayConnection(Database);

                ClearAllScripts();

                if (Database != null)
                {
                    var startingVersion = Version.Parse(Database.Version);

                    Releases = GetReleases(startingVersion);

                    DisplayScripts(Releases);
                }

                UpdateProgress(0);

                State = FormState.Ready;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        #endregion

        #region private
        private void ExceptionHandler(Exception ex)
        {
            Console.WriteLine(ex.ToString());
            MessageBox.Show(ex.Message);
        }
        private void Template()
        {
            try
            {

            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        #endregion

        #region protected
        protected virtual IList<IReleaseVersion> GetReleases(Version startVersion)
        {
            IList<IReleaseVersion> releases = null;

            try
            {
                IDbUpgradeRepository _dbRepo = new DbUpgradeRepository();

                releases = _dbRepo.GetReleases(startVersion);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

            return releases;
        }

        protected virtual void ClearAllScripts()
        {
            try
            {
                trvScripts.BeginUpdate();

                trvScripts.Nodes.Clear();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
            finally
            {
                trvScripts.EndUpdate();
            }
        }
        protected virtual void DisplayScripts(IList<IReleaseVersion> releases)
        {
            if (releases == null)
                return;

            try
            {
                trvScripts.BeginUpdate();

                foreach (var release in releases.OrderBy(s => s.Version))
                {
                    var releaseNode = new TreeNode(release.Version.ToString(), 0, 0);
                    releaseNode.Tag = release;

                    foreach (var dbVersion in release.DatabaseVersions.OrderBy(s => s.Version))
                    {
                        var dbVersionNode = new TreeNode(dbVersion.Version.ToString(), 1, 1);
                        dbVersionNode.Tag = dbVersion;

                        foreach (var script in dbVersion.SqlScripts.OrderBy(s => s.Version))
                        {
                            var scriptNode = new TreeNode(script.Target, 2, 2);
                            scriptNode.Tag = script;

                            dbVersionNode.Nodes.Add(scriptNode);
                        }

                        releaseNode.Nodes.Add(dbVersionNode);
                    }

                    trvScripts.Nodes.Add(releaseNode);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
            finally
            {
                trvScripts.EndUpdate();
            }
        }

        protected virtual void DisplayConnection(DatabaseInstance database)
        {
            lblDatabaseName.Text = database?.Name;
            lblServerName.Text = database?.ServerInstance?.Name;
            lblVersionNumber.Text = database?.Version.ToString();
        }

        protected virtual void UpdateStatus(FormState state)
        {
            lblProgressStatus.Text = state.ToString();
        }

        protected virtual void UpdateProgress(int percentComplete)
        {
            progressBar1.Value = percentComplete;
        }
        #endregion

        private bool _settingCheckedValues = false;
        private void trvScripts_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (_settingCheckedValues)
                return;

            try
            {
                _settingCheckedValues = true;

                foreach (TreeNode childNode in e.Node.Nodes)
                {
                    SetChildNodeChecked(childNode, !e.Node.Checked);
                }

                SetParentNodeChecked(e.Node, !e.Node.Checked);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _settingCheckedValues = false;
            }
        }

        private void SetChildNodeChecked(TreeNode node, bool isChecked)
        {
            node.Checked = isChecked;

            foreach (TreeNode childNode in node.Nodes)
            {
                SetChildNodeChecked(childNode, isChecked);
            }
        }
        private void SetParentNodeChecked(TreeNode node, bool isChecked)
        {
            if (node.Parent != null)
            {
                bool foundOppositeCheckedValue = false;
                foreach (TreeNode childNode in node.Parent.Nodes)
                {
                    if (childNode.Checked != isChecked && childNode != node)
                    {
                        foundOppositeCheckedValue = true;
                        break;
                    }
                }

                if (foundOppositeCheckedValue)
                    node.Parent.Checked = false;
                else
                    node.Parent.Checked = isChecked;
            }
        }

        private void selectAllAboveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _settingCheckedValues = true;

                var selectedTag = trvScripts.SelectedNode?.Tag;
                var startingVersion = ((IVersioned)selectedTag).Version;

                foreach (TreeNode node in trvScripts.Nodes)
                {
                    SelectNodeVersionAndAbove(node, startingVersion);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _settingCheckedValues = false;
            }
        }

        private void SelectNodeVersionAndAbove(TreeNode node, Version startingVersion)
        {
            var version = ((IVersioned)node.Tag).Version;

            node.Checked = ((version.Major > startingVersion.Major) ||
                                    (version.Major == startingVersion.Major && version.Minor > startingVersion.Minor) ||
                                    (version.Major == startingVersion.Major && version.Minor == startingVersion.Minor && version.Build >= startingVersion.Build));

            foreach (TreeNode childNode in node.Nodes)
            {
                SelectNodeVersionAndAbove(childNode, startingVersion);
            }
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = (trvScripts.SelectedNode == null);
        }

        private void selectAllBelowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _settingCheckedValues = true;

                var selectedTag = trvScripts.SelectedNode?.Tag;
                var startingVersion = ((IVersioned)selectedTag).Version;

                foreach (TreeNode node in trvScripts.Nodes)
                {
                    SelectNodeVersionAndBelow(node, startingVersion);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _settingCheckedValues = false;
            }
        }

        private void SelectNodeVersionAndBelow(TreeNode node, Version startingVersion)
        {
            var version = ((IVersioned)node.Tag).Version;

            node.Checked = ((version.Major < startingVersion.Major) ||
                                    (version.Major == startingVersion.Major && version.Minor < startingVersion.Minor) ||
                                    (version.Major == startingVersion.Major && version.Minor == startingVersion.Minor && version.Build <= startingVersion.Build));

            foreach (TreeNode childNode in node.Nodes)
            {
                SelectNodeVersionAndBelow(childNode, startingVersion);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateStatus(FormState.Busy);

                UpdateProgress(1);

                txtMessages.AppendText("Starting...\r\n");

                var executeList = new List<IDatabaseVersionScript>();

                foreach (TreeNode node in trvScripts.Nodes)
                {
                    CollectScriptsToExecute(node, executeList);
                }

                UpdateProgress(10);

                var countToExecute = executeList.Count;
                int executedCount = 0;

                foreach (var script in executeList.OrderBy(s => s.Version))
                {
                    ExecuteScript(script);
                    var percentComplete = (int)((executedCount / countToExecute) * 90);
                    UpdateProgress(percentComplete);
                }


                UpdateStatus(FormState.Ready);

                UpdateProgress(100);

                txtMessages.AppendText("Done!\r\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CollectScriptsToExecute(TreeNode node, IList<IDatabaseVersionScript> executeList)
        {
            if (node.Checked)
            {
                if (node.Tag is IDatabaseVersionScript)
                {
                    var script = node.Tag as IDatabaseVersionScript;

                    executeList.Add(script);
                }

                foreach (TreeNode childNode in node.Nodes)
                {
                    CollectScriptsToExecute(childNode, executeList);
                }
            }
        }

        private void ExecuteScript(IDatabaseVersionScript script)
        {
            txtMessages.AppendText($"{script.Version.ToString(),-20} {script.ActionDescription} {script.Target}...\r\n");
        }
    }
}
