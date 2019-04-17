using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseManagement
{
    // setTip(StatusType.下列狀態) 即可針對狀態動作給予相對應TIP
    enum StatusType
    {
        NONE,           // 提示窗口
        SELECT,         // 搜尋成功
        UPDATE,         // 資料修改成功
        INSERT,         // 資料新增成功
        DELETE,         // 資料刪除成功
        REFRESH,        // 資料重新載入成功
        NULL,           // 查無資料
        BLANKWARN,      // 執行失敗，數字欄不得空白
        EXPORTSCCESS,   // Excel匯出成功
        ERROR,          // 發生錯誤，執行失敗
        PRINTSCCESS,    // 列印成功
        CANCEL,         // 動作取消
        NUMCOLUMNONLY   // 只允許輸入進貨數量
    }
}
