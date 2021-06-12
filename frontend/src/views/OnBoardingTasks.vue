<template>
  <div class="onBoardingTasks">
    <NavBar />
    <div id="onBoardingTasksData">
      <div v-show="checkListTemplateQuestions[0].ID != null">
        <div
          v-for="(question, i) in checkListTemplateQuestions"
          :key="i"
          class="mt-3 onBoardingTasksQuestions"
        >
          <label style="font-size: 20px"> Task: {{ question.Descr }} </label>
          <label
            v-show="'range' === takeInputType(question.ControlTypeID)"
            class="ml-2"
          >
            <strong> 0 % - 100 %</strong>
          </label>
          <br />
          <label class="mb-4">
            Status:
            <strong>
              {{ question.IsSubmitted == false ? "Pending" : "Completed" }}
            </strong> </label
          ><br />
          <input
            :itemid="i + 'input'"
            :type="takeInputType(question.ControlTypeID)"
            v-model="question.Answer"
            :disabled="question.IsSubmitted"
            class="textInputQuestion"
          />
          <label
            v-show="'range' === takeInputType(question.ControlTypeID)"
            class="ml-4"
          >
            <strong>{{ question.Answer }}%</strong>
          </label>
          <br />
          <label class="mt-2"> Points: {{ question.Points }} </label>
          <b-button
            variant="primary"
            class="questionActionButtons"
            :disabled="question.IsSubmitted"
            @click="submitAnswer(question.ID, question.AchievementID, i)"
            >Submit</b-button
          >
          <b-button
            variant="secondary"
            class="questionActionButtons mr-2"
            :disabled="!question.IsSubmitted"
            @click="revertAnswer(i)"
            >Revert</b-button
          >
          <hr />
        </div>
      </div>
      <div
        style="text-align: center"
        v-show="checkListTemplateQuestions[0].ID == null"
      >
        <label style="font-size: 20px">
          There is not an assigned on boarding task yet ! </label
        ><br />
        <label style="font-size: 15px">
          <a class="nav-link"
            ><router-link :to="getHomePath()" style="color: inherit"
              >Click here to go to Home screen...</router-link
            ></a
          >
        </label>
      </div>
    </div>
    <b-modal
      id="mdlAchievement"
      size="sm"
      ref="modal-achievement"
      title="Congratulations"
      ok-only
      ok-variant="secondary"
      ok-title="Close"
      :header-bg-variant="headerBgVariant"
      :body-bg-variant="bodyBgVariant"
      :body-text-variant="headerTextVariant"
      :header-text-variant="light"
    >
      <div class="d-block">
        <p class="text-center mb-3">{{ achievementTextOnModal }}</p>
        <div>
          <b-img :src="imageOnModal" thumbnail rounded="circle" />
        </div>
      </div>
    </b-modal>
    <b-modal
      id="mdlError"
      size="sm"
      ref="modal-error"
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
        <p> Something went wrong !</p>
      </div>
    </b-modal>
  </div>
</template>

<script>
import NavBar from "@/components/NavBar.vue";
import api from "../helpers/ApiRequest.js";

