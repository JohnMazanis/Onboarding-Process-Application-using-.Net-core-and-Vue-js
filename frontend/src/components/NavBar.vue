<template>
  <div class="theNavigationBar">
    <b-navbar
      class="navbar navbar-expand-lg navbar-dark bg-dark"
      toggleable="lg"
      type="dark"
      variant="dark"
    >
      <b-navbar-brand>
        <img
          src="../../wwwroot/img/ess_default_logo.png"
          class="mr-4"
          width="60"
          height="60"
          alt="logo"
        />
      </b-navbar-brand>
      <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>
      <b-collapse id="nav-collapse" is-nav>
        <b-navbar-nav>
          <b-nav-item class="navbar-nav mr-auto">
            <a class="nav-link"
              ><router-link
                :to="getHomePath()"
                style="text-decoration: none; color: inherit"
                >Home</router-link
              ></a
            >
          </b-nav-item>
          <b-nav-item class="navbar-nav mr-auto">
            <a class="nav-link"
              ><router-link
                to="/aboutus"
                style="text-decoration: none; color: inherit"
                >About</router-link
              ></a
            >
          </b-nav-item>
          <b-nav-item
            class="navbar-nav mr-auto nav-item-managePlayers"
            v-if="displayManagement()"
          >
            <a class="nav-link"
              ><router-link
                to="/PlayersManagement"
                style="text-decoration: none; color: inherit"
                >Manage Players</router-link
              ></a
            >
          </b-nav-item>
          <b-nav-item
            class="navbar-nav mr-auto nav-item-checklisttemplates"
            v-if="displayManagement()"
          >
            <a class="nav-link"
              ><router-link
                to="/CheckListTemplates"
                style="text-decoration: none; color: inherit"
                >Checklist Customization</router-link
              ></a
            >
          </b-nav-item>
        </b-navbar-nav>
        <b-navbar-nav class="ml-auto">
          <form class="form-inline my-2 my-lg-0">
            <button
              id="btnLogout"
              class="btn btn-primary my-2 my-sm-0"
              @click="goToLogin()"
            >
              Logout
            </button>
          </form>
        </b-navbar-nav>
      </b-collapse>
    </b-navbar>
  </div>
</template>

<script>
export default {
  name: "theNavigationBar",
  data: function () {
    return {
      seen: false,
    };
  },
  methods: {
    goToLogin() {
      this.$router.push("/");
    },
    getHomePath() {
      return "/home";
    },
    displayManagement() {
      return localStorage.getItem("username") == "admin";
    },
  },
  created() {
    if (localStorage.getItem("username") == undefined) {
      this.$router.push("/");
    }
  },
};
</script>

<style>
.navbar{
  background: #053860 !important;
  border-bottom: 1px solid #ccc!important;
  border-color: aliceblue;
}

#btnLogout{
  background: #07868d !important;
  border-color: #07868d !important;
}
</style>