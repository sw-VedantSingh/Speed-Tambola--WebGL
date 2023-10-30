	mergeInto(LibraryManager.library, {


  CloseWindow: function () {
  ExitGame();
  },
  GetLoginData: function () {
  GetLoginDetails("game");
  },
  FullScreen: function () {
  SetFullScreen();
  },
  ShowDeposit: function () {
  Deposit();
  },

  InitPlayerBet: function (type,index,game_user_Id,game_Id,metaData,isAbleToCancel,bet_amount) {
   
    InitBet(UTF8ToString(type),index,UTF8ToString(game_user_Id),UTF8ToString(game_Id),UTF8ToString(metaData),UTF8ToString(isAbleToCancel),bet_amount);
  },
  CancelPlayerBet: function (type,id,metaData,game_user_Id,game_Id,amount) {
    
  CancelBet(UTF8ToString(type),UTF8ToString(id),UTF8ToString(metaData),UTF8ToString(game_user_Id),UTF8ToString(game_Id),amount);
  },
  FinilizePlayerBet: function (type,id,metaData,game_user_Id,game_Id) {
      
  FinilizeBet(UTF8ToString(type),UTF8ToString(id),UTF8ToString(metaData),UTF8ToString(game_user_Id),UTF8ToString(game_Id));
  },
  WinningsPlayerBet: function (type,id,metaData,game_user_Id,game_Id,win_amount,spend_amount) {
   
  WinningsBet(UTF8ToString(type),UTF8ToString(id),UTF8ToString(metaData),UTF8ToString(game_user_Id),UTF8ToString(game_Id),win_amount,spend_amount);
  },
  AppPlayerBet: function (type,index,id,metaData,game_user_Id,game_Id,amount) {
   
  AddBet(UTF8ToString(type),UTF8ToString(index),UTF8ToString(id),UTF8ToString(metaData),UTF8ToString(game_user_Id),UTF8ToString(game_Id),amount);
  }	


});