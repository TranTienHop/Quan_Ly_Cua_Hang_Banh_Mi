using System;
using System.Windows.Forms;
using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Forms
{
    public partial class AdministrationControl : UserControl
    {
        private bool _applyingPermissions;

        public AdministrationControl()
        {
            InitializeComponent();
            tabAdministration.SelectedIndexChanged += LoadSelectedTab;
        }

        public void ApplyPermissionsAndLoad()
        {
            _applyingPermissions = true;
            try
            {
                SetTabAvailability(tabAccounts, UserSession.HasPermission("SYS_ACCOUNT"));
                SetTabAvailability(tabRoles, UserSession.HasPermission("SYS_ROLE"));
                SetTabAvailability(tabBranches, UserSession.HasPermission("SYS_BRANCH"));
                SetTabAvailability(tabShifts, UserSession.HasPermission("SYS_SHIFT"));
                SetTabAvailability(tabShiftSchedule, UserSession.HasPermission("SYS_SHIFT"));
            }
            finally
            {
                _applyingPermissions = false;
            }

        }

        public void NavigateToSection(bool accounts, bool roles, bool branches, bool shifts)
        {
            TabPage page = accounts ? tabAccounts :
                roles ? tabRoles : branches ? tabBranches : tabShifts;
            if (!tabAdministration.TabPages.Contains(page))
                throw new UnauthorizedAccessException("Bạn không có quyền truy cập chức năng này.");
            bool alreadySelected = tabAdministration.SelectedTab == page;
            tabAdministration.SelectedTab = page;
            if (alreadySelected)
                LoadSelectedTab(this, EventArgs.Empty);
        }

        private void LoadSelectedTab(object? sender, EventArgs e)
        {
            if (_applyingPermissions) return;
            if (tabAdministration.SelectedTab == tabAccounts)
                accountManagement.LoadData();
            else if (tabAdministration.SelectedTab == tabRoles)
                roleManagement.LoadData();
            else if (tabAdministration.SelectedTab == tabBranches)
                branchManagement.LoadData();
            else if (tabAdministration.SelectedTab == tabShifts)
                shiftManagement.LoadData();
            else if (tabAdministration.SelectedTab == tabShiftSchedule)
                shiftSchedule.LoadData();
        }

        private void SetTabAvailability(TabPage page, bool allowed)
        {
            bool exists = tabAdministration.TabPages.Contains(page);
            if (allowed && !exists)
                tabAdministration.TabPages.Add(page);
            else if (!allowed && exists)
                tabAdministration.TabPages.Remove(page);
        }
    }
}
