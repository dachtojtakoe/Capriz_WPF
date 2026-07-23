using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data.Entity;
using System.Windows;
using System.Diagnostics;
using System.Data;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using Capriz_WPF.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace Capriz_WPF.Database
{
    public static class DB
    {
        //public DbSet<DBData> DBData { get; set; } = null;
        // Путь к файлу базы данных

        static SQLiteConnection connection;
        static SQLiteCommand command;
        static SQLiteTransaction transaction;

        static public bool Connect(string fileName)
        {
            try
            {
                connection = new SQLiteConnection("Data Source=" + fileName + ";Version=3; FailIfMissing=False");
                connection.Open();
                return true;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show($"Ошибка доступа к базе данных. Исключение: {ex.Message}");
                return false;
            }
        }

        static public void StartDb()
        {
            if (Connect("DataBase.sqlite"))
            {
                command = new SQLiteCommand(connection)
                {
                    CommandText = @"
                    CREATE TABLE IF NOT EXISTS [Data] (
                        [id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE,
                        [DateTime] Datetime,
                        [Temperature] TEXT,
                        [Humidity] TEXT,
                        [PressureGPa] TEXT,
                        [PressureRtSt] TEXT,
                        [BarTend] TEXT,
                        [Trend] TEXT,
                        [Speed_K] TEXT,
                        [Direction_K] TEXT,
                        [Speed_I] TEXT,
                        [Direction_I] TEXT,
                        [Direction_2Kmid] TEXT,
                        [Direction_2Kmin] TEXT,
                        [Direction_2Kmax] TEXT,
                        [Direction_10Kmid] TEXT,
                        [Direction_10Kmin] TEXT,
                        [Direction_10Kmax] TEXT,
                        [Direction_2Imid] TEXT,
                        [Direction_2Imin] TEXT,
                        [Direction_2Imax] TEXT,
                        [Direction_10Imid] TEXT,
                        [Direction_10Imin] TEXT,
                        [Direction_10Imax] TEXT,
                        [Speed_2Kmid] TEXT,
                        [Speed_2Kmin] TEXT,
                        [Speed_2Kmax] TEXT,
                        [Speed_10Kmid] TEXT,
                        [Speed_10Kmin] TEXT,
                        [Speed_10Kmax] TEXT,
                        [Speed_2Imid] TEXT,
                        [Speed_2Imin] TEXT,
                        [Speed_2Imax] TEXT,
                        [Speed_10Imid] TEXT,
                        [Speed_10Imin] TEXT,
                        [Speed_10Imax] TEXT,
                        [Visibility10] TEXT,
                        [Visibility1] TEXT,
                        [AmountPrecipitation] TEXT,
                        [ShipSpeed] TEXT,
                        [CourseShip] TEXT,
                        [NGO1] TEXT,
                        [NGO2] TEXT,
                        [NGO3] TEXT,
                        [StatusTemp1] TEXT,
                        [StatusTemp2] TEXT,
                        [StatusHum1] TEXT,
                        [StatusHum2] TEXT,
                        [StatusDirect1] TEXT,
                        [StatusDirect2] TEXT,
                        [StatusSpeed1] TEXT,
                        [StatusSpeed2] TEXT,
                        [StatusSpeedNasal] TEXT,
                        [StatusDirectNasal] TEXT,
                        [StatusPressure] TEXT,
                        [StatusDVGO] TEXT,
                        [AmountClouds] TEXT,
                        [StatusDMDV] TEXT,
                        [Temp_1mid1] TEXT,
                        [Hum_1mid1] TEXT,
                        [Hum_1mid2] TEXT,
                        [Temp_1mid2] TEXT,
                        [Speed_2K1] TEXT,
                        [Speed_2K2] TEXT,
                        [Direction_2Kmid1] TEXT,
                        [Direction_2Kmid2] TEXT,
                        [Speed_Knasal] TEXT,
                        [Direction_Knasal] TEXT,
                        [Direction_2Knasalmid] TEXT,
                        [Direction_2Knasalmin] TEXT,
                        [Direction_2Knasalmax] TEXT,
                        [Direction_10Knasalmid] TEXT,
                        [Direction_10Knasalmin] TEXT,
                        [Direction_10Knasalmax] TEXT,
                        [Speed_2Knasalmid] TEXT,
                        [Speed_2Knasalmin] TEXT,
                        [Speed_2Knasalmax] TEXT,
                        [Speed_10Knasalmid] TEXT,
                        [Speed_10Knasalmin] TEXT,
                        [Speed_10Knasalmax] TEXT
                    );"

                    //CommandText = @"
                    //CREATE TABLE IF NOT EXISTS [Data] (
                    //    [id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE,
                    //    [DateTime] Datetime,
                    //    [Temperature] REAL,
                    //    [Humidity] INTEGER,
                    //    [PressureGPa] REAL,
                    //    [PressureRtSt] REAL,
                    //    [BarTend] REAL,
                    //    [Trend] INTEGER,
                    //    [Speed_K] REAL,
                    //    [Direction_K] INTEGER,
                    //    [Speed_I] REAL,
                    //    [Direction_I] INTEGER,
                    //    [Direction_2Kmid] INTEGER,
                    //    [Direction_2Kmin] INTEGER,
                    //    [Direction_2Kmax] INTEGER,
                    //    [Direction_10Kmid] INTEGER,
                    //    [Direction_10Kmin] INTEGER,
                    //    [Direction_10Kmax] INTEGER,
                    //    [Direction_2Imid] INTEGER,
                    //    [Direction_2Imin] INTEGER,
                    //    [Direction_2Imax] INTEGER,
                    //    [Direction_10Imid] INTEGER,
                    //    [Direction_10Imin] INTEGER,
                    //    [Direction_10Imax] INTEGER,
                    //    [Speed_2Kmid] REAL,
                    //    [Speed_2Kmin] REAL,
                    //    [Speed_2Kmax] REAL,
                    //    [Speed_10Kmid] REAL,
                    //    [Speed_10Kmin] REAL,
                    //    [Speed_10Kmax] REAL,
                    //    [Speed_2Imid] REAL,
                    //    [Speed_2Imin] REAL,
                    //    [Speed_2Imax] REAL,
                    //    [Speed_10Imid] REAL,
                    //    [Speed_10Imin] REAL,
                    //    [Speed_10Imax] REAL,
                    //    [Visibility10] INTEGER,
                    //    [Visibility1] INTEGER,
                    //    [AmountPrecipitation] REAL,
                    //    [ShipSpeed] REAL,
                    //    [CourseShip] INTEGER,
                    //    [NGO1] INTEGER,
                    //    [NGO2] INTEGER,
                    //    [NGO3] INTEGER,
                    //    [StatusTemp1] INTEGER,
                    //    [StatusTemp2] INTEGER,
                    //    [StatusHum1] INTEGER,
                    //    [StatusHum2] INTEGER,
                    //    [StatusDirect1] INTEGER,
                    //    [StatusDirect2] INTEGER,
                    //    [StatusSpeed1] INTEGER,
                    //    [StatusSpeed2] INTEGER,
                    //    [StatusSpeedNasal] INTEGER,
                    //    [StatusDirectNasal] INTEGER,
                    //    [StatusPressure] INTEGER,
                    //    [StatusDVGO] INTEGER,
                    //    [AmountClouds] INTEGER,
                    //    [StatusDMDV] INTEGER,
                    //    [Temp_1mid1] REAL,
                    //    [Hum_1mid1] INTEGER,
                    //    [Hum_1mid2] INTEGER,
                    //    [Temp_1mid2] REAL,
                    //    [Speed_2K1] REAL,
                    //    [Speed_2K2] REAL,
                    //    [Direction_2Kmid1] INTEGER,
                    //    [Direction_2Kmid2] INTEGER,
                    //    [Speed_Knasal] REAL,
                    //    [Direction_Knasal] INTEGER,
                    //    [Direction_2Knasalmid] INTEGER,
                    //    [Direction_2Knasalmin] INTEGER,
                    //    [Direction_2Knasalmax] INTEGER,
                    //    [Direction_10Knasalmid] INTEGER,
                    //    [Direction_10Knasalmin] INTEGER,
                    //    [Direction_10Knasalmax] INTEGER,
                    //    [Speed_2Knasalmid] REAL,
                    //    [Speed_2Knasalmin] REAL,
                    //    [Speed_2Knasalmax] REAL,
                    //    [Speed_10Knasalmid] REAL,
                    //    [Speed_10Knasalmin] REAL,
                    //    [Speed_10Knasalmax] REAL
                    //);"
                };
                command.ExecuteNonQuery();

                command.CommandText = @"
                    CREATE TRIGGER IF NOT EXISTS DeleteOldData
                    AFTER INSERT ON Data
                    BEGIN
                        DELETE FROM Data WHERE DateTime < datetime('now', '-3 months');
                    END;";
                command.ExecuteNonQuery();

            }
        }


        static public void TestWrite(bool BD)
        {

            Stopwatch sw = new Stopwatch();

            if (BD)
            {
                sw.Restart();
                command.CommandText = "INSERT INTO Person (name, family, age) VALUES (:name, :family, :age)";
                transaction = connection.BeginTransaction();//запускаем транзакцию
                try
                {
                    for (int i = 1; i < 10000; i++)
                    {
                        command.Parameters.AddWithValue("name", "Сергей");
                        command.Parameters.AddWithValue("family", "Петров");
                        command.Parameters.AddWithValue("age", i);
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit(); //применяем изменения
                    sw.Stop();
                    MessageBox.Show("Добавлены 10 000 новых строк за: " + sw.Elapsed);
                }
                catch
                {
                    transaction.Rollback(); //откатываем изменения, если произошла ошибка
                    throw;
                }
            }

            sw.Restart();

            command.CommandText = "SELECT * FROM Person";
            DataTable data = new DataTable();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(data);
            sw.Stop();
            MessageBox.Show($"Прочитано {data.Rows.Count} записей из таблицы БД за {sw.Elapsed}");

            //sw.Restart();
            //command.CommandText = "SELECT * FROM Person";
            //DataTable data = new DataTable();
            //SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            //adapter.Fill(data);
            //MessageBox.Show($"Прочитано {data.Rows.Count} записей из таблицы БД за {sw.Elapsed}");
            //foreach (DataRow row in data.Rows)
            //{
            //    MessageBox.Show($"id = {row.Field<long>("id")} name = {row.Field<string>("name")} family = {row.Field<string>("family")}");
            //}
        }

        static public void WriteDataToDB(string param)
        {
            var columns = param.Split('\t', '\x02', '\x03', '*');
            
            var Temperature = columns[1].Contains("/") ? "Н.Д." : columns[1];
            var Humidity = columns[2].Contains("/") ? "Н.Д." : columns[2];
            var PressureGPa = columns[3].Contains("/") ? "Н.Д." : columns[3];
            var PressureRtSt = columns[4].Contains("/") ? "Н.Д." : columns[4];
            var BarTend = columns[5].Contains("/") ? "Н.Д." : columns[5];
            var Trend = columns[6].Contains("/") ? "Н.Д." : columns[6];
            var Speed_K = columns[7].Contains("/") ? "Н.Д." : columns[7];
            var Direction_K = columns[8].Contains("/") ? "Н.Д." : columns[8];
            var Speed_I = columns[9].Contains("/") ? "Н.Д." : columns[9];
            var Direction_I = columns[10].Contains("/") ? "Н.Д." : columns[10];
            var Direction_2Kmid = columns[11].Contains("/") ? "Н.Д." : columns[11];
            var Direction_2Kmin = columns[12].Contains("/") ? "Н.Д." : columns[12];
            var Direction_2Kmax = columns[13].Contains("/") ? "Н.Д." : columns[13];
            var Direction_10Kmid = columns[14].Contains("/") ? "Н.Д." : columns[14];
            var Direction_10Kmin = columns[15].Contains("/") ? "Н.Д." : columns[15];
            var Direction_10Kmax = columns[16].Contains("/") ? "Н.Д." : columns[16];
            var Direction_2Imid = columns[17].Contains("/") ? "Н.Д." : columns[17];
            var Direction_2Imin = columns[18].Contains("/") ? "Н.Д." : columns[18];
            var Direction_2Imax = columns[19].Contains("/") ? "Н.Д." : columns[19];
            var Direction_10Imid = columns[20].Contains("/") ? "Н.Д." : columns[20];
            var Direction_10Imin = columns[21].Contains("/") ? "Н.Д." : columns[21];
            var Direction_10Imax = columns[22].Contains("/") ? "Н.Д." : columns[22];
            var Speed_2Kmid = columns[23].Contains("/") ? "Н.Д." : columns[23];
            var Speed_2Kmin = columns[24].Contains("/") ? "Н.Д." : columns[24];
            var Speed_2Kmax = columns[25].Contains("/") ? "Н.Д." : columns[25];
            var Speed_10Kmid = columns[26].Contains("/") ? "Н.Д." : columns[26];
            var Speed_10Kmin = columns[27].Contains("/") ? "Н.Д." : columns[27];
            var Speed_10Kmax = columns[28].Contains("/") ? "Н.Д." : columns[28];
            var Speed_2Imid = columns[29].Contains("/") ? "Н.Д." : columns[29];
            var Speed_2Imin = columns[30].Contains("/") ? "Н.Д." : columns[30];
            var Speed_2Imax = columns[31].Contains("/") ? "Н.Д." : columns[31];
            var Speed_10Imid = columns[32].Contains("/") ? "Н.Д." : columns[32];
            var Speed_10Imin = columns[33].Contains("/") ? "Н.Д." : columns[33];
            var Speed_10Imax = columns[34].Contains("/") ? "Н.Д." : columns[34];
            var Visibility10 = columns[35].Contains("/") ? "Н.Д." : columns[35];
            var Visibility1 = columns[36].Contains("/") ? "Н.Д." : columns[36];
            var AmountPrecipitation = columns[37].Contains("/") ? "Н.Д." : columns[37];
            var ShipSpeed = columns[38].Contains("/") ? "Н.Д." : columns[38];
            var CourseShip = columns[39].Contains("/") ? "Н.Д." : columns[39];
            var NGO1 = columns[40].Contains("/") ? "Н.Д." : columns[40];
            var NGO2 = columns[41].Contains("/") ? "Н.Д." : columns[41];
            var NGO3 = columns[42].Contains("/") ? "Н.Д." : columns[42];
            var StatusTemp1 = columns[43].Contains("/") ? "/" : columns[43];
            var StatusTemp2 = columns[44].Contains("/") ? "/" : columns[44];
            var StatusHum1 = columns[45].Contains("/") ? "/" : columns[45];
            var StatusHum2 = columns[46].Contains("/") ? "/" : columns[46];
            var StatusDirect1 = columns[47].Contains("/") ? "/" : columns[47];
            var StatusDirect2 = columns[48].Contains("/") ? "/" : columns[48];
            var StatusSpeed1 = columns[49].Contains("/") ? "/" : columns[49];
            var StatusSpeed2 = columns[50].Contains("/") ? "/" : columns[50];
            var StatusSpeedNasal = columns[51].Contains("/") ? "/" : columns[51];
            var StatusDirectNasal = columns[52].Contains("/") ? "/" : columns[52];
            var StatusPressure = columns[53].Contains("/") ? "/" : columns[53];
            var StatusDVGO = columns[54].Contains("/") ? "/" : columns[54];
            var AmountClouds = columns[55].Contains("/") ? "Н.Д." : columns[55];
            var StatusDMDV = columns[56].Contains("/") ? "/" : columns[56];
            var Temp_1mid1 = columns[57].Contains("/") ? "Н.Д." : columns[57];
            var Hum_1mid1 = columns[58].Contains("/") ? "Н.Д." : columns[58];
            var Hum_1mid2 = columns[59].Contains("/") ? "Н.Д." : columns[59];
            var Temp_1mid2 = columns[60].Contains("/") ? "Н.Д." : columns[60];
            var Speed_2K1 = columns[61].Contains("/") ? "Н.Д." : columns[61];
            var Speed_2K2 = columns[62].Contains("/") ? "Н.Д." : columns[62];
            var Direction_2Kmid1 = columns[63].Contains("/") ? "Н.Д." : columns[63];
            var Direction_2Kmid2 = columns[64].Contains("/") ? "Н.Д." : columns[64];
            var Speed_Knasal = columns[65].Contains("/") ? "Н.Д." : columns[65];
            var Direction_Knasal = columns[66].Contains("/") ? "Н.Д." : columns[66];
            var Direction_2Knasalmid = columns[67].Contains("/") ? "Н.Д." : columns[67];
            var Direction_2Knasalmin = columns[68].Contains("/") ? "Н.Д." : columns[68];
            var Direction_2Knasalmax = columns[69].Contains("/") ? "Н.Д." : columns[69];
            var Direction_10Knasalmid = columns[70].Contains("/") ? "Н.Д." : columns[70];
            var Direction_10Knasalmin = columns[71].Contains("/") ? "Н.Д." : columns[71];
            var Direction_10Knasalmax = columns[72].Contains("/") ? "Н.Д." : columns[72];
            var Speed_2Knasalmid = columns[73].Contains("/") ? "Н.Д." : columns[73];
            var Speed_2Knasalmin = columns[74].Contains("/") ? "Н.Д." : columns[74];
            var Speed_2Knasalmax = columns[75].Contains("/") ? "Н.Д." : columns[75];
            var Speed_10Knasalmid = columns[76].Contains("/") ? "Н.Д." : columns[76];
            var Speed_10Knasalmin = columns[77].Contains("/") ? "Н.Д." : columns[77];
            var Speed_10Knasalmax = columns[78].Contains("/") ? "Н.Д." : columns[78];


            command = new SQLiteCommand(connection);

            command.CommandText = @"
            INSERT INTO Data (
                DateTime, Temperature, Humidity, PressureGPa, PressureRtSt, BarTend, Trend, Speed_K, Direction_K, Speed_I, Direction_I,
                Direction_2Kmid, Direction_2Kmin, Direction_2Kmax, Direction_10Kmid, Direction_10Kmin, Direction_10Kmax, Direction_2Imid,
                Direction_2Imin, Direction_2Imax, Direction_10Imid, Direction_10Imin, Direction_10Imax, Speed_2Kmid, Speed_2Kmin, Speed_2Kmax,
                Speed_10Kmid, Speed_10Kmin, Speed_10Kmax, Speed_2Imid, Speed_2Imin, Speed_2Imax, Speed_10Imid, Speed_10Imin, Speed_10Imax,
                Visibility10, Visibility1, AmountPrecipitation, ShipSpeed, CourseShip, NGO1, NGO2, NGO3, StatusTemp1, StatusTemp2,
                StatusHum1, StatusHum2, StatusDirect1, StatusDirect2, StatusSpeed1, StatusSpeed2, StatusSpeedNasal, StatusDirectNasal,
                StatusPressure, StatusDVGO, AmountClouds, StatusDMDV, Temp_1mid1, Hum_1mid1, Hum_1mid2, Temp_1mid2, Speed_2K1, Speed_2K2,
                Direction_2Kmid1, Direction_2Kmid2, Speed_Knasal, Direction_Knasal, Direction_2Knasalmid, Direction_2Knasalmin, Direction_2Knasalmax,
                Direction_10Knasalmid, Direction_10Knasalmin, Direction_10Knasalmax, Speed_2Knasalmid, Speed_2Knasalmin, Speed_2Knasalmax,
                Speed_10Knasalmid, Speed_10Knasalmin, Speed_10Knasalmax
            ) VALUES (
                @DateTime, @Temperature, @Humidity, @PressureGPa, @PressureRtSt, @BarTend, @Trend, @Speed_K, @Direction_K, @Speed_I, @Direction_I,
                @Direction_2Kmid, @Direction_2Kmin, @Direction_2Kmax, @Direction_10Kmid, @Direction_10Kmin, @Direction_10Kmax, @Direction_2Imid,
                @Direction_2Imin, @Direction_2Imax, @Direction_10Imid, @Direction_10Imin, @Direction_10Imax, @Speed_2Kmid, @Speed_2Kmin, @Speed_2Kmax,
                @Speed_10Kmid, @Speed_10Kmin, @Speed_10Kmax, @Speed_2Imid, @Speed_2Imin, @Speed_2Imax, @Speed_10Imid, @Speed_10Imin, @Speed_10Imax,
                @Visibility10, @Visibility1, @AmountPrecipitation, @ShipSpeed, @CourseShip, @NGO1, @NGO2, @NGO3, @StatusTemp1, @StatusTemp2,
                @StatusHum1, @StatusHum2, @StatusDirect1, @StatusDirect2, @StatusSpeed1, @StatusSpeed2, @StatusSpeedNasal, @StatusDirectNasal,
                @StatusPressure, @StatusDVGO, @AmountClouds, @StatusDMDV, @Temp_1mid1, @Hum_1mid1, @Hum_1mid2, @Temp_1mid2, @Speed_2K1, @Speed_2K2,
                @Direction_2Kmid1, @Direction_2Kmid2, @Speed_Knasal, @Direction_Knasal, @Direction_2Knasalmid, @Direction_2Knasalmin, @Direction_2Knasalmax,
                @Direction_10Knasalmid, @Direction_10Knasalmin, @Direction_10Knasalmax, @Speed_2Knasalmid, @Speed_2Knasalmin, @Speed_2Knasalmax,
                @Speed_10Knasalmid, @Speed_10Knasalmin, @Speed_10Knasalmax
            );";

            // Добавление параметров
            command.Parameters.AddWithValue("@DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            //command.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
            //command.Parameters.AddWithValue("@Time", DateTime.Now.ToString("HH:mm:ss"));
            command.Parameters.AddWithValue("@Temperature", Temperature);
            command.Parameters.AddWithValue("@Humidity", Humidity);
            command.Parameters.AddWithValue("@PressureGPa", PressureGPa);
            command.Parameters.AddWithValue("@PressureRtSt", PressureRtSt);
            command.Parameters.AddWithValue("@BarTend", BarTend);
            command.Parameters.AddWithValue("@Trend", Trend);
            command.Parameters.AddWithValue("@Speed_K", Speed_K);
            command.Parameters.AddWithValue("@Direction_K", Direction_K);
            command.Parameters.AddWithValue("@Speed_I", Speed_I);
            command.Parameters.AddWithValue("@Direction_I", Direction_I);
            command.Parameters.AddWithValue("@Direction_2Kmid", Direction_2Kmid);
            command.Parameters.AddWithValue("@Direction_2Kmin", Direction_2Kmin);
            command.Parameters.AddWithValue("@Direction_2Kmax", Direction_2Kmax);
            command.Parameters.AddWithValue("@Direction_10Kmid", Direction_10Kmid);
            command.Parameters.AddWithValue("@Direction_10Kmin", Direction_10Kmin);
            command.Parameters.AddWithValue("@Direction_10Kmax", Direction_10Kmax);
            command.Parameters.AddWithValue("@Direction_2Imid", Direction_2Imid);
            command.Parameters.AddWithValue("@Direction_2Imin", Direction_2Imin);
            command.Parameters.AddWithValue("@Direction_2Imax", Direction_2Imax);
            command.Parameters.AddWithValue("@Direction_10Imid", Direction_10Imid);
            command.Parameters.AddWithValue("@Direction_10Imin", Direction_10Imin);
            command.Parameters.AddWithValue("@Direction_10Imax", Direction_10Imax);
            command.Parameters.AddWithValue("@Speed_2Kmid", Speed_2Kmid);
            command.Parameters.AddWithValue("@Speed_2Kmin", Speed_2Kmin);
            command.Parameters.AddWithValue("@Speed_2Kmax", Speed_2Kmax);
            command.Parameters.AddWithValue("@Speed_10Kmid", Speed_10Kmid);
            command.Parameters.AddWithValue("@Speed_10Kmin", Speed_10Kmin);
            command.Parameters.AddWithValue("@Speed_10Kmax", Speed_10Kmax);
            command.Parameters.AddWithValue("@Speed_2Imid", Speed_2Imid);
            command.Parameters.AddWithValue("@Speed_2Imin", Speed_2Imin);
            command.Parameters.AddWithValue("@Speed_2Imax", Speed_2Imax);
            command.Parameters.AddWithValue("@Speed_10Imid", Speed_10Imid);
            command.Parameters.AddWithValue("@Speed_10Imin", Speed_10Imin);
            command.Parameters.AddWithValue("@Speed_10Imax", Speed_10Imax);
            command.Parameters.AddWithValue("@Visibility10", Visibility10);
            command.Parameters.AddWithValue("@Visibility1", Visibility1);
            command.Parameters.AddWithValue("@AmountPrecipitation", AmountPrecipitation);
            command.Parameters.AddWithValue("@ShipSpeed", ShipSpeed);
            command.Parameters.AddWithValue("@CourseShip", CourseShip);
            command.Parameters.AddWithValue("@NGO1", NGO1);
            command.Parameters.AddWithValue("@NGO2", NGO2);
            command.Parameters.AddWithValue("@NGO3", NGO3);
            command.Parameters.AddWithValue("@StatusTemp1", StatusTemp1);
            command.Parameters.AddWithValue("@StatusTemp2", StatusTemp2);
            command.Parameters.AddWithValue("@StatusHum1", StatusHum1);
            command.Parameters.AddWithValue("@StatusHum2", StatusHum2);
            command.Parameters.AddWithValue("@StatusDirect1", StatusDirect1);
            command.Parameters.AddWithValue("@StatusDirect2", StatusDirect2);
            command.Parameters.AddWithValue("@StatusSpeed1", StatusSpeed1);
            command.Parameters.AddWithValue("@StatusSpeed2", StatusSpeed2);
            command.Parameters.AddWithValue("@StatusSpeedNasal", StatusSpeedNasal);
            command.Parameters.AddWithValue("@StatusDirectNasal", StatusDirectNasal);
            command.Parameters.AddWithValue("@StatusPressure", StatusPressure);
            command.Parameters.AddWithValue("@StatusDVGO", StatusDVGO);
            command.Parameters.AddWithValue("@AmountClouds", AmountClouds);
            command.Parameters.AddWithValue("@StatusDMDV", StatusDMDV);
            command.Parameters.AddWithValue("@Temp_1mid1", Temp_1mid1);
            command.Parameters.AddWithValue("@Hum_1mid1", Hum_1mid1);
            command.Parameters.AddWithValue("@Hum_1mid2", Hum_1mid2);
            command.Parameters.AddWithValue("@Temp_1mid2", Temp_1mid2);
            command.Parameters.AddWithValue("@Speed_2K1", Speed_2K1);
            command.Parameters.AddWithValue("@Speed_2K2", Speed_2K2);
            command.Parameters.AddWithValue("@Direction_2Kmid1", Direction_2Kmid1);
            command.Parameters.AddWithValue("@Direction_2Kmid2", Direction_2Kmid2);
            command.Parameters.AddWithValue("@Speed_Knasal", Speed_Knasal);
            command.Parameters.AddWithValue("@Direction_Knasal", Direction_Knasal);
            command.Parameters.AddWithValue("@Direction_2Knasalmid", Direction_2Knasalmid);
            command.Parameters.AddWithValue("@Direction_2Knasalmin", Direction_2Knasalmin);
            command.Parameters.AddWithValue("@Direction_2Knasalmax", Direction_2Knasalmax);
            command.Parameters.AddWithValue("@Direction_10Knasalmid", Direction_10Knasalmid);
            command.Parameters.AddWithValue("@Direction_10Knasalmin", Direction_10Knasalmin);
            command.Parameters.AddWithValue("@Direction_10Knasalmax", Direction_10Knasalmax);
            command.Parameters.AddWithValue("@Speed_2Knasalmid", Speed_2Knasalmid);
            command.Parameters.AddWithValue("@Speed_2Knasalmin", Speed_2Knasalmin);
            command.Parameters.AddWithValue("@Speed_2Knasalmax", Speed_2Knasalmax);
            command.Parameters.AddWithValue("@Speed_10Knasalmid", Speed_10Knasalmid);
            command.Parameters.AddWithValue("@Speed_10Knasalmin", Speed_10Knasalmin);
            command.Parameters.AddWithValue("@Speed_10Knasalmax", Speed_10Knasalmax);

            command.ExecuteNonQuery();
        }

        static public void WriteDataToDBALot(string param, int day)
        {
            var columns = param.Split('\t', '\x02', '\x03', '*');

            var Temperature = columns[1].Contains("/") ? "Н.Д." : columns[1];
            var Humidity = columns[2].Contains("/") ? "Н.Д." : columns[2];
            var PressureGPa = columns[3].Contains("/") ? "Н.Д." : columns[3];
            var PressureRtSt = columns[4].Contains("/") ? "Н.Д." : columns[4];
            var BarTend = columns[5].Contains("/") ? "Н.Д." : columns[5];
            var Trend = columns[6].Contains("/") ? "Н.Д." : columns[6];
            var Speed_K = columns[7].Contains("/") ? "Н.Д." : columns[7];
            var Direction_K = columns[8].Contains("/") ? "Н.Д." : columns[8];
            var Speed_I = columns[9].Contains("/") ? "Н.Д." : columns[9];
            var Direction_I = columns[10].Contains("/") ? "Н.Д." : columns[10];
            var Direction_2Kmid = columns[11].Contains("/") ? "Н.Д." : columns[11];
            var Direction_2Kmin = columns[12].Contains("/") ? "Н.Д." : columns[12];
            var Direction_2Kmax = columns[13].Contains("/") ? "Н.Д." : columns[13];
            var Direction_10Kmid = columns[14].Contains("/") ? "Н.Д." : columns[14];
            var Direction_10Kmin = columns[15].Contains("/") ? "Н.Д." : columns[15];
            var Direction_10Kmax = columns[16].Contains("/") ? "Н.Д." : columns[16];
            var Direction_2Imid = columns[17].Contains("/") ? "Н.Д." : columns[17];
            var Direction_2Imin = columns[18].Contains("/") ? "Н.Д." : columns[18];
            var Direction_2Imax = columns[19].Contains("/") ? "Н.Д." : columns[19];
            var Direction_10Imid = columns[20].Contains("/") ? "Н.Д." : columns[20];
            var Direction_10Imin = columns[21].Contains("/") ? "Н.Д." : columns[21];
            var Direction_10Imax = columns[22].Contains("/") ? "Н.Д." : columns[22];
            var Speed_2Kmid = columns[23].Contains("/") ? "Н.Д." : columns[23];
            var Speed_2Kmin = columns[24].Contains("/") ? "Н.Д." : columns[24];
            var Speed_2Kmax = columns[25].Contains("/") ? "Н.Д." : columns[25];
            var Speed_10Kmid = columns[26].Contains("/") ? "Н.Д." : columns[26];
            var Speed_10Kmin = columns[27].Contains("/") ? "Н.Д." : columns[27];
            var Speed_10Kmax = columns[28].Contains("/") ? "Н.Д." : columns[28];
            var Speed_2Imid = columns[29].Contains("/") ? "Н.Д." : columns[29];
            var Speed_2Imin = columns[30].Contains("/") ? "Н.Д." : columns[30];
            var Speed_2Imax = columns[31].Contains("/") ? "Н.Д." : columns[31];
            var Speed_10Imid = columns[32].Contains("/") ? "Н.Д." : columns[32];
            var Speed_10Imin = columns[33].Contains("/") ? "Н.Д." : columns[33];
            var Speed_10Imax = columns[34].Contains("/") ? "Н.Д." : columns[34];
            var Visibility10 = columns[35].Contains("/") ? "Н.Д." : columns[35];
            var Visibility1 = columns[36].Contains("/") ? "Н.Д." : columns[36];
            var AmountPrecipitation = columns[37].Contains("/") ? "Н.Д." : columns[37];
            var ShipSpeed = columns[38].Contains("/") ? "Н.Д." : columns[38];
            var CourseShip = columns[39].Contains("/") ? "Н.Д." : columns[39];
            var NGO1 = columns[40].Contains("/") ? "Н.Д." : columns[40];
            var NGO2 = columns[41].Contains("/") ? "Н.Д." : columns[41];
            var NGO3 = columns[42].Contains("/") ? "Н.Д." : columns[42];
            var StatusTemp1 = columns[43].Contains("/") ? "/" : columns[43];
            var StatusTemp2 = columns[44].Contains("/") ? "/" : columns[44];
            var StatusHum1 = columns[45].Contains("/") ? "/" : columns[45];
            var StatusHum2 = columns[46].Contains("/") ? "/" : columns[46];
            var StatusDirect1 = columns[47].Contains("/") ? "/" : columns[47];
            var StatusDirect2 = columns[48].Contains("/") ? "/" : columns[48];
            var StatusSpeed1 = columns[49].Contains("/") ? "/" : columns[49];
            var StatusSpeed2 = columns[50].Contains("/") ? "/" : columns[50];
            var StatusSpeedNasal = columns[51].Contains("/") ? "/" : columns[51];
            var StatusDirectNasal = columns[52].Contains("/") ? "/" : columns[52];
            var StatusPressure = columns[53].Contains("/") ? "/" : columns[53];
            var StatusDVGO = columns[54].Contains("/") ? "/" : columns[54];
            var AmountClouds = columns[55].Contains("/") ? "Н.Д." : columns[55];
            var StatusDMDV = columns[56].Contains("/") ? "/" : columns[56];
            var Temp_1mid1 = columns[57].Contains("/") ? "Н.Д." : columns[57];
            var Hum_1mid1 = columns[58].Contains("/") ? "Н.Д." : columns[58];
            var Hum_1mid2 = columns[59].Contains("/") ? "Н.Д." : columns[59];
            var Temp_1mid2 = columns[60].Contains("/") ? "Н.Д." : columns[60];
            var Speed_2K1 = columns[61].Contains("/") ? "Н.Д." : columns[61];
            var Speed_2K2 = columns[62].Contains("/") ? "Н.Д." : columns[62];
            var Direction_2Kmid1 = columns[63].Contains("/") ? "Н.Д." : columns[63];
            var Direction_2Kmid2 = columns[64].Contains("/") ? "Н.Д." : columns[64];
            var Speed_Knasal = columns[65].Contains("/") ? "Н.Д." : columns[65];
            var Direction_Knasal = columns[66].Contains("/") ? "Н.Д." : columns[66];
            var Direction_2Knasalmid = columns[67].Contains("/") ? "Н.Д." : columns[67];
            var Direction_2Knasalmin = columns[68].Contains("/") ? "Н.Д." : columns[68];
            var Direction_2Knasalmax = columns[69].Contains("/") ? "Н.Д." : columns[69];
            var Direction_10Knasalmid = columns[70].Contains("/") ? "Н.Д." : columns[70];
            var Direction_10Knasalmin = columns[71].Contains("/") ? "Н.Д." : columns[71];
            var Direction_10Knasalmax = columns[72].Contains("/") ? "Н.Д." : columns[72];
            var Speed_2Knasalmid = columns[73].Contains("/") ? "Н.Д." : columns[73];
            var Speed_2Knasalmin = columns[74].Contains("/") ? "Н.Д." : columns[74];
            var Speed_2Knasalmax = columns[75].Contains("/") ? "Н.Д." : columns[75];
            var Speed_10Knasalmid = columns[76].Contains("/") ? "Н.Д." : columns[76];
            var Speed_10Knasalmin = columns[77].Contains("/") ? "Н.Д." : columns[77];
            var Speed_10Knasalmax = columns[78].Contains("/") ? "Н.Д." : columns[78];

            DateTime currentTime = new DateTime(2024, 8, 26);
            currentTime = currentTime.AddDays(-92);
            currentTime = currentTime.AddDays(day);

            for (int i = 0; i < 144; i++)
            {

                command = new SQLiteCommand(connection);

                command.CommandText = @"
                INSERT INTO Data (
                    DateTime, Temperature, Humidity, PressureGPa, PressureRtSt, BarTend, Trend, Speed_K, Direction_K, Speed_I, Direction_I,
                    Direction_2Kmid, Direction_2Kmin, Direction_2Kmax, Direction_10Kmid, Direction_10Kmin, Direction_10Kmax, Direction_2Imid,
                    Direction_2Imin, Direction_2Imax, Direction_10Imid, Direction_10Imin, Direction_10Imax, Speed_2Kmid, Speed_2Kmin, Speed_2Kmax,
                    Speed_10Kmid, Speed_10Kmin, Speed_10Kmax, Speed_2Imid, Speed_2Imin, Speed_2Imax, Speed_10Imid, Speed_10Imin, Speed_10Imax,
                    Visibility10, Visibility1, AmountPrecipitation, ShipSpeed, CourseShip, NGO1, NGO2, NGO3, StatusTemp1, StatusTemp2,
                    StatusHum1, StatusHum2, StatusDirect1, StatusDirect2, StatusSpeed1, StatusSpeed2, StatusSpeedNasal, StatusDirectNasal,
                    StatusPressure, StatusDVGO, AmountClouds, StatusDMDV, Temp_1mid1, Hum_1mid1, Hum_1mid2, Temp_1mid2, Speed_2K1, Speed_2K2,
                    Direction_2Kmid1, Direction_2Kmid2, Speed_Knasal, Direction_Knasal, Direction_2Knasalmid, Direction_2Knasalmin, Direction_2Knasalmax,
                    Direction_10Knasalmid, Direction_10Knasalmin, Direction_10Knasalmax, Speed_2Knasalmid, Speed_2Knasalmin, Speed_2Knasalmax,
                    Speed_10Knasalmid, Speed_10Knasalmin, Speed_10Knasalmax
                ) VALUES (
                    @DateTime, @Temperature, @Humidity, @PressureGPa, @PressureRtSt, @BarTend, @Trend, @Speed_K, @Direction_K, @Speed_I, @Direction_I,
                    @Direction_2Kmid, @Direction_2Kmin, @Direction_2Kmax, @Direction_10Kmid, @Direction_10Kmin, @Direction_10Kmax, @Direction_2Imid,
                    @Direction_2Imin, @Direction_2Imax, @Direction_10Imid, @Direction_10Imin, @Direction_10Imax, @Speed_2Kmid, @Speed_2Kmin, @Speed_2Kmax,
                    @Speed_10Kmid, @Speed_10Kmin, @Speed_10Kmax, @Speed_2Imid, @Speed_2Imin, @Speed_2Imax, @Speed_10Imid, @Speed_10Imin, @Speed_10Imax,
                    @Visibility10, @Visibility1, @AmountPrecipitation, @ShipSpeed, @CourseShip, @NGO1, @NGO2, @NGO3, @StatusTemp1, @StatusTemp2,
                    @StatusHum1, @StatusHum2, @StatusDirect1, @StatusDirect2, @StatusSpeed1, @StatusSpeed2, @StatusSpeedNasal, @StatusDirectNasal,
                    @StatusPressure, @StatusDVGO, @AmountClouds, @StatusDMDV, @Temp_1mid1, @Hum_1mid1, @Hum_1mid2, @Temp_1mid2, @Speed_2K1, @Speed_2K2,
                    @Direction_2Kmid1, @Direction_2Kmid2, @Speed_Knasal, @Direction_Knasal, @Direction_2Knasalmid, @Direction_2Knasalmin, @Direction_2Knasalmax,
                    @Direction_10Knasalmid, @Direction_10Knasalmin, @Direction_10Knasalmax, @Speed_2Knasalmid, @Speed_2Knasalmin, @Speed_2Knasalmax,
                    @Speed_10Knasalmid, @Speed_10Knasalmin, @Speed_10Knasalmax
                );";

                // Добавление параметров
                command.Parameters.AddWithValue("@DateTime", currentTime.ToString("yyyy-MM-dd HH:mm:ss"));

                //command.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                //command.Parameters.AddWithValue("@Time", DateTime.Now.ToString("HH:mm:ss"));
                command.Parameters.AddWithValue("@Temperature", Temperature);
                command.Parameters.AddWithValue("@Humidity", Humidity);
                command.Parameters.AddWithValue("@PressureGPa", PressureGPa);
                command.Parameters.AddWithValue("@PressureRtSt", PressureRtSt);
                command.Parameters.AddWithValue("@BarTend", BarTend);
                command.Parameters.AddWithValue("@Trend", Trend);
                command.Parameters.AddWithValue("@Speed_K", Speed_K);
                command.Parameters.AddWithValue("@Direction_K", Direction_K);
                command.Parameters.AddWithValue("@Speed_I", Speed_I);
                command.Parameters.AddWithValue("@Direction_I", Direction_I);
                command.Parameters.AddWithValue("@Direction_2Kmid", Direction_2Kmid);
                command.Parameters.AddWithValue("@Direction_2Kmin", Direction_2Kmin);
                command.Parameters.AddWithValue("@Direction_2Kmax", Direction_2Kmax);
                command.Parameters.AddWithValue("@Direction_10Kmid", Direction_10Kmid);
                command.Parameters.AddWithValue("@Direction_10Kmin", Direction_10Kmin);
                command.Parameters.AddWithValue("@Direction_10Kmax", Direction_10Kmax);
                command.Parameters.AddWithValue("@Direction_2Imid", Direction_2Imid);
                command.Parameters.AddWithValue("@Direction_2Imin", Direction_2Imin);
                command.Parameters.AddWithValue("@Direction_2Imax", Direction_2Imax);
                command.Parameters.AddWithValue("@Direction_10Imid", Direction_10Imid);
                command.Parameters.AddWithValue("@Direction_10Imin", Direction_10Imin);
                command.Parameters.AddWithValue("@Direction_10Imax", Direction_10Imax);
                command.Parameters.AddWithValue("@Speed_2Kmid", Speed_2Kmid);
                command.Parameters.AddWithValue("@Speed_2Kmin", Speed_2Kmin);
                command.Parameters.AddWithValue("@Speed_2Kmax", Speed_2Kmax);
                command.Parameters.AddWithValue("@Speed_10Kmid", Speed_10Kmid);
                command.Parameters.AddWithValue("@Speed_10Kmin", Speed_10Kmin);
                command.Parameters.AddWithValue("@Speed_10Kmax", Speed_10Kmax);
                command.Parameters.AddWithValue("@Speed_2Imid", Speed_2Imid);
                command.Parameters.AddWithValue("@Speed_2Imin", Speed_2Imin);
                command.Parameters.AddWithValue("@Speed_2Imax", Speed_2Imax);
                command.Parameters.AddWithValue("@Speed_10Imid", Speed_10Imid);
                command.Parameters.AddWithValue("@Speed_10Imin", Speed_10Imin);
                command.Parameters.AddWithValue("@Speed_10Imax", Speed_10Imax);
                command.Parameters.AddWithValue("@Visibility10", Visibility10);
                command.Parameters.AddWithValue("@Visibility1", Visibility1);
                command.Parameters.AddWithValue("@AmountPrecipitation", AmountPrecipitation);
                command.Parameters.AddWithValue("@ShipSpeed", ShipSpeed);
                command.Parameters.AddWithValue("@CourseShip", CourseShip);
                command.Parameters.AddWithValue("@NGO1", NGO1);
                command.Parameters.AddWithValue("@NGO2", NGO2);
                command.Parameters.AddWithValue("@NGO3", NGO3);
                command.Parameters.AddWithValue("@StatusTemp1", StatusTemp1);
                command.Parameters.AddWithValue("@StatusTemp2", StatusTemp2);
                command.Parameters.AddWithValue("@StatusHum1", StatusHum1);
                command.Parameters.AddWithValue("@StatusHum2", StatusHum2);
                command.Parameters.AddWithValue("@StatusDirect1", StatusDirect1);
                command.Parameters.AddWithValue("@StatusDirect2", StatusDirect2);
                command.Parameters.AddWithValue("@StatusSpeed1", StatusSpeed1);
                command.Parameters.AddWithValue("@StatusSpeed2", StatusSpeed2);
                command.Parameters.AddWithValue("@StatusSpeedNasal", StatusSpeedNasal);
                command.Parameters.AddWithValue("@StatusDirectNasal", StatusDirectNasal);
                command.Parameters.AddWithValue("@StatusPressure", StatusPressure);
                command.Parameters.AddWithValue("@StatusDVGO", StatusDVGO);
                command.Parameters.AddWithValue("@AmountClouds", AmountClouds);
                command.Parameters.AddWithValue("@StatusDMDV", StatusDMDV);
                command.Parameters.AddWithValue("@Temp_1mid1", Temp_1mid1);
                command.Parameters.AddWithValue("@Hum_1mid1", Hum_1mid1);
                command.Parameters.AddWithValue("@Hum_1mid2", Hum_1mid2);
                command.Parameters.AddWithValue("@Temp_1mid2", Temp_1mid2);
                command.Parameters.AddWithValue("@Speed_2K1", Speed_2K1);
                command.Parameters.AddWithValue("@Speed_2K2", Speed_2K2);
                command.Parameters.AddWithValue("@Direction_2Kmid1", Direction_2Kmid1);
                command.Parameters.AddWithValue("@Direction_2Kmid2", Direction_2Kmid2);
                command.Parameters.AddWithValue("@Speed_Knasal", Speed_Knasal);
                command.Parameters.AddWithValue("@Direction_Knasal", Direction_Knasal);
                command.Parameters.AddWithValue("@Direction_2Knasalmid", Direction_2Knasalmid);
                command.Parameters.AddWithValue("@Direction_2Knasalmin", Direction_2Knasalmin);
                command.Parameters.AddWithValue("@Direction_2Knasalmax", Direction_2Knasalmax);
                command.Parameters.AddWithValue("@Direction_10Knasalmid", Direction_10Knasalmid);
                command.Parameters.AddWithValue("@Direction_10Knasalmin", Direction_10Knasalmin);
                command.Parameters.AddWithValue("@Direction_10Knasalmax", Direction_10Knasalmax);
                command.Parameters.AddWithValue("@Speed_2Knasalmid", Speed_2Knasalmid);
                command.Parameters.AddWithValue("@Speed_2Knasalmin", Speed_2Knasalmin);
                command.Parameters.AddWithValue("@Speed_2Knasalmax", Speed_2Knasalmax);
                command.Parameters.AddWithValue("@Speed_10Knasalmid", Speed_10Knasalmid);
                command.Parameters.AddWithValue("@Speed_10Knasalmin", Speed_10Knasalmin);
                command.Parameters.AddWithValue("@Speed_10Knasalmax", Speed_10Knasalmax);

                command.ExecuteNonQuery();
                currentTime = currentTime.AddMinutes(10);
            }
        }

        static public DataTable GetData(string column)
        {
            command.CommandText = $"SELECT DateTime, {column} FROM Data Where {column} != \"Н.Д.\"";
            DataTable data = new DataTable();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(data);
            return data;
        }

        static public DataTable GetDataFiltered(string dtFrom, string dtTo, string column = null)
        {
            if (column == null)
            {
                command.CommandText = $"SELECT DateTime, Temperature, Humidity, PressureGPa, Speed_I, Visibility10 FROM Data WHERE datetime >= '{dtFrom}' AND datetime <= '{dtTo}'";
            }
            else
            {
                var dt1 = Convert.ToDateTime(dtFrom);
                var dt2 = Convert.ToDateTime(dtTo);
                TimeSpan diff = dt2 - dt1;
                int days = diff.Days;
                int hours = diff.Hours;

                int delimeter = /*days > 60 ? 90 : days > 30 ? 54 :*/ days > 7 ? 18 : days > 1 ? 9 : days == 1 ? 3 : hours > 12 ? 2 : 1;
                if(delimeter != 1)
                {
                    command.CommandText = $@"
                    WITH NumberedRows AS (
                    SELECT 
                        DateTime, 
                        [{column}], 
                        ROW_NUMBER() OVER (ORDER BY DateTime) AS RowNum,
                        COUNT(*) OVER () AS TotalRows
                    FROM 
                        Data
                    WHERE 
                        datetime >= '{dtFrom}' 
                        AND datetime <= '{dtTo}' 
                        AND [{column}] != 'Н.Д.'
                )
                SELECT 
                    DateTime, 
                    [{column}]
                FROM 
                    NumberedRows
                WHERE 
                    RowNum % {delimeter} = 1
                    OR RowNum = TotalRows;";
                }
                else
                    command.CommandText = $"SELECT DateTime, {column} FROM Data WHERE datetime >= '{dtFrom}' AND datetime <= '{dtTo}' AND {column} != 'Н.Д.'";
            }
            DataTable data = new DataTable();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(data);
            return data;
        }

        static public string GetFirstDate(string column = null)
        {
            command.CommandText = column == null ? $"SELECT DateTime FROM Data ORDER BY datetime ASC LIMIT 1" :  $"SELECT DateTime FROM Data Where {column} != \"Н.Д.\" ORDER BY datetime ASC LIMIT 1";
            object result = command.ExecuteScalar();

            if (result != null)
                return result.ToString();
            else
                return null;
        }

        static public string GetLastDate(string column = null)
        {
            command.CommandText = column == null ? $"SELECT DateTime FROM Data ORDER BY datetime DESC LIMIT 1" :  $"SELECT DateTime FROM Data Where {column} != \"Н.Д.\" ORDER BY datetime DESC LIMIT 1";
            object result = command.ExecuteScalar();

            if (result != null)
                return result.ToString();
            else
                return null;
        }
    }
}
        

