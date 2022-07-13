// Import the functions you need from the SDKs you need
import { initializeApp } from "https://www.gstatic.com/firebasejs/9.8.4/firebase-app.js";
import { getAnalytics } from "https://www.gstatic.com/firebasejs/9.8.4/firebase-analytics.js";
import { getDatabase } from "firebase/database";

// TODO: Replace the following with your app's Firebase project configuration
// See: https://firebase.google.com/docs/web/learn-more#config-object

const firebaseConfig = {
    apiKey: "AIzaSyB-qBlQZFnMJ_optJ7a3xqEDC0f3YzXNLs",
    authDomain: "filament-zombies.firebaseapp.com",
    databaseURL: "https://filament-zombies-default-rtdb.firebaseio.com",
    projectId: "filament-zombies",
    storageBucket: "filament-zombies.appspot.com",
    messagingSenderId: "721683477410",
    appId: "1:721683477410:web:fa0a9cc8070ccf4fd8b8fb",
    measurementId: "G-GCD00YTR04"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);
const database = getDatabase(app);
const analytics = getAnalytics(app);