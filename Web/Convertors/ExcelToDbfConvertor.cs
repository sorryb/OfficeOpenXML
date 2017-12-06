using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Collections;
using OfficeOpenXml;

namespace Convertors
{
    /// <summary>
    /// Excel to dbf convertor
    /// </summary>
    public static class ExcelToDbfConvertor
    {
        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    if (rows.Length > 1)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i].Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                }

            }


            return dt;
        }

        /// <summary>
        /// Excel to datatable.
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static DataTable ConvertXSLXtoDataTable(string strFilePath, string connString)
        {
            //connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=G:\\Work\\MoneyCorp\\Web\\dbfConvertor\\src\\dbfConvertor\\wwwroot\\uploads\\import50.xlsx;Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
            //connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=G:\\import50.xlsx;Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                using (OleDbConnection oledbConn = new OleDbConnection(connString))
                {
                    OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select data_rap,cod_banca,cod_suc,tip_c,pep_c from [import$]", connString); //here we read data from sheet1  
                    oleAdpt.Fill(dt); //fill excel data into dataTable

                    //oledbConn.Open();
                    //using (DataTable Sheets = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null))
                    //{
                    //    for (int i = 0; i < Sheets.Rows.Count; i++)
                    //    {
                    //        string worksheets = Sheets.Rows[i]["TABLE_NAME"].ToString();
                    //        OleDbCommand cmd = new OleDbCommand(String.Format("SELECT data_rap,cod_banca,cod_suc,tip_c,pep_c FROM [{0}]", worksheets), oledbConn);//select * from [{0}]
                    //        OleDbDataAdapter oleda = new OleDbDataAdapter();
                    //        oleda.SelectCommand = cmd;

                    //        oleda.Fill(ds);
                    //    }

                    //    dt = ds.Tables[0];
                    //}
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                
            }

            return dt;

        }

        /// <summary>
        /// Datatable to dbf.
        /// </summary>
        /// <param name="dataTableExcel"></param>
        /// <param name="pathToDBF"></param>
        /// <returns>dbf file name</returns>
        public static string DataTableToDBF(DataTable dataTableExcel, string pathToDBF)
        {
            string dbfTableName = "money_corp" + DateTime.Now.ToString("dd_mm_yyyy_hh_mm").Replace("_","");
            int iCounter = 0;
            try
            {


            using (var vfpConn = new OleDbConnection(@"Provider=VFPOLEDB.1;Data Source=" + pathToDBF)) //    C:\SomePathOnYourMachine\
            {
                using (var vfpCmd = new OleDbCommand("", vfpConn))
                {
                    // Create table command for VFP    data_rap,cod_banca,cod_suc,tip_c,pep_c
                    vfpCmd.CommandText = "CREATE TABLE " + dbfTableName + " ( [data_rap] DATETIME NULL, [cod_banca] Numeric(18,0) NULL, [cod_suc] Char(200) NULL, [tip_c] Char(200) NULL,[pep_c] Numeric(18,0) NULL)";
                                           //"CREATE TABLE testFromSQL ( ID Numeric(18,0), [Name] Char(100) NULL, [Details] Char(200) NULL, [Status] Logical NULL, [CreateDate] DATETIME NULL)";
                    vfpConn.Open();
                    vfpCmd.ExecuteNonQuery();

                    // Now, change the command to a SQL-Insert command, but PARAMETERIZE IT.
                    // "?" is a place-holder for the data
                    vfpCmd.CommandText = "insert into "+ dbfTableName + " "
                       + "( [data_rap], [cod_banca], [cod_suc], [tip_c] ,[pep_c]) values ( ?, ?, ?, ?, ? )";

                    // Parameters added in order of the INSERT command above.. 
                    // SAMPLE values just to establish a basis of the column types
                    vfpCmd.Parameters.Add(new OleDbParameter("data_rap", DateTime.Now));
                    vfpCmd.Parameters.Add(new OleDbParameter("cod_banca", 24334));
                    vfpCmd.Parameters.Add(new OleDbParameter("cod_suc", "sample string"));
                    vfpCmd.Parameters.Add(new OleDbParameter("tip_c", "sample string"));
                    vfpCmd.Parameters.Add(new OleDbParameter("pep_c", 456));

                    // Now, for each row in the ORIGINAL SQL table, apply the insert to VFP
                    foreach (DataRow excelRow in dataTableExcel.Rows)
                        {
                            // set the parameters based on whatever current record is
                            vfpCmd.Parameters[0].Value = ClearDataReader(excelRow["data_rap"]) ? DateTime.MinValue : Convert.ToDateTime(excelRow["data_rap"]);
                            vfpCmd.Parameters[1].Value = ClearDataReader(excelRow["cod_banca"]) ? 0 : Convert.ToInt32(excelRow["cod_banca"]);
                            vfpCmd.Parameters[2].Value = ClearDataReader(excelRow["cod_suc"]) ? "" : excelRow["cod_suc"].ToString();
                            vfpCmd.Parameters[3].Value = ClearDataReader(excelRow["tip_c"]) ? "" : excelRow["tip_c"].ToString();
                            vfpCmd.Parameters[4].Value = ClearDataReader(excelRow["pep_c"]) ? 0 : Convert.ToInt32(excelRow["pep_c"]);
                            // execute it
                            vfpCmd.ExecuteNonQuery();
                            iCounter++;

                            // if (iCounter == 35) break;
                        }
                        // close VFP connection
                        vfpConn.Close();

                }
            }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + " pe rand " + iCounter.ToString() + " !");
            }

            return dbfTableName + ".dbf";
        }

        private static bool ClearDataReader(object dr)
        {
            return string.IsNullOrEmpty(dr.ToString()) || dr.ToString() == "Null";
        }

        public static DataTable ExcelToDataTable(MemoryStream memoryStream)
        {
            DataTable dt;
            using (var package = new ExcelPackage(memoryStream))
            {
                dt = package.ToDataTable();
            }

            return dt;
        }

        //-------------------------------- DataSetIntoDBF ---------------------------------------------------------------------------------------------
        //https://stackoverflow.com/questions/322792/how-can-i-save-a-datatable-to-a-dbf

        public static string Path;

        public static void DataSetIntoDBF(string fileName, DataSet dataSet)
        {
            ArrayList list = new ArrayList();

            if (File.Exists(Path + fileName + ".dbf"))
            {
                File.Delete(Path + fileName + ".dbf");
            }

            string createSql = "create table " + fileName + " (";

            foreach (DataColumn dc in dataSet.Tables[0].Columns)
            {
                string fieldName = dc.ColumnName;

                string type = dc.DataType.ToString();

                switch (type)
                {
                    case "System.String":
                        type = "varchar(100)";
                        break;

                    case "System.Boolean":
                        type = "varchar(10)";
                        break;

                    case "System.Int32":
                        type = "int";
                        break;

                    case "System.Double":
                        type = "Double";
                        break;

                    case "System.DateTime":
                        type = "TimeStamp";
                        break;
                }

                createSql = createSql + "[" + fieldName + "]" + " " + type + ",";

                list.Add(fieldName);
            }

            createSql = createSql.Substring(0, createSql.Length - 1) + ")";

            OleDbConnection con = new OleDbConnection(GetConnection(Path));

            OleDbCommand cmd = new OleDbCommand();

            cmd.Connection = con;

            con.Open();

            cmd.CommandText = createSql;

            cmd.ExecuteNonQuery();

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                string insertSql = "insert into " + fileName + " values(";

                for (int i = 0; i < list.Count; i++)
                {
                    insertSql = insertSql + "'" + ReplaceEscape(row[list[i].ToString()].ToString()) + "',";
                }

                insertSql = insertSql.Substring(0, insertSql.Length - 1) + ")";

                cmd.CommandText = insertSql;

                cmd.ExecuteNonQuery();
            }

            con.Close();
        }

        private static string GetConnection(string path)
        {
            return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=dBASE IV;";
        }

        public static string ReplaceEscape(string str)
        {
            str = str.Replace("'", "''");
            return str;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------

    }

    public static class ExcelPackageExtensions
    {
        public static DataTable ToDataTable(this ExcelPackage package)
        {
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            DataTable table = new DataTable();
            foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                table.Columns.Add(firstRowCell.Text);
            }
            for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {
                var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                var newRow = table.NewRow();
                foreach (var cell in row)
                {
                    newRow[cell.Start.Column - 1] = cell.Text;
                }
                table.Rows.Add(newRow);
            }
            return table;
        }
    }
}
