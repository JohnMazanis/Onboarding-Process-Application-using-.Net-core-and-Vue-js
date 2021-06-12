<template>
  <div class="PlayersManagement">
    <NavBar />
    <div id="divplayersData">
      <b-table
        id="playersTable"
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
            @click="createPlayer()"
            >Add</b-button
          >
          <b-button
            size="md"
            class="mr-2 mb-3"
            variant="primary"
            @click="editPlayer()"
            >Edit</b-button
          >
          <b-button
            size="md"
            variant="danger"
            class="mb-3"
            @click="deletePlayer()"
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
      ok-only
      ok-variant="secondary"
      ok-title="Cancel"
      title="Information Grid"
      :header-bg-variant="headerBgVariant"
      :body-bg-variant="bodyBgVariant"
      :body-text-variant="bodyTextVariant"
      :header-text-variant="headerTextVariant"
    >
      <div class="d-block">
        <p>{{ modalBodyTextSelect }}</p>
      </div>
    </b-modal>

    <b-modal
      id="mdlDelete"
      size="sm"
      ref="modal-delete"
      ok-only
      ok-variant="secondary"
      ok-title="Cancel"
      title="Delete Player"
      :header-bg-variant="headerDeleteBgVariant"
      :body-bg-variant="headerDeleteTextVariant"
      :body-text-variant="bodyTextVariant"
      :header-text-variant="headerTextVariant"
    >
      <div class="d-block">
        <p>{{ modalBodyTextDelete }}</p>
      </div>
    </b-modal>
  </div>
</template>

<script>
import NavBar from "@/components/NavBar.vue";
import api from "../helpers/ApiRequest.js";

export default {
  name: "PlayersManagement",
  data: function () {
    return {
      items: [],
      fields: [
        {
          key: "ID",
          tdClass: "d-none",
          thClass: "d-none",
        },
        {
          key: "Player",
        },
        {
          key: "UserName",
        },
        {
          key: "Password",
          tdClass: "d-none",
          thClass: "d-none",
        },
        {
          key: "Email",
        },
        {
          key: "CreateDate",
        },
        {
          key: "HireDate",
        },
        {
          key: "PositionID",
          tdClass: "d-none",
          thClass: "d-none",
        },
        {
          key: "PositionDescr",
        },
        {
          key: "DepartmentID",
          tdClass: "d-none",
          thClass: "d-none",
        },
        {
          key: "DepartmentDescr",
        },
        {
          key: "StatusID",
          tdClass: "d-none",
          thClass: "d-none",
        },
        {
          key: "StatusDescr",
        },
        {
          key: "CheckList",
        },
      ],
      isBusy: false,
      selected: [],
      selectMode: "single",
      headerBgVariant: "light",
      headerTextVariant: "dark",
      bodyBgVariant: "light",
      bodyTextVariant: "dark",
      headerDeleteBgVariant: "light",
      headerDeleteTextVariant: "Dark",
      modalBodyTextSelect: "Please select a player",
      modalBodyTextDelete: "Fail to delete a player",
      currentPage: 1,
      perPage: 5,
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
      var data = await api.GetRequest("get", "loadplayers");
      this.items = data;
      this.isBusy = false;
    }
  },
  methods: {
    showModalSelect() {
      this.$refs["modal-select"].show();
    },
    showModalDelete() {
      this.$refs["modal-delete"].show();
    },
    onRowSelected(items) {
      this.selected = items;
      this.$store.commit("chnagePlayersValues", items);
    },
    createPlayer() {
      this.$store.commit("chnagePlayersValues", null);
      this.$store.commit("changeIsNewValue", true);
      this.$router.push("/addEditPlayer");
    },
    editPlayer() {
      // Move to create edit player
      if (this.selected.length > 0) {
        this.$store.commit("changeIsNewValue", false);
        this.$router.push("/addEditPlayer");
      } else {
        this.showModalSelect();
      }
    },
    async deletePlayer() {
      if (this.selected.length > 0) {
        this.isBusy = true;
        var result = await api.PostRequest(
          this.selected[0].ID.toString(),
          "deletePlayer"
        );

        if (result == "success") {
          this.isBusy = true;
          var data = await api.GetRequest("get", "loadplayers");
          this.items = data;
        } else {
          this.showModalDelete();
        }

        this.isBusy = false;
      } else {
        this.showModalSelect();
      }
    },
  },
};
</script>
  
<style>
#divplayersData {
  margin-top: 4%;
  margin-left: 10%;
  margin-right: 10%;
  height: 100%;
  text-align: center;
}

label {
  color: aliceblue;
}

#playersTable {
  color: aliceblue;
}

.page-link {
  color: white !important;
  background-color: #053860 !important;
}
</style>