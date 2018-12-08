using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;

namespace AdoNetLibrary_Console1
{
    class Program
    {
        SqlConnection conn = null;

        public Program()
        {
            conn = new SqlConnection();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
        }

        static void Main(string[] args)
        {
            Program pr = new Program();
            //pr.InsertQuery();
            //pr.ReadData();
            //pr.ReadData2();
            pr.ReadQueryOne();
        }
        public void ReadQueryOne()
        {
            conn.Open();

            SqlCommand cmd = new SqlCommand();

            string firstName = "Alex";

            SqlParameter param1 = new SqlParameter();
            //param1.ParameterName = "@p1";
            //param1.SqlDbType = System.Data.SqlDbType.NVarChar;

            cmd.Parameters.Add("@p1", SqlDbType.NVarChar).Value = firstName;
            cmd.Connection = conn;
            cmd.CommandText = @"select * from Authors Where FirstName = @p1";

            SqlDataReader rdr = null;
            try
            {
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine(rdr[1] + "\t" + rdr[2]);
                }
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        public void InsertQuery()
        {
            try
            {
                conn.Open();

                string[] inserts = new string[4];

                inserts[0] = @"insert into Authors
                                    (FirstName, LastName)
                                    values ('Roger', 'Zelazny')";
                inserts[1] = @"insert into Authors
                                    (FirstName, LastName)
                                    values ('Alex', 'Tolstov')";
                inserts[2] = @"insert into Authors
                                    (FirstName, LastName)
                                    values ('Petr', 'Petrov')";
                inserts[3] = @"insert into Authors
                                    (FirstName, LastName)
                                    values ('Igor', 'Dolgov')";

                foreach (var insert in inserts)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = insert;

                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if(conn != null)
                {
                    conn.Close();
                }
            }
        }
        public void ReadData()
        {
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Authors", conn);

                rdr = cmd.ExecuteReader();
                int line = 0;
                while (rdr.Read())
                {
                    if(line == 0)
                    {
                        for (int i = 1; i < rdr.FieldCount; i++)
                        {
                            Console.Write(rdr.GetName(i).ToString() + "\t");
                        }
                        Console.WriteLine();
                        Console.WriteLine();
                    }
                    line++;
                    Console.WriteLine(rdr[1] + "\t\t" + rdr[2]);
                }
                Console.WriteLine("Обработано записей: " + line.ToString());
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if(conn != null)
                {
                    conn.Close();
                }
            }
        }
        public void ReadData2()
        {
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Authors; select * from Books", conn);

                rdr = cmd.ExecuteReader();
                int line = 0;
                do
                {
                    while (rdr.Read())
                    {
                        if (line == 0)
                        {
                            for (int i = 0; i < rdr.FieldCount; i++)
                            {
                                Console.Write(rdr.GetName(i).ToString() + "\t");
                            }
                            Console.WriteLine();
                            Console.WriteLine();
                        }
                        line++;
                        Console.WriteLine(rdr[0] + "\t" + rdr[1] + "\t" + rdr[2]);
                    }
                    Console.WriteLine("Total records processed: " + line.ToString());
                } while (rdr.NextResult());
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
    }
}
