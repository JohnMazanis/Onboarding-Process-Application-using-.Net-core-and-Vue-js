<template>
  <div class="home">
    <NavBar />
    <div id="homeData">
      <div>
        <h4 class="home_headers">Welcome, {{ username }}</h4>
      </div>
      <div v-show="username == 'admin'" style="margin-top: 40px !important;">
        <b-table
                id="playerCompletionData"
                :items="playerCompletionData"
                :busy="isBusyPlayerCompetion"
                caption-top
                responsive
                :select-mode="selectMode"
                selectable
                :current-page="currentProgressPage"
                :per-page="perProgressPage"
                @row-selected="onRowSelected"
            >
            <template #table-busy>
              <div class="text-center text-light my-2">
                <b-spinner class="align-middle"></b-spinner>
                <strong> Loading...</strong>
              </div>
          </template>
          <template #table-caption>
            <b-button
                size="md"
                class="mr-2 mb-3 ml-1"
                variant="success"
                @click="checkProgress()"
                >Check Progress</b-button
              >
              <b-button
                size="md"
                class="mr-2 mb-3 ml-1"
                variant="success"
                @click="exportData(playerCompletionData)"
                >Export to CSV</b-button
              >
          </template>
          <template #cell(selected)="{ rowSelected }">
              <template v-if="rowSelected">
                <span aria-hidden="true">&check;</span>
                <span class="sr-only">Selected</span>
              </template>
              <template v-else>
                <span aria-hidden="true">&nbsp;</span>
                <span class="sr-only">Not selected</span>
              </template>
           </template>
            </b-table>
            <div>
            <b-pagination
              size="md"
              :total-rows="this.playerCompletionData.length"
              :per-page="perProgressPage"
              v-model="currentProgressPage"
            />
            </div>
      </div>
      <div v-show="username == 'admin'" style="margin-top: 80px !important;">
        <template>
          <b-button
                size="md"
                class="mr-2 mb-3 ml-1"
                variant="success"
                @click="exportData(totalLeaderBoard)"
                >Export to CSV</b-button
              >
        </template>
        <b-table
                id="totalleaderboardplayers"
                :items="totalLeaderBoard"
                :busy="isBusyTotal"
                caption-top
                responsive
                :current-page="currentPage"
                :per-page="perPage"
                @row-selected="onRowSelected"
            >
            <template #table-busy>
              <div class="text-center text-light my-2">
                <b-spinner class="align-middle"></b-spinner>
                <strong> Loading...</strong>
              </div>
          </template>
            </b-table>
            <div>
            <b-pagination
              size="md"
              :total-rows="this.totalLeaderBoard.length"
              :per-page="perPage"
              v-model="currentPage"
            />
            </div>
      </div>
      <div class="row" v-show="username != 'admin'">
        <div class="col-6">
        <div>
          <div style="margin-bottom: 30px !important;">
          <h5 class="home_headers">Points: {{ playerPoints }}</h5>
        </div>
        <div>
          <b-button class="mt-2" size="lg" variant="primary" @click="goToOnBoardingTasks()" style="margin-bottom: 25px !important;"
            >Play</b-button
          >
        </div>
        <div class="mt-1" style="width: 90%">
          <div class="mb-2" v-show="PlayerPendingTasks[0].Submitted > 0">
            <label class="mr-4">Progress: </label>
            <b-progress
              :max="PlayerPendingTasks[0].AllTasks"
              height="1rem"
              variant="primary"
              animated
            >
              <b-progress-bar
                :value="PlayerPendingTasks[0].Submitted"
                :label="`${(
                  (PlayerPendingTasks[0].Submitted /
                    PlayerPendingTasks[0].AllTasks) *
                  100
                ).toFixed(0)}%`"
              ></b-progress-bar>
            </b-progress>
          </div>
        </div>
        <div class="mt-4" v-show="countDownDate >= 0">
          <b-avatar variant="primary" :text="countDownDate" size="4rem"></b-avatar>
          <label class="ml-2">Days till new experience begins</label> 
        </div>
        <div class="mt-4">
          <img
          id="infoImage"
          src="../../wwwroot/img/startonboard.jpg"
          alt="logo"
        />
        </div>
      </div>
      </div>
      <div class="col-6" v-show="username != 'admin'">
        <div class="row" v-show="PlayerPendingTasks[0].Submitted > 0">
          <label clas="mt-2" style="margin-right: 20px !important; margin-top: 5px !important; margin-left: 15px !important">Your Achievement: </label>
          <div class="row">
            <div v-for="(pAchievement, i) in playerAchievements" :key="i">
              <b-avatar
                class="ml-2"
                :src="getImgUrl(pAchievement.Achievements)"
                size="2rem"
              ></b-avatar>
            </div>
          </div>
        </div>
        <div style="margin-top: 100px; margin-bottom: 100px;">
            <b-table
                id="leaderboardplayers"
                :items="top10LeaderBoard"
                :fields="fields"
                :busy="isBusy"
                caption-top
                responsive
                :tbody-tr-class="rowClass"
            >
            <template #table-busy>
              <div class="text-center text-light my-2">
                <b-spinner class="align-middle"></b-spinner>
                <strong> Loading...</strong>
              </div>
          </template>
          </div>
        </div>
      </div>
    </div>
    <b-modal
      id="mdlSelect"
      size="sm"
      ref="modal-select"
      title="Information Grid"
      ok-only
      ok-variant="secondary"
      ok-title="Close"
      :header-bg-variant="headerBgVariant"
      :body-bg-variant="bodyBgVariant"
      :body-text-variant="headerTextVariant"
      :header-text-variant="light"
    >
      <div class="d-block">
        <p>{{ modalBodyTextSelect }}</p>
      </div>
    </b-modal>
  </div>
</template>

<script>
import NavBar from "@/components/NavBar.vue";
import api from "../helpers/ApiRequest.js";

export default {
  name: "home",
  data: function () {
    return {
      username: localStorage.getItem("username"),
      playerID: null,
      isBusy: false,
      isBusyTotal: false,
      isBusyPlayerCompetion: false,
      headerBgVariant: "light",
      headerTextVariant: "dark",
      bodyBgVariant: "light",
      bodyTextVariant: "dark",
      modalBodyTextSelect: "Please select a player to check the progress",
      selected: [],
      selectMode: "single",
      currentPage: 1,
      perPage: 5,
      perProgressPage: 5,
      currentProgressPage: 1,
      fields: [
        {
          key: "Rank",
        },
        {
          key: "PlayerID",
          tdClass: "d-none",
          thClass: "d-none",
        },
        {
          key: "Player",
        },
        {
          key: "TotalPoints",
        },
      ],
      top10LeaderBoard: [],
      totalLeaderBoard: [],
      playerCompletionData: [],
      PlayerPendingTasks: [
        {
          Submitted: 0,
          AllTasks: 0,
        },
      ],
      playerAchievements: [],
      playerCheckLists: [
        {
          ID: null,
          PlayerID: null,
          CheckListTemplateID: null,
          IsMain: null,
          DateAssigned: null,
          DateCompleted: null,
        },
      ],
      playerPoints: 0,
      achievements: [
        { value: 0, text: "Thumbs Up" },
        { value: 1, text: "Top Learner" },
        { value: 2, text: "Tech Wizard" },
        { value: 3, text: "Superstar" },
      ],
      countDownDate: null,
    };
  },
  components: {
    NavBar,
  },
  async created() {
    this.playerID = await api.GetRequest(this.username, "Get_Player_ID");
    if (this.playerID != null) {
      if (this.username != "admin") {
        localStorage.setItem("selectedUser", this.username);
        this.playerCheckLists = await api.GetRequest(
          this.playerID,
          "Player_Checklists_Load"
        );

        if (this.playerCheckLists.length > 0) {
          this.PlayerPendingTasks = await api.GetRequest(
            JSON.stringify(this.playerCheckLists[0]),
            "Player_Pending_Tasks"
          );
          this.playerPoints = await api.GetRequest(
            this.playerID,
            "Player_Points"
          );

          this.playerAchievements = await api.GetRequest(
            JSON.stringify(this.playerCheckLists[0]),
            "Player_Achievements"
          );
        }
        this.isBusy = true;
        this.top10LeaderBoard = await api.GetRequest(
          this.playerID,
          "Player_Top10LeaderBoard"
        );
        this.isBusy = false;
        this.countDownDate = await api.GetRequest(
          this.playerID,
          "CountDown_HireDate"
        );
      } else {
        this.isBusyPlayerCompetion = true;
        this.playerCompletionData = await api.GetRequest(
          "get",
          "Player_Completion"
        );
        this.isBusyPlayerCompetion = false;
        this.isBusyTotal = true;
        this.totalLeaderBoard = await api.GetRequest(
          "get",
          "Player_TotalLeaderBoard"
        );
        this.isBusyTotal = false;
      }
    }
  },
  methods: {
    showModalSelect() {
      this.$refs["modal-select"].show();
    },
    onRowSelected(items) {
      this.selected = items;
      localStorage.setItem("selectedUser", this.selected[0].UserName);
    },
    checkProgress() {
      if (this.selected.length > 0) {
        this.$router.push("/OnBoardingTasks");
      } else {
        this.showModalSelect();
      }
    },
    goToOnBoardingTasks() {
      this.$router.push("/OnBoardingTasks");
    },
    getImgUrl(image) {
      var images = require.context("../../wwwroot/img/", false, /\.png$/);
      try {
        return images("./" + image + "Achievement.png");
      } catch (error) {
        return null;
      }
    },
    rowClass(item, type) {
      if (!item || type !== "row") return;
      if (item.PlayerID === this.playerID) return "table-dark";
    },
    exportData(data) {
      this.csvExport(data);
    },
    csvExport(arrData) {
      let csvContent = "data:text/csv;charset=utf-8,";
      csvContent += [
        Object.keys(arrData[0]).join(";"),
        ...arrData.map((item) => Object.values(item).join(";")),
      ]
        .join("\n")
        .replace(/(^\[)|(\]$)/gm, "");

      const data = encodeURI(csvContent);
      const link = document.createElement("a");
      link.setAttribute("href", data);
      link.setAttribute("download", "EnboardHomePagePlayerInfo.csv");
      link.click();
    },
  },
};
</script>

<style>
#homeData {
  margin-top: 4%;
  margin-left: 10%;
  margin-right: 10%;
  height: 100%;
}

.home_headers {
  color: aliceblue;
}

label {
  color: aliceblue;
}

.bg-primary,
.badge-primary {
  background-color: #07868d !important;
}

#leaderboardplayers,
#totalleaderboardplayers,
#playerCompletionData {
  text-align: center;
  color: aliceblue;
}

#infoImage {
  border-radius: 5px;
  transition: 0.3s;
  display: block;
  margin: 0 auto;
  max-width: 99%;
  left: 50%;
}

.table-dark td{
  background-color: #07868d !important;
}
</style>