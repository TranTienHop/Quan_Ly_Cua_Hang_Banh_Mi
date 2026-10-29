using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyCuaHangBanhMi.Controls;

namespace QuanLyCuaHangBanhMi.Forms.Administration
{
    /// <summary>
    /// Designer-editable account list view; persistence is provided by the host control.
    /// </summary>
    public partial class UserManagementView : UserControl
    {
        private bool _compactToolbar;

        public UserManagementView()
        {
            InitializeComponent();
            cmbStatus.SelectedIndex = 0;
            ConfigureSearchPlaceholder();
            ConfigureUsersGrid();
            ApplyResponsiveLayout();
        }

        public TextBox SearchBox => txtSearch;
        public ComboBox StatusFilter => cmbStatus;
        public ModernButton AddButton => btnAdd;
        public ModernButton EditButton => btnEdit;
        public ModernButton LockButton => btnLock;
        public DataGridView UsersGrid => usersGrid;

        public event EventHandler<int>? RowEditClicked;
        public event EventHandler<int>? RowToggleStatusClicked;

        public bool IsActionColumn(int columnIndex) => columnIndex == colActions.Index;

        private void ConfigureUsersGrid()
        {
            // Keep the same standard DataGridView presentation used by the shift screen.
            usersGrid.EnableHeadersVisualStyles = false;
            usersGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            usersGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            usersGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            usersGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            usersGrid.CellPainting += UsersGrid_CellPainting;
            usersGrid.CellMouseClick += UsersGrid_CellMouseClick;
        }

        private void UsersGrid_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colActions.Index)
            {
                return;
            }

            e.PaintBackground(e.CellBounds, true);
            e.Paint(e.CellBounds, DataGridViewPaintParts.Border);
            if (e.Graphics == null)
            {
                e.Handled = true;
                return;
            }

            (Rectangle editButton, Rectangle statusButton) = GetActionButtonBounds(e.CellBounds);
            string status = Convert.ToString(usersGrid.Rows[e.RowIndex].Cells[colStatus.Index].Value)
                ?? string.Empty;
            string statusAction = string.Equals(status, "Đang hoạt động", StringComparison.OrdinalIgnoreCase)
                ? "Khóa"
                : "Mở khóa";

            DrawTextButton(e.Graphics, editButton, "Sửa");
            DrawTextButton(e.Graphics, statusButton, statusAction);
            e.Handled = true;
        }

        private void UsersGrid_CellMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colActions.Index)
            {
                return;
            }

            usersGrid.ClearSelection();
            usersGrid.Rows[e.RowIndex].Selected = true;
            usersGrid.CurrentCell = usersGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];

            Rectangle cellBounds = usersGrid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            var localBounds = new Rectangle(0, 0, cellBounds.Width, cellBounds.Height);
            (Rectangle editButton, Rectangle statusButton) = GetActionButtonBounds(localBounds);
            Point clickPoint = new(e.X, e.Y);

            if (editButton.Contains(clickPoint))
            {
                RowEditClicked?.Invoke(this, e.RowIndex);
            }
            else if (statusButton.Contains(clickPoint))
            {
                RowToggleStatusClicked?.Invoke(this, e.RowIndex);
            }
        }

        private static (Rectangle Edit, Rectangle Status) GetActionButtonBounds(Rectangle cellBounds)
        {
            const int editWidth = 50;
            const int statusWidth = 76;
            const int gap = 6;
            int buttonHeight = Math.Min(26, Math.Max(20, cellBounds.Height - 6));
            int totalWidth = editWidth + gap + statusWidth;
            int startX = cellBounds.X + Math.Max(2, (cellBounds.Width - totalWidth) / 2);
            int startY = cellBounds.Y + Math.Max(2, (cellBounds.Height - buttonHeight) / 2);

            return (
                new Rectangle(startX, startY, editWidth, buttonHeight),
                new Rectangle(startX + editWidth + gap, startY, statusWidth, buttonHeight));
        }

        private static void DrawTextButton(Graphics graphics, Rectangle bounds, string text)
        {
            using var background = new SolidBrush(Color.FromArgb(245, 246, 250));
            using var border = new Pen(Color.FromArgb(160, 166, 173));
            using var font = new Font("Segoe UI", 8.5F);
            graphics.FillRectangle(background, bounds);
            graphics.DrawRectangle(border, bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
            TextRenderer.DrawText(graphics, text, font, bounds,
                Color.FromArgb(44, 62, 80),
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
        }

        public void SetRecordCount(int count)
        {
            lblRecordCount.Text = count == 0
                ? "Hiển thị 0-0 trong 0 bản ghi"
                : $"Hiển thị 1-{count} trong {count} bản ghi";
        }

        private void ConfigureSearchPlaceholder()
        {
            string placeholder = txtSearch.Tag as string ?? string.Empty;
            txtSearch.GotFocus += (_, _) =>
            {
                if (txtSearch.Text == placeholder)
                {
                    txtSearch.Clear();
                    txtSearch.ForeColor = Color.FromArgb(50, 35, 15);
                }
            };
            txtSearch.LostFocus += (_, _) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Text = placeholder;
                    txtSearch.ForeColor = Color.Gray;
                }
            };
        }

        private void pagerLayout_Paint(object? sender, PaintEventArgs e)
        {
            using var pen = new Pen(Color.FromArgb(225, 210, 190));
            e.Graphics.DrawLine(pen, 0, 0, pagerLayout.Width, 0);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ApplyResponsiveLayout();
        }

        private void ApplyResponsiveLayout()
        {
            if (rootLayout == null || toolbarLayout == null || rootLayout.RowStyles.Count < 2)
            {
                return;
            }

            bool compact = ClientSize.Width < 930;
            if (_compactToolbar == compact && toolbarLayout.RowCount == (compact ? 2 : 1))
            {
                return;
            }

            _compactToolbar = compact;
            toolbarLayout.SuspendLayout();
            rootLayout.SuspendLayout();

            toolbarLayout.RowStyles.Clear();
            if (compact)
            {
                rootLayout.RowStyles[1].Height = 92;
                toolbarLayout.RowCount = 2;
                toolbarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
                toolbarLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
                toolbarLayout.SetCellPosition(filterFlow, new TableLayoutPanelCellPosition(0, 0));
                toolbarLayout.SetColumnSpan(filterFlow, 2);
                toolbarLayout.SetCellPosition(actionFlow, new TableLayoutPanelCellPosition(0, 1));
                toolbarLayout.SetColumnSpan(actionFlow, 2);
                actionFlow.Dock = DockStyle.Right;
            }
            else
            {
                rootLayout.RowStyles[1].Height = 52;
                toolbarLayout.RowCount = 1;
                toolbarLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                toolbarLayout.SetCellPosition(filterFlow, new TableLayoutPanelCellPosition(0, 0));
                toolbarLayout.SetColumnSpan(filterFlow, 1);
                toolbarLayout.SetCellPosition(actionFlow, new TableLayoutPanelCellPosition(1, 0));
                toolbarLayout.SetColumnSpan(actionFlow, 1);
                actionFlow.Dock = DockStyle.Right;
            }

            rootLayout.ResumeLayout(true);
            toolbarLayout.ResumeLayout(true);
        }
    }
}
