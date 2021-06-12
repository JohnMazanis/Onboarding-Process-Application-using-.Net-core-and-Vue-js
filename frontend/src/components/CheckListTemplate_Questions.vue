<template>
  <div class="CheckListTemplate_Questions">
    <div>
        <div id="divHeaderbuttonsCheckListeTemplate">
          <b-button id="saveCheckListQuestionTemplate" variant="primary" @click="saveCheckListQuestionTemplate()"
          >Save</b-button
          >
          <b-button
            class="ml-2"
            variant="secondary"
            @click="cancelCheckListQuestionTemplate()"
            >Cancel</b-button
          >
          <br>
          <br>
          <label>
            {{ displayCategoryMsg }}
          </label>
        </div>
    <div id="CheckListTemplateCard">
        <div>
          <label class="mt-5">Category</label>
          <br />
          <b-form-select
          id="cmbCategory"
          v-model="selectedCheckListTemplateItem.Category"
          :options="categories"
        >
          <template #first>
            <b-form-select-option :value="null"
              >Please select a category...</b-form-select-option
            >
          </template>
        </div>
        <div class="mt-3">
          <label>Description</label>
          <br />
          <input
            v-model="selectedCheckListTemplateItem.Descr"
            class="inputchecklisttemplatequestions"
          />
        </div>
        <div class="mt-3">
          <label>Is Active</label>
          <b-form-checkbox 
            size="lg" 
            v-model="selectedCheckListTemplateItem.Active">
            </b-form-checkbox>
        </div>
        <div id="divQuestions" class="mt-5">
            <b-table
              id="questionsTable"
              :items="selectedCheckListTemplateItem.Questions"
              :fields="fields"
              :busy="isBusy"
              caption-top
              :select-mode="selectMode"
              selectable
              :current-page="currentPage"
              :per-page="perPage"
              @row-selected="onRowSelected"
              responsive
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
                @click="createQuestion()"
                >Add</b-button
              >
              <b-button
                size="md"
                class="mr-2 mb-3"
                variant="primary"
                @click="editQuestion()"
                >Edit</b-button
              >
              <b-button
                size="md"
                variant="danger"
                class="mb-3"
                @click="deleteQuestion()"
                >Delete</b-button
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
              :total-rows="this.selectedCheckListTemplateItem.Questions.length"
              :per-page="perPage"
              v-model="currentPage"
            />
      </div>
        </div>
      </div>
    </div>

    <b-modal
      id="modalAddEditQuestion"
      ref="modal-AddEditQuestion"
      title="Add / Edit Question"
      :body-text-variant="headerTextVariant"
      @ok="handleOk"
    >
      <form ref="form" @submit.stop.prevent="handleSubmit">
        <b-form-group
          label="Question Name"
          label-for="inpQuestionName"
          invalid-feedback="Question Name is required."
          :state="questionNameState"
        >
          <b-form-input
            id="inpQuestionName"
            v-model="questionTemplate.Descr"
            :state="questionNameState"
            required
          ></b-form-input>
        </b-form-group>
        
        <b-form-group
          label="Question Type"
          label-for="slcQuestionType"
          :state="slcQuestionTypeState"
        >
          <b-form-select 
            id="slcQuestionType" 
              v-model="questionTemplate.ControlTypeID" 
              :options="optionsQuestionType"
              :state="slcQuestionTypeState" 
              class="mt-1">
              </b-form-select>
        </b-form-group>

        <b-form-group
          label="Points (10-100)"
          label-for="numPoints"
          invalid-feedback="Points are required."
          :state="numPointsState"
        >
          <b-form-input
            id="numPoints"
            v-model="questionTemplate.Points"
            :state="numPointsState"
            type="number"
            max="100" min="10"
            required
          ></b-form-input>
        </b-form-group>

        <b-form-group
          label="Achievement"
          label-for="slcAchievement"
          :state="slcAchievementState"
        >
          <b-form-select 
            id="slcAchievement" 
              v-model="questionTemplate.AchievementID" 
              :options="achievements"
              :state="slcAchievementState" 
              class="mt-1">
              </b-form-select>
        </b-form-group>
      </form>
      <div class="mt-2">
        <label class="labelFormAddEdit">
          {{ msgQuestionExistOnThisTemplate }}
        </label>
         <label class="labelFormAddEdit">
          {{ msgError }}
        </label>
      </div>
    </b-modal>

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

    <b-modal
      id="mdlFailSaveTemplateQuestions"
      size="sm"
      ref="modal-FailSaveTemplateQuestions"
      title="Information On Save Template Question"
      ok-only
      ok-variant="secondary"
      ok-title="Close"
      :header-bg-variant="headerBgVariant"
      :body-bg-variant="bodyBgVariant"
      :body-text-variant="headerTextVariant"
      :header-text-variant="light"
    >
      <div class="d-block">
        <p>Fail on Save Try again or speak with the IT department.</p>
      </div>
    </b-modal>
  </div>
</template>

<script>
import api from "../helpers/ApiRequest.js";

