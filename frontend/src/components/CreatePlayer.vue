<template>
  <div class="createPlayer">
    <div class="add_edit_player mt-5">
      <label for="usernamename">Username</label><br />
      <input
        id="txtUsernamecreate"
        v-model="playersData.txtUsernamecreate"
        type="text"
        name="usernamename"
        class="inputcreateplayer"
      />
      <br />
      <label for="passwordname">Password</label><br />
      <input
        id="txtPasswordcreate"
        v-model="playersData.txtPasswordcreate"
        type="password"
        name="passwordname"
        class="inputcreateplayer"
        disabled
      />
      <b-overlay
      :show="busy"
      rounded
      opacity="0.6"
      spinner-small
      spinner-variant="default"
      class="d-inline-block"
      >
        <b-button
          id="createAutoGeneratePassword"
          variant="primary"
          class="ml-2"
          @click="generatePassword()"
          :disabled="busy"
          >Auto Password</b-button
        >
      </b-overlay>
      <div id="divFirstname">
        <label for="firstnamename">Last Name</label><br />
        <input
          id="txtfirstname"
          v-model="playersData.txtfirstname"
          type="text"
          name="firstnamename"
          class="inputcreateplayer"
        />
      </div>
      <div>
        <label for="lastnamename">First Name</label><br />
        <input
          id="txtlastname"
          v-model="playersData.txtlastname"
          type="text"
          name="lastnamename"
          class="inputcreateplayer"
        />
      </div>
      <div>
        <label for="emailname">Email</label><br />
        <input
          id="txtemail"
          v-model="playersData.txtemail"
          type="text"
          name="emailname"
          class="inputcreateplayer"
        />
      </div>
      <div>
        <label for="hiredatepicker">Hiredate</label>
        <b-form-datepicker
          id="hiredatepicker"
          v-model="playersData.dthiredate"
        ></b-form-datepicker>
      </div>
      <div class="mt-4">
        <label for="department">Department</label><br />
        <b-form-select
          id="cmbDepartment"
          v-model="playersData.department"
          :options="departments"
        >
          <template #first>
            <b-form-select-option :value="null"
              >Please select a department...</b-form-select-option
            >
          </template>
        </b-form-select>
      </div>
      <div class="mt-4">
        <label for="position">Position</label><br />
        <b-form-select
          id="cmbPosition"
          v-model="playersData.position"
          :options="positions"
        >
          <template #first>
            <b-form-select-option :value="null"
              >Please select a position...</b-form-select-option
            >
          </template>
      </div>
      <br />
      <br />
      <div class="mb-5">
        <b-button id="savePlayerInfo" variant="primary" @click="saveData()"
        >Save</b-button
      >
      <b-button
        class="ml-2"
        variant="secondary"
        @click="cancelSave()"
        >Cancel</b-button
      >
      </div>
    </div>

    <b-modal
      id="mdlSave"
      size="sm"
      ref="modal-save"
      hide-footer
      title="Player Save"
      :header-bg-variant="headerBgVariant"
      :body-bg-variant="bodyBgVariant"
      :body-text-variant="bodyTextVariant"
      :header-text-variant="headerTextVariant"
    >
      <div class="d-block">
        <p>{{ modalBodyText }}</p>
      </div>
    </b-modal>
  </div>
   
</template>

<script>
import api from "../helpers/ApiRequest.js";

