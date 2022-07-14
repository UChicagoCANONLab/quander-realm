mergeInto(LibraryManager.library, {
  SaveData: function(json){
    json = Pointer_stringify(json);
    var str = email.toLowerCase();
    var emailNoPeriod = str.split('.').join(',')
    database.ref('users/' + emailNoPeriod).set(JSON.parse(json));
  },
  LoadData: function() {
    var str = email.toLowerCase();
    var emailNoPeriod = str.split('.').join(',')
    database.ref('users/' + emailNoPeriod).once('value').then(function(snapshot) {
      console.log(snapshot.val());
      SendMessage('SaveSystem', 'LoadCallback', JSON.stringify(snapshot.val()));
    }, function (error) {
       console.log("Error: " + error.code);
    });
  }
});
