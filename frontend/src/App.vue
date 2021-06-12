<template>
  <div id="app">
    <div id="divRooterView">
      <router-view />
    </div>
    <Footer />
  </div>
</template>

<script>
import Footer from "@/components/Footer.vue";
export default {
  name: "app",
  components: {
    Footer,
  },
};
</script>

<style>
#app {
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  position: relative;
  min-height: 100vh;
}

#divRooterView {
  padding-bottom: 100px;
}

.nav-link {
  color: white !important;
}
</style>
