using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Reflection;

namespace MaxRobotServerApp.Extensions.Utils
{
    public class NpoiUtil<T> where T : new()
    {
        /// <summary>
        /// 加载execl
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public List<T> LoadExecl(string filename)
        {
            List<T> modelList = new List<T>();

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
            ISheet sheet = workbook.GetSheetAt(0);  //获取第一个工作表
            for (int i = 0; i < sheet.LastRowNum; i++)  //对工作表每一行
            {
                T model = new T();
                IRow row = sheet.GetRow(i);   //row读入第i行数据
                if (row != null)
                {
                    for (int j = 0; j < row.LastCellNum; j++)  //对工作表每一列
                    {
                        string cellValue = row.GetCell(j).ToString(); //获取i行j列数据
                        PropertyInfo propertyInfo = model.GetType().GetProperties()[j + 1];
                        if (propertyInfo != null)
                        {
                            propertyInfo.SetValue(model, cellValue, null);
                        }   
                    }
                }
                modelList.Add(model);
            }
            fileStream.Close();
            workbook.Close();
            
            return modelList;
        }

        /// <summary>
        /// 加载execl
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="sheetindex">sheet</param>
        /// <param name="startrow">开始行</param>
        /// <returns></returns>
        public List<T> LoadExecl(string filename,int sheetindex, int startrow)
        {
            List<T> modelList = new List<T>();

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
                T model = new T();
                IRow row = sheet.GetRow(i);   //row读入第i行数据
                if (row != null)
                {
                    for (int j = 0; j < row.LastCellNum; j++)  //对工作表每一列
                    {
                        string cellValue = row.GetCell(j).ToString(); //获取i行j列数据
                        PropertyInfo propertyInfo = model.GetType().GetProperties()[j + 1];
                        if (propertyInfo != null)
                        {
                            propertyInfo.SetValue(model, cellValue, null);
                        }
                    }
                }
                modelList.Add(model);
            }
            fileStream.Close();
            workbook.Close();

            return modelList;
        }

        /// <summary>
        /// 加载execl
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="sheetindex">sheet</param>
        /// <param name="startrow">开始行</param>
        /// <param name="endrow">结束行</param>
        /// <returns></returns>
        public List<T> LoadExecl(string filename, int sheetindex, int startrow, int endrow)
        {
            List<T> modelList = new List<T>();

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
            for (int i = startrow; i < endrow; i++)  //对工作表每一行
            {
                T model = new T();
                IRow row = sheet.GetRow(i);   //row读入第i行数据
                if (row != null)
                {
                    for (int j = 0; j < row.LastCellNum; j++)  //对工作表每一列
                    {
                        string cellValue = row.GetCell(j).ToString(); //获取i行j列数据
                        PropertyInfo propertyInfo = model.GetType().GetProperties()[j + 1];
                        if (propertyInfo != null)
                        {
                            propertyInfo.SetValue(model, cellValue, null);
                        }
                    }
                }
                modelList.Add(model);
            }
            fileStream.Close();
            workbook.Close();

            return modelList;
        }
    }
}
