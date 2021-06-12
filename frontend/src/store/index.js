import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

export default new Vuex.Store({
  state:{
    SelectedPlayers: [],
    isNew: true,
    selectedCheckListTemplate: []
  },
  mutations: {
    chnagePlayersValues (state, n) {
      state.SelectedPlayers = n
    },
    changeIsNewValue (state, isNew) {
      state.isNew = isNew
    },
    changeCheckListTemplate (state, n) {
      state.selectedCheckListTemplate = n
    },
  },
  actions: {
  },
  modules: {
  }
})
