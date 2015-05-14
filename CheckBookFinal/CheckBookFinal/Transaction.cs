using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckBookFinal
{
    public class Transaction : BaseVM
    {
        public int Id { get; set; }


        /*
        public IEnumerable<Transaction> SimilarTransactions {
            get {
                return from t in VM.Transactions
                       where t.Payee == this.Payee
                       select t;
            }
        }
        */
        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value; OnPropertyChanged(); }
        }

        private string _Payee;
        public string Payee
        {
            get { return _Payee; }
            set { _Payee = value; OnPropertyChanged(); }
        }

        public int AccountId { get; set; }

        private Account _Account;
        public virtual Account Account
        {
            get { return _Account; }
            set { _Account = value; OnPropertyChanged(); }
        }

        private decimal _Amount;
        public decimal Amount
        {
            get { return _Amount; }
            set { _Amount = value; OnPropertyChanged(); OnPropertyChanged("Currency2"); }
        }

        
        private string _Tag;
        public string Tag
        {
            get { return _Tag; }
            set { _Tag = value; OnPropertyChanged(); }
        }


    }


    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Institution { get; set; }
 
        public virtual IList<Transaction> Transactions { get; set; }

        public decimal Balance { get; set; }
    }
}
