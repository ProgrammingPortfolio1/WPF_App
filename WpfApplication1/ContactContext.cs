using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace WpfApplication1
{
    public class ContactContext : DbContext
    {
        public ContactContext() : base("name=ContactContext")
        {
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
