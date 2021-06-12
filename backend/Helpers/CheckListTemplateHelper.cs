using backend.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Helpers
{
    public class CheckListTemplateHelper
    {

        public static DataTable GetCheckListTemplates()
        {
            var dt = new DataTable("Data");
            using (SqlConnection conn = new SqlConnection(MainHelper.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("CheckListTemplates_Load", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var da = new SqlDataAdapter();
                    da.SelectCommand = cmd;

                    da.Fill(dt);
                }
            }
            return dt;
        }

        public static string CheckListTemplatesInsert(string description)
        {
            SqlConnection con = new SqlConnection(MainHelper.GetConnectionString());
            using (var command = con.CreateCommand())
            {
                command.CommandText = "CheckListTemplates_Insert";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Descr", description);

                con.Open();

                try
                {
                    command.ExecuteNonQuery();
                    return "success";
                }
                catch (Exception)
                {

                    return "fail";
                }
            }
        }

        public static string DeleteCheckListTemplate(string id)
        {
            using (SqlConnection conn = new SqlConnection(MainHelper.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("CheckListTemplates_Delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    conn.Open();

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return "success";
                    }
                    catch (Exception)
                    {
                        return "fail";
                    }
                }
            }
        }

        public static string GetCheckListNameExists(string descr)
        {
            string result = "0";
            using (SqlConnection conn = new SqlConnection(MainHelper.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("CheckListTemplate_Exists", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CheckListTemplateName", descr);

                    var returnParameter = cmd.Parameters.Add("@Exists", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.Output;

                    conn.Open();

                    try
                    {
                        cmd.ExecuteNonQuery();
                        result = Convert.ToString(returnParameter.Value);
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message;
                    }
                }
            }

            return result;
        }

        public static DataTable GetCheckListTemplatesQuestionsLoad(string id)
        {
            var dt = new DataTable("Data");
            using (SqlConnection conn = new SqlConnection(MainHelper.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("CheckListTemplates_Only_Questions_Load", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChecklistTemplateID", id != null ? Int32.Parse(id) : (object)DBNull.Value);

                    var da = new SqlDataAdapter();
                    da.SelectCommand = cmd;

                    da.Fill(dt);
                }
            }
            return dt;
        }

        public static DataTable GetPlayersToAssign()
        {
            var dt = new DataTable("Data");
            using (SqlConnection conn = new SqlConnection(MainHelper.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("Player_Load_To_Assign", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var da = new SqlDataAdapter();
                    da.SelectCommand = cmd;

                    da.Fill(dt);
                }
            }
            return dt;
        }

        public static string CheckListTemplatesAssignPlayer(AssignPlayerToCheckList data)
        {
            SqlConnection con = new SqlConnection(MainHelper.GetConnectionString());
            using (var command = con.CreateCommand())
            {
                command.CommandText = "CheckListTemplates_Assign_Player";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@CheckListTemplateID", data.CheckListTemplateID);
                command.Parameters.AddWithValue("@PlayerID", data.PlayerID);

                con.Open();

                try
                {
                    command.ExecuteNonQuery();
                    return "success";
                }
                catch (Exception)
                {

                    return "fail";
                }
            }
        }

        public static string SaveCheckListTemplateQuestions(CheckListTemplateItem data)
        {
            string result = "success";

            List<TempQuestions> tempQuestions = new List<TempQuestions>();

            if (data.Questions != null)
            {
                tempQuestions = data.Questions.Select(x => new TempQuestions
                {
                    ID = x.ID,
                    CheckListTemplateID = x.CheckListTemplateID,
                    Descr = x.Descr,
                    ControlTypeID = x.ControlTypeID,
                    Points = x.Points,
                    AchievementID = x.AchievementID
                }).ToList();
            }

            using (SqlConnection conn = new SqlConnection(MainHelper.GetConnectionString()))
            {
                var sp = MainHelper.SetStoredProcedureTempTable(conn, "TempQuestions", tempQuestions);
                if (sp.ErrorFound)
                {
                    result = "fail";
                    return sp.ErrorMessage;
                }

                using (SqlCommand cmd = new SqlCommand("CheckListTemplate_Update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", data.ID);
                    cmd.Parameters.AddWithValue("@Descr", data.Descr);
                    cmd.Parameters.AddWithValue("@Category", data.Category);
                    cmd.Parameters.AddWithValue("@Active", data.Active == true ? 1 : 0);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        result = "success";
                    }
                    catch (Exception ex)
                    {
                        result = "fail";
                    }
                }
            }

            return result;
        }

        public static DataTable GetPlayerLoadToUnAssign(int ID)
        {
            var dt = new DataTable("Data");
            using (SqlConnection conn = new SqlConnection(MainHelper.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("Player_Load_To_UnAssign", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChecklistTemplateID", ID);

                    var da = new SqlDataAdapter();
                    da.SelectCommand = cmd;

                    da.Fill(dt);
                }
            }
            return dt;
        }

        public static string CheckListTemplatesUnAssignPlayer(int playerID)
        {
            SqlConnection con = new SqlConnection(MainHelper.GetConnectionString());
            using (var command = con.CreateCommand())
            {
                command.CommandText = "CheckListTemplates_UnAssign_Player";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@PlayerID", playerID);

                con.Open();

                try
                {
                    command.ExecuteNonQuery();
                    return "success";
                }
                catch (Exception ex)
                {

                    return "fail";
                }
            }
        }
    }
}
