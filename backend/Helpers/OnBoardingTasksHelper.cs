using backend.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace backend.Helpers
{
    public class OnBoardingTasksHelper
    {
        public static string GetPlayerID(string usermame)
        {
            string result = null;
            using (SqlConnection conn = new SqlConnection(MainHelper.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("Get_Player_ID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", usermame);

                    var returnParameter = cmd.Parameters.Add("@ID", SqlDbType.Int);
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

        public static DataTable GetChecklistsLoad(int playerID)
        {
            var dt = new DataTable("Data");
            using (SqlConnection conn = new SqlConnection(MainHelper.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("Player_Checklists_Load", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PlayerID", playerID);
                    var da = new SqlDataAdapter();
                    da.SelectCommand = cmd;

                    da.Fill(dt);
                }
            }
            return dt;
        }

        public static DataTable GetCheckListTemplateQuestions(List<string> infoData)
        {
            var dt = new DataTable("Data");
            using (SqlConnection conn = new SqlConnection(MainHelper.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("CheckListTemplates_Questions_Load", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChecklistTemplateID", Int32.Parse(infoData[0]));
                    cmd.Parameters.AddWithValue("@PlayerID", Int32.Parse(infoData[1]));
                    var da = new SqlDataAdapter();
                    da.SelectCommand = cmd;

                    da.Fill(dt);
                }
            }
            return dt;
        }

        public static string PlayerCheckListAnswersInsert(CheckListTemplateQuestionsAndAnswers data)
        {
            SqlConnection con = new SqlConnection(MainHelper.GetConnectionString());
            using (var command = con.CreateCommand())
            {
                command.CommandText = "PlayerCheckist_Answers_Insert";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@PlayerID", data.PlayerID);
                command.Parameters.AddWithValue("@ChecklistTemplateID", data.ChecklistTemplateID);
                command.Parameters.AddWithValue("@ChecklistTemplateQuestionID", data.ID);
                command.Parameters.AddWithValue("@Answer", data.Answer ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PointsEarned", data.Points);
                command.Parameters.AddWithValue("@IsSubmitted", data.IsSubmitted == true ? 1 : 0);

                con.Open();

                try
                {
                    command.ExecuteNonQuery();
                    return "success";
                }
                catch (Exception ex)
                {

                    return ex.Message;
                }
            }
        }


        public static string PlayerCheckistAnswersRevert(string id)
        {
            SqlConnection con = new SqlConnection(MainHelper.GetConnectionString());
            using (var command = con.CreateCommand())
            {
                command.CommandText = "PlayerCheckist_Answers_Revert";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ID", Int32.Parse(id));

                con.Open();

                try
                {
                    command.ExecuteNonQuery();
                    return "success";
                }
                catch (Exception ex)
                {

                    return ex.Message;
                }
            }
        }
    }
}