export default {
  name: "createPlayer",
  data: function () {
    return {
      playersData: {
        id: null,
        txtUsernamecreate: null,
        txtPasswordcreate: null,
        txtfirstname: null,
        txtlastname: null,
        txtemail: null,
        dthiredate: new Date().toJSON().slice(0, 10).replace(/-/g, "-"),
        position: null,
        department: null,
        statusID: 0,
      },
      elementhere: {},
      departments: [
        { value: 0, text: "Research and Development" },
        { value: 1, text: "Support" },
        { value: 2, text: "Marketing" },
        { value: 3, text: "Human Resource Management" },
        { value: 4, text: "Accounting and Finance" },
      ],
      positions: [
        { value: 0, text: "Developer" },
        { value: 1, text: "Analyst" },
        { value: 2, text: "Technical Support Engineer" },
      ],
      modalBodyText: "",
      headerBgVariant: "secondary",
      headerTextVariant: "light",
      bodyBgVariant: "secondary",
      bodyTextVariant: "light",
      resultForMail: null,
      isNew: true,
      busy: false,
      timeout: null,
    };
  },
  methods: {
    beforeDestroy() {
      this.clearTimeout();
    },
    onHidden() {
      // Return focus to the button once hidden
      this.$refs.button.focus();
    },
    clearTimeout() {
      if (this.timeout) {
        clearTimeout(this.timeout);
        this.timeout = null;
      }
    },
    setTimeout(callback) {
      this.clearTimeout();
      this.timeout = setTimeout(() => {
        this.clearTimeout();
        callback();
      }, 2000);
    },
    cancelSave() {
      this.$router.push("/PlayersManagement");
    },
    showModal() {
      this.$refs["modal-save"].show();
    },
    generatePassword() {
      this.busy = true;
      this.playersData.txtPasswordcreate = null;
      // Simulate an async request
      this.setTimeout(() => {
        var length = 8,
          charset =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
          retVal = "";
        for (var i = 0, n = charset.length; i < length; ++i) {
          retVal += charset.charAt(Math.floor(Math.random() * n));
        }
        this.playersData.txtPasswordcreate = retVal;
        this.busy = false;
      });
    },
    isNotNullableFields() {
      for (var key in this.playersData) {
        if (this.playersData[key] === "" || this.playersData[key] === null) {
          if (key != "id") {
            return false;
          }
        }
      }
      return true;
    },
    async saveData() {
      // ID can be null
      if (this.isNotNullableFields()) {
        try {
          var userNameExistValue = await api.GetRequest(
            this.playersData.txtUsernamecreate,
            "userNameExists"
          );

          let selectedData = this.$store.state.SelectedPlayers;
          let selectedUser = null;
          let selectedEmail = null;
          let selectedPass = null;
          if (selectedData != null) {
            selectedUser = selectedData[0]?.UserName;
            selectedEmail = selectedData[0]?.Email;
            selectedPass = selectedData[0]?.Password;
          }
          var userNameIsTheSame =
            this.playersData.txtUsernamecreate == selectedUser;
          var emailChanged = this.playersData.txtemail != selectedEmail;
          var passwordChanged =
            this.playersData.txtPasswordcreate != selectedPass;
          // Check user exist and not the same as the selected one
          if (userNameExistValue == "0" || userNameIsTheSame) {
            var result = await api.PostRequest(
              JSON.stringify(this.playersData),
              "savePlayer"
            );
            if (result == "success") {
              if (
                this.isNew ||
                emailChanged ||
                !userNameIsTheSame ||
                passwordChanged
              ) {
                this.sendEmail();
              }
              this.$router.push("/PlayersManagement");
            } else {
              this.headerBgVariant = "danger";
              this.headerTextVariant = "light";
              this.modalBodyText = "There was an error with the save.";
              this.showModal();
            }
          } else {
            this.headerBgVariant = "warning";
            this.headerTextVariant = "dark";
            this.modalBodyText = "The user name Exist, choose another";
            this.showModal();
          }
        } catch (error) {
          this.modalBodyText = "There was an error with the save";
          this.headerBgVariant = "danger";
          this.headerTextVariant = "light";
          this.showModal();
        }
      } else {
        this.headerBgVariant = "warning";
        this.headerTextVariant = "dark";
        this.modalBodyText = "Empty Fields!";
        this.showModal();
      }
    },
    async sendEmail() {
      try {
        var result = await api.PostRequest(
          JSON.stringify(this.playersData),
          "sendEmailWithLogin"
        );
        this.resultForMail = result;
      } catch (error) {
        this.resultForMail = "fail";
      }
    },
    loadData() {
      let selectedData = this.$store.state.SelectedPlayers;
      if (selectedData != null) {
        // Load all fields with the selcted values
        if (selectedData.length == 1) {
          this.playersData.id = selectedData[0].ID;

          this.playersData.txtUsernamecreate = selectedData[0].UserName;
          this.playersData.txtPasswordcreate = selectedData[0].Password;

          var items = selectedData[0].Player.split(" ");
          var firstName = items[0];
          var lastName = items[1];
          this.playersData.txtfirstname = firstName;
          this.playersData.txtlastname = lastName;
          this.playersData.txtemail = selectedData[0].Email;

          var date = new Date(selectedData[0].HireDate);
          var formatedDate =
            date.getFullYear() +
            "-" +
            ("0" + (date.getMonth() + 1)).slice(-2) +
            "-" +
            ("0" + date.getDate()).slice(-2);
          this.playersData.dthiredate = formatedDate;
          this.playersData.position = selectedData[0].PositionID;
          this.playersData.department = selectedData[0].DepartmentID;
          this.playersData.statusID = selectedData[0].StatusID;
        }
      }
    },
  },
  created() {
    // init load
    this.isNew = this.$store.state.isNew ?? true;
    this.loadData();
  },
};
</script>


<style>
label {
  color: aliceblue;
}

.add_edit_player {
  width: 20%;
  margin-left: 40%;
  text-align: center;
}
#txtPasswordcreate {
  display: inline;
  height: 14%;
}

#createAutoGeneratePassword {
  display: inline;
  height: 13%;
  margin-bottom: 9%;
}

#savePlayerInfo {
  float: none;
}

#divFirstname {
  clear: both;
}

#cmbDepartment {
  background: rgba(0, 0, 0, 0.3);
  border: none;
  outline: none;
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
  font-size: 13px;
}

#cmbPosition {
  background: rgba(0, 0, 0, 0.3);
  border: none;
  outline: none;
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
  font-size: 13px;
}

#hiredatepicker__outer_ {
  background: rgba(0, 0, 0, 0.3);
  box-shadow: inset 0 -5px 45px rgba(100, 100, 100, 0.2),
    0 1px 1px rgba(255, 255, 255, 0.2);
  border: none;
  outline: none;
  -webkit-transition: box-shadow 0.5s ease;
  -moz-transition: box-shadow 0.5s ease;
  -o-transition: box-shadow 0.5s ease;
  -ms-transition: box-shadow 0.5s ease;
  transition: box-shadow 0.5s ease;
  border-radius: 4px;
}

#hiredatepicker {
  color: #fff;
}

#hiredatepicker__value_ {
  color: #fff;
}

.inputcreateplayer {
  width: 350px;
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
  margin-bottom: 2% !important;
}

</style>