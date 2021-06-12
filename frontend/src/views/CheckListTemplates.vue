<template>
  <div class="CheckListTemplatesPage">
    <NavBar />
    <div id="CheckListTemplatesData">
      <b-table
        id="ChecklistTable"
        :items="items"
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
            @click="createCheckList()"
            >Add</b-button
          >
          <b-button
            size="md"
            class="mr-2 mb-3"
            variant="primary"
            @click="customizeCheckList()"
            >Edit</b-button
          >
          <b-button
            size="md"
            variant="danger"
            class="mb-3 mr-5"
            @click="deleteChecklist()"
            >Delete</b-button
          >
          <b-button
            size="md"
            variant="danger"
            class="mb-3 mr-1"
            style="float: right"
            @click="removePlayer()"
            >UnAssign Player
          </b-button>
          <b-button
            size="md"
            variant="primary"
            class="mb-3 mr-2"
            style="float: right"
            @click="addPlayer()"
            >Assign Player
          </b-button>
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
          :total-rows="this.items.length"
          :per-page="perPage"
          v-model="currentPage"
        />
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

    <b-modal
      id="mdlSuccessAssign"
      size="sm"
      ref="modal-successAssign"
      title="Information for Assign"
      ok-only
      ok-variant="secondary"
      ok-title="Close"
      :header-bg-variant="headerBgVariant"
      :body-bg-variant="bodyBgVariant"
      :body-text-variant="headerTextVariant"
      :header-text-variant="light"
    >
      <div class="d-block">
        <p>{{ modalBodyAssignEmployee }}</p>
      </div>
    </b-modal>

    <b-modal
      id="mdlDelete"
      size="sm"
      ref="modal-delete"
      title="Delete Player"
      ok-only
      ok-variant="secondary"
      ok-title="Cancel"
      :header-bg-variant="headerDeleteBgVariant"
      :body-bg-variant="headerDeleteTextVariant"
      :body-text-variant="bodyTextVariant"
      :header-text-variant="headerTextVariant"
    >
      <div class="d-block">
        <p>{{ modalBodyTextDelete }}</p>
      </div>
    </b-modal>

    <b-modal
      id="modalAddTemplate"
      ref="modal-add"
      title="Add Template"
      :body-text-variant="headerTextVariant"
      @show="resetModal"
      @hidden="resetModal"
      @ok="handleOk"
    >
      <form ref="form" @submit.stop.prevent="handleSubmit">
        <b-form-group
          label="Template Name"
          label-for="templatrNameInput"
          invalid-feedback="Template Name is required"
          :state="templateNameState"
        >
          <b-form-input
            id="templatrNameInput"
            v-model="templateName"
            :state="templateNameState"
            required
          ></b-form-input>
        </b-form-group>
      </form>
      <div class="mt-2">
        <label id="msgCheckListTemplateExist">
          {{ msgCheckListTemplateExist }}
        </label>
      </div>
    </b-modal>

    <b-modal
      id="modalAssignPlayer"
      ref="modal-assign-player"
      title="Assign Player"
      :body-text-variant="headerTextVariant"
      @ok="handleOkOPlayerToAssign"
    >
      <form ref="form" @submit.stop.prevent="handleSubmit">
        <b-form-group label="Players" label-for="slcPlayerToAssign">
          <b-form-select
            id="slcPlayerToAssign"
            v-model="selectedPlayerToAssign"
            :options="playersToAssign"
            class="mt-1"
          >
          </b-form-select>
        </b-form-group>
      </form>
      <div class="mt-2">
        <label id="infoFromAssignPlayer">
          {{ msgInfoFromAssignPlayer }}
        </label>
      </div>
    </b-modal>

    <b-modal
      id="modalUnAssignPlayer"
      ref="modal-unassign-player"
      title="UnAssign Player"
      :body-text-variant="headerTextVariant"
      @ok="handleOkOPlayerToUnAssign"
    >
      <form ref="form" @submit.stop.prevent="handleSubmit">
        <b-form-group label="Players" label-for="slcPlayerToUnAssign">
          <b-form-select
            id="slcPlayerToUnAssign"
            v-model="selectedPlayerToUnAssign"
            :options="playersToUnAssign"
            class="mt-1"
          >
          </b-form-select>
        </b-form-group>
      </form>
      <div class="mt-2">
        <label id="infoFromAssignPlayer">
          {{ msgInfoFromAssignPlayer }}
        </label>
      </div>
    </b-modal>
  </div>
