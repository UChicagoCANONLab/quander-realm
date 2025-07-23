using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Wrapper
{
    public static class Events
    {
    #region Game Management

        /// GameManager ///
        public static Action<Minigame> OpenMinigame;
        public static Func<RewardAsset, GameObject, DisplayType, GameObject> CreatRewardCard;
        public static Func<BonusAsset, GameObject, GameObject> CreateBonus;
        public static Action<string> ShowCardPopup;
        public static Action ToggleLoadingScreen;
        public static Action<Game, int> CollectAndDisplayReward;
        public static Action<Game, int, int> CollectAndDisplayBadge;
        public static Action<Game> UnlockAndDisplayGame;
        public static Action DisplayAgeSelector;
        public static Action<int> DisplayCoinsCollected;
        public static Action<bool> ToggleBackButton;
        public static Action Logout;
        public static Action PlayIntroDialog;
        public static Action MinigameClosed;
        public static Func<Game, string> GetMinigameTitle;
        public static Func<Game> GetCurrentGame;
        public static Func<bool> IsDebugEnabled;
        public static Action<float> Delay;

        /// Trackers ///
        public static Action InitializeStarTracker;
        public static Action ResetStarCounts;

        /// MapManager ///
        public static Action InitializeMap;
        public static Action ResetMap;

        /// TitleScreen ///
        public static Action<bool> SetNewPlayerStatus;
        public static Action<bool> ToggleTitleScreen;

    #endregion

    #region Save System

        /// SaveManager ///
        public static Func<string, bool> AddReward;
        public static Func<string, bool> AddBadge;
        public static Func<string, bool> AddBonus;
        public static Func<string, bool> UseAvailableBonus;
        public static Func<string, int> NumberBonuses;
        public static Action ClearSaveFile;
        public static Action<string> SubmitResearchCode;
        public static Func<string, bool> IsRewardUnlocked;
        public static Action UpdateRemoteSave;
        public static Func<string> GetPlayerResearchCode;
        public static Func<Game, string> GetMinigameSaveData;
        public static Action<Game, object> UpdateMinigameSaveData;
        public static Action<Game, object> SaveMinigameResearchData;
        public static Func<(bool, bool)> GetRewardDialogStats;
        public static Action<bool> SetRewardTextSeen;
        public static Func<string, bool> GetFirstRewardBool;
        public static Action<int> UpdateUserSaveTotalStars;
        public static Action<int> UpdateUserSaveTotalCoins;
        public static Func<int> GetUserSaveTotalCoins;
        public static Action<Game, int> UpdateMinigameStarCount;
        public static Func<Game, int> GetMinigameStarCount;
        // public static Action UpdateStreakLength;
        public static Func<int> GetStreakLength;
        public static Func<int> GetStreakFreeze;
        public static Action<int> SetStreakFreeze;
        public static Func<Game, int> HasRewardsFromGame;
        public static Func<Game, int> GetMinigameMapIcon;
        public static Action<Game, int> UpgradeMinigameMapIcon;

        /// MinigameUserSaves ///
        public static Func<Game, int> GetMinigameTotalStars;
        public static Func<int> GetOverallTotalStars;
        public static Func<Game, bool> GetGameUnlocked;
        public static Func<Game, int> GetMinigameMaxLevel;
        public static Func<Game, bool> GetMinigameAllLevelsUnlocked;
        public static Action<Game> LoadMinigameSave;
        public static Action LoadAllMinigameSaves;
        public static Action<Game> UnlockNextLevel;

        /// UploadFailurePopups /// 
        public static Action<bool> ToggleUploadFailurePopup;

        /// LoginScreen OR QuantimeLoginScreen ///
        public static Action OpenLoginScreen;
        public static Action<LoginStatus> UpdateLoginStatus;
        public static Action CloseLoginScreen;

    #endregion

    #region Dialogue System

        /// DialogueManager ///
        public static Action<string> PrintDialogue;
        public static Action<string> StartDialogueSequence;
        public static Action<int> ChangeDialogue;

        /// DialogueView ///
        public static Action<Dialogue> OpenDialogueView;
        public static Action CloseDialogueView;
        public static Action<Dialogue, int> UpdateDialogueView;
        public static Action<bool> SwitchNextButton;

        /// DialogueSequence ///
        public static Action SortSequences;

        /// DialogueButton ///
        public static Action<bool> TogglePreviousButton;
        public static Action<bool> ToggleSkipButton;

        /// Within Minigame Scripts and RewardResearchData ///
        public static Action DialogueSequenceEnded;

    #endregion

    #region Reward System

        /// RewardCenter ///
        // public static Func<bool> ReturnToRewardCenter;

        /// Reward ///
        public static Action UnselectAllCards;

        /// RewardJournal ///
        public static Func<int, Toggle> GetNavDot;
        public static Action<JournalPage> SwitchPage;
        public static Action<string> FeatureCard;
        // public static Action<JournalPage> OpenJournalPage;

        /// JournalSection ///
        public static Action<JournalPage> UpdateTab;
        public static Action ResetPageNumbers;

    #endregion

    #region Screen Fading, Audio System
    
        /// AudioManager ///
        public static Action<string> PlayMusic;
        public static Action StopMusic;
        public static Action<string> PlaySound;

        /// ScreenFader ///
        public static Action<Action, float> ScreenFadeMidAction;
        public static Action StopScreenFade;
    
    #endregion

    #region Debug

        /// ??? ///
        public static Action<string> BBGotoLevel;
        public static Action BBToggleDebug;
        public static Action BBClearMarkers;

    #endregion
    }
}
