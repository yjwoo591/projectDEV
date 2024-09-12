using System;
using System.Windows.Forms;
using ForexAITradingSystem.Managers;
using ForexAITradingSystem.PopupForms;
using ForexAITradingSystem.Utilities;
using ForexAITradingSystem.Properties;

namespace ForexAITradingSystem
{
    public partial class MainForm : Form
    {
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileMenu;
        private ToolStripMenuItem settingsMenu;
        private ToolStripMenuItem exitMenuItem;
        private ToolStripMenuItem apiConfigMenuItem;
        private ToolStripMenuItem dbConfigMenuItem;
        private ToolStripMenuItem orderMenuItem;

        private readonly ApiManager _apiManager;
        private readonly DataManager _dataManager;
        private readonly OrderManager _orderManager;
        private Config _config;

        public MainForm()
        {
            InitializeComponent();
            LoadWindowSettings();
            _apiManager = new ApiManager();

            _dataManager = new DataManager();
            _orderManager = new OrderManager();

            LoadConfiguration();
            CreateMenu();
        }

        private void LoadConfiguration()
        {
            _config = ConfigManager.LoadConfig();
            if (string.IsNullOrEmpty(_config.DbServerIp))
            {
                // �ʱ� ������ �ʿ��� ���
                using (var configForm = new ConfigForm(_config))
                {
                    if (configForm.ShowDialog() == DialogResult.OK)
                    {
                        _config = configForm.UpdatedConfig;
                        ConfigManager.SaveConfig(_config);
                    }
                    else
                    {
                        MessageBox.Show("Initial configuration is required to run the application.", "Configuration Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Application.Exit();
                    }
                }
            }

            // ���⿡�� �ε�� ������ ����Ͽ� �ʿ��� �ʱ�ȭ�� �����մϴ�.
            // ��: _dataManager.Initialize(_config.DbServerIp, _config.DbUserId, _config.DbPassword);
        }

        private void LoadWindowSettings()
        {
            if (Settings.Default.MainFormLocation != Point.Empty)
            {
                this.Location = Settings.Default.MainFormLocation;
            }
            if (Settings.Default.MainFormSize != Size.Empty)
            {
                this.Size = Settings.Default.MainFormSize;
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            SaveWindowSettings();
        }

        private void SaveWindowSettings()
        {
            Settings.Default.MainFormLocation = this.Location;
            Settings.Default.MainFormSize = this.Size;
            Settings.Default.Save();
        }
        private void CreateMenu()
        {
            menuStrip = new MenuStrip();
            fileMenu = new ToolStripMenuItem("File");
            settingsMenu = new ToolStripMenuItem("Settings");
            exitMenuItem = new ToolStripMenuItem("Exit");
            apiConfigMenuItem = new ToolStripMenuItem("API Configuration");
            dbConfigMenuItem = new ToolStripMenuItem("Database Configuration");
            orderMenuItem = new ToolStripMenuItem("Place Order");

            fileMenu.DropDownItems.Add(exitMenuItem);
            settingsMenu.DropDownItems.Add(apiConfigMenuItem);
            settingsMenu.DropDownItems.Add(dbConfigMenuItem);
            fileMenu.DropDownItems.Add(orderMenuItem);

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(settingsMenu);

            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            exitMenuItem.Click += ExitMenuItem_Click;
            apiConfigMenuItem.Click += ApiConfigMenuItem_Click;
            dbConfigMenuItem.Click += DbConfigMenuItem_Click;
            orderMenuItem.Click += OrderMenuItem_Click;
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ApiConfigMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new ApiConfigForm(_apiManager))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("API configuration updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void DbConfigMenuItem_Click(object sender, EventArgs e)
        {
            using (var configForm = new ConfigForm(_config))
            {
                if (configForm.ShowDialog() == DialogResult.OK)
                {
                    _config = configForm.UpdatedConfig;
                    ConfigManager.SaveConfig(_config);
                    MessageBox.Show("Database configuration updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // ���⿡�� ������Ʈ�� ������ ����Ͽ� �ʿ��� ���ʱ�ȭ�� �����մϴ�.
                    // ��: _dataManager.Initialize(_config.DbServerIp, _config.DbUserId, _config.DbPassword);
                }
            }
        }

        private void OrderMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new OrderForm(_orderManager))
            {
                form.ShowDialog();
            }
        }

        // ���⿡ �߰����� �޼������ ������ �� �ֽ��ϴ�.
        // ��: �ǽð� ������ ������Ʈ, ��Ʈ ���� ��

        private void UpdateRealTimeData()
        {
            // �ǽð� ������ ������Ʈ ����
        }

        private void UpdateCharts()
        {
            // ��Ʈ ���� ����
        }
    }
}