export default {
  name: "CheckListTemplate_Questions",
  data: function () {
    return {
      headerBgVariant: "light",
      headerTextVariant: "dark",
      bodyBgVariant: "light",
      bodyTextVariant: "dark",
      selectedCheckListTemplateItem: {
        ID: null,
        Descr: null,
        Category: null,
        Active: null,
        Questions: [
          {
            ID: null,
            CheckListTemplateID: null,
            Descr: null,
            ControlTypeID: 1,
            ControlTypeDescr: null,
            Points: null,
            AchievementID: null,
            Index: null,
          },
        ],
      },
      categories: [
        { value: 0, text: "Developement" },
        { value: 1, text: "Analysis" },
        { value: 2, text: "Support" },
        { value: 3, text: "Custom" },
      ],
      achievements: [
        { value: 0, text: "Thumbs Up" },
        { value: 1, text: "Top Learner" },
        { value: 2, text: "Tech Wizard" },
        { value: 3, text: "Superstar" },
      ],
      fields: [
        {
          key: "Index",
        },
        {
          key: "ID",
          tdClass: "d-none",
          thClass: "d-none",
        },
        {
          key: "ChecklistTemplateID",
          tdClass: "d-none",
          thClass: "d-none",
        },
        {
          key: "Descr",
        },
        {
          key: "ControlTypeID",
          tdClass: "d-none",
          thClass: "d-none",
        },
        {
          key: "ControlTypeDescr",
        },
        {
          key: "Points",
        },
        {
          key: "AchievementID",
          tdClass: "d-none",
          thClass: "d-none",
        },
      ],
      isBusy: false,
      selected: [],
      selectMode: "single",
      currentPage: 1,
      perPage: 5,
      questionTemplate: {
        ID: null,
        CheckListTemplateID: null,
        Descr: null,
        ControlTypeID: 1,
        ControlTypeDescr: null,
        Points: null,
        AchievementID: null,
        Index: null,
      },
      questionNameState: null,
      questionName: "",
      msgQuestionExistOnThisTemplate: null,
      modalBodyTextSelect: "Please select a question to edit",
      optionsQuestionType: [
        { value: 1, text: "CheckBox" },
        { value: 2, text: "Slider" },
        { value: 3, text: "Textbox" },
      ],
      slcQuestionTypeState: null,
      numPointsState: null,
      slcAchievementState: null,
      msgError: null,
      isNew: false,
      displayCategoryMsg: null,
    };
  },
  async created() {
    let selectedTemplateData = this.$store.state.selectedCheckListTemplate;

    if (selectedTemplateData.length == 1) {
      this.selectedCheckListTemplateItem.ID = selectedTemplateData[0].ID;
      this.selectedCheckListTemplateItem.Descr = selectedTemplateData[0].Descr;
      this.selectedCheckListTemplateItem.Category =
        selectedTemplateData[0].Category;
      this.selectedCheckListTemplateItem.Active =
        selectedTemplateData[0].IsActive == null
          ? false
          : selectedTemplateData[0].IsActive == 0
          ? false
          : true;
      this.isBusy = true;
      this.selectedCheckListTemplateItem.Questions = await api.GetRequest(
        selectedTemplateData[0].ID,
        "CheckListTemplates_Questions_Load"
      );

      let indexNumber = 0;
      this.selectedCheckListTemplateItem.Questions.forEach(function (value) {
        value["Index"] = indexNumber;
        indexNumber++;
      });

      this.isBusy = false;
    } else {
      this.$router.push("/CheckListTemplates");
      this.initQuestionsTable();
    }
  },
  methods: {
    onRowSelected(items) {
      this.selected = items;
    },
    showModalSelect() {
      this.$refs["modal-select"].show();
    },
    showModalAddEditQuestuion() {
      this.msgCheckListTemplateExist = null;
      this.msgError = null;
      this.$refs["modal-AddEditQuestion"].show();
    },
    async saveCheckListQuestionTemplate() {
      var data = this.selectedCheckListTemplateItem;

      if (data.Category == null || this.isBlank(data.Descr)) {
        this.displayCategoryMsg = "Please select fill category or Description.";
      } else {
        this.displayCategoryMsg = null;
        var JsonData = data;
        var result = await api.PostRequest(
          JsonData,
          "CheckListTemplates_Questions_Save"
        );
        if (result == "success") {
          this.$router.push("/CheckListTemplates");
        } else {
          this.$refs["modal-FailSaveTemplateQuestions"].show();
        }
      }
    },
    isBlank(str) {
      return !str || /^\s*$/.test(str);
    },
    cancelCheckListQuestionTemplate() {
      this.$router.push("/CheckListTemplates");
    },
    initQuestionsTable() {
      //
    },
    createQuestion() {
      this.isNew = true;
      this.questionTemplate.Descr = "";
      this.questionTemplate.ControlTypeID = 1;
      this.questionTemplate.Points = 10;
      let selectedTemplateData = this.$store.state.selectedCheckListTemplate;
      this.questionTemplate.CheckListTemplateID = selectedTemplateData[0].ID;
      this.questionTemplate.ControlTypeDescr = "CheckBox";
      this.questionTemplate.AchievementID = 0;
      this.questionTemplate.Index = this.takeLastIndex();
      this.showModalAddEditQuestuion();
    },
    editQuestion() {
      this.isNew = false;
      if (this.selected.length > 0) {
        this.questionTemplate.Descr = this.selected[0].Descr;
        this.questionTemplate.ControlTypeID = this.selected[0].ControlTypeID;
        this.questionTemplate.ControlTypeDescr = this.selected[0].ControlTypeDescr;
        this.questionTemplate.Points = Number(this.selected[0].Points);
        this.questionTemplate.CheckListTemplateID = this.selected[0].ChecklistTemplateID;
        this.questionTemplate.ID = this.selected[0].ID;
        this.questionTemplate.AchievementID = this.selected[0].AchievementID;
        this.questionTemplate.Index = this.selected[0].Index;
        this.showModalAddEditQuestuion();
      } else {
        this.showModalSelect();
      }
    },
    deleteQuestion() {
      if (this.selected.length > 0) {
        const index = this.selected[0].Index;
        for (
          let i = 0;
          i < this.selectedCheckListTemplateItem.Questions.length;
          i++
        ) {
          if (this.selectedCheckListTemplateItem.Questions[i].Index == index) {
            this.selectedCheckListTemplateItem.Questions.splice(i, 1);
          }
        }
      } else {
        this.showModalSelect();
      }
    },
    handleOk(bvModalEvt) {
      // Prevent modal from closing
      bvModalEvt.preventDefault();
      // Trigger submit handler
      this.handleSubmit();
    },
    checkFormValidity() {
      const valid = this.$refs.form.checkValidity();
      return valid;
    },
    handleSubmit() {
      if (!this.checkFormValidity()) {
        this.msgError =
          "One or more fields are incorrect please fill them all correctly.";
        return;
      }
      this.$nextTick(() => {
        this.$bvModal.hide("modalAddEditQuestion");
        this.isBusy = true;

        var data = this.questionTemplate;

        // add
        if (this.isNew == true) {
          this.selectedCheckListTemplateItem.Questions.push({
            AchievementID: data.AchievementID,
            CheckListTemplateID: data.CheckListTemplateID,
            ControlTypeDescr: this.takeCategoryDescr(data.ControlTypeID),
            ControlTypeID: data.ControlTypeID,
            Descr: data.Descr,
            ID: data.ID,
            Points: Number(data.Points),
            Index: data.Index,
          });
          data = null;

          // Edit
        } else {
          const index = this.selected[0].Index;
          this.selectedCheckListTemplateItem.Questions[index].AchievementID =
            data.AchievementID;
          this.selectedCheckListTemplateItem.Questions[
            index
          ].CheckListTemplateID = data.CheckListTemplateID;
          this.selectedCheckListTemplateItem.Questions[
            index
          ].ControlTypeDescr = this.takeCategoryDescr(data.ControlTypeID);
          this.selectedCheckListTemplateItem.Questions[index].ControlTypeID =
            data.ControlTypeID;
          this.selectedCheckListTemplateItem.Questions[index].Descr =
            data.Descr;
          this.selectedCheckListTemplateItem.Questions[index].ID = data.ID;
          this.selectedCheckListTemplateItem.Questions[index].Points = Number(
            data.Points
          );
        }

        this.isBusy = false;
      });
    },
    takeLastIndex() {
      let index = 0;
      if (this.selectedCheckListTemplateItem.Questions.length == 0) {
        return index;
      } else {
        this.selectedCheckListTemplateItem.Questions.forEach(function (value) {
          index = value.Index;
        });
        return index + 1;
      }
    },
    takeCategoryDescr(categoryID) {
      var descr = "";
      this.optionsQuestionType.forEach(function (value) {
        if (categoryID == value["value"]) {
          descr = value["text"];
        }
      });
      return descr;
    },
  },
};
</script>


<style>
label {
  color: aliceblue;
}

label[for="inpQuestionName"],
label[for="slcQuestionType"],
label[for="numPoints"],
label[for="slcAchievement"],
.labelFormAddEdit {
  color: black;
}

#questionsTable {
  text-align: center;
  color: aliceblue;
}

#divHeaderbuttonsCheckListeTemplate {
  margin-top: 5%;
  margin-left: 12%;
}

#CheckListTemplateCard {
  margin-top: 1%;
  margin-left: 12%;
  margin-right: 12%;
  margin-bottom: 15%;
  border-top: 1px solid #ccc !important;
  border-color: rgba(0, 0, 0, 0.3);
}

.inputchecklisttemplatequestions {
  width: 20%;
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

#cmbCategory {
  width: 20% !important;
  background: rgba(0, 0, 0, 0.3);
  border: none;
  outline: none;
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

#msgQuestionExistOnThisTemplate {
  color: black !important;
}
</style>