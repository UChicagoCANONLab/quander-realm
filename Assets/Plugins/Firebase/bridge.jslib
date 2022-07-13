mergeInto(LibraryManager.library, {

  SaveData: function(json){
    json = Pointer_stringify(json);
    var str = email.toLowerCase();
    var emailNoPeriod = str.split('.').join(',')
    database.ref('users/' + emailNoPeriod).set(JSON.parse(json));
  },

  DoesResearchCodeExist: function(codeString) {
    database.ref('researchCodes/' + codeString).once('value').then(function(snapshot) {
      console.log(snapshot.val());
      SendMessage('SaveManager', 'LoadCallback', JSON.stringify(snapshot.val()));
    }, function (error) {
       console.log("Error: " + error.code);
    }
    
    );
  }
});
