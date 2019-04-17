using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WareHouseManagement
{
    class InitialSetting
    {
        public readonly string systemName = "秋田機械倉儲料品管理系統";  // 專案名稱
        public readonly string version = "V1.0.8"; // 版本號

        public InitialSetting(StatusStrip statusStrip, Timer timer)
        {
            RefreshStatusStrip(statusStrip);
            statusStrip.Items[3].Text = systemName;
            statusStrip.Items[4].Text = version;
            timer.Start();
        }

        public static void RefreshStatusStrip(StatusStrip statusStrip)
        {
            
            statusStrip.Items[0].Text = DateTime.Now.ToString("yyyy-MM-dd");
            statusStrip.Items[1].Text = DateTime.Now.ToString("tt HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            statusStrip.Items[2].Text = DateTime.Now.ToString("ddddd");
        }
    }
}
