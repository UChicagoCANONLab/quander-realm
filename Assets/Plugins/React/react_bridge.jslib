mergeInto(LibraryManager.library, {
  SaveResearchData: function (gameCode, data) {
    window.dispatchReactUnityEvent("SaveResearchData",  gameCode, UTF8ToString(data));
  },
});
