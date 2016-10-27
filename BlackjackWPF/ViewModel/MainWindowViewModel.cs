using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Spewspeak.ViewModel;
using BlackjackWPF.Model;
using System.Windows.Threading;
using System.Windows.Media;

namespace BlackjackWPF.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Static members definition
        public enum LABELS { Wins, Ties, Losses };
        public enum PLAYERS { Dealer, Human };

        public const string WINS_DISPLAY_LABEL = "Wins: ";
        public const string TIES_DISPLAY_LABEL = "Ties: ";
        public const string LOSSES_DISPLAY_LABEL = "Losses: ";
        public const string PLAYER_WIN_STATUS_TEXT = "Player wins.";
        public const string DEALER_WIN_STATUS_TEXT = "Dealer wins.";
        public const string TIE_STATUS_TEXT = "Hand was a tie.";
        public const string PLAYER_HEADER_TEXT = "Your Hand: {0}";
        public const string PLAYER_HIDDEN_HEADER_TEXT = "Your Hand";
        public const string DEALER_HEADER_TEXT = "Dealer's Hand: {0}";
        public const string DEALER_HIDDEN_HEADER_TEXT = "Dealer's Hand";
        #endregion

        #region INotifyPropertyChangedMembers
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Members definition
        private AsyncObservableCollection<VMCard> dealerCardsList;
        public AsyncObservableCollection<VMCard> DealerCardsList
        {
            get { return dealerCardsList; }
            set
            {
                dealerCardsList = value;
            }
        }

        private AsyncObservableCollection<VMCard> playerCardsList;
        public AsyncObservableCollection<VMCard> PlayerCardsList
        {
            get { return playerCardsList; }
            set { playerCardsList = value; }
        }

        private string winsDisplay;
        public string WinsDisplay
        {
            get { return winsDisplay; }
            set { winsDisplay = value; }
        }

        private string tiesDisplay;
        public string TiesDisplay
        {
            get { return tiesDisplay; }
            set { tiesDisplay = value; }
        }

        private string lossesDisplay;
        public string LossesDisplay
        {
            get { return lossesDisplay; }
            set { lossesDisplay = value; }
        }

        private Visibility isStartVisible;
        public Visibility IsStartVisible
        {
            get { return isStartVisible; }
            set { isStartVisible = value; }
        }

        private bool isStartEnabled;
        public bool IsStartEnabled
        {
            get { return isStartEnabled; }
            set { isStartEnabled = value; }
        }

        private bool isStandReady;
        public bool IsStandReady
        {
            get { return isStandReady; }
            set { isStandReady = value; }
        }

        private bool isHitReady;
        public bool IsHitReady
        {
            get { return isHitReady; }
            set { isHitReady = value; }
        }

        public bool IsInputReady
        {
            get { return (IsStandReady && IsHitReady); }
            set
            {
                IsStandReady = value;
                RaisePropertyChanged(nameof(IsStandReady));
                IsHitReady = value;
                RaisePropertyChanged(nameof(IsHitReady));
            }
        }

        private Visibility isRestartVisible;
        public Visibility IsRestartVisible
        {
            get { return isRestartVisible; }
            set { isRestartVisible = value; }
        }

        private bool isRestartEnabled;
        public bool IsRestartEnabled
        {
            get { return isRestartEnabled; }
            set { isRestartEnabled = value; }
        }

        private Visibility isResetVisible;
        public Visibility IsResetVisible
        {
            get { return isResetVisible; }
            set { isResetVisible = value; }
        }

        private bool isResetEnabled;
        public bool IsResetEnabled
        {
            get { return isResetEnabled; }
            set { isResetEnabled = value; }
        }

        private Visibility isDisplayLabelVisible;
        public Visibility IsDisplayLabelVisible
        {
            get { return isDisplayLabelVisible; }
            set { isDisplayLabelVisible = value; }
        }

        private Brush dealerBorderColor;
        public Brush DealerBorderColor
        {
            get { return dealerBorderColor; }
            set { dealerBorderColor = value; }
        }

        private Brush playerBorderColor;
        public Brush PlayerBorderColor
        {
            get { return playerBorderColor; }
            set { playerBorderColor = value; }
        }

        private BlackjackGame blackjackGame;
        public BlackjackGame BlackjackGame
        {
            get { return blackjackGame; }
            set { blackjackGame = value; }
        }

        private string statusLabelText;
        public string StatusLabelText
        {
            get { return statusLabelText; }
            set { statusLabelText = value; }
        }

        private string dealerBoxHeader;
        public string DealerBoxHeader
        {
            get { return dealerBoxHeader; }
            set { dealerBoxHeader = value; }
        }

        private string playerBoxHeader;
        public string PlayerBoxHeader
        {
            get { return playerBoxHeader; }
            set { playerBoxHeader = value; }
        }

        private ICommand startupCommand;
        public ICommand StartupCommand
        {
            get
            {
                if (startupCommand == null)
                {
                    startupCommand = new NoParamRelayCommand<object>(() => { this.RunStartupActions(); });
                }
                return startupCommand;
            }
        }

        private ICommand startGameCommand;
        public ICommand StartGameCommand
        {
            get
            {
                if (startGameCommand == null)
                {
                    startGameCommand = new NoParamAsyncDelegateCommand(() => { this.StartGame(); });
                }
                return startGameCommand;
            }
        }

        private ICommand hitCommand;
        public ICommand HitCommand
        {
            get
            {
                if (hitCommand == null)
                {
                    hitCommand = new NoParamAsyncDelegateCommand(() => { this.DoPlayerHit(); });
                }
                return hitCommand;
            }
        }

        private ICommand standCommand;
        public ICommand StandCommand
        {
            get
            {
                if (standCommand == null)
                {
                    standCommand = new NoParamAsyncDelegateCommand(() => { this.DoPlayerStand(); });
                }
                return standCommand;
            }
        }

        private ICommand restartGameCommand;
        public ICommand RestartGameCommand
        {
            get
            {
                if (restartGameCommand == null)
                {
                    restartGameCommand = new NoParamAsyncDelegateCommand(() => { this.RestartGame(); });
                }
                return restartGameCommand;
            }
        }

        private ICommand resetGameCommand;
        public ICommand ResetGameCommand
        {
            get
            {
                if (resetGameCommand == null)
                {
                    resetGameCommand = new NoParamAsyncDelegateCommand(() => { this.ResetGame(); });
                }
                return resetGameCommand;
            }
        }
        #endregion

        #region Constructors definition
        /// <summary>
        /// Represents the ViewModel for the MainWindow.xaml View.
        /// </summary>
        public MainWindowViewModel()
        {
            
        }
        #endregion

        #region Methods definition
        private void RunStartupActions()
        {
            this.DealerCardsList = new AsyncObservableCollection<VMCard>()
            {
                new VMCard(),
                new VMCard(),
                new VMCard(),
                new VMCard(),
                new VMCard(),
                new VMCard(),
                new VMCard(),
            };
            RaisePropertyChanged("DealerCardsList");

            this.PlayerCardsList = new AsyncObservableCollection<VMCard>()
            {
                new VMCard(),
                new VMCard(),
                new VMCard(),
                new VMCard(),
                new VMCard(),
                new VMCard(),
                new VMCard(),
            };
            RaisePropertyChanged("PlayerCardsList");

            ChangeDisplayLabel(0, LABELS.Wins);
            ChangeDisplayLabel(0, LABELS.Ties);
            ChangeDisplayLabel(0, LABELS.Losses);

            this.IsResetEnabled = false;
            RaisePropertyChanged(nameof(IsResetEnabled));
            this.IsResetVisible = Visibility.Hidden;
            RaisePropertyChanged(nameof(IsResetVisible));
            this.IsRestartEnabled = false;
            RaisePropertyChanged("IsRestartEnabled");
            this.IsRestartVisible = Visibility.Hidden;
            RaisePropertyChanged("IsRestartVisible");
            this.IsStartEnabled = true;
            RaisePropertyChanged("IsStartEnabled");
            this.IsStartVisible = Visibility.Visible;
            RaisePropertyChanged("IsStartVisible");
            this.IsDisplayLabelVisible = Visibility.Hidden;
            RaisePropertyChanged(nameof(IsDisplayLabelVisible));
            this.IsInputReady = false;
            RaisePropertyChanged("IsInputReady");
            this.DealerBorderColor = Brushes.White;
            RaisePropertyChanged(nameof(DealerBorderColor));
            this.PlayerBorderColor = Brushes.White;
            RaisePropertyChanged(nameof(PlayerBorderColor));
            ChangeTotalLabel(0, PLAYERS.Dealer);
            ChangeTotalLabel(0, PLAYERS.Human);

            return;
        }

        private void StartGame()
        {
            this.IsStartEnabled = false;
            RaisePropertyChanged("IsStartEnabled");
            this.IsStartVisible = Visibility.Hidden;
            RaisePropertyChanged("IsStartVisible");
            this.IsDisplayLabelVisible = Visibility.Visible;
            RaisePropertyChanged(nameof(IsDisplayLabelVisible));
            this.IsResetEnabled = true;
            RaisePropertyChanged(nameof(IsResetEnabled));
            this.IsResetVisible = Visibility.Visible;
            RaisePropertyChanged(nameof(IsResetVisible));

            ChangeTotalLabel(0, PLAYERS.Dealer);
            ChangeTotalLabel(0, PLAYERS.Human);
            this.StatusLabelText = "";
            RaisePropertyChanged(nameof(StatusLabelText));

            if (this.BlackjackGame == null)
            {
                this.BlackjackGame = new BlackjackGame();
                this.BlackjackGame.SetDealer(new Dealer());
                this.BlackjackGame.Dealer.HandChanged += new Player.HandChangedEventHandler(UpdateHands);
                this.BlackjackGame.SetHuman(new Human());
                this.BlackjackGame.Human.HandChanged += new Player.HandChangedEventHandler(UpdateHands);
            }

            this.BlackjackGame.DistributeHands();

            if (this.BlackjackGame.Human.GetHandTotal() == 21)
            {
                // Player got natural 21.
                this.IsResetEnabled = false;
                RaisePropertyChanged(nameof(IsResetEnabled));
                this.IsResetVisible = Visibility.Hidden;
                RaisePropertyChanged(nameof(IsResetVisible));
                this.IsRestartEnabled = true;
                RaisePropertyChanged(nameof(IsRestartEnabled));
                this.IsRestartVisible = Visibility.Visible;
                RaisePropertyChanged(nameof(IsRestartVisible));
                this.IsDisplayLabelVisible = Visibility.Hidden;
                RaisePropertyChanged(nameof(IsDisplayLabelVisible));
                this.IsInputReady = false;
                RaisePropertyChanged(nameof(IsInputReady));

                this.DealerBorderColor = Brushes.Red;
                RaisePropertyChanged(nameof(DealerBorderColor));
                this.PlayerBorderColor = Brushes.Green;
                RaisePropertyChanged(nameof(PlayerBorderColor));
                BlackjackGame.Human.Wins++;
                ChangeDisplayLabel(BlackjackGame.Human.Wins, LABELS.Wins);
                this.StatusLabelText = PLAYER_WIN_STATUS_TEXT;
                RaisePropertyChanged(nameof(StatusLabelText));

                DisplayDealerHand();
            }
            else
            {
                this.IsInputReady = true;
                RaisePropertyChanged("IsInputReady");
            }
        }

        private void DoPlayerHit()
        {
            this.IsInputReady = false;
            RaisePropertyChanged("IsInputReady");

            if (BlackjackGame.ActingPlayer == BlackjackGame.Human)
            {
                BlackjackGame.DoPlayerAction(BlackjackGame.ACTIONS.Hit);
            }

            if (BlackjackGame.Human.IsBusted)
            {
                this.DealerBorderColor = Brushes.Green;
                RaisePropertyChanged(nameof(DealerBorderColor));
                this.PlayerBorderColor = Brushes.Red;
                RaisePropertyChanged(nameof(PlayerBorderColor));
                this.BlackjackGame.Human.Losses++;
                ChangeDisplayLabel(BlackjackGame.Human.Losses, LABELS.Losses);
                this.StatusLabelText = DEALER_WIN_STATUS_TEXT;
                RaisePropertyChanged(nameof(StatusLabelText));
                this.IsInputReady = false;
                RaisePropertyChanged("IsInputReady");
                this.IsResetEnabled = false;
                RaisePropertyChanged(nameof(IsResetEnabled));
                this.IsResetVisible = Visibility.Hidden;
                RaisePropertyChanged(nameof(IsResetVisible));
                this.IsDisplayLabelVisible = Visibility.Hidden;
                RaisePropertyChanged(nameof(IsDisplayLabelVisible));
                this.IsRestartEnabled = true;
                RaisePropertyChanged(nameof(IsRestartEnabled));
                this.IsRestartVisible = Visibility.Visible;
                RaisePropertyChanged(nameof(IsRestartVisible));

                DisplayDealerHand();
            }
            else
            {
                this.IsInputReady = true;
                RaisePropertyChanged("IsInputReady");
            }
        }

        private void DoPlayerStand()
        {
            this.IsInputReady = false;
            RaisePropertyChanged(nameof(IsInputReady));

            BlackjackGame.ActingPlayer = BlackjackGame.Dealer;

            if (BlackjackGame.Dealer.GetHandTotal() <= BlackjackGame.Human.GetHandTotal())
            {
                do
                {
                    BlackjackGame.DoDealerAction();
                } while (BlackjackGame.ActingPlayer == BlackjackGame.Dealer);
            }
            
            if (BlackjackGame.Dealer.IsBusted && !BlackjackGame.Human.IsBusted)
            {
                this.DealerBorderColor = Brushes.Red;
                RaisePropertyChanged(nameof(DealerBorderColor));
                this.PlayerBorderColor = Brushes.Green;
                RaisePropertyChanged(nameof(PlayerBorderColor));
                BlackjackGame.Human.Wins++;
                ChangeDisplayLabel(BlackjackGame.Human.Wins, LABELS.Wins);
                this.StatusLabelText = PLAYER_WIN_STATUS_TEXT;
                RaisePropertyChanged(nameof(StatusLabelText));
            }
            else
            {
                Player winner = BlackjackGame.GetWinner();
                if (winner == null)
                {
                    // It was a tie.
                    this.DealerBorderColor = Brushes.Green;
                    RaisePropertyChanged(nameof(DealerBorderColor));
                    this.PlayerBorderColor = Brushes.Green;
                    RaisePropertyChanged(nameof(PlayerBorderColor));
                    this.StatusLabelText = TIE_STATUS_TEXT;
                    RaisePropertyChanged(nameof(StatusLabelText));
                    BlackjackGame.Human.Ties++;
                    ChangeDisplayLabel(BlackjackGame.Human.Ties, LABELS.Ties);
                }
                else if (winner == BlackjackGame.Human)
                {
                    // Human player won.
                    this.DealerBorderColor = Brushes.Red;
                    RaisePropertyChanged(nameof(DealerBorderColor));
                    this.PlayerBorderColor = Brushes.Green;
                    RaisePropertyChanged(nameof(PlayerBorderColor));
                    BlackjackGame.Human.Wins++;
                    ChangeDisplayLabel(BlackjackGame.Human.Wins, LABELS.Wins);
                    this.StatusLabelText = PLAYER_WIN_STATUS_TEXT;
                    RaisePropertyChanged(nameof(StatusLabelText));
                }
                else if (winner == BlackjackGame.Dealer)
                {
                    // Dealer won.
                    this.DealerBorderColor = Brushes.Green;
                    RaisePropertyChanged(nameof(DealerBorderColor));
                    this.PlayerBorderColor = Brushes.Red;
                    RaisePropertyChanged(nameof(PlayerBorderColor));
                    BlackjackGame.Human.Losses++;
                    ChangeDisplayLabel(BlackjackGame.Human.Losses, LABELS.Losses);
                    this.StatusLabelText = DEALER_WIN_STATUS_TEXT;
                    RaisePropertyChanged(nameof(StatusLabelText));
                }
            }

            this.IsResetEnabled = false;
            RaisePropertyChanged(nameof(IsResetEnabled));
            this.IsResetVisible = Visibility.Hidden;
            RaisePropertyChanged(nameof(IsResetVisible));
            this.IsDisplayLabelVisible = Visibility.Hidden;
            RaisePropertyChanged(nameof(IsDisplayLabelVisible));
            this.IsRestartEnabled = true;
            RaisePropertyChanged(nameof(IsRestartEnabled));
            this.IsRestartVisible = Visibility.Visible;
            RaisePropertyChanged(nameof(IsRestartVisible));

            DisplayDealerHand();
        }

        private void RestartGame()
        {
            BlackjackGame.ResetGame();

            this.DealerBorderColor = Brushes.White;
            RaisePropertyChanged(nameof(DealerBorderColor));
            this.PlayerBorderColor = Brushes.White;
            RaisePropertyChanged(nameof(PlayerBorderColor));

            this.IsRestartEnabled = false;
            RaisePropertyChanged(nameof(IsRestartEnabled));
            this.IsRestartVisible = Visibility.Hidden;
            RaisePropertyChanged(nameof(IsRestartVisible));

            this.IsStartEnabled = true;
            RaisePropertyChanged(nameof(IsStartEnabled));
            this.IsStartVisible = Visibility.Visible;
            RaisePropertyChanged(nameof(IsStartVisible));

            ChangeTotalLabel(0, PLAYERS.Dealer);
            ChangeTotalLabel(0, PLAYERS.Human);
        }

        private void DisplayDealerHand()
        {
            ChangeTotalLabel(this.BlackjackGame.Dealer.GetHandTotal(), PLAYERS.Dealer);

            for (int i = 0; i < this.BlackjackGame.Dealer.CurrentHand.Length; i++)
            {
                UpdateHands(this.BlackjackGame.Dealer, new HandChangedEventArgs(i, this.BlackjackGame.Dealer.CurrentHand[i]));
            }
        }

        private void UpdateHands(object sender, HandChangedEventArgs args)
        {
            if (sender is Dealer)
            {
                Card cCard = this.BlackjackGame.Dealer.GetCard(args.Index, !this.IsRestartEnabled);
                this.DealerCardsList[args.Index] = new VMCard(cCard.Pos, cCard.Suit);
            }
            else if (sender is Human)
            {
                this.PlayerCardsList[args.Index] = new VMCard(args.Card.Pos, args.Card.Suit);
                ChangeTotalLabel(this.BlackjackGame.Human.GetHandTotal(), PLAYERS.Human);
            }
        }

        private void ResetGame()
        {
            this.RestartGame();
            ChangeDisplayLabel(0, LABELS.Wins);
            ChangeDisplayLabel(0, LABELS.Ties);
            ChangeDisplayLabel(0, LABELS.Losses);
            this.BlackjackGame.Human.Wins = 0;
            this.BlackjackGame.Human.Ties = 0;
            this.BlackjackGame.Human.Losses = 0;
            this.StartGame();
        }

        /// <summary>
        /// Set the specified label to the specified number.
        /// </summary>
        /// <param name="num">Number to set the label to.</param>
        /// <param name="label">Label to set the number on.</param>
        private void ChangeDisplayLabel(int num, LABELS label)
        {
            if (num < 0)
            {
                throw new ArgumentOutOfRangeException("The number to write to the label cannot be negative.", nameof(num));
            }

            switch (label)
            {
                case LABELS.Wins:
                    this.WinsDisplay = WINS_DISPLAY_LABEL + num.ToString();
                    RaisePropertyChanged(nameof(WinsDisplay));
                    break;
                case LABELS.Ties:
                    this.TiesDisplay = TIES_DISPLAY_LABEL + num.ToString();
                    RaisePropertyChanged(nameof(TiesDisplay));
                    break;
                case LABELS.Losses:
                    this.LossesDisplay = LOSSES_DISPLAY_LABEL + num.ToString();
                    RaisePropertyChanged(nameof(LossesDisplay));
                    break;
            }

            return;
        }

        private void ChangeTotalLabel(int num, PLAYERS player)
        {
            if (num < 0)
            {
                throw new ArgumentOutOfRangeException("The number to write to the label cannot be negative.", nameof(num));
            }

            switch (player)
            {
                case PLAYERS.Dealer:
                    if (num == 0)
                        { this.DealerBoxHeader = DEALER_HIDDEN_HEADER_TEXT; }
                    else
                        { this.DealerBoxHeader = String.Format(DEALER_HEADER_TEXT, num); }
                    RaisePropertyChanged(nameof(DealerBoxHeader));
                    break;
                case PLAYERS.Human:
                    if (num == 0)
                        { this.PlayerBoxHeader = PLAYER_HIDDEN_HEADER_TEXT; }
                    else
                        { this.PlayerBoxHeader = String.Format(PLAYER_HEADER_TEXT, num); }
                    RaisePropertyChanged(nameof(PlayerBoxHeader));
                    break;
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            // Uses null-conditional operator to invoke the event handler if it is not null.
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            return;
        }
        #endregion
    }
}
