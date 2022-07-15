mergeInto(LibraryManager.library, {

  DoesResearchCodeExist: function(codeString) {
    JSDoesResearchCodeExist(UTF8ToString(codeString));
  },

  SaveData: function(codeString, json){
    JSSaveData(UTF8ToString(codeString), UTF8ToString(json));
  },

  LoadData: function(codeString){
    JSLoadData(UTF8ToString(codeString));
  }

});
