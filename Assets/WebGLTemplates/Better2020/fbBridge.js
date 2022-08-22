// Import the functions you need from the SDKs you need
import { initializeApp } from "https://www.gstatic.com/firebasejs/9.8.4/firebase-app.js";
import { getAnalytics } from "https://www.gstatic.com/firebasejs/9.8.4/firebase-analytics.js";
import { getDatabase, ref, child, get, set } from "https://www.gstatic.com/firebasejs/9.8.4/firebase-database.js";

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
	const dbRef = ref(getDatabase());
	get(child(dbRef, 'researchCodes/' + codeString)).then((snapshot) => {
	  if (snapshot.exists()) {
			GameInstance.SendMessage('SaveManager', 'ResearchCodeCallback', 'T');
	  } else {
			GameInstance.SendMessage('SaveManager', 'ResearchCodeCallback', 'F');
	  }
	}).catch((error) => {
	  console.error(error);
	  GameInstance.SendMessage('SaveManager', 'ResearchCodeCallback', 'F');
	});
}

JSSaveData = function(codeString, json) {
    const dbRef = getDatabase();
    set(ref(dbRef, 'userData/' + codeString), JSON.parse(json)).then(() => {
			GameInstance.SendMessage('SaveManager', 'SaveDataCallback', 'success');
		}).catch((error) => {
			GameInstance.SendMessage('SaveManager', 'SaveDataCallback', 'failure');
		});
}

JSLoadData = function(codeString) {
	const dbRef = ref(getDatabase());
	get(child(dbRef, 'userData/' + codeString)).then((snapshot) => {
	  if (snapshot.exists()) {
			GameInstance.SendMessage('SaveManager', 'LoadDataCallback', JSON.stringify(snapshot.val()));
	  } else {
			GameInstance.SendMessage('SaveManager', 'LoadDataCallback', 'none');
	  }
	}).catch((error) => {
	  console.error(error);
	  GameInstance.SendMessage('SaveManager', 'LoadDataCallback', 'none');
	});
}