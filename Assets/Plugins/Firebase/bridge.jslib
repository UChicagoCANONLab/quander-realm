mergeInto(LibraryManager.library, {

  SaveData: function(json){
    json = Pointer_stringify(json);
    var str = email.toLowerCase();
    var emailNoPeriod = str.split('.').join(',')
    database.ref('users/' + emailNoPeriod).set(JSON.parse(json));
  },

  DoesResearchCodeExist: function(codeString) {
    return DCE(UTF8ToString(codeString));
  }
});
