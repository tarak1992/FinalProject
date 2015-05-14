using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.Drawing;
using System.Windows.Media;

namespace CheckBookFinal
{
         public class CheckBookVM : BaseVM
        {
            public CheckBookVM()
            {
            }

            CbDb _Db = new CbDb();
            private int _RowsPerPage = 5;
            private int _CurrentPage = 1;
            public int CurrentPage
            {
                get { return _CurrentPage; }
                set { _CurrentPage = value; OnPropertyChanged(); OnPropertyChanged("CurrentTransactions"); }
            }

            public IEnumerable<Transaction> CurrentTransactions
            {
                get { return Transactions.Skip((_CurrentPage - 1) * _RowsPerPage).Take(_RowsPerPage); }
            }

            public DelegateCommand MoveNext
            {
                get
                {
                    return new DelegateCommand
                    {
                        ExecuteFunction = _ => CurrentPage++,
                        CanExecuteFunction = _ => CurrentPage * _RowsPerPage < Transactions.Count
                    };
                }
            }

            public Transaction _NewTransaction = new Transaction { Date = DateTime.Now };
            public Transaction NewTransaction
            {
                get { return _NewTransaction; }
                set { _NewTransaction = value; ; OnPropertyChanged(); }
            }

            private ObservableCollection<Transaction> _Transactions;
            public ObservableCollection<Transaction> Transactions
            {
                get { return _Transactions; }
                set { _Transactions = value; OnPropertyChanged(); OnPropertyChanged("Accounts"); }
            }

            private ObservableCollection<Account> _Accounts;
            public ObservableCollection<Account> Accounts
            {
                get { return _Accounts; }
                set { _Accounts = value; OnPropertyChanged(); }
            }

            public DelegateCommand Save
            {
                get
                {      
                    return new DelegateCommand
                    {
                        ExecuteFunction = _ =>
                        {
                            _Db.Transactions.Add(_NewTransaction);
                            Account balancesum = (from bal in Accounts where bal == _NewTransaction.Account select bal).Single();
                            balancesum.Balance += _NewTransaction.Amount;
                           _Db.SaveChanges();
                        }
                     };
                }
            }

          /*  public static Brush Red
            {
                get
                {
                    return 
                    {
                    Transaction morePayee = (from a in Transactions where a.Amount > _NewTransaction.Amount select a).Single();
                    }; 
                }
            }
*/
            private Account _newInfo = new Account { };
            public Account newInfo
            {
                get { return _newInfo; }
                set { _newInfo = value; OnPropertyChanged(); }
             }

            public DelegateCommand NewAccount
            {
                get
                {
                    return new DelegateCommand
                    {
                        ExecuteFunction = _ =>
                        {
                            var ids = _Db.Accounts.Select(x => x.Name).ToList();
                            if (!ids.Contains(_newInfo.Name) && _newInfo.Name != null)
                            {
                                _Db.Accounts.Add(_newInfo);
                                _Db.SaveChanges();
                            }
                        },
                    };
                 }
            }

            public DelegateCommand DeleteAccount
            {
                get
                {
                    return new DelegateCommand
                    {
                        ExecuteFunction = _ =>
                        {
                            var ids = _Db.Accounts.Select(x => x.Name);
                            if (ids.Contains(_newInfo.Name))
                            {
                                _Db.Accounts.Remove(_newInfo);//Need to modify
                                _Db.SaveChanges();
                            }
                        },

                    };
                }
            }
            public void Fill()
            {
                Accounts = _Db.Accounts.Local;
                Transactions = _Db.Transactions.Local;
                _Db.Accounts.ToList();
                _Db.Transactions.ToList();
            }
       }
}
