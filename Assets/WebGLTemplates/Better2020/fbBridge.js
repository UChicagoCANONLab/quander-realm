// Import the functions you need from the SDKs you need
import { initializeApp } from "https://www.gstatic.com/firebasejs/9.8.4/firebase-app.js";
import { getAnalytics } from "https://www.gstatic.com/firebasejs/9.8.4/firebase-analytics.js";
import { getDatabase, ref, child, get } from "https://www.gstatic.com/firebasejs/9.8.4/firebase-database.js";

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
console.log("Initializing firebase");
const app = initializeApp(firebaseConfig);
const database = getDatabase(app);
const analytics = getAnalytics(app);

DCE= function(codeString) {
	console.log('researchCodes/' + codeString);
	const dbRef = ref(getDatabase());
	get(child(dbRef, `researchCodes/` + codeString)).then((snapshot) => {
	  if (snapshot.exists()) {
		console.log(snapshot.val());
		GameInstance.SendMessage('SaveManager', 'LoadCallback', 'T');
	  } else {
		console.log("No data available");
		GameInstance.SendMessage('SaveManager', 'LoadCallback', 'F');
	  }
	}).catch((error) => {
	  console.error(error);
	  GameInstance.SendMessage('SaveManager', 'LoadCallback', 'F');
	});
}

/*
// This works to talk to firebase

const dbRef = ref(getDatabase());
get(child(dbRef, `researchCodes/`)).then((snapshot) => {
  if (snapshot.exists()) {
    console.log(snapshot.val());
  } else {
    console.log("No data available");
  }
}).catch((error) => {
  console.error(error);
});
*/