export default {
  name: "onBoardingTasks",
  data: function () {
    return {
      username: localStorage.getItem("selectedUser"),
      playerID: null,
      headerBgVariant: "light",
      headerTextVariant: "dark",
      bodyBgVariant: "light",
      bodyTextVariant: "dark",
      playerCheckLists: [
        {
          ID: null,
          PlayerID: null,
          CheckListTemplateID: null,
          IsMain: null,
          DateAssigned: new Date(),
          DateCompleted: new Date(),
        },
      ],
      checkListTemplateQuestions: [
        {
          ID: null,
          CheckListTemplateID: null,
          Descr: null,
          ControlTypeID: null,
          Points: null,
          AchievementID: null,
          Answer: null,
          IsSubmitted: false,
          AnswerID: null,
          PlayerID: null,
          QuestionAnswerID: null,
        },
      ],
      inputTypes: [
        { value: 1, text: "checkbox" },
        { value: 2, text: "range" },
        { value: 3, text: "text" },
      ],
      achievements: [
        { value: 0, text: "Thumbs Up" },
        { value: 1, text: "Top Learner" },
        { value: 2, text: "Tech Wizard" },
        { value: 3, text: "Superstar" },
      ],
      achievementTextOnModal: null,
      imageOnModal: null,
    };
  },
  components: {
    NavBar,
  },
  async created() {
    // take data and all the info needed to create the page dynamically
    this.playerID = await api.GetRequest(this.username, "Get_Player_ID");
    if (this.playerID != null) {
      this.playerCheckLists = await api.GetRequest(
        this.playerID,
        "Player_Checklists_Load"
      );
    }

    if (this.playerCheckLists.length > 0) {
      if (this.playerCheckLists[0].CheckListTemplateID != null) {
        let infoData = [
          this.playerCheckLists[0].CheckListTemplateID.toString(),
          this.playerID.toString(),
        ];
        this.checkListTemplateQuestions = await api.GetRequest(
          JSON.stringify(infoData),
          "CheckListTemplate_Questions_Load"
        );
      }
      await this.onCreateQuestions();
    }
  },
  methods: {
    showModalError() {
      this.$refs["modal-error"].show();
    },
    async onCreateQuestions() {
      var questions = this.checkListTemplateQuestions;

      for (let index = 0; index < questions.length; index++) {
        if (questions[index].ControlTypeID == 2) {
          questions[index].Answer =
            questions[index].Answer == null ? 0 : questions[index].Answer;
        } else if (questions[index].ControlTypeID == 1) {
          questions[index].Answer =
            questions[index].Answer == 0 || questions[index].Answer == null
              ? false
              : true;
        }
        questions[index].IsSubmitted =
          questions[index].IsSubmitted == null ||
          questions[index].IsSubmitted == 0
            ? false
            : true;
        questions[index].PlayerID = this.playerID;
      }
    },
    getHomePath() {
      return "/home";
    },
    takeInputType(inputType) {
      var descr = "";
      this.inputTypes.forEach(function (value) {
        if (inputType == value["value"]) {
          descr = value["text"];
        }
      });
      return descr;
    },
    getImgUrl(image) {
      var images = require.context("../../wwwroot/img/", false, /\.png$/);
      try {
        return images("./" + image + ".png");
      } catch (error) {
        return null;
      }
    },
    takeAchievementText(achievementID) {
      var descr = "";
      this.achievements.forEach(function (value) {
        if (achievementID == value["value"]) {
          descr = value["text"];
        }
      });
      return descr;
    },
    async submitAnswer(id, achievementID, index) {
      var questions = this.checkListTemplateQuestions;

      questions[index].IsSubmitted = true;

      if (questions[index].ControlTypeID == 1) {
        questions[index].Answer = questions[index].Answer == true ? "1" : "0";
      } else if (questions[index].ControlTypeID == 2) {
        questions[index].Answer = questions[index].Answer.toString();
      }
      var result = await api.PostRequest(
        questions[index],
        "PlayerCheckist_Answers_Insert"
      );
      if (result == "success") {
        this.achievementTextOnModal = this.takeAchievementText(achievementID);
        this.imageOnModal = this.getImgUrl(
          achievementID.toString() + "Achievement"
        );
        this.$refs["modal-achievement"].show();
        if (this.playerCheckLists.length > 0) {
          if (this.playerCheckLists[0].CheckListTemplateID != null) {
            let infoData = [
              this.playerCheckLists[0].CheckListTemplateID.toString(),
              this.playerID.toString(),
            ];
            this.checkListTemplateQuestions = await api.GetRequest(
              JSON.stringify(infoData),
              "CheckListTemplate_Questions_Load"
            );
          }
          await this.onCreateQuestions();
        }
      } else {
        this.showModalError();
      }
    },
    async revertAnswer(index) {
      var questions = this.checkListTemplateQuestions;

      var result = await api.PostRequest(
        questions[index].QuestionAnswerID.toString(),
        "PlayerCheckist_Answers_Revert"
      );

      if (result == "success") {
        questions[index].Answer =
          questions[index].ControlTypeID == 2
            ? 0
            : questions[index].ControlTypeID == 1
            ? false
            : null;

        questions[index].IsSubmitted = false;
      } else {
        this.showModalError();
      }
    },
    createModalAchivement(title, mainBody, image) {
      const h = this.$createElement;
      // Using HTML string
      const titleVNode = h("div", { domProps: { innerHTML: title } });
      // More complex structure
      const messageVNode = h("div", { class: ["foobar"] }, [
        h("p", { class: ["text-center"] }, ["Achievement: " + mainBody]),
        h("b-img", {
          props: {
            src: "../../wwwroot/img/" + image,
            thumbnail: true,
            center: true,
            fluid: true,
            rounded: "circle",
          },
        }),
      ]);
      // We must pass the generated VNodes as arrays
      this.$bvModal.msgBoxOk([messageVNode], {
        title: [titleVNode],
        buttonSize: "md",
        centered: false,
        size: "sm",
      });
    },
  },
};
</script>

<style>
label {
  color: aliceblue;
}

#onBoardingTasksData {
  margin-top: 4%;
  margin-left: 10%;
  margin-right: 10%;
  height: 100%;
}

.onBoardingTasksQuestions > input[type="checkbox"] {
  /* Double-sized Checkboxes */
  -ms-transform: scale(2); /* IE */
  -moz-transform: scale(2); /* FF */
  -webkit-transform: scale(2); /* Safari and Chrome */
  -o-transform: scale(2); /* Opera */
  transform: scale(2);
  margin-left: 6px;
  padding: 10px;
}

.onBoardingTasksQuestions > input[type="text"] {
  width: 40%;
  background: rgba(0, 0, 0, 0.3);
  border: none;
  outline: none;
  padding: 10px;
  font-size: 13px;
  color: #fff;
  text-shadow: 1px 1px 1px rgba(0, 0, 0, 0.3);
  border: 1px solid rgba(0, 0, 0, 0.3);
  border-radius: 4px;
  box-shadow: inset 0 -5px 45px rgba(100, 100, 100, 0.2),
    0 1px 1px rgba(255, 255, 255, 0.2);
  -webkit-transition: box-shadow 0.5s ease;
  -moz-transition: box-shadow 0.5s ease;
  -o-transition: box-shadow 0.5s ease;
  -ms-transition: box-shadow 0.5s ease;
  transition: box-shadow 0.5s ease;
}

.onBoardingTasksQuestions > input[type="range"] {
  width: 40%;
}

hr {
  background-color: aliceblue;
  margin-bottom: 20px;
}

.questionActionButtons {
  float: right;
}
</style>