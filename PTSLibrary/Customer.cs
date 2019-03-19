using System;
using System.Collections.Generic;
using System.Text;

namespace PTSLibrary
{
    class Customer : User
    {
        public Customer(string name, int id)
        {
            this.name = name;
            this.id = id;
        }
    }
}
