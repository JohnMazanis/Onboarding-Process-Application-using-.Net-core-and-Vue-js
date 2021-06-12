<template>
  <div id="loginPage" class="login">
    <img
      src="../../wwwroot/img/ess_default_logo.png"
      class="mb-4"
      width="150"
      height="150"
      alt="logo"
    />
    <form method="post">
      <input
        id="txtUsername"
        v-model="user.txtUsername"
        type="text"
        name="u"
        placeholder="Username"
        required="required"
        class="inputloginpage"
      />
      <input
        id="txtPassword"
        v-model="user.txtPassword"
        type="password"
        name="p"
        placeholder="Password"
        required="required"
        class="inputloginpage"
      />
      <b-button
        id="btnGoToLogin"
        type="button"
        class="btn-large"
        variant="primary"
        @click="goToHome()"
        :disabled="busy"
        >Login</b-button
      >
    </form>
    <div class="mt-4">
      <label>
        {{ msgOnClickLogin }}
      </label>
    </div>
  </div>
</template>

<script>
import api from "../helpers/ApiRequest.js";

export default {
  name: "login",
  data: function () {
    return {
      user: {
        txtUsername: "",
        txtPassword: "",
      },
      userExist: "0",
      msgOnClickLogin: null,
      busy: false,
    };
  },
  methods: {
    async goToHome() {
      this.busy = true;
      if (this.user.txtUsername != "" && this.user.txtPassword != "") {
        await this.checkUserExist();
        if (this.userExist == "1") {
          localStorage.setItem("username", this.user.txtUsername);
          this.busy = false;
          this.$router.push("/home");
        } else {
          this.msgOnClickLogin = "Username or password is incorect.";
        }
      } else {
        this.msgOnClickLogin = "Please fill username and password to login.";
      }
      this.busy = false;
    },
    async checkUserExist() {
      try {
        var userExistValue = await api.GetRequest(
          JSON.stringify(this.user),
          "userExists"
        );
        this.userExist = userExistValue;
      } catch (error) {
        this.userExist = "0";
        this.msgOnClickLogin = "Something went wrong with the login action.";
      }
    },
  },
};
</script>

<style>
@import url(https://fonts.googleapis.com/css?family=Open+Sans);

* {
  -webkit-box-sizing: border-box;
  -moz-box-sizing: border-box;
  -ms-box-sizing: border-box;
  -o-box-sizing: border-box;
  box-sizing: border-box;
}

html {
  width: 100%;
  height: 100%;
  overflow: auto;
}

body {
  width: 100%;
  height: 100%;
  font-family: "Open Sans", sans-serif;
  background: #053860;
}
.login {
  position: absolute;
  top: 30%;
  left: 50%;
  margin: -150px 0 0 -150px;
  width: 300px;
  height: 300px;
  text-align: center;
}
.login h1 {
  color: #fff;
  text-shadow: 0 0 10px rgba(0, 0, 0, 0.3);
  letter-spacing: 1px;
  text-align: center;
}

.inputloginpage {
  width: 100%;
  margin-bottom: 10px;
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
  margin-bottom: 5% !important;
}
input:focus {
  box-shadow: inset 0 -5px 45px rgba(100, 100, 100, 0.4),
    0 1px 1px rgba(255, 255, 255, 0.2);
}

.btn-primary {
  background: #07868d !important;
  border-color: #07868d !important;
}

.btn-success {
  background: #07868d !important;
  border-color: #07868d !important;
}

.btn-warning {
  background: #07868d !important;
  border-color: #07868d !important;
  color: white !important;
}

#btnGoToLogin {
  width: 100%;
}
</style>