mergeInto(LibraryManager.library, {  

  ShowFullScreenAdvertisement: function () {
    ysdk.adv.showFullscreenAdv({
      callbacks: {
        onOpen: function() {          
          myGameInstance.SendMessage('YandexAdv', 'OpenFullScreenAdvertisement');
        },
        onClose: function(wasShown) {
          myGameInstance.SendMessage('YandexAdv', 'CloseFullScreenAdvertisement');
        },
        onError: function(error) {
          // some action on error
        }
      }
    })
  },

  ShowRevardedAdv: function () {
    ysdk.adv.showRewardedVideo({
      callbacks: {
        onOpen: () => {
          myGameInstance.SendMessage('YandexAdv', 'OpenShowRevardedAdv');
        },
        onRewarded: () => {
          myGameInstance.SendMessage('YandexAdv', 'RewardedShowRevardedAdv');		      
        },
        onClose: () => {
          myGameInstance.SendMessage('YandexAdv', 'CloseShowRevardedAdv');
        }, 
        onError: (e) => {
          console.log('Error while open video ad:', e);
        }
      }
    })
  },
});