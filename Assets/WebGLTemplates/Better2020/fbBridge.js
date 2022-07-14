// Import the functions you need from the SDKs you need
import { initializeApp } from "https://www.gstatic.com/firebasejs/9.8.4/firebase-app.js";
import { getAnalytics } from "https://www.gstatic.com/firebasejs/9.8.4/firebase-analytics.js";
import { getDatabase, ref, child, get } from "https://www.gstatic.com/firebasejs/9.8.4/firebase-database.js";

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

const app = initializeApp(firebaseConfig);
const database = getDatabase(app);
const analytics = getAnalytics(app);

JSDoesResearchCodeExist = function(codeString) {
	console.log('researchCodes/' + codeString);
	const dbRef = ref(getDatabase());
	get(child(dbRef, `researchCodes/` + codeString)).then((snapshot) => {
	  if (snapshot.exists()) {
		console.log(snapshot.val());
		GameInstance.SendMessage('SaveManager', 'ResearchCodeCallback', 'T');
	  } else {
		console.log("No data available");
		GameInstance.SendMessage('SaveManager', 'ResearchCodeCallback', 'F');
	  }
	}).catch((error) => {
	  console.error(error);
	  GameInstance.SendMessage('SaveManager', 'ResearchCodeCallback', 'F');
	});
}