</template>


<script>
import NavBar from "@/components/NavBar.vue";
import api from "../helpers/ApiRequest.js";

export default {
  name: "CheckListTemplatesPage",
  data: function () {
    return {
      items: [],
      fields: [
        {
          key: "ID",
        },
        {
          key: "Descr",
        },
        {
          key: "IsActive",
          tdClass: "d-none",
          thClass: "d-none",
        },
        {
          key: "Category",
          tdClass: "d-none",
          thClass: "d-none",
        },
      ],
      isBusy: false,
      selected: [],
      selectMode: "single",
      currentPage: 1,
      perPage: 5,
      headerBgVariant: "light",
      headerTextVariant: "dark",
      bodyBgVariant: "light",
      bodyTextVariant: "dark",
      modalBodyTextSelect: "Please select a check list template",
      modalBodyTextDelete: "Fail to delete a check list template",
      modalBodyAssignEmployee: "Employee assigned to checklist successfully",
      templateName: "",
      templateNameState: null,
      msgCheckListTemplateExist: null,
      playersToAssign: [],
      playersToUnAssign:[],
      msgInfoFromAssignPlayer: null,
      msgInfoFromUnAssignPlayer: null,
      selectedPlayerToAssign: null,
      selectedPlayerToUnAssign: null,
      assignPlayerToCheckList: {
        CheckListTemplateID: null,
        PlayerID: null,
      },
    };
  },
  components: {
    NavBar,
  },
  async created() {
    if (localStorage.getItem("username") != "admin") {
      this.isBusy = !this.isBusy;
      this.$router.push("/home");
    } else {
      this.isBusy = true;
      var data = await api.GetRequest("get", "loadCheckListTemplates");
      this.items = data;
      this.isBusy = false;
    }
  },
  methods: {
    showModalAdd() {
      this.msgCheckListTemplateExist = null;
      this.$refs["modal-add"].show();
    },
    showModalAssignPlayer() {
      this.playersToAssign = [];
      this.msgInfoFromAssignPlayer = null;
      this.$refs["modal-assign-player"].show();
    },
    showModalUnAssignPlayer() {
      this.playersToUnAssign = [];
      this.msgInfoFromUnAssignPlayer = null;
      this.$refs["modal-unassign-player"].show();
    },
    showModalSelect() {
      this.$refs["modal-select"].show();
    },
    showModalAssignSuccess() {
      this.$refs["modal-successAssign"].show();
    },
    showModalDelete() {
      this.$refs["modal-delete"].show();
    },
    onRowSelected(items) {
      this.selected = items;
      //this.$store.commit("chnagePlayersValues", items);
    },
    createCheckList() {
      this.showModalAdd();
    },
    customizeCheckList() {
      if (this.selected.length > 0) {
        this.$store.commit("changeCheckListTemplate", this.selected);
        this.$router.push("/CheckListTemplate_Questions");
      } else {
        this.showModalSelect();
      }
    },
    async addPlayer() {
      if (this.selected.length > 0) {
        this.showModalAssignPlayer();
        this.playersToAssign = await api.GetRequest(
          "get",
          "Player_Load_To_Assign"
        );
        this.selectedPlayerToAssign = this.playersToAssign[0].value;
      } else {
        this.showModalSelect();
      }
    },
    async removePlayer() {
      if (this.selected.length > 0) {
        this.showModalUnAssignPlayer();
        this.playersToUnAssign = await api.GetRequest(
          this.selected[0].ID.toString(),
          "Player_Load_To_UnAssign"
        );
        this.selectedPlayerToUnAssign = this.playersToUnAssign[0].value;
      } else {
        this.showModalSelect();
      }
    },
    async deleteChecklist() {
      if (this.selected.length > 0) {
        if (this.selected[0].ID > 3) {
          this.isBusy = true;
          var result = await api.PostRequest(
            this.selected[0].ID.toString(),
            "tryDeleteCheckListTemplate"
          );

          if (result == "success") {
            this.isBusy = true;
            var data = await api.GetRequest("get", "loadCheckListTemplates");
            this.items = data;
          } else {
            this.modalBodyTextDelete = "Fail to delete a check list template, it is in use by a player!";
            this.showModalDelete();
          }
          this.isBusy = false;
        } else {
          this.modalBodyTextDelete =
            "You cannot delete the default check list templates";
          this.showModalDelete();
        }
      } else {
        this.showModalSelect();
      }
    },
    resetModal() {
      this.templateName = "";
      this.templateNameState = null;
    },
    isBlank(str) {
      return !str || /^\s*$/.test(str);
    },
    checkFormValidity() {
      const valid = this.$refs.form.checkValidity();
      this.templateNameState = valid;

      if (this.isBlank(this.templateName)) {
        this.templateNameState = false;
        return false;
      }

      return valid;
    },
    handleOk(bvModalEvt) {
      // Prevent modal from closing
      bvModalEvt.preventDefault();
      // Trigger submit handler
      this.handleSubmit();
    },
    async handleSubmit() {
      // Exit when the form isn't valid
      if (!this.checkFormValidity()) {
        return;
      }

      var exists = await api.GetRequest(
        this.templateName,
        "checkListTemplate_Exists"
      );

      if (exists == 1) {
        this.msgCheckListTemplateExist =
          "The template name seems fine but you cannot use this name because it exists already.";
        return;
      } else {
        var result = await api.PostRequest(
          this.templateName,
          "tryCheckListTemplatesInsert"
        );

        if (result == "success") {
          this.msgCheckListTemplateExist = null;
          // Hide the modal manually
          this.$nextTick(async () => {
            this.$bvModal.hide("modalAddTemplate");
            this.isBusy = true;
            var data = await api.GetRequest("get", "loadCheckListTemplates");
            this.items = data;
            this.isBusy = false;
          });
        } else {
          return;
        }
      }
    },
    handleOkOPlayerToAssign(bvModalEvt) {
      // Prevent modal from closing
      bvModalEvt.preventDefault();
      // Trigger submit handler
      this.handleSubmitPlayerToAssign();
    },
    handleOkOPlayerToUnAssign(bvModalEvt) {
      // Prevent modal from closing
      bvModalEvt.preventDefault();
      // Trigger submit handler
      this.handleSubmitPlayerToUnAssign();
    },
    handleSubmitPlayerToAssign() {
      this.$nextTick(async () => {
        this.assignPlayerToCheckList.CheckListTemplateID = this.selected[0].ID;
        this.assignPlayerToCheckList.PlayerID = this.selectedPlayerToAssign;
        var result = await api.PostRequest(
          this.assignPlayerToCheckList,
          "CheckListTemplates_Assign_Player"
        );

        if (result == "success") {
          this.$bvModal.hide("modalAssignPlayer");
          this.modalBodyAssignEmployee = "Employee assigned to checklist successfully";
          this.showModalAssignSuccess();
        } else {
          this.msgInfoFromAssignPlayer =
            "Error while assign a checklist into a Player.";
        }
      });
    },
    handleSubmitPlayerToUnAssign() {
      this.$nextTick(async () => {
        var result = await api.PostRequest(
          this.selectedPlayerToUnAssign,
          "CheckListTemplates_UnAssign_Player"
        );

        if (result == "success") {
          this.$bvModal.hide("modalUnAssignPlayer");
          this.modalBodyAssignEmployee = "Employee unassigned to checklist successfully";
          this.showModalAssignSuccess();
        } else {
          this.msgInfoFromUnAssignPlayer =
            "Error while unassign a checklist into a Player.";
        }
      });
    }
  },
};
</script>

<style>
.CheckListTemplatesPage {
  text-align: center;
}

#CheckListTemplatesData {
  margin-top: 4%;
  margin-left: 10%;
  margin-right: 10%;
  height: 100%;
}

label {
  color: aliceblue;
}

#ChecklistTable {
  color: aliceblue;
}

label[for="templatrNameInput"],
label[for="slcPlayerToAssign"],
label[for="slcPlayerToUnAssign"],
#msgCheckListTemplateExist {
  color: black !important;
}
</style>