using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.IO;

namespace WinProductImage
{
    class ProductDB : IDisposable
    {
        MySqlConnection conn;
        public ProductDB()
        {
            string strConn = ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;
            conn = new MySqlConnection(strConn);
            conn.Open();

        }

        public void Dispose()
        {
            conn.Close();
        }

        /// <summary>
        /// 등록된 제품목록을 조회
        /// </summary>
        /// <returns></returns>
        public DataTable GetProductList()
        {
            string sql = @"select productID, productName, productPrice
                        from class_product; ";

            MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();

            da.Fill(dt);
            return dt;
        }
        public bool AddProductImage(int pid, string Path)
        {
            string sql = @"insert into class_productimage ( productID,productImgFileName)
                        values (@pid,@path);";
            MySqlCommand cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.Add("@pid", MySqlDbType.Int32);
            cmd.Parameters["@pid"].Value = pid;

            cmd.Parameters.Add("@path", MySqlDbType.VarChar);
            cmd.Parameters["@path"].Value = Path;

            int iResult = cmd.ExecuteNonQuery();
            if (iResult > 0)
                return true;
            else
                return false;
        }

        public DataTable GetProductImageList(int pid)
        {
            string sql = @"select productImageID, productID, productImage,
                        ifnull(productImgFileName, concat('BLOB이미지/',productImageID)) productImgFileName 
                        from class_productimage
                        where productID = @pid";

            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);

            da.SelectCommand.Parameters.Add("@pid", MySqlDbType.Int32);
            da.SelectCommand.Parameters["@pid"].Value = pid;

            da.Fill(dt);
            return dt;
        }

        public bool DelProductImage(int pid, string path)
        {
            string sql = @"delete from class_productimage 
                            where productID = @pid
                            and productImgFileName = @path";

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.Add("@pid", MySqlDbType.Int32);
            cmd.Parameters["@pid"].Value = pid;

            cmd.Parameters.Add("@path", MySqlDbType.VarChar);
            cmd.Parameters["@path"].Value = path;

            int iRowAffect = cmd.ExecuteNonQuery();
            if (iRowAffect > 0)
                return true;
            else
                return false;
            
        }

        public bool AddProductImage(int pid, byte[] data)
        {
            string sql = @"insert into class_productimage (productID, productImage)
                values (@pid, @data);";

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.Add("@pid", MySqlDbType.Int32);
            cmd.Parameters["@pid"].Value = pid;

            cmd.Parameters.Add("@data", MySqlDbType.Blob);
            cmd.Parameters["@data"].Value = data;

            int iRowsAffet = cmd.ExecuteNonQuery();

            if (iRowsAffet > 0)
                return true;
            else
                return false;
            
        }
    }
}
