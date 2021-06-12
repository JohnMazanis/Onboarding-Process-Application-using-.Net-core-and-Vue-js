using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using backend.Objects;
using backend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;

namespace backend.Controllers
{
    
    [ApiController]
    [Route("api/controller")]
    public class RequestController : ControllerBase
    {
        // AddEditPlayerHelper ---------------------------------------------------------------------

        [HttpPost("savePlayer/{data}")]
        public string SavePlayer(string data)
        {
            try
            {
                var userData = System.Text.Json.JsonSerializer.Deserialize<UsersData>(data);
                AddEditPlayerHelper.CreatePlayer(userData);
                return "success";
            }
            catch (Exception ex )
            {
                return ex.Message;
            }
        }

        [HttpGet("loadplayers/get")]
        public string LoadPlayers()
        {
            try
            {
                var data = AddEditPlayerHelper.GetPlayers();
                var reult = (from rw in data.AsEnumerable()
                             select new
                             {
                                 ID = Convert.ToInt32(rw["ID"]),
                                 Player = Convert.ToString(rw["Player"]),
                                 UserName = Convert.ToString(rw["UserName"]),
                                 Password = Convert.ToString(rw["Password"]),
                                 Email = Convert.ToString(rw["Email"]),
                                 CreateDate = Convert.ToString(rw["CreateDate"]),
                                 HireDate = Convert.ToString(rw["HireDate"]),
                                 PositionID = Convert.ToInt32(rw["PositionID"]),
                                 PositionDescr = Convert.ToString(rw["PositionDescr"]),
                                 DepartmentID = Convert.ToInt32(rw["DepartmentID"]),
                                 DepartmentDescr = Convert.ToString(rw["DepartmentDescr"]),
                                 StatusID = Convert.ToInt32(rw["StatusID"]),
                                 StatusDescr = Convert.ToString(rw["StatusDescr"]),
                                 CheckList = Convert.ToString(rw["CheckList"])
                             }).ToList();

                return JsonConvert.SerializeObject(reult);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost("deletePlayer/{id}")]
        public string DeletePlayer(string id)
        {
            try
            {
                AddEditPlayerHelper.DeletePlayer(id);
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet("userNameExists/{username}")]
        public string UserNameExists(string userName)
        {
            try
            {
                string result = AddEditPlayerHelper.GetUserNameExists(userName);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet("userExists/{data}")]
        public string UserExists(string data)
        {
            try
            {
                var userData = System.Text.Json.JsonSerializer.Deserialize<UserAuth>(data);
                string result = AddEditPlayerHelper.GetUserExists(userData);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost("sendEmailWithLogin/{data}")]
        public string SendEmail(string data)
        {
            var userData = System.Text.Json.JsonSerializer.Deserialize<UsersData>(data);
            string result = AddEditPlayerHelper.TrySendEmail(userData);
            return result;
        }

        // CheckListTemplateHelper ---------------------------------------------------------------------

        [HttpGet("loadCheckListTemplates/get")]
        public string LoadCheckListTemplates()
        {
            try
            {
                var data = CheckListTemplateHelper.GetCheckListTemplates();
                return JsonConvert.SerializeObject(data);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost("tryCheckListTemplatesInsert/{descr}")]
        public string TryCheckListTemplatesInsert(string descr)
        {
            return CheckListTemplateHelper.CheckListTemplatesInsert(descr);
        }

        [HttpPost("tryDeleteCheckListTemplate/{id}")]
        public string TryDeleteCheckListTemplate(string id)
        {
            return CheckListTemplateHelper.DeleteCheckListTemplate(id);
        }

        [HttpGet("checkListTemplate_Exists/{templateName}")]
        public string CheckListTemplate_Exists(string templateName)
        {
            return CheckListTemplateHelper.GetCheckListNameExists(templateName);
        }

        [HttpGet("CheckListTemplates_Questions_Load/{id}")]
        public string CheckListTemplatesQuestionsLoad(string id)
        {
            try
            {
                var data = CheckListTemplateHelper.GetCheckListTemplatesQuestionsLoad(id);
                return JsonConvert.SerializeObject(data);
            }
            catch (Exception ex)
            {
                var dt = new DataTable("Data");
                return JsonConvert.SerializeObject(dt); ;
            }
        }

        [HttpPost("CheckListTemplates_Questions_Save/{data}")]
        public string TrySaveCheckListTemplateQuestions(CheckListTemplateItem data)
        {
            try
            {
                var checkListTemplateItems = CheckListTemplateHelper.SaveCheckListTemplateQuestions(data);
                return checkListTemplateItems;
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
           
        }

        [HttpGet("Player_Load_To_Assign/get")]
        public string PlayersToAssign()
        {
            try
            {
                var data = CheckListTemplateHelper.GetPlayersToAssign();
                return JsonConvert.SerializeObject(data);

            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost("CheckListTemplates_Assign_Player/{data}")]
        public string PlayersToAssign(AssignPlayerToCheckList data)
        {
            var result = CheckListTemplateHelper.CheckListTemplatesAssignPlayer(data);
            return result;
        }

        [HttpGet("Player_Load_To_UnAssign/{id}")]
        public string PlayerLoadToUnAssign(string id)
        {
            try
            {
                var data = CheckListTemplateHelper.GetPlayerLoadToUnAssign(Int32.Parse(id));
                return JsonConvert.SerializeObject(data);
            }
            catch (Exception )
            {
                var dt = new DataTable("Data");
                return JsonConvert.SerializeObject(dt);
            }
        }

        [HttpPost("CheckListTemplates_UnAssign_Player/{id}")]
        public string UnnAssignPlayer(string id)
        {
            var result = CheckListTemplateHelper.CheckListTemplatesUnAssignPlayer(Int32.Parse(id));
            return result;
        }

        // OnBoardingTasksHelper ---------------------------------------------------------------------

        [HttpGet("Get_Player_ID/{username}")]
        public string UserID(string username)
        {
            try
            {
                string id = OnBoardingTasksHelper.GetPlayerID(username);
                return id;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet("Player_Checklists_Load/{playerID}")]
        public string ChecklistsLoad(int playerID)
        {
            try
            {
                var data = OnBoardingTasksHelper.GetChecklistsLoad(playerID);
                return JsonConvert.SerializeObject(data);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet("CheckListTemplate_Questions_Load/{infoData}")]
        public string CheckListTemplateQuestion(string infoData)
        {
            try
            {
                var info = System.Text.Json.JsonSerializer.Deserialize<List<string>>(infoData);
                var data = OnBoardingTasksHelper.GetCheckListTemplateQuestions(info);
                return JsonConvert.SerializeObject(data);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost("PlayerCheckist_Answers_Insert/{data}")]
        public string TryPlayerCheckListAnswersInsert(CheckListTemplateQuestionsAndAnswers data)
        {
            try
            {
                var result = OnBoardingTasksHelper.PlayerCheckListAnswersInsert(data);
                return result;
            }
            catch (Exception ex)
            {

                return ex.Message;
            }  
        }

        [HttpPost("PlayerCheckist_Answers_Revert/{ID}")]
        public string TryPlayerCheckistAnswersRevert(string id)
        {
            var result = OnBoardingTasksHelper.PlayerCheckistAnswersRevert(id);
            return result;
        }

        // HomeHelper ---------------------------------------------------------------------
        [HttpGet("Player_Pending_Tasks/{playerCheckLists}")]
        public string PlayerPendingTasks(string playerCheckLists)
        {
            try
            {
                var infoData = System.Text.Json.JsonSerializer.Deserialize<PlayerCheckLists>(playerCheckLists);
                var data = HomeHelper.GetPlayerPendingTasks(infoData);
                return JsonConvert.SerializeObject(data);
            }
            catch (Exception ex)
            {
                var dt = new DataTable("Data");
                return JsonConvert.SerializeObject(dt);
            }
        }

        [HttpGet("Player_Points/{playerID}")]
        public string PlayerPoints(int playerID)
        {
            return HomeHelper.GetPlayerPoints(playerID);
        }

        [HttpGet("Player_Achievements/{playerCheckLists}")]
        public string PlayerPoints(string playerCheckLists)
        {
            try
            {
                var infoData = System.Text.Json.JsonSerializer.Deserialize<PlayerCheckLists>(playerCheckLists);
                var data = HomeHelper.GetPlayerAchievements(infoData);
                return JsonConvert.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("Player_Top10LeaderBoard/{playerID}")]
        public string Top10LeaderBoard(string playerID)
        {
            try
            {
                var data = HomeHelper.GetTop10LeaderBoard(playerID);
                return JsonConvert.SerializeObject(data);
            }
            catch (Exception ex)
            {
                var dt = new DataTable("Data");
                return JsonConvert.SerializeObject(dt);
            }
        }

        [HttpGet("Player_TotalLeaderBoard/get")]
        public string TotalLeaderBoard()
        {
            try
            {
                var data = HomeHelper.GetTotalLeaderBoard();
                return JsonConvert.SerializeObject(data);
            }
            catch (Exception ex)
            {
                var dt = new DataTable("Data");
                return JsonConvert.SerializeObject(dt);
            }
        }

        [HttpGet("Player_Completion/get")]
        public string PlayerCompletion()
        {
            try
            {
                var data = HomeHelper.GetPlayerCompletion();
                return JsonConvert.SerializeObject(data);
            }
            catch (Exception ex)
            {
                var dt = new DataTable("Data");
                return JsonConvert.SerializeObject(dt);
            }
        }

        [HttpGet("CountDown_HireDate/{playerID}")]
        public string CountDownHireDate(string playerID)
        {
            try
            {
                var date = HomeHelper.GetCountDownHireDate(playerID);
                return date;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}


