import AppView from "./AppView.js";

//  Root app object
const app = document.getElementById("app");

const view = new AppView(app, {
    onCloseMobileModal() {
        console.log("Responding to close modal button click");
    }
});