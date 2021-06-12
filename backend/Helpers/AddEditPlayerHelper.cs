using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using backend.Objects;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using MimeKit;
using MailKit.Net.Smtp;

namespace backend.Helpers
{
    public class AddEditPlayerHelper
    {
        
        public static void CreatePlayer(UsersData data)
        {

            SqlConnection con = new SqlConnection(MainHelper.GetConnectionString());
            using (var command = con.CreateCommand())
            {
                command.CommandText = "Player_InsertOrUpdate";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ID", data.id ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@UserName", data.txtUsernamecreate);
                command.Parameters.AddWithValue("@Password", data.txtPasswordcreate);
                command.Parameters.AddWithValue("@FirstName", data.txtfirstname);
                command.Parameters.AddWithValue("@LastName", data.txtlastname);
                command.Parameters.AddWithValue("@Email", data.txtemail);
                command.Parameters.AddWithValue("@HireDate", data.dthiredate);
                command.Parameters.AddWithValue("@PositionID", data.position);
                command.Parameters.AddWithValue("@DepartmentID", data.department ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@StatusID", data.statusID ?? (object)DBNull.Value);


                con.Open();
                command.ExecuteNonQuery();
            }
        }

        public static DataTable GetPlayers()
        {
            var dt = new DataTable("Data");
            using (SqlConnection conn = new SqlConnection(MainHelper.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("Player_Load", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var da = new SqlDataAdapter();
                    da.SelectCommand = cmd;

                    da.Fill(dt);
                }
            }
            return dt;
        }

        public static string GetUserNameExists(string userName)
        {
            string result = "0";
            using (SqlConnection conn = new SqlConnection(MainHelper.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("Player_UserNameExists", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", userName);

                    var returnParameter = cmd.Parameters.Add("@Exists", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.Output;

                    conn.Open();

                    try
                    {
                        cmd.ExecuteNonQuery();
                        result = Convert.ToString(returnParameter.Value);
                    }
                    catch (Exception)
                    {
                        result = "0";
                    }
                }
            }

            return result;
        }

        public static void DeletePlayer(string id)
        {
            using (SqlConnection conn = new SqlConnection(MainHelper.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("Player_Delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static string GetUserExists(UserAuth userdata)
        {
            string result = "0";
            using (SqlConnection conn = new SqlConnection(MainHelper.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("Player_UserExists", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", userdata.txtUsername);
                    cmd.Parameters.AddWithValue("@Password", userdata.txtPassword);

                    var returnParameter = cmd.Parameters.Add("@Exists", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.Output;

                    conn.Open();

                    try
                    {
                        cmd.ExecuteNonQuery();
                        result = Convert.ToString(returnParameter.Value);
                    }
                    catch (Exception)
                    {
                        result = "0";
                    }
                }
            }

            return result;
        }

        public static string TrySendEmail(UsersData user)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Enboard Admin", "eenboard@gmail.com"));
                message.To.Add(new MailboxAddress(user.txtlastname, user.txtemail));
                message.Subject = "Login Information";
                message.Body = new TextPart("Plain")
                {
                    Text = "Your username and password is " + user.txtUsernamecreate + " and " + user.txtPasswordcreate
                };

               
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("dummy@gmail.com", "dummy12345");

                    client.Send(message);
                    client.Disconnect(true);
                }

                return "success";
            }
            catch (Exception ex)
            {
                return "fail";
            }
        }

    }
}
