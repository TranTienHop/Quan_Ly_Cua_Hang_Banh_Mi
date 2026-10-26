namespace QuanLyCuaHangBanhMi.UI_QLKho
{
    public class FrmBase : Form, IRefreshable
    {
        protected readonly ErrorProvider Err = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };

        public FrmBase()
        {
            Font = Theme.Base;
            BackColor = Theme.Background;
            Load += (_, _) => RefreshData();    // chỉ chạy lúc form mở thật, không chạy trong Designer
        }

        public virtual void RefreshData() { }
    }
}