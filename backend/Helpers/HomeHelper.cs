using backend.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Helpers
{
    public class HomeHelper
    {
        public static DataTable GetPlayerPendingTasks(PlayerCheckLists infoData)
        {
            var dt = new DataTable("Data");
            using (SqlConnection conn = new SqlConnection(MainHelper.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("Player_Pending_Tasks", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChecklistTemplateID", infoData.CheckListTemplateID);
                    cmd.Parameters.AddWithValue("@PlayerID", infoData.PlayerID);
                    var da = new SqlDataAdapter();
                    da.SelectCommand = cmd;

                    da.Fill(dt);
                }
            }
            return dt;
        }

        public static string GetPlayerPoints(int playerID)
        {
            string result = null;
            using (SqlConnection conn = new SqlConnection(MainHelper.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("Player_Points", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PlayerID", playerID);

                    var returnParameter = cmd.Parameters.Add("@Points", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.Output;

                    conn.Open();

                    try
                    {
                        cmd.ExecuteNonQuery();
                        result = Convert.ToString(returnParameter.Value) == "" ? "0" : Convert.ToString(returnParameter.Value);
                    }
                    catch (Exception)
                    {
                        result = null;
                    }
                }
            }

            return result;
        }

        public static DataTable GetPlayerAchievements(PlayerCheckLists infoData)
        {
            var dt = new DataTable("Data");
            using (SqlConnection conn = new SqlConnection(MainHelper.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("Player_Achievements", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChecklistTemplateID", infoData.CheckListTemplateID);
                    cmd.Parameters.AddWithValue("@PlayerID", infoData.PlayerID);
                    var da = new SqlDataAdapter();
                    da.SelectCommand = cmd;

                    da.Fill(dt);
                }
            }
            return dt;
        }

        public static DataTable GetTop10LeaderBoard(string playerID)
        {
            var dt = new DataTable("Data");
            using (SqlConnection conn = new SqlConnection(MainHelper.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("Player_Top10LeaderBoard", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CurrentPlayerID", playerID);
                    var da = new SqlDataAdapter();
                    da.SelectCommand = cmd;

                    da.Fill(dt);
                }
            }
            return dt;
        }

        public static DataTable GetTotalLeaderBoard()
        {
            var dt = new DataTable("Data");
            using (SqlConnection conn = new SqlConnection(MainHelper.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("Player_TotalLeaderBoard", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var da = new SqlDataAdapter();
                    da.SelectCommand = cmd;

                    da.Fill(dt);
                }
            }
            return dt;
        }

        public static DataTable GetPlayerCompletion()
        {
            var dt = new DataTable("Data");
            using (SqlConnection conn = new SqlConnection(MainHelper.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("Player_Completion", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChecklistTemplateID", (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PlayerID", (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ShowOnlyPending", (object)DBNull.Value);
                    var da = new SqlDataAdapter();
                    da.SelectCommand = cmd;

                    da.Fill(dt);
                }
            }
            return dt;
        }

        public static string GetCountDownHireDate(string playerID)
        {
            string result = null;
            using (SqlConnection conn = new SqlConnection(MainHelper.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("CountDown_HireDate", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PlayerID", playerID);

                    var returnParameter = cmd.Parameters.Add("@CountDownDate", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.Output;

                    conn.Open();

                    try
                    {
                        cmd.ExecuteNonQuery();
                        result = Convert.ToString(returnParameter.Value);
                    }
                    catch (Exception)
                    {
                        result = null;
                    }
                }
            }

            return result;
        }
    }
}
