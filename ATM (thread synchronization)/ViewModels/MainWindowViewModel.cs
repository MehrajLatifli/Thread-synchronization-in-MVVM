using ATM__thread_synchronization_.Commands;
using ATM__thread_synchronization_.Models;
using ATM__thread_synchronization_.Repo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ATM__thread_synchronization_.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindow MainWindows { get; set; }

        public ObservableCollection<Card> _Cards { get; set; }
        public ObservableCollection<Card> Cards { get { return _Cards; } set { _Cards = value; OnPropertyChanged(); } }


        private Card _Card;
        public Card Card { get { return _Card; } set { _Card = value; OnPropertyChanged(); } }

        public FakeRepo CardRepository { get; set; }

        public RelayCommand InsertCartCommand { get; set; }
        public RelayCommand LoadDataCommand { get; set; }
        public RelayCommand TransferMoneyCommand { get; set; }
        public RelayCommand OnlyNumberCommand1 { get; set; }
        public RelayCommand OnlyNumberCommand2 { get; set; }

        int threadcount0 = 1001;
        int threadcount1 = 1001;
        int threadcount2 = 1002;

        double b = 0;
        double t = 0;
        double d = 0;

        bool cardNumberTBoxEnable = true;

        bool hastransver = true;
        bool hasLoad = true;

        Button insertbutton = new Button();
        TextBox cardNumberTBox = new TextBox();
        TextBlock userNameTblock = new TextBlock();
        TextBlock balanceTblock = new TextBlock();

        public static object obj = new object();

        public string _cardNumberTBoxText = string.Empty;
        public string cardNumberTBoxText { get { return _cardNumberTBoxText; } set { _cardNumberTBoxText = value; OnPropertyChanged(); } }

        private string _userNameTblockText;
        public string userNameTblockText { get { return _userNameTblockText; } set { _userNameTblockText = value; OnPropertyChanged(); } }

        private string _balanceTblockText;
        public string balanceTblockText { get { return _balanceTblockText; } set { _balanceTblockText = value; OnPropertyChanged(); } }

        string transferMoneyTboxText = string.Empty;

        private string _decliningBalanceTblockText = string.Empty;

        public string decliningBalanceTblockText { get { return _decliningBalanceTblockText; } set { _decliningBalanceTblockText = value; OnPropertyChanged(); } }

        public MainWindowViewModel()
        {

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;

            DispatcherTimer timer2 = new DispatcherTimer();
            timer2.Tick += Timer2_Tick;

            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer2.Interval = new TimeSpan(0, 0, 0, 0, 500);

            timer.Start();

            Cards = new ObservableCollection<Card>();

            CardRepository = new FakeRepo();
            Cards = new ObservableCollection<Card>(CardRepository.GetAll());



            Thread[] threads0 = new Thread[threadcount0];


            for (int i = 0; i < threadcount0; i++)
            {
                threads0[i] = new Thread(() =>
                {

                    EnableInsert(cardNumberTBoxEnable);

                });
            }


            Thread[] threads1 = new Thread[threadcount1];


            for (int i = 0; i < threadcount1; i++)
            {
                threads1[i] = new Thread(() =>
                {

                    CheckUser(cardNumberTBoxText);

                });
            }


            Thread[] threads2 = new Thread[threadcount2];


            for (int i = 0; i < threadcount2; i++)
            {
                threads2[i] = new Thread(() =>
                {

                    TransferMoney(transferMoneyTboxText, decliningBalanceTblockText);

                });
            }

      

            InsertCartCommand = new RelayCommand((e) =>
            {

                timer.Start();
                timer2.Start();

                threadcount0--;

                // cardNumberTBox.IsEnabled = true;

                threads0.ElementAt(threadcount0).Start();

                //hasLoad = true;

        

            },
            (check) =>
            {
                if (string.IsNullOrEmpty(MainWindows.CardNumberTbox.Text))
                {
                    return true;
                }
                return false;

            });

            LoadDataCommand = new RelayCommand((e) =>
            {
                timer.Start();
                threadcount1--;

                threads1.ElementAt(threadcount1).Start();

                //hasLoad = false;


  
            },
            (check) =>
            {

            if (hastransver )
            {
                return true;
            }

            return false;
    
            });
        
            TransferMoneyCommand = new RelayCommand((e) =>
            {
                try
                {

                    if (MainWindows.CardNumberTbox.Text != string.Empty && MainWindows.TransferMoneyTbox.Text != string.Empty)
                    {
                        hastransver = false;
                        hasLoad = false;

                        timer.Start();
                        threadcount2--;

                        threads2.ElementAt(threadcount2).Start();


                        Thread.Sleep(500);

                        threads0.ElementAt(threadcount0).Abort();

                        Thread.Sleep(500);

                        threads1.ElementAt(threadcount1).Abort();

                        Thread.Sleep(500);
                    }
                    else
                    {
                        MessageBox.Show("Empty field");
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("Error");
                
                }

            },
            (check) =>
            {

                if (hasLoad)
                {
                    return true;
                }

                return false;


            });

            OnlyNumberCommand1 = new RelayCommand((sender) =>
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(MainWindows.CardNumberTbox.Text, @"[^0-9.]"))
                {
                    MessageBox.Show("Please enter only number.");
                    MainWindows.CardNumberTbox.Text = MainWindows.CardNumberTbox.Text.Remove(MainWindows.CardNumberTbox.Text.Length - 1);
                }

            });

            OnlyNumberCommand2 = new RelayCommand((sender) =>
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(MainWindows.TransferMoneyTbox.Text, @"[^0-9]"))
                {
                    MessageBox.Show("Please enter only number.");
                    MainWindows.TransferMoneyTbox.Text = MainWindows.TransferMoneyTbox.Text.Remove(MainWindows.TransferMoneyTbox.Text.Length - 1);
                }

            });

        }


        private void Timer2_Tick(object sender, EventArgs e)
        {
           

            MainWindows.InsertCardButton.Dispatcher.BeginInvoke(new Action(() =>
            {

                MainWindows.CardNumberTbox.IsEnabled = cardNumberTBoxEnable;



            }));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MainWindows.Dispatcher.BeginInvoke(new Action(() =>
            {

                insertbutton = MainWindows.InsertCardButton;
                cardNumberTBox = MainWindows.CardNumberTbox;


                cardNumberTBoxText = MainWindows.CardNumberTbox.Text;
                balanceTblockText = MainWindows.BalanceTblock.Text;
                userNameTblockText = MainWindows.UserNameTblock.Text;


                transferMoneyTboxText = MainWindows.TransferMoneyTbox.Text;


                decliningBalanceTblockText = MainWindows.DecliningBalanceTblock.Text;

            }));



        }

        private void EnableInsert( bool b)
        {
            lock (obj)
            {

                MessageBox.Show($" Insert status: {cardNumberTBoxEnable}");


                MessageBox.Show($" Thread[ ] threads0 {Thread.CurrentThread.ManagedThreadId} starting. \n threadcount0: {threadcount2}");
                Thread.Sleep(500);
                MessageBox.Show($" Thread[ ] threads0 {Thread.CurrentThread.ManagedThreadId} ended. \n threadcount0: {threadcount2}");
            }
        }


        private void CheckUser(string cardNumberTBoxText)
        {
            lock (obj)
            {
                Card = Cards.FirstOrDefault(b => b.CardCode == cardNumberTBoxText);

                if (Card != null)
                {
                    if (cardNumberTBoxText != null)
                    {
                        userNameTblockText = Card.UserName;
                        balanceTblockText = Card.Balance.ToString();
                        decliningBalanceTblockText = Card.DecliningBalance.ToString();
                    }

                    MessageBox.Show($"{userNameTblockText} { balanceTblockText}", "Welcome");
                }



                MessageBox.Show($" Thread[ ] threads1 {Thread.CurrentThread.ManagedThreadId} starting. \n threadcount1: {threadcount1}");
                Thread.Sleep(500);
                MessageBox.Show($" Thread[ ] threads1 {Thread.CurrentThread.ManagedThreadId} ended. \n threadcount1: {threadcount1}");
            }

    }

        private void TransferMoney(string transferMoneyTboxText, string decliningBalanceTblock)
        {
            lock (obj)
            {
              
    

                Thread.Sleep(5000);

                if (double.TryParse(Card.Balance, out b)) { }
                if (double.TryParse(transferMoneyTboxText, out t)) { }
                if (double.TryParse(decliningBalanceTblock, out d)) { }



                if (t > b)
                {
                    MessageBox.Show($"Balance: {b} < {t}");


                    Card.DecliningBalance = d.ToString();
                    Card.Balance = b.ToString();
                }
                else
                {
                    b -= t;

                    Card.Balance = b.ToString();


                    d += t;
                    Card.DecliningBalance = d.ToString();
                }


                MessageBox.Show($" Thread[ ] threads2 {Thread.CurrentThread.ManagedThreadId} starting. \n threadcount2: {threadcount2}");
                Thread.Sleep(500);
                MessageBox.Show($" Thread[ ] threads2 {Thread.CurrentThread.ManagedThreadId} ended. \n threadcount2: {threadcount2}");

                hastransver = true;
                hasLoad = true;
            }
        }
    }
}
