using Maxvision.Robot.Sdk.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRobotServerApp.Extensions
{
    public class SemanticNpoi
    {
        /// <summary>
        /// 加载execl
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="sheetindex">sheet</param>
        /// <param name="startrow">开始行</param>
        /// <returns></returns>
        public static List<SemanticInfo> LoadExecl(string filename, int sheetindex, int startrow)
        {
            List<SemanticInfo> lst = new List<SemanticInfo>();

            IWorkbook workbook = null;  //新建IWorkbook对象
            FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            if (filename.IndexOf(".xlsx") > 0) // 2007版本
            {
                workbook = new XSSFWorkbook(fileStream);  //xlsx数据读入workbook
            }
            else if (filename.IndexOf(".xls") > 0) // 2003版本
            {
                workbook = new HSSFWorkbook(fileStream);  //xls数据读入workbook
            }
            ISheet sheet = workbook.GetSheetAt(sheetindex);  //获取第一个工作表
            for (int i = startrow; i < sheet.LastRowNum; i++)  //对工作表每一行
            {
                SemanticInfo model = new SemanticInfo();
                IRow row = sheet.GetRow(i);   //row读入第i行数据
                if (row != null)
                {
                    for (int j = 0; j < row.LastCellNum; j++)  //对工作表每一列
                    {
                        switch (j)
                        {
                            case 0:
                                model.Guid = row.GetCell(j).ToString(); //获取i行j列数据
                                break;
                            case 1:
                                model.Question = row.GetCell(j).ToString(); //获取i行j列数据
                                break;
                            case 2:
                                model.Answer = row.GetCell(j).ToString(); //获取i行j列数据
                                break;
                            case 3:
                                model.Keyword = row.GetCell(j).ToString(); //获取i行j列数据
                                break;
                        }
                    }
                }
                lst.Add(model);
            }
            fileStream.Close();
            workbook.Close();

            return lst;
        }

    }
}
