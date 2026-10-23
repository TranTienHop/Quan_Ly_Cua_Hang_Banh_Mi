namespace QuanLyCuaHangBanhMi.Forms
{
    partial class AdministrationControl
    {
        private System.ComponentModel.IContainer components = null!;
        private System.Windows.Forms.TabControl tabAdministration = null!;
        private System.Windows.Forms.TabPage tabAccounts = null!;
        private System.Windows.Forms.TabPage tabRoles = null!;
        private System.Windows.Forms.TabPage tabBranches = null!;
        private System.Windows.Forms.TabPage tabShifts = null!;
        private AccountManagementControl accountManagement = null!;
        private RoleManagementControl roleManagement = null!;
        private BranchManagementControl branchManagement = null!;
        private ShiftManagementControl shiftManagement = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabAdministration = new System.Windows.Forms.TabControl();
            this.tabAccounts = new System.Windows.Forms.TabPage();
            this.tabRoles = new System.Windows.Forms.TabPage();
            this.tabBranches = new System.Windows.Forms.TabPage();
            this.tabShifts = new System.Windows.Forms.TabPage();
            this.accountManagement = new AccountManagementControl();
            this.roleManagement = new RoleManagementControl();
            this.branchManagement = new BranchManagementControl();
            this.shiftManagement = new ShiftManagementControl();
            this.tabAdministration.SuspendLayout();
            this.tabAccounts.SuspendLayout();
            this.tabRoles.SuspendLayout();
            this.tabBranches.SuspendLayout();
            this.tabShifts.SuspendLayout();
            this.SuspendLayout();

            this.tabAdministration.Controls.Add(this.tabAccounts);
            this.tabAdministration.Controls.Add(this.tabRoles);
            this.tabAdministration.Controls.Add(this.tabBranches);
            this.tabAdministration.Controls.Add(this.tabShifts);
            this.tabAdministration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabAdministration.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.tabAdministration.ItemSize = new System.Drawing.Size(150, 38);
            this.tabAdministration.Location = new System.Drawing.Point(0, 0);
            this.tabAdministration.Name = "tabAdministration";
            this.tabAdministration.Padding = new System.Drawing.Point(18, 5);
            this.tabAdministration.SelectedIndex = 0;
            this.tabAdministration.Size = new System.Drawing.Size(1180, 650);

            this.tabAccounts.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.tabAccounts.Controls.Add(this.accountManagement);
            this.tabAccounts.Location = new System.Drawing.Point(4, 42);
            this.tabAccounts.Name = "tabAccounts";
            this.tabAccounts.Padding = new System.Windows.Forms.Padding(3);
            this.tabAccounts.Size = new System.Drawing.Size(1172, 604);
            this.tabAccounts.Text = "Tài khoản";

            this.tabRoles.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.tabRoles.Controls.Add(this.roleManagement);
            this.tabRoles.Location = new System.Drawing.Point(4, 42);
            this.tabRoles.Name = "tabRoles";
            this.tabRoles.Padding = new System.Windows.Forms.Padding(3);
            this.tabRoles.Size = new System.Drawing.Size(1172, 604);
            this.tabRoles.Text = "Vai trò & quyền";

            this.tabBranches.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.tabBranches.Controls.Add(this.branchManagement);
            this.tabBranches.Location = new System.Drawing.Point(4, 42);
            this.tabBranches.Name = "tabBranches";
            this.tabBranches.Padding = new System.Windows.Forms.Padding(3);
            this.tabBranches.Size = new System.Drawing.Size(1172, 604);
            this.tabBranches.Text = "Chi nhánh";

            this.tabShifts.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.tabShifts.Controls.Add(this.shiftManagement);
            this.tabShifts.Location = new System.Drawing.Point(4, 42);
            this.tabShifts.Name = "tabShifts";
            this.tabShifts.Padding = new System.Windows.Forms.Padding(3);
            this.tabShifts.Size = new System.Drawing.Size(1172, 604);
            this.tabShifts.Text = "Ca làm";

            this.accountManagement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accountManagement.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.accountManagement.Location = new System.Drawing.Point(3, 3);
            this.accountManagement.Name = "accountManagement";
            this.accountManagement.Size = new System.Drawing.Size(1166, 598);

            this.roleManagement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.roleManagement.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.roleManagement.Location = new System.Drawing.Point(3, 3);
            this.roleManagement.Name = "roleManagement";
            this.roleManagement.Size = new System.Drawing.Size(1166, 598);

            this.branchManagement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.branchManagement.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.branchManagement.Location = new System.Drawing.Point(3, 3);
            this.branchManagement.Name = "branchManagement";
            this.branchManagement.Size = new System.Drawing.Size(1166, 598);

            this.shiftManagement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shiftManagement.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.shiftManagement.Location = new System.Drawing.Point(3, 3);
            this.shiftManagement.Name = "shiftManagement";
            this.shiftManagement.Size = new System.Drawing.Size(1166, 598);

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.Controls.Add(this.tabAdministration);
            this.Name = "AdministrationControl";
            this.Size = new System.Drawing.Size(1180, 650);
            this.tabAdministration.ResumeLayout(false);
            this.tabAccounts.ResumeLayout(false);
            this.tabRoles.ResumeLayout(false);
            this.tabBranches.ResumeLayout(false);
            this.tabShifts.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
