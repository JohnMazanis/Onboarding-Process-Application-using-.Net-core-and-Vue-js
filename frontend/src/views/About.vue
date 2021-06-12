<template>
  <div class="about">
    <NavBar />
    <div id="aboutData" style="text-align: center">
      <label style="font-size: 20px"> How to PLAY </label><br />
      <label style="font-size: 15px">
        <a class="nav-link"
          ><router-link :to="getHomePath()" style="color: inherit"
            >Click here to go to Home screen...</router-link
          ></a
        >
      </label>
      <div>
        <label style="font-size: 15px; margin-top: 20px">
          After you have successfully logged in, you will be navigated to your
          personal dashboard. Just click on the <strong>Play</strong> button to
          start!</label
        >
      </div>
      <br />
      <br />

      <div>
        <label style="font-size: 15px">
          You will be presented with a list of tasks that you should complete.
          Please read the instructions carefully, and once you have completed
          doing what the task is asking, fill in the appropriate information and
          press the <strong>Submit</strong> button on the right side of each
          task. Each task gives you points and an Achievement!</label
        >
      </div>
      <br />
      <br />
      <div>
        <label style="font-size: 15px">
          Depending on the type of question, you will be asked to fill in
          information in the following ways:</label
        >
        <br />
        <label style="font-size: 15px"
          >-- Provide a score (from 0 to 100) regarding the question you were
          asked --</label
        >
        <br />
        <label style="font-size: 15px"
          >-- Fill in some information, writing some text --</label
        >
        <br />
        <label style="font-size: 15px">
          -- Confirm you have completed the task, by clicking in a checkbox
          --</label
        >
      </div>
      <br />
      <br />

      <div>
        <label style="font-size: 15px"
          >If you need to make any changes after you have submitted your input,
          you can click on the Revert button next to the
          <strong>Submit</strong> button. Your input will be cleared, and you
          can resubmit your new input.</label
        >
      </div>
      <br />
      <br />

      <div>
        <label style="font-size: 15px"
          >When you return to your personal dashboard, you can view several
          important information:</label
        >
        <br />
        <label style="font-size: 15px">Your progress, in percentage</label>
        <br />
        <label style="font-size: 15px"
          >-- The total amount of points you accumulated as well as your
          Achievements --</label
        >
        <br />
        <label style="font-size: 15px"
          >-- How many days till you start your new experience at the company
          --</label
        >
        <br />
        <label style="font-size: 15px"
          >-- The leaderboard and how you stand in it --</label
        >
      </div>
      <br />
      <br />

      <div><label style="font-size: 15px">Good luck and have fun! Give us a feedback bellow...</label></div>
      <br />
      <br />
      <div>
        <iframe src="https://docs.google.com/forms/d/e/1FAIpQLSeUA49woeHXGQ5aKn-8YJuqUHouG4a5v0lMbi7m4izso6ymGQ/viewform?embedded=true" width="640" height="1810" frameborder="0" marginheight="0" marginwidth="0">Loading…</iframe>
      </div>
    </div>
  </div>
</template>

<script>
import NavBar from "@/components/NavBar.vue";
export default {
  name: "about",
  components: {
    NavBar,
  },
  methods: {
    getHomePath() {
      return "/home";
    },
  },
};
</script>

<style>
#aboutData {
  margin-top: 4%;
  margin-left: 10%;
  margin-right: 10%;
  height: 100%;
}
</style>