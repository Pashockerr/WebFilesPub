const app = Vue.createApp({
    data() {
        return {
            login: true
        }
    },
    methods: {
        loginSwitch() {
            this.login = true;
        },
        registrationSwitch() {
            this.login = false;
        }
    }
});

app.mount("#app");