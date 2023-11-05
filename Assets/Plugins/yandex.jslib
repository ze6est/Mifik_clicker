mergeInto(LibraryManager.library, {  

  ShowFullScreenAdvertisement: function () {
    ysdk.adv.showFullscreenAdv({
      callbacks: {
        onOpen: function() {
          myGameInstance.SendMessage('YandexAdv', 'AddPointsPerClick');
        },
        onClose: function(wasShown) {
          
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
          console.log('Video ad open.');
        },
        onRewarded: () => {
          myGameInstance.SendMessage('YandexAdv', 'AddPointsPerClick');
		      myGameInstance.SendMessage('YandexAdv', 'RefreshPoints');
        },
        onClose: () => {
          console.log('Video ad closed.');
        }, 
        onError: (e) => {
          console.log('Error while open video ad:', e);
        }
      }
    })
  },
});