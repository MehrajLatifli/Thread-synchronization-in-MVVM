using ATM__thread_synchronization_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM__thread_synchronization_.Repo
{
    public class FakeRepo
    {

        public List<Card> GetAll()
        {
            return new List<Card>
            {

                new Card
                {
                  CardCode="12345678901234",
                  UserName="Username1",
                  Balance="566233.221",
                  DecliningBalance="0"
                },

                new Card
                {
                  CardCode="43210987654321",
                  UserName="Username2",
                  Balance="20.5",
                  DecliningBalance="0"
                },
            };
        }
    }
}
