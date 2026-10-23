using System;
using System.Windows.Forms;
using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Forms
{
    public partial class AdministrationControl : UserControl
    {
        public AdministrationControl()
        {
            InitializeComponent();
        }

        public void ApplyPermissionsAndLoad()
        {
            SetTabAvailability(tabAccounts, UserSession.HasPermission("SYS_ACCOUNT"));
            SetTabAvailability(tabRoles, UserSession.HasPermission("SYS_ROLE"));
            SetTabAvailability(tabBranches, UserSession.HasPermission("SYS_BRANCH"));
            SetTabAvailability(tabShifts, UserSession.HasPermission("SYS_SHIFT"));

            if (tabAdministration.TabPages.Contains(tabAccounts))
                accountManagement.LoadData();
            if (tabAdministration.TabPages.Contains(tabRoles))
                roleManagement.LoadData();
            if (tabAdministration.TabPages.Contains(tabBranches))
                branchManagement.LoadData();
            if (tabAdministration.TabPages.Contains(tabShifts))
                shiftManagement.LoadData();
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
