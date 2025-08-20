using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DATAGRID_NATIVE_ATTEMPT_1
{
    public partial class Form1 : UserControl
    {
        private System.Windows.Forms.Timer refreshTimer;
        private string selectedID;
        [ComVisible(true)]
        public string SelectedID
        {
            get { return selectedID; }
            set { selectedID = value; }
        }

        private string selectedDescription;
        [ComVisible(true)]
        public string SelectedDescription
        {
            get { return selectedDescription; }
            set { selectedDescription = value; }
        }

        private string selectedState;
        [ComVisible(true)]
        public string SelectedState
        {
            get { return selectedState; }
            set { selectedState = value; }
        }

        private string selectedStatus;
        [ComVisible(true)]
        public string SelectedStatus
        {
            get { return selectedStatus; }
            set { selectedStatus = value; }
        }

        private string selectedHiHiLimit;
        [ComVisible(true)]
        public string SelectedHiHiLimit
        {
            get { return selectedHiHiLimit; }
            set { selectedHiHiLimit = value; }
        }

        private string selectedHiLimit;
        [ComVisible(true)]
        public string SelectedHiLimit
        {
            get { return selectedHiLimit; }
            set { selectedHiLimit = value; }
        }

        private string selectedLoLimit;
        [ComVisible(true)]
        public string SelectedLoLimit
        {
            get { return selectedLoLimit; }
            set { selectedLoLimit = value; }
        }

        private string selectedLoLoLimit;
        [ComVisible(true)]
        public string SelectedLoLoLimit
        {
            get { return selectedLoLoLimit; }
            set { selectedLoLoLimit = value; }
        }

        private string selectedInhibit;
        [ComVisible(true)]
        public string SelectedInhibit
        {
            get { return selectedInhibit; }
            set { selectedInhibit = value; }
        }

        private string selectedDelay;
        [ComVisible(true)]
        public string SelectedDelay
        {
            get { return selectedDelay; }
            set { selectedDelay = value; }
        }

        private string selectedGroupDesc;
        [ComVisible(true)]
        public string SelectedGroupDesc
        {
            get { return selectedGroupDesc; }
            set { selectedGroupDesc = value; }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];

                // Populate properties with selected row data
                SelectedID = selectedRow.Cells["ID"].Value?.ToString() ?? "";
                SelectedDescription = selectedRow.Cells["Description"].Value?.ToString() ?? "";
                SelectedState = selectedRow.Cells["State"].Value?.ToString() ?? "";
                SelectedStatus = selectedRow.Cells["Status"].Value?.ToString() ?? "";
                SelectedHiHiLimit = selectedRow.Cells["HiHiLimit"].Value?.ToString() ?? "";
                SelectedHiLimit = selectedRow.Cells["HiLimit"].Value?.ToString() ?? "";
                SelectedLoLimit = selectedRow.Cells["LoLimit"].Value?.ToString() ?? "";
                SelectedLoLoLimit = selectedRow.Cells["LoLoLimit"].Value?.ToString() ?? "";
                SelectedInhibit = selectedRow.Cells["Inhibit"].Value?.ToString() ?? "";
                SelectedDelay = selectedRow.Cells["Delay"].Value?.ToString() ?? "";
                SelectedGroupDesc = selectedRow.Cells["GroupDesc"].Value?.ToString() ?? "";

            }
        }

        private void AdjustFormSize()
        {
            int rowHeight = dataGridView1.RowTemplate.Height;
            int totalRows = dataGridView1.Rows.Count;

            int headerHeight = dataGridView1.ColumnHeadersHeight;
            int padding = 0;  

            int newHeight = headerHeight + (totalRows * rowHeight) + padding;

            // Set a maximum height 
            int maxHeight = 800;
            if (newHeight > maxHeight)
                newHeight = maxHeight;

            this.Size = new System.Drawing.Size(this.Width, newHeight);
            dataGridView1.Size = new System.Drawing.Size(this.Width, newHeight);
        }

        // Define your connection strings for redundant databases
        private string connStringPrimary = "Server=;Database=;User Id=;Password=;";
        private string connStringSecondary = "Server=;Database=;User Id=;Password=;";
        private SqlConnection activeConnection;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (TryConnect(connStringPrimary))
            {
                activeConnection = new SqlConnection(connStringPrimary);
            }
            else if (TryConnect(connStringSecondary))
            {
                activeConnection = new SqlConnection(connStringSecondary);
            }
            else
            {
                MessageBox.Show("Both databases are unavailable!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadData();
        }
        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            RefreshData();  
        }
        private string previousDataHash = "";  

        public void RefreshData()
        {
            try
            {
                string query = "SELECT ID, Description, State, Status, HiHiLimit, HiLimit, LoLimit, LoLoLimit, Inhibit, Delay, GroupDesc FROM dbo.alarmstable";
                SqlDataAdapter adapter = new SqlDataAdapter(query, activeConnection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                string newDataHash = GetDataTableHash(dataTable);

                if (newDataHash != previousDataHash)
                {
                    previousDataHash = newDataHash;

                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = dataTable;

                    CustomizeDataGridView();
                 
                    Console.WriteLine("Data refreshed successfully.");
                }
                else
                {
                    Console.WriteLine("No new data received.");
                }
            }
            catch (Exception ex)
            {
    
            }

        }
        private string GetDataTableHash(DataTable dataTable)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                using (var ms = new System.IO.MemoryStream())
                {
                    serializer.Serialize(ms, dataTable);
                    byte[] hashBytes = md5.ComputeHash(ms.ToArray());
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }
            }
        }
        private bool TryConnect(string connectionString)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT ID, Description, State, Status, HiHiLimit, HiLimit, LoLimit, LoLoLimit, Inhibit, Delay, GroupDesc FROM dbo.alarmstable";
                SqlDataAdapter adapter = new SqlDataAdapter(query, activeConnection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
                CustomizeDataGridView();
            }
            catch (Exception ex)
            {
            }
        }


        private void CustomizeDataGridView()
        {
            dataGridView1.BackgroundColor = Color.FromArgb(18, 18, 18);
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView1.GridColor = Color.FromArgb(18, 18, 18);
            dataGridView1.RowHeadersVisible = false;

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(25, 25, 25);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;   // white text
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersHeight = 32;

            dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(18, 18, 18);
            dataGridView1.DefaultCellStyle.ForeColor = Color.Gainsboro;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(50, 50, 50);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(22, 22, 22);
            dataGridView1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Gainsboro;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridView1.AllowUserToResizeColumns = true;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.ScrollBars = ScrollBars.Both;
            dataGridView1.Dock = DockStyle.Fill;

            this.BackColor = Color.FromArgb(15, 15, 15);
            this.ForeColor = Color.WhiteSmoke;
            this.Size = new System.Drawing.Size(1400, 800);
        }





        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Rows[e.RowIndex].DataBoundItem != null)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                string status = row.Cells["Status"].Value?.ToString().Trim().ToLower() ?? "";
                string inhibit = row.Cells["Inhibit"].Value?.ToString().Trim().ToLower() ?? "";

                if (e.RowIndex % 2 == 0)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(18, 18, 18);
                    row.DefaultCellStyle.ForeColor = Color.Gainsboro;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(22, 22, 22);
                    row.DefaultCellStyle.ForeColor = Color.Gainsboro;
                }

                if (status == "alarm active" || status == "active" || status == "alarm")
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(180, 50, 50);
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
                else if (inhibit == "true" || inhibit == "active" || inhibit == "yes")
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(220, 200, 60);
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }





        private void DataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex >= 0)  
            {
                e.PaintBackground(e.ClipBounds, false);

                Color startColor = Color.FromArgb(64, 64, 64);  
                Color middleColor = Color.FromArgb(96, 96, 96); 
                Color endColor = Color.FromArgb(48, 48, 48);    

                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(e.CellBounds, startColor, endColor, 90F))
                {
                    var blend = new System.Drawing.Drawing2D.ColorBlend
                    {
                        Colors = new[] { startColor, middleColor, endColor },
                        Positions = new[] { 0.0f, 0.5f, 1.0f }
                    };
                    brush.InterpolationColors = blend;

                    e.Graphics.FillRectangle(brush, e.CellBounds);
                }

                TextRenderer.DrawText(e.Graphics, e.Value?.ToString(), e.CellStyle.Font, e.CellBounds, Color.LightGray, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                using (var penDark = new Pen(Color.FromArgb(32, 32, 32)))
                using (var penLight = new Pen(Color.FromArgb(128, 128, 128)))
                {
                    e.Graphics.DrawLine(penDark, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Right, e.CellBounds.Top);
                    e.Graphics.DrawLine(penDark, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom);
                    e.Graphics.DrawLine(penLight, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);
                    e.Graphics.DrawLine(penLight, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);
                }

                e.Handled = true; 
            }
        }
    }
}